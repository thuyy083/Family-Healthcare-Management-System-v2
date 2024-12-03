using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class Thuoc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaThuoc { get; set; }

    public int MaDvt { get; set; }

    public string TenThuoc { get; set; } = null!;

    public string MoTaThuoc { get; set; } = null!;

    public int HanSuDung { get; set; }

    public int SoLuong {  get; set; }

    public bool IsDelete { get; set; } = false;

    public virtual ICollection<ChiTietDonThuoc> ChiTietDonThuocs { get; set; } = new List<ChiTietDonThuoc>();

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual ICollection<DonGiaThuoc> DonGiaThuocs { get; set; } = new List<DonGiaThuoc>();

    public virtual DonViTinhThuoc? MaDvtNavigation { get; set; }
}
