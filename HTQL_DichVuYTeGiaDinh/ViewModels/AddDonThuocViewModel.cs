using HTQL_DichVuYTeGiaDinh.Models;

namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class AddDonThuocViewModel
    {
        public int MaHoSo { get; set; }
        public DateTime NgayKeDon { get; set; }
        public string TrangThaiDonThuoc { get; set; } = "Chưa phát thuốc";
        public string? GhiChuDonThuoc { get; set; }

        public List<ChiTietDonThuocViewModel> ChiTietDonThuocs { get; set; } = new List<ChiTietDonThuocViewModel>();

        public List<Thuoc> DanhSachThuoc { get; set; } = new List<Thuoc>();
    }
}
