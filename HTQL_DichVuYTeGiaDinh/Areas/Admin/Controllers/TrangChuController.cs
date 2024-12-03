using HTQL_DichVuYTeGiaDinh.Repository;
using HTQL_DichVuYTeGiaDinh.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrangChuController : Controller
    {
        private readonly DataContext _context;

        public TrangChuController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy MaTk từ Session
            var maTk = HttpContext.Session.GetInt32("MaTk");
            Console.WriteLine("Mã tài khoản: "+maTk);
            if (maTk == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // Tìm thông tin quản trị viên
            var admin = _context.NhanVienYTes.FirstOrDefault(nv => nv.MaTk == maTk);
            if (admin == null)
            {
                // Nếu không tìm thấy tài khoản, quay lại trang đăng nhập
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // Truyền thông tin admin vào View
            return View(admin);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var nhanVien = _context.NhanVienYTes.FirstOrDefault(nv => nv.MaTk == maTk);
            if (nhanVien == null)
            {
                return NotFound("Không tìm thấy thông tin nhân viên.");
            }

            // Chuyển dữ liệu từ NhanVienYTe sang ViewModel
            var viewModel = new NhanVienEditViewModel
            {
                HoTen = nhanVien.HoTen,
                NgaySinh = nhanVien.NgaySinh,
                SoCccd = nhanVien.SoCccd,
                GioiTinh = nhanVien.GioiTinh,
                DiaChi = nhanVien.DiaChi,
                SoDienThoai = nhanVien.SoDienThoai,
                DiaChiEmail = nhanVien.DiaChiEmail
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NhanVienEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var nhanVien = _context.NhanVienYTes.FirstOrDefault(nv => nv.MaTk == maTk);
            if (nhanVien == null)
            {
                return NotFound("Không tìm thấy thông tin nhân viên.");
            }

            var taiKhoan = _context.TaiKhoans.FirstOrDefault(tk => tk.MaTk == maTk);
            if (taiKhoan == null)
            {
                return NotFound("Không tìm thấy tài khoản.");
            }

            // Kiểm tra trùng lặp số CCCD
            if (_context.TaiKhoans.Any(tk => tk.SoCccd == model.SoCccd && tk.MaTk != nhanVien.MaTk))
            {
                ModelState.AddModelError("SoCccd", "Số CCCD đã tồn tại trong hệ thống.");
                return View(model);
            }
            if (_context.TaiKhoans.Any(tk => tk.SoDienThoai == model.SoDienThoai && tk.MaTk != nhanVien.MaTk))
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại đã tồn tại trong hệ thống.");
                return View(model);
            }

            // Cập nhật thông tin trong bảng NhanVienYTe
            nhanVien.HoTen = model.HoTen;
            nhanVien.NgaySinh = model.NgaySinh;
            nhanVien.SoCccd = model.SoCccd;
            nhanVien.GioiTinh = model.GioiTinh;
            nhanVien.DiaChi = model.DiaChi;
            nhanVien.SoDienThoai = model.SoDienThoai;
            nhanVien.DiaChiEmail = model.DiaChiEmail;

            // Cập nhật thông tin trong bảng TaiKhoan
            taiKhoan.HoTen = model.HoTen;
            taiKhoan.NgaySinh = model.NgaySinh;
            taiKhoan.SoCccd = model.SoCccd;
            taiKhoan.GioiTinh = model.GioiTinh;
            taiKhoan.DiaChi = model.DiaChi;
            taiKhoan.SoDienThoai = model.SoDienThoai;
            taiKhoan.DiaChiEmail = model.DiaChiEmail;

            _context.SaveChanges();

            TempData["Message"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Index", "TrangChu");
        }
    }
}
