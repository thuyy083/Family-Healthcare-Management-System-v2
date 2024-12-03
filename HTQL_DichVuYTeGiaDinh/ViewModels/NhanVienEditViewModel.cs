using System.ComponentModel.DataAnnotations;

namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class NhanVienEditViewModel
    {
        [Required(ErrorMessage = "Họ tên không được để trống.")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống.")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Số CCCD không được để trống.")]
        [StringLength(12, ErrorMessage = "Số CCCD phải có 12 ký tự.", MinimumLength = 12)]
        public string SoCccd { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống.")]
        public short GioiTinh { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string DiaChiEmail { get; set; }
    }

}
