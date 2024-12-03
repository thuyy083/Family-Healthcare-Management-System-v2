namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class DichVuViewModel
    {
        public int MaDichVu { get; set; }
        public string TenDichVu { get; set; }
        public string MoTa { get; set; }
        public string DonViTinh { get; set; }
        public decimal DonGiaDv { get; set; }
        public decimal? DonGiaKhuyenMai { get; set; } // Đơn giá khuyến mãi

    }
}
