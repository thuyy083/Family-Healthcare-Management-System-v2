using HTQL_DichVuYTeGiaDinh.Hubs;
using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HTQL_DichVuYTeGiaDinh.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    public class TuVanController : Controller
    {
        private readonly DataContext _context;
        private readonly IHubContext<TuVanHub> _hubContext;

        public TuVanController(IHubContext<TuVanHub> hubContext, DataContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int maBacSi, int maKhachHang, string noiDungTinNhan)
        {
            // Lưu tin nhắn vào cơ sở dữ liệu
            var tinNhan = new TuVan
            {
                MaTk = maKhachHang,
                BacMaTk = maBacSi,
                NoiDungTinNhan = noiDungTinNhan,
                ThoiDiemNhanTin = DateTime.Now,
                TrangThaiTinNhan = 0 // Chưa đọc
            };
            _context.TuVans.Add(tinNhan);
            await _context.SaveChangesAsync();

            // Gửi tin nhắn tới các client qua SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", maBacSi, maKhachHang, noiDungTinNhan);

            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
