using System;
using System.Collections.Generic;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class ChiTietDonThuoc
{
    public int MaDonThuoc { get; set; }

    public int MaThuoc { get; set; }

    public int SoLuong { get; set; }

    public string HuongDanSuDung { get; set; } = null!;

    public virtual DonThuoc? MaDonThuocNavigation { get; set; }

    public virtual Thuoc? MaThuocNavigation { get; set; }
}
