using HTQL_DichVuYTeGiaDinh.Models;
using System.ComponentModel.DataAnnotations;

namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class AddCanLamSangViewModel
    {
        public int HoSoId { get; set; } 

        [Required(ErrorMessage = "Vui lòng chọn phương pháp cận lâm sàng.")]
        public int SelectedPhuongPhap { get; set; }

        public IFormFile? HinhAnhKetQua { get; set; } 

        public List<PhuongPhapCanLamSang> PhuongPhapCanLamSangs { get; set; } = new List<PhuongPhapCanLamSang>();
    }
}
