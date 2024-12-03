using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HTQL_DichVuYTeGiaDinh.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string soDienThoai, string matKhau)
        {
            if (string.IsNullOrEmpty(soDienThoai))
            {
                ModelState.AddModelError("soDienThoai", "Số điện thoại không được để trống.");
            }

            if (string.IsNullOrEmpty(matKhau))
            {
                ModelState.AddModelError("matKhau", "Mật khẩu không được để trống.");
            }

            if (!ModelState.IsValid)
            {
                // Lưu lại số điện thoại để hiển thị lại
                ViewData["SoDienThoai"] = soDienThoai;
                return View();
            }

            // Tìm tài khoản dựa trên số điện thoại, mật khẩu và isDeleted = false
            var taiKhoan = _context.TaiKhoans
                .FirstOrDefault(tk => tk.SoDienThoai == soDienThoai && tk.MatKhau == matKhau && !tk.IsDeleted);

            if (taiKhoan == null)
            {
                // Kiểm tra nếu tài khoản tồn tại nhưng bị vô hiệu hóa (isDeleted = true)
                var taiKhoanBiAn = _context.TaiKhoans
                    .FirstOrDefault(tk => tk.SoDienThoai == soDienThoai && tk.MatKhau == matKhau && tk.IsDeleted);

                if (taiKhoanBiAn != null)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn đã bị vô hiệu hóa. Vui lòng liên hệ quản trị viên.");
                }
                else
                {
                    ModelState.AddModelError("", "Số điện thoại hoặc mật khẩu không chính xác.");
                }

                ViewData["SoDienThoai"] = soDienThoai;
                return View();
            }

            // Lưu thông tin tài khoản vào session
            HttpContext.Session.SetInt32("MaTk", taiKhoan.MaTk);
            HttpContext.Session.SetString("HoTen", taiKhoan.HoTen);
            HttpContext.Session.SetInt32("MaQuyen", taiKhoan.MaQuyen);

            // Điều hướng dựa vào quyền truy cập
            switch (taiKhoan.MaQuyen)
            {
                case 1: // Admin
                    return RedirectToAction("Index", "TrangChu", new { area = "Admin" });
                case 2: // BacSi
                    return RedirectToAction("Index", "TrangChu", new { area = "Doctor" });
                case 3: // NhanVienYTe
                    return RedirectToAction("Index", "TrangChu", new { area = "Staff" });
                case 4: // KhachHang
                    return RedirectToAction("Index", "TrangChu", new { area = "Customer" });
                default:
                    ModelState.AddModelError("", "Quyền truy cập không hợp lệ.");
                    return View();
            }
        }



        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa toàn bộ session
            return RedirectToAction("Login", "Account"); // Chuyển về trang đăng nhập
        }




    }
}
