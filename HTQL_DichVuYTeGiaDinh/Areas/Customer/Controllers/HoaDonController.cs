using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTQL_DichVuYTeGiaDinh.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HoaDonController : Controller
    {
        private readonly DataContext _context;

        public HoaDonController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult XemHoaDon(int id)
        {
            // Tìm hóa đơn theo mã lần sử dụng dịch vụ
            var hoaDon = _context.HoaDons
                .Include(hd => hd.MaLanSuDungNavigation)
                    .ThenInclude(sd => sd.MaDichVuNavigation)
                .Include(hd => hd.MaLanSuDungNavigation)
                    .ThenInclude(sd => sd.MaTkNavigation)
                .FirstOrDefault(hd => hd.MaLanSuDung == id);

            if (hoaDon == null)
            {
                return NotFound("Không tìm thấy hóa đơn.");
            }

            return View(hoaDon);
        }
    }
}
