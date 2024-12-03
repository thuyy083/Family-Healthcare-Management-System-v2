using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using HTQL_DichVuYTeGiaDinh.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class QuanTriVienController : Controller
    {
        private readonly DataContext _context;

        public QuanTriVienController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            int pageSize = 4; 
            int pageNumber = (page ?? 1); 

            var nhanVienYTeQuery = _context.NhanVienYTes
                .Where(nv => nv.MaQuyen == 1)
                .AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                nhanVienYTeQuery = nhanVienYTeQuery.Where(nv => nv.HoTen.Contains(searchString));
            }

            int totalItems = await nhanVienYTeQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString; 

            var paginatedList = await nhanVienYTeQuery
                .OrderBy(nv => nv.HoTen)
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize) 
                .ToListAsync();

            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }


        // GET: Admin/NhanVienYTe/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Admin/NhanVienYTe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNhanVienYTeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Nếu có lỗi validate, trả lại View và hiển thị thông tin lỗi
                return View(viewModel);
            }

            // Kiểm tra số điện thoại và số CCCD
            var isPhoneExists = await _context.TaiKhoans.AnyAsync(tk => tk.SoDienThoai == viewModel.SoDienThoai);
            var isCccdExists = await _context.TaiKhoans.AnyAsync(tk => tk.SoCccd == viewModel.SoCccd);

            if (isPhoneExists)
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại đã tồn tại trong hệ thống.");
            }

            if (isCccdExists)
            {
                ModelState.AddModelError("SoCccd", "Số CCCD đã tồn tại trong hệ thống.");
            }

            if (viewModel.HinhAnh != null && viewModel.HinhAnh.Length > 0)
            {
                var extension = Path.GetExtension(viewModel.HinhAnh.FileName).ToLower();

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận các định dạng file: .jpg, .jpeg, .png, .gif");
                    return View(viewModel);
                }

                var newFileName = Guid.NewGuid().ToString() + extension;

                var imagePath = Path.Combine("wwwroot/images/quantrivien", newFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await viewModel.HinhAnh.CopyToAsync(stream);
                }

                // Tạo tài khoản trước
                var taiKhoan = new TaiKhoan
                {
                    HoTen = viewModel.HoTen,
                    NgaySinh = viewModel.NgaySinh,
                    SoCccd = viewModel.SoCccd,
                    GioiTinh = viewModel.GioiTinh,
                    DiaChi = viewModel.DiaChi,
                    SoDienThoai = viewModel.SoDienThoai,
                    MatKhau = viewModel.MatKhau,
                    HinhAnh = newFileName,
                    DiaChiEmail = viewModel.DiaChiEmail,
                    MaQuyen = 1
                };

                _context.TaiKhoans.Add(taiKhoan);
                await _context.SaveChangesAsync();

                // Lưu thông tin Nhân viên y tế với mã tài khoản đã tạo
                var nhanVienYTe = new NhanVienYTe
                {
                    MaTk = taiKhoan.MaTk, // Liên kết với tài khoản vừa tạo
                    HoTen = viewModel.HoTen,
                    NgaySinh = viewModel.NgaySinh,
                    SoCccd = viewModel.SoCccd,
                    GioiTinh = viewModel.GioiTinh,
                    DiaChi = viewModel.DiaChi,
                    SoDienThoai = viewModel.SoDienThoai,
                    MatKhau = viewModel.MatKhau,
                    HinhAnh = newFileName,
                    DiaChiEmail = viewModel.DiaChiEmail,
                    MaQuyen = 1
                };

                _context.NhanVienYTes.Add(nhanVienYTe);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("HinhAnh", "Vui lòng chọn file hình ảnh.");
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            // Lấy thông tin nhân viên y tế theo mã
            var nhanVienYTe = await _context.NhanVienYTes
                .FirstOrDefaultAsync(nv => nv.MaTk == id);

            if (nhanVienYTe == null)
            {
                return NotFound(); // Nếu không tìm thấy nhân viên
            }

            return View(nhanVienYTe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Kiểm tra nhân viên
            var nhanVienYTe = await _context.NhanVienYTes.FindAsync(id);
            if (nhanVienYTe == null)
            {
                return NotFound();
            }

            // Kiểm tra tài khoản liên quan
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            // Xóa ảnh nếu có
            if (!string.IsNullOrEmpty(nhanVienYTe.HinhAnh))
            {
                var imagePath = Path.Combine("wwwroot/images/quantrivien", nhanVienYTe.HinhAnh);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Xóa trong database
            _context.NhanVienYTes.Remove(nhanVienYTe);
            _context.TaiKhoans.Remove(taiKhoan);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



    }
}
