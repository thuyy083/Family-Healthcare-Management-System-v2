using System;
using System.Collections.Generic;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class ChiTietPhieuNhap
{
    public int MaThuoc { get; set; }

    public int MaPhieuNhap { get; set; }

    public int SoLuongNhap { get; set; }

    public decimal DonGiaNhap { get; set; }

    public virtual PhieuNhapThuoc? MaPhieuNhapNavigation { get; set; }

    public virtual Thuoc? MaThuocNavigation { get; set; }
}
