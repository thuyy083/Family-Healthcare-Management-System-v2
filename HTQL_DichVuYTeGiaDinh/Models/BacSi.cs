using HTQL_DichVuYTeGiaDinh.Repository.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class BacSi
{
    [Key]
    public int MaTk { get; set; }

    public int MaChuyenKhoa { get; set; }

    public int MaQuyen { get; set; }

    public string HoTen { get; set; } = null!;

    public DateTime NgaySinh { get; set; }

    public string SoCccd { get; set; } = null!;

    public short GioiTinh { get; set; }

    public string DiaChi { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm ít nhất một chữ cái in hoa, một chữ cái thường, một chữ số và một ký tự đặc biệt.")]
    public string MatKhau { get; set; } = null!;

    public string HinhAnh { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
    public string DiaChiEmail { get; set; } = null!;

    public string? TrinhDoHocVan { get; set; }

    public bool IsDeleted { get; set; } = false; 

    public virtual ICollection<HoSoYTe> HoSoYTes { get; set; } = new List<HoSoYTe>();

    public virtual ChuyenKhoa? MaChuyenKhoaNavigation { get; set; }

    public virtual TaiKhoan? MaTkNavigation { get; set; }

    public virtual ICollection<PhanCongBacSi> PhanCongBacSis { get; set; } = new List<PhanCongBacSi>();

    public virtual ICollection<TuVan> TuVans { get; set; } = new List<TuVan>();
}
