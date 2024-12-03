using System.ComponentModel.DataAnnotations;

namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class KhachHangViewModel
    {
        public int MaHgd { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Số CCCD là bắt buộc")]
        public string SoCccd { get; set; }

        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        public short GioiTinh { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string MatKhau { get; set; }

        public IFormFile? HinhAnhFile { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        public string DiaChiEmail { get; set; }

        [Required(ErrorMessage = "Mối quan hệ với chủ hộ là bắt buộc")]
        public string MoiQuanHeVoiChuHo { get; set; }

        public string TenNguoiLienHeKhanCap { get; set; }
        public string SdtNguoiLienHeKhanCap { get; set; }
    }
}
