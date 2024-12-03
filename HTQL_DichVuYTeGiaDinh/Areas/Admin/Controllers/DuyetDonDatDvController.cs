using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HTQL_DichVuYTeGiaDinh.ViewModels;
using HTQL_DichVuYTeGiaDinh.Models;

namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DuyetDonDatDvController : Controller
    {
        private readonly DataContext _context;

        public DuyetDonDatDvController(DataContext context)
        {
            _context = context;
        }

        public IActionResult DanhSachSuDungDichVu(string searchString, int? page)
        {
            int pageSize = 5; // Số mục trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại (mặc định là 1)

            // Lấy danh sách dịch vụ sử dụng với tìm kiếm (nếu có)
            var danhSachSuDungDichVuQuery = _context.SuDungDichVus
                .Include(s => s.MaDichVuNavigation)
                .Include(s => s.MaTkNavigation)
                .OrderBy(s => s.TrangThai)
                .AsQueryable();

            // Lọc theo tên dịch vụ hoặc khách hàng nếu có tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                danhSachSuDungDichVuQuery = danhSachSuDungDichVuQuery.Where(s =>
                    s.MaDichVuNavigation.TenDichVu.Contains(searchString) ||
                    s.MaTkNavigation.HoTen.Contains(searchString));
            }

            // Tổng số mục để tính số trang
            int totalItems = danhSachSuDungDichVuQuery.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString; // Giữ lại giá trị tìm kiếm

            // Lấy dữ liệu phân trang
            var danhSachSuDungDichVu = danhSachSuDungDichVuQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(danhSachSuDungDichVu);
        }


        public IActionResult Details(int id)
        {
            var suDungDichVu = _context.SuDungDichVus
                .Include(s => s.MaDichVuNavigation) 
                .Include(s => s.MaTkNavigation)
                .Include(s => s.HoaDons)
                    .ThenInclude(h => h.MaTkNavigation)
                .FirstOrDefault(s => s.MaLanSuDung == id);

            if (suDungDichVu == null)
            {
                return NotFound();
            }

            return View(suDungDichVu);
        }

        public IActionResult DuyetDon(int id)
        {
            var suDungDichVu = _context.SuDungDichVus
                .Include(s => s.MaDichVuNavigation)
                .FirstOrDefault(s => s.MaLanSuDung == id);

            if (suDungDichVu == null || suDungDichVu.TrangThai == 1)
            {
                return NotFound("Không tìm thấy dịch vụ hoặc dịch vụ đã được duyệt.");
            }

            ViewBag.DanhSachNhanVien = _context.NhanVienYTes.ToList();

            return View(suDungDichVu);
        }

        [HttpPost]
        public IActionResult DuyetDon(int id, int maNhanVien)
        {


            var suDungDichVu = _context.SuDungDichVus
                .FirstOrDefault(s => s.MaLanSuDung == id);

            if (suDungDichVu == null || suDungDichVu.TrangThai == 1)
            {
                return NotFound("Không tìm thấy dịch vụ hoặc dịch vụ đã được duyệt.");
            }

            suDungDichVu.TrangThai = 1;

            var hoaDon = new HoaDon
            {
                MaLanSuDung = suDungDichVu.MaLanSuDung,
                MaTk = maNhanVien,
                NgayLapHoaDon = DateTime.Now,
                TongTien = suDungDichVu.TongTien
            };

            _context.HoaDons.Add(hoaDon);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = suDungDichVu.MaLanSuDung });
        }

        [HttpPost]
        public IActionResult TuChoiDon(int id)
        {
            var suDungDichVu = _context.SuDungDichVus.FirstOrDefault(s => s.MaLanSuDung == id);

            if (suDungDichVu == null)
            {
                return NotFound("Không tìm thấy đơn đặt dịch vụ.");
            }

            // Cập nhật trạng thái thành 2 (Từ chối)
            suDungDichVu.TrangThai = 2;
            _context.SaveChanges();

            // Giữ nguyên ở trang chi tiết
            return RedirectToAction("Details", new { id = suDungDichVu.MaLanSuDung });
        }


    }
}
