using HTQL_DichVuYTeGiaDinh.Models;
using System.ComponentModel.DataAnnotations;

namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class EditThuocViewModel
    {
        public int MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public string MoTaThuoc { get; set; }
        public int HanSuDung { get; set; }
        public int SoLuong { get; set; }
        [Required]
        public int MaDvt { get; set; }

        public List<DonViTinhThuoc> DonViTinhs { get; set; } = new List<DonViTinhThuoc>();
    }
}
