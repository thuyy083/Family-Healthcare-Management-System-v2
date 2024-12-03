using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using HTQL_DichVuYTeGiaDinh.ViewModels;
using Microsoft.Extensions.Hosting;

namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhachHangsController : Controller
    {
        private readonly DataContext _context;

        public KhachHangsController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/KhachHangs
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            int pageSize = 4; 
            int pageNumber = page ?? 1; 

            var khachHangsQuery = _context.KhachHangs.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                khachHangsQuery = khachHangsQuery.Where(kh =>
                    kh.HoTen.Contains(searchString) ||
                    kh.SoDienThoai.Contains(searchString) ||
                    kh.SoCccd.Contains(searchString));
            }

            int totalItems = await khachHangsQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber; 
            ViewBag.SearchString = searchString; 

            var paginatedList = await khachHangsQuery
                .OrderBy(kh => kh.HoTen)
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)
                .ToListAsync();

            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1; 

            return View(paginatedList);
        }


        // GET: Admin/KhachHangs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // Tìm khách hàng theo id
            var khachHang = await _context.KhachHangs
                .Include(kh => kh.MaHgdNavigation)
                .Include(kh => kh.MaTkNavigation) 
                .Include(kh => kh.SuDungDichVus)
                    .ThenInclude(sd => sd.MaDichVuNavigation)
                .Include(kh => kh.HoSoYTes)      
                .FirstOrDefaultAsync(kh => kh.MaTk == id);

            if (khachHang == null)
            {
                return NotFound(); 
            }

            return View(khachHang);
        }


        // GET: Admin/KhachHangs/Create
        [HttpGet]
        public IActionResult Create(int id) // id là MaHgd
        {
            var khachHangViewModel = new KhachHangViewModel
            {
                NgaySinh = DateTime.Now,
                MaHgd = id
            };
            return View(khachHangViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhachHangViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Lỗi tại trường {state.Key}: {error.ErrorMessage}");
                    }
                }
                // Nếu có lỗi validate, trả lại View và hiển thị thông tin lỗi
                return View(viewModel);
            }

            // Kiểm tra số điện thoại đã tồn tại hay chưa
            var existingKhachHang = await _context.TaiKhoans
                .FirstOrDefaultAsync(kh => kh.SoDienThoai == viewModel.SoDienThoai);
            if (existingKhachHang != null)
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại đã tồn tại.");
                return View(viewModel);
            }

            if (viewModel.HinhAnhFile != null && viewModel.HinhAnhFile.Length > 0)
            {

                // Lấy phần mở rộng của file
                var extension = Path.GetExtension(viewModel.HinhAnhFile.FileName).ToLower();

                // Các phần mở rộng file được cho phép
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                // Kiểm tra xem phần mở rộng có hợp lệ không
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận các định dạng file: .jpg, .jpeg, .png, .gif");
                    return View(viewModel);
                }

                // Tạo tên file duy nhất bằng cách sử dụng GUID và phần mở rộng gốc của file
                var newFileName = Guid.NewGuid().ToString() + extension;

                // Đường dẫn nơi file sẽ được lưu
                var imagePath = Path.Combine("wwwroot/images/khachhang", newFileName);

                // Lưu file vào thư mục wwwroot/uploads
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await viewModel.HinhAnhFile.CopyToAsync(stream);
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
                    MaQuyen = 4
                };

                _context.TaiKhoans.Add(taiKhoan);
                await _context.SaveChangesAsync();

                // Lưu thông tin Bác sĩ với mã tài khoản đã tạo
                var khachHang = new KhachHang
                {
                    MaHgd = viewModel.MaHgd,
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
                    MaQuyen = 4,
                    MoiQuanHeVoiChuHo = viewModel.MoiQuanHeVoiChuHo,
                    TenNguoiLienHeKhanCap = viewModel.TenNguoiLienHeKhanCap,
                    SdtNguoiLienHeKhanCap = viewModel.SdtNguoiLienHeKhanCap
                };

                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "HoGiaDinhs", new { id = viewModel.MaHgd });
            }
            ModelState.AddModelError("HinhAnh", "Vui lòng chọn file hình ảnh.");
            return View(viewModel);
        }


        // GET: Admin/KhachHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            ViewData["MaHgd"] = new SelectList(_context.HoGiaDinhs, "MaHgd", "MaHgd", khachHang.MaHgd);
            ViewData["MaTk"] = new SelectList(_context.TaiKhoans, "MaTk", "MaTk", khachHang.MaTk);
            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTk,MaHgd,MaQuyen,HoTen,NgaySinh,SoCccd,GioiTinh,DiaChi,SoDienThoai,MatKhau,HinhAnh,DiaChiEmail,MoiQuanHeVoiChuHo,TenNguoiLienHeKhanCap,SdtNguoiLienHeKhanCap")] KhachHang khachHang)
        {
            if (id != khachHang.MaTk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaTk))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHgd"] = new SelectList(_context.HoGiaDinhs, "MaHgd", "MaHgd", khachHang.MaHgd);
            ViewData["MaTk"] = new SelectList(_context.TaiKhoans, "MaTk", "MaTk", khachHang.MaTk);
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.MaHgdNavigation)
                .Include(k => k.MaTkNavigation)
                .FirstOrDefaultAsync(m => m.MaTk == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KhachHangs == null)
            {
                return Problem("Entity set 'DataContext.KhachHangs'  is null.");
            }
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHangs.Remove(khachHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
          return (_context.KhachHangs?.Any(e => e.MaTk == id)).GetValueOrDefault();
        }
    }
}
