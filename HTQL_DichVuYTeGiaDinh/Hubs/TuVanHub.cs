using Microsoft.AspNetCore.SignalR;

namespace HTQL_DichVuYTeGiaDinh.Hubs
{
    public class TuVanHub : Hub
    {
        public async Task SendMessage(int maBacSi, int maKhachHang, string noiDungTinNhan)
        {
            // Thông báo tới các client (bác sĩ và khách hàng) để cập nhật tin nhắn mới
            await Clients.All.SendAsync("ReceiveMessage", maBacSi, maKhachHang, noiDungTinNhan);
        }
    }
}
