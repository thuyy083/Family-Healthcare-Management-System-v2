using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class NhanVienYTe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaTk { get; set; }

    public int? MaQuyen { get; set; }

    public string HoTen { get; set; } = null!;

    public DateTime NgaySinh { get; set; }

    public string SoCccd { get; set; } = null!;

    public short GioiTinh { get; set; }

    public string DiaChi { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string HinhAnh { get; set; } = null!;

    public string DiaChiEmail { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual TaiKhoan? MaTkNavigation { get; set; }

    public virtual ICollection<PhieuNhapThuoc> PhieuNhapThuocs { get; set; } = new List<PhieuNhapThuoc>();

    public virtual ICollection<PhieuThuTienThuoc> PhieuThuTienThuocs { get; set; } = new List<PhieuThuTienThuoc>();
}
