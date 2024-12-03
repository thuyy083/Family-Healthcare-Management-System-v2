using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class PhieuNhapThuoc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaPhieuNhap { get; set; }

    public int MaTk { get; set; }

    public DateTime NgayLapPhieuNhap { get; set; }

    public decimal TongTienNhap { get; set; }

    public string TenNhaCungCap { get; set; } = null!;

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual NhanVienYTe? MaTkNavigation { get; set; }
}
