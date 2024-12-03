using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class KhachHang
{
    [Key]
    public int MaTk { get; set; }

    public int MaHgd { get; set; }

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

    public string MoiQuanHeVoiChuHo { get; set; } = null!;

    public string TenNguoiLienHeKhanCap { get; set; } = null!;

    public string SdtNguoiLienHeKhanCap { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual ICollection<HoSoYTe> HoSoYTes { get; set; } = new List<HoSoYTe>();

    public virtual HoGiaDinh? MaHgdNavigation { get; set; }

    public virtual TaiKhoan? MaTkNavigation { get; set; }

    public virtual ICollection<SuDungDichVu> SuDungDichVus { get; set; } = new List<SuDungDichVu>();

    public virtual ICollection<TuVan> TuVans { get; set; } = new List<TuVan>();
}
