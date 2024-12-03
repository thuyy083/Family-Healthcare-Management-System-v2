using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTQL_DichVuYTeGiaDinh.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class SuDungDichVuController : Controller
    {
        private readonly DataContext _context;

        public SuDungDichVuController(DataContext context)
        {
            _context = context;
        }
        public IActionResult DatDichVu()
        {
            var danhSachDichVu = _context.DichVuYTes
                    .Where(dv => !dv.IsDelete)
                    .ToList(); var danhSachNhanVien = _context.NhanVienYTes.ToList();

            ViewBag.DanhSachDichVu = danhSachDichVu;
            ViewBag.DanhSachNhanVien = danhSachNhanVien;

            return View(new SuDungDichVu());
        }

        [HttpPost]
        public IActionResult DatDichVu(SuDungDichVu model)
        {
            // Lấy mã tài khoản từ session
            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // Kiểm tra ngày bắt đầu
            if (model.NgayBatDau < DateTime.Today)
            {
                ModelState.AddModelError("NgayBatDau", "Ngày bắt đầu sử dụng dịch vụ phải lớn hơn hoặc bằng ngày hiện tại.");
            }

            // Kiểm tra xem dịch vụ đã bị ẩn hay chưa
            var dichVu = _context.DichVuYTes.FirstOrDefault(dv => dv.MaDichVu == model.MaDichVu && !dv.IsDelete);
            if (dichVu == null)
            {
                ModelState.AddModelError("MaDichVu", "Dịch vụ y tế này không khả dụng.");
            }

            if (!ModelState.IsValid)
            {
                var danhSachDichVu = _context.DichVuYTes
                    .Where(dv => !dv.IsDelete) 
                    .ToList();
                var danhSachNhanVien = _context.NhanVienYTes.ToList();

                ViewBag.DanhSachDichVu = danhSachDichVu;
                ViewBag.DanhSachNhanVien = danhSachNhanVien;

                return View(model);
            }

            model.MaTk = maTk.Value;
            model.TrangThai = 0; // Trạng thái mặc định là "chưa duyệt"
            model.NgayBatDau = DateTime.Now; // Ngày bắt đầu là ngày hiện tại

            // Tìm đơn giá gần nhất cho dịch vụ
            var donGiaGanNhat = _context.DonGiaDichVus
                .Where(d => d.MaDichVu == model.MaDichVu)
                .OrderByDescending(d => d.ThoiDiem) // Sắp xếp theo thời điểm từ mới nhất đến cũ nhất
                .Select(d => d.DonGiaDv)
                .FirstOrDefault();

            // Kiểm tra nếu không tìm thấy đơn giá, thiết lập TongTien = 0
            if (donGiaGanNhat != null)
            {
                model.TongTien = model.SoLuong * donGiaGanNhat;
            }
            else
            {
                model.TongTien = 0;
            }

            // Áp dụng khuyến mãi (nếu có)
            var khuyenMais = _context.ChiTietKhuyenMais
                .Where(km => km.MaDichVu == model.MaDichVu)
                .Select(km => new
                {
                    km.MaKhuyenMaiNavigation.PhanTramGiamGia,
                    km.MaKhuyenMaiNavigation.NgayBatDauKm,
                    km.MaKhuyenMaiNavigation.NgayKetThucKm
                })
                .Where(km => km.NgayBatDauKm <= DateTime.Now && km.NgayKetThucKm >= DateTime.Now)
                .ToList();

            if (khuyenMais.Any())
            {
                var giamGiaCaoNhat = khuyenMais.Max(km => km.PhanTramGiamGia);
                decimal giamGia = (decimal)giamGiaCaoNhat;
                model.TongTien -= model.TongTien * (giamGia / 100);
            }

            // Lưu vào cơ sở dữ liệu
            _context.SuDungDichVus.Add(model);
            _context.SaveChanges();

            // Lưu ID của dịch vụ vừa đặt vào TempData để hiển thị trong trang xác nhận
            TempData["SuDungDichVuId"] = model.MaLanSuDung;

            // Chuyển hướng đến trang xác nhận
            return RedirectToAction("ThongBaoThanhCong");
        }

        public IActionResult ThongBaoThanhCong()
        {
            // Lấy ID của dịch vụ đã đặt từ TempData
            var suDungDichVuId = TempData["SuDungDichVuId"] as int?;
            if (suDungDichVuId == null)
            {
                return RedirectToAction("DatDichVu");
            }

            // Lấy thông tin chi tiết của dịch vụ đã đặt
            var suDungDichVu = _context.SuDungDichVus
                .Include(d => d.MaDichVuNavigation)
                .FirstOrDefault(d => d.MaLanSuDung == suDungDichVuId);

            if (suDungDichVu == null)
            {
                return RedirectToAction("DatDichVu");
            }

            return View(suDungDichVu);
        }

        public async Task<IActionResult> DanhSachDichVuDaDat(int? page)
        {
            int pageSize = 5; 
            int pageNumber = (page ?? 1);

            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var danhSachDichVuDaDatQuery = _context.SuDungDichVus
                .Where(s => s.MaTk == maTk.Value)
                .Include(s => s.MaDichVuNavigation)
                .AsQueryable();

            int totalItems = await danhSachDichVuDaDatQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;

            var paginatedList = await danhSachDichVuDaDatQuery
                .OrderByDescending(s => s.NgayBatDau)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(paginatedList);
        }

        [HttpGet]
        public IActionResult BinhLuanDanhGia(int maDichVu)
        {
            var dichVu = _context.DichVuYTes.FirstOrDefault(dv => dv.MaDichVu == maDichVu);
            if (dichVu == null)
            {
                return NotFound("Không tìm thấy dịch vụ.");
            }

            ViewBag.TenDichVu = dichVu.TenDichVu;
            return View(new BinhLuan { MaDichVu = maDichVu });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BinhLuanDanhGia(BinhLuan model)
        {
            if (!ModelState.IsValid)
            {
                var dichVu = _context.DichVuYTes.FirstOrDefault(dv => dv.MaDichVu == model.MaDichVu);
                if (dichVu != null)
                {
                    ViewBag.TenDichVu = dichVu.TenDichVu;
                }
                return View(model);
            }

            // Lấy mã tài khoản từ session
            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Tạo đối tượng bình luận mới
            var binhLuan = new BinhLuan
            {
                MaDichVu = model.MaDichVu,
                MaTk = maTk.Value,
                NoiDungBinhLuan = model.NoiDungBinhLuan,
                DiemDanhGia = model.DiemDanhGia,
                ThoiDiemBinhLuan = DateTime.Now
            };

            // Lưu vào cơ sở dữ liệu
            _context.BinhLuans.Add(binhLuan);
            _context.SaveChanges();

            TempData["Message"] = "Cảm ơn bạn đã đánh giá dịch vụ!";
            return RedirectToAction("DanhSachBinhLuan", "SuDungDichVu");
        }

        public async Task<IActionResult> DanhSachBinhLuan(int? page)
        {
            int pageSize = 5; // Số bình luận mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại

            // Lấy mã tài khoản từ session
            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách bình luận của người dùng
            var binhLuanQuery = _context.BinhLuans
                .Include(bl => bl.MaDichVuNavigation)
                .Where(bl => bl.MaTk == maTk)
                .OrderByDescending(bl => bl.ThoiDiemBinhLuan)
                .AsQueryable();

            // Tính tổng số bình luận để phân trang
            int totalItems = await binhLuanQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;

            // Lấy dữ liệu theo trang
            var binhLuans = await binhLuanQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Đánh số thứ tự
            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(binhLuans);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyDichVu(int id)
        {
            // Lấy mã tài khoản từ session
            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Tìm dịch vụ đã đặt theo mã
            var suDungDichVu = await _context.SuDungDichVus
                .FirstOrDefaultAsync(s => s.MaLanSuDung == id && s.MaTk == maTk.Value);

            if (suDungDichVu == null)
            {
                return NotFound();
            }

            // Kiểm tra trạng thái dịch vụ (chỉ hủy nếu trạng thái là "Chưa duyệt")
            if (suDungDichVu.TrangThai != 0)
            {
                TempData["ErrorMessage"] = "Chỉ có thể hủy dịch vụ có trạng thái 'Chưa duyệt'.";
                return RedirectToAction(nameof(DanhSachDichVuDaDat));
            }

            // Cập nhật trạng thái thành "Đã hủy"
            suDungDichVu.TrangThai = 3;

            _context.SuDungDichVus.Update(suDungDichVu);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Dịch vụ đã được hủy thành công.";
            return RedirectToAction(nameof(DanhSachDichVuDaDat));
        }

    }
}
