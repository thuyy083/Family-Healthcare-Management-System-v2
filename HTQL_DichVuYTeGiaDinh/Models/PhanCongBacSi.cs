using System;
using System.Collections.Generic;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class PhanCongBacSi
{
    public int MaHgd { get; set; }

    public int MaTk { get; set; }

    public DateTime ThoiGianBatDau { get; set; }

    public DateTime ThoiGianKetThuc { get; set; }

    public virtual HoGiaDinh? MaHgdNavigation { get; set; }

    public virtual BacSi? MaTkNavigation { get; set; }
}
