using System.ComponentModel.DataAnnotations;

namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class DonGiaDichVuViewModel
    {
        public int MaDichVu { get; set; }
        [Required]
        public decimal DonGiaDv { get; set; }
    }
}
