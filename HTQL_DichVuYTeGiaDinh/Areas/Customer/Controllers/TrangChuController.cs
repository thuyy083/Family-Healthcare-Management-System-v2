using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using HTQL_DichVuYTeGiaDinh.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTQL_DichVuYTeGiaDinh.Areas.Customer.Controllers
{
    [Area("Customer")]
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
            var khach = _context.KhachHangs.FirstOrDefault(nv => nv.MaTk == maTk);
            if (khach == null)
            {
                // Nếu không tìm thấy tài khoản, quay lại trang đăng nhập
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // Truyền thông tin admin vào View
            return View(khach);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var khachHang = _context.KhachHangs.FirstOrDefault(kh => kh.MaTk == maTk);
            if (khachHang == null)
            {
                return NotFound("Không tìm thấy thông tin khách hàng.");
            }

            // Chuyển dữ liệu từ KhachHang sang ViewModel
            var viewModel = new KhachHangEditViewModel
            {
                HoTen = khachHang.HoTen,
                NgaySinh = khachHang.NgaySinh,
                SoCccd = khachHang.SoCccd,
                GioiTinh = khachHang.GioiTinh,
                DiaChi = khachHang.DiaChi,
                SoDienThoai = khachHang.SoDienThoai,
                DiaChiEmail = khachHang.DiaChiEmail,
                TenNguoiLienHeKhanCap = khachHang.TenNguoiLienHeKhanCap,
                SdtNguoiLienHeKhanCap = khachHang.SdtNguoiLienHeKhanCap,
                MoiQuanHeVoiChuHo = khachHang.MoiQuanHeVoiChuHo
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(KhachHangEditViewModel model)
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

            var khachHang = _context.KhachHangs.FirstOrDefault(kh => kh.MaTk == maTk);
            if (khachHang == null)
            {
                return NotFound("Không tìm thấy thông tin khách hàng.");
            }

            var taiKhoan = _context.TaiKhoans.FirstOrDefault(tk => tk.MaTk == maTk);
            if (taiKhoan == null)
            {
                return NotFound("Không tìm thấy tài khoản.");
            }

            // Kiểm tra trùng lặp số CCCD
            if (_context.TaiKhoans.Any(tk => tk.SoCccd == model.SoCccd && tk.MaTk != khachHang.MaTk))
            {
                ModelState.AddModelError("SoCccd", "Số CCCD đã tồn tại trong hệ thống.");
                return View(model);
            }
            if (_context.TaiKhoans.Any(tk => tk.SoDienThoai == model.SoDienThoai && tk.MaTk != khachHang.MaTk))
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại đã tồn tại trong hệ thống.");
                return View(model);
            }

            // Cập nhật thông tin trong bảng KhachHang
            khachHang.HoTen = model.HoTen;
            khachHang.NgaySinh = model.NgaySinh;
            khachHang.SoCccd = model.SoCccd;
            khachHang.GioiTinh = model.GioiTinh;
            khachHang.DiaChi = model.DiaChi;
            khachHang.SoDienThoai = model.SoDienThoai;
            khachHang.DiaChiEmail = model.DiaChiEmail;
            khachHang.TenNguoiLienHeKhanCap = model.TenNguoiLienHeKhanCap;
            khachHang.SdtNguoiLienHeKhanCap = model.SdtNguoiLienHeKhanCap;
            khachHang.MoiQuanHeVoiChuHo = model.MoiQuanHeVoiChuHo;

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
