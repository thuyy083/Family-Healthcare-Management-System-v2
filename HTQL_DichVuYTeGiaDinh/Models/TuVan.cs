using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class TuVan
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaTinNhan { get; set; }

    public int MaTk { get; set; }

    public int BacMaTk { get; set; }

    public string NoiDungTinNhan { get; set; } = null!;

    public DateTime ThoiDiemNhanTin { get; set; }

    public short TrangThaiTinNhan { get; set; }

    public virtual BacSi? BacMaTkNavigation { get; set; }

    public virtual KhachHang? MaTkNavigation { get; set; }
}
