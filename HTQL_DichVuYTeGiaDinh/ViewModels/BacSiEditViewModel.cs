using System.ComponentModel.DataAnnotations;

namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class BacSiEditViewModel
    {
        [Required]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; }

        [Required]
        [Display(Name = "Số CCCD")]
        public string SoCccd { get; set; } = null!;

        [Required]
        [Display(Name = "Giới tính")]
        public short GioiTinh { get; set; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; } = null!;

        [Required]
        [Phone]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string DiaChiEmail { get; set; } = null!;

        [Display(Name = "Trình độ học vấn")]
        public string? TrinhDoHocVan { get; set; }
    }

}
