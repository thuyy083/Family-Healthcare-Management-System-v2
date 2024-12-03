using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class SuDungDichVu
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaLanSuDung { get; set; }

    public int MaDichVu { get; set; }

    public int MaTk { get; set; }

    public DateTime NgayBatDau { get; set; }

    public int SoLuong { get; set; }

    public string SoDienThoaiLienHe { get; set; } = null!;

    public string DiaChiLienLac { get; set; } = null!;

    public short YeuCauChiDinhNhanVien { get; set; }

    public string? TenNhanVienChiDinh { get; set; }

    public string? MoTaBenhLy { get; set; }

    public short TrangThai { get; set; }

    public decimal TongTien { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual DichVuYTe? MaDichVuNavigation { get; set; }

    public virtual KhachHang? MaTkNavigation { get; set; }
}
