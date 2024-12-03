using System;
using System.Collections.Generic;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class ChiTietKetQuaCanLamSang
{
    public int MaPhuongPhap { get; set; }

    public int MaHoSo { get; set; }

    public string HinhAnhKetQua { get; set; } = null!;

    public virtual HoSoYTe? MaHoSoNavigation { get; set; }

    public virtual PhuongPhapCanLamSang? MaPhuongPhapNavigation { get; set; }
}
