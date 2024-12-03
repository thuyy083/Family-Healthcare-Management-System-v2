using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HTQL_DichVuYTeGiaDinh.ViewModels;

namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ThongKeController : Controller
    {
        private readonly DataContext _context;

        public ThongKeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult ThongKeThuoc(int? month, int? year)
        {
            // Lấy năm và tháng hiện tại nếu không có giá trị đầu vào
            int selectedYear = year ?? DateTime.Now.Year;
            int selectedMonth = month ?? DateTime.Now.Month;

            var thongKeThuoc = _context.ChiTietDonThuocs
                .Where(ctdt => ctdt.MaDonThuocNavigation.NgayKeDon.Year == selectedYear &&
                   ctdt.MaDonThuocNavigation.NgayKeDon.Month == selectedMonth)
                .GroupBy(ctdt => ctdt.MaThuocNavigation.TenThuoc)
                .Select(group => new ThongKeThuocViewModel
                {
                    TenThuoc = group.Key,
                    TongSoLuong = group.Sum(ctdt => ctdt.SoLuong),
                    TanSuat = group.Count()
                })
                .OrderByDescending(x => x.TongSoLuong)
                .ToList();


            // Truyền dữ liệu sang view
            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.SelectedYear = selectedYear;
            ViewBag.Months = Enumerable.Range(1, 12).ToList(); // Danh sách tháng
            ViewBag.Years = Enumerable.Range(2020, DateTime.Now.Year - 2019).ToList(); // Danh sách năm từ 2020 đến hiện tại

            return View(thongKeThuoc);
        }

        public IActionResult ThongKeDichVu(int? month, int? year)
        {
            int selectedMonth = month ?? DateTime.Now.Month;
            int selectedYear = year ?? DateTime.Now.Year;

            var thongKeDichVu = _context.DichVuYTes
                .Select(dv => new ThongKeDichVuViewModel
                {
                    TenDichVu = dv.TenDichVu,
                    TongSoLuot = dv.SuDungDichVus
                        .Where(sdv => sdv.NgayBatDau.Year == selectedYear &&
                                      sdv.NgayBatDau.Month == selectedMonth &&
                                      sdv.TrangThai == 1)
                        .Count(),
                    DanhGiaTrungBinh = dv.BinhLuans.Any()
                        ? dv.BinhLuans.Average(bl => (decimal?)bl.DiemDanhGia) ?? 0
                        : null
                })
                .OrderByDescending(x => x.TongSoLuot)
                .ToList();

            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.SelectedYear = selectedYear;
            ViewBag.Months = Enumerable.Range(1, 12).ToList();
            ViewBag.Years = Enumerable.Range(2020, DateTime.Now.Year - 2019 + 1).ToList();

            return View(thongKeDichVu);
        }


    }
}
