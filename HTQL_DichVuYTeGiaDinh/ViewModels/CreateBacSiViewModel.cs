using HTQL_DichVuYTeGiaDinh.Models;
using System.ComponentModel.DataAnnotations;

namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class CreateBacSiViewModel
    {
        public int MaTk { get; set; }


        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        public string HoTen { get; set; } = null!;

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Số CCCD là bắt buộc.")]
        public string SoCccd { get; set; } = null!;

        [Required(ErrorMessage = "Giới tính là bắt buộc.")]
        public short GioiTinh { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string DiaChi { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        public string SoDienThoai { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        public string MatKhau { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string DiaChiEmail { get; set; } = null!;

        public string? TrinhDoHocVan { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chuyên khoa.")]
        public int MaChuyenKhoa { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn file hình ảnh.")]
        [DataType(DataType.Upload)]
        public IFormFile HinhAnh { get; set; } = null!;
        public string? HinhAnhCu { get; set; } = null!;
        public List<ChuyenKhoa> ChuyenKhoas { get; set; } = new List<ChuyenKhoa>();
    }
}
