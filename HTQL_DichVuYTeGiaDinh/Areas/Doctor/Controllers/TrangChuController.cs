using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HTQL_DichVuYTeGiaDinh.ViewModels;


namespace HTQL_DichVuYTeGiaDinh.Areas.Doctor.Controllers
{
    [Area("Doctor")]
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
            Console.WriteLine("Mã tài khoản: " + maTk);
            if (maTk == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // Lấy thông tin bác sĩ cùng tên chuyên khoa
            var bacSi = _context.BacSis
                .Include(bs => bs.MaChuyenKhoaNavigation) // Bao gồm thông tin ChuyenKhoa
                .FirstOrDefault(bs => bs.MaTk == maTk);

            if (bacSi == null)
            {
                // Nếu không tìm thấy tài khoản, quay lại trang đăng nhập
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // Truyền thông tin bác sĩ vào View
            return View(bacSi);
        }


        [HttpGet]
        public IActionResult Edit()
        {
            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var bacSi = _context.BacSis.FirstOrDefault(bs => bs.MaTk == maTk);
            if (bacSi == null)
            {
                return NotFound("Không tìm thấy thông tin bác sĩ.");
            }

            // Chuyển dữ liệu từ BacSi sang ViewModel
            var viewModel = new BacSiEditViewModel
            {
                HoTen = bacSi.HoTen,
                NgaySinh = bacSi.NgaySinh,
                SoCccd = bacSi.SoCccd,
                GioiTinh = bacSi.GioiTinh,
                DiaChi = bacSi.DiaChi,
                SoDienThoai = bacSi.SoDienThoai,
                DiaChiEmail = bacSi.DiaChiEmail,
                TrinhDoHocVan = bacSi.TrinhDoHocVan
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BacSiEditViewModel model)
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

            var bacSi = _context.BacSis.FirstOrDefault(bs => bs.MaTk == maTk);
            if (bacSi == null)
            {
                return NotFound("Không tìm thấy thông tin bác sĩ.");
            }

            var taiKhoan = _context.TaiKhoans.FirstOrDefault(tk => tk.MaTk == maTk);
            if (taiKhoan == null)
            {
                return NotFound("Không tìm thấy tài khoản.");
            }

            // Kiểm tra trùng lặp số CCCD
            if (_context.TaiKhoans.Any(tk => tk.SoCccd == model.SoCccd && tk.MaTk != bacSi.MaTk))
            {
                ModelState.AddModelError("SoCccd", "Số CCCD đã tồn tại trong hệ thống.");
                return View(model);
            }
            if (_context.TaiKhoans.Any(tk => tk.SoDienThoai == model.SoDienThoai && tk.MaTk != bacSi.MaTk))
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại đã tồn tại trong hệ thống.");
                return View(model);
            }

            // Cập nhật thông tin trong bảng BacSi
            bacSi.HoTen = model.HoTen;
            bacSi.NgaySinh = model.NgaySinh;
            bacSi.SoCccd = model.SoCccd;
            bacSi.GioiTinh = model.GioiTinh;
            bacSi.DiaChi = model.DiaChi;
            bacSi.SoDienThoai = model.SoDienThoai;
            bacSi.DiaChiEmail = model.DiaChiEmail;
            bacSi.TrinhDoHocVan = model.TrinhDoHocVan;

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
