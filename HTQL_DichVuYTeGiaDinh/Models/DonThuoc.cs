using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class DonThuoc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaDonThuoc { get; set; }

    public int MaHoSo { get; set; }

    public DateTime NgayKeDon { get; set; }

    public string TrangThaiDonThuoc { get; set; } = null!;

    public string? GhiChuDonThuoc { get; set; }

    public virtual ICollection<ChiTietDonThuoc> ChiTietDonThuocs { get; set; } = new List<ChiTietDonThuoc>();

    public virtual HoSoYTe? MaHoSoNavigation { get; set; }

    public virtual ICollection<PhieuThuTienThuoc> PhieuThuTienThuocs { get; set; } = new List<PhieuThuTienThuoc>();
}
