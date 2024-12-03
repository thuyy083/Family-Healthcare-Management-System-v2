using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HTQL_DichVuYTeGiaDinh.ViewModels;

namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhanVienYTeController : Controller
    {
        private readonly DataContext _context;

        public NhanVienYTeController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            int pageSize = 4; 
            int pageNumber = (page ?? 1); 

            var nhanVienYTeQuery = _context.NhanVienYTes
                .Where(nv => nv.MaQuyen == 3 && !nv.IsDeleted)
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

            // Đánh số thứ tự cho dữ liệu
            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }

        public async Task<IActionResult> HiddenStaff(string searchString, int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            // Truy vấn danh sách nhân viên y tế bị ẩn (isDeleted = true)
            var hiddenNhanVienQuery = _context.NhanVienYTes
                .Where(nv => nv.IsDeleted == true)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                hiddenNhanVienQuery = hiddenNhanVienQuery.Where(nv => nv.HoTen.Contains(searchString));
            }

            int totalItems = await hiddenNhanVienQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString;

            var paginatedList = await hiddenNhanVienQuery
                .OrderBy(nv => nv.HoTen)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SoftDelete(int id)
        {
            var nhanVien = _context.NhanVienYTes.FirstOrDefault(nv => nv.MaTk == id);
            if (nhanVien == null)
            {
                TempData["ErrorMessage"] = "Nhân viên y tế không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            // Cập nhật trạng thái isDeleted
            nhanVien.IsDeleted = true;

            // Cập nhật trạng thái tài khoản liên quan
            var taiKhoan = _context.TaiKhoans.FirstOrDefault(tk => tk.MaTk == id);
            if (taiKhoan != null)
            {
                taiKhoan.IsDeleted = true;
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Xóa nhân viên y tế thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restore(int id)
        {
            var nhanVien = _context.NhanVienYTes.FirstOrDefault(nv => nv.MaTk == id);
            if (nhanVien == null)
            {
                TempData["ErrorMessage"] = "Nhân viên y tế không tồn tại.";
                return RedirectToAction(nameof(HiddenStaff));
            }

            // Cập nhật trạng thái isDeleted
            nhanVien.IsDeleted = false;

            // Cập nhật trạng thái tài khoản liên quan
            var taiKhoan = _context.TaiKhoans.FirstOrDefault(tk => tk.MaTk == id);
            if (taiKhoan != null)
            {
                taiKhoan.IsDeleted = false;
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Nhân viên y tế đã được hiện lại.";
            return RedirectToAction(nameof(HiddenStaff));
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
            var isPhoneExists = await _context.TaiKhoans
                .AnyAsync(tk => tk.SoDienThoai == viewModel.SoDienThoai);
            var isCccdExists = await _context.TaiKhoans
                .AnyAsync(tk => tk.SoCccd == viewModel.SoCccd);

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

                // Tạo tên file duy nhất bằng cách sử dụng GUID và phần mở rộng gốc của file
                var newFileName = Guid.NewGuid().ToString() + extension;

                // Đường dẫn nơi file sẽ được lưu
                var imagePath = Path.Combine("wwwroot/images/nhanvienyte", newFileName);

                // Lưu file vào thư mục wwwroot/images/nhanvienyte
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await viewModel.HinhAnh.CopyToAsync(stream);
                }

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
                    MaQuyen = 3
                };

                _context.TaiKhoans.Add(taiKhoan);
                await _context.SaveChangesAsync();

                var nhanVienYTe = new NhanVienYTe
                {
                    MaTk = taiKhoan.MaTk, 
                    HoTen = viewModel.HoTen,
                    NgaySinh = viewModel.NgaySinh,
                    SoCccd = viewModel.SoCccd,
                    GioiTinh = viewModel.GioiTinh,
                    DiaChi = viewModel.DiaChi,
                    SoDienThoai = viewModel.SoDienThoai,
                    MatKhau = viewModel.MatKhau,
                    HinhAnh = newFileName,
                    DiaChiEmail = viewModel.DiaChiEmail,
                    MaQuyen = 3
                };

                _context.NhanVienYTes.Add(nhanVienYTe);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm nhân viên y tế thành công.";

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("HinhAnh", "Vui lòng chọn file hình ảnh.");
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Lấy thông tin nhân viên y tế
            var nhanVienYTe = await _context.NhanVienYTes
                .Include(nv => nv.HoaDons)
                .Include(nv => nv.PhieuNhapThuocs)
                .Include(nv => nv.PhieuThuTienThuocs)
                .FirstOrDefaultAsync(nv => nv.MaTk == id);

            if (nhanVienYTe == null)
            {
                return NotFound("Không tìm thấy thông tin nhân viên y tế.");
            }

            return View(nhanVienYTe);
        }


    }
}
