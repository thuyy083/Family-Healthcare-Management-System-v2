using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class PhieuThuTienThuoc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaPhieuThu { get; set; }

    public int MaDonThuoc { get; set; }

    public int MaTk { get; set; }

    public DateTime NgayLapPhieuThu { get; set; }

    public decimal TongTien { get; set; }

    public virtual DonThuoc? MaDonThuocNavigation { get; set; }

    public virtual NhanVienYTe? MaTkNavigation { get; set; }
}
