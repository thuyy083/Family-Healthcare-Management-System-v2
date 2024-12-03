using HTQL_DichVuYTeGiaDinh.Models;

namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class AddPhieuNhapThuocViewModel
    {
        public DateTime NgayLapPhieuNhap { get; set; }
        public string TenNhaCungCap { get; set; } = string.Empty;
        public List<ChiTietPhieuNhapViewModel> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhapViewModel>();
        public List<Thuoc> DanhSachThuoc { get; set; } = new List<Thuoc>();
    }
}
