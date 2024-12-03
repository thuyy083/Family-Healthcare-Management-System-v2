using HTQL_DichVuYTeGiaDinh.Repository;
using HTQL_DichVuYTeGiaDinh.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTQL_DichVuYTeGiaDinh.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class LichLamViecController : Controller
    {
        private readonly DataContext _context;

        public LichLamViecController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ThoiGianBieu(DateTime? startDate, DateTime? endDate)
        {
            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Ngày mặc định là từ đầu tuần đến cuối tuần hiện tại nếu không truyền vào
            DateTime start = startDate ?? DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
            DateTime end = endDate ?? start.AddDays(6);

            // Lấy danh sách sử dụng dịch vụ liên quan đến nhân viên y tế đang đăng nhập
            var suDungDichVus = await _context.SuDungDichVus
                .Include(sd => sd.MaDichVuNavigation)
                .Include(sd => sd.HoaDons)
                .Where(sd => sd.HoaDons.Any(hd => hd.MaTk == maTk.Value)) 
                .Where(sd => sd.NgayBatDau >= start && sd.NgayBatDau <= end) 
                .OrderBy(sd => sd.NgayBatDau)
                .ToListAsync();

            // Tạo dữ liệu thời gian biểu
            var thoiGianBieu = new List<ThoiGianBieuViewModel>();

            foreach (var suDung in suDungDichVus)
            {
                for (int i = 0; i < suDung.SoLuong; i++)
                {
                    var ngayThucHien = suDung.NgayBatDau.AddDays(i);
                    thoiGianBieu.Add(new ThoiGianBieuViewModel
                    {
                        Ngay = ngayThucHien,
                        TenDichVu = suDung.MaDichVuNavigation?.TenDichVu,
                        DiaChi = suDung.DiaChiLienLac,
                        SoDienThoai = suDung.SoDienThoaiLienHe,
                        MoTaBenhLy = suDung.MoTaBenhLy
                    });
                }
            }

            ViewBag.StartDate = start;
            ViewBag.EndDate = end;

            return View(thoiGianBieu);
        }

    }
}
