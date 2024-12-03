namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class ChiTietDichVuViewModel
    {
        public int MaDichVu { get; set; }
        public string TenDichVu { get; set; }
        public string MoTa { get; set; }
        public string DonViTinh { get; set; }
        public double DiemTrungBinh { get; set; }
        public List<BinhLuanViewModel> BinhLuans { get; set; }
    }

}
