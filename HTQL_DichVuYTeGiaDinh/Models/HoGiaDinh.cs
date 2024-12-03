using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class HoGiaDinh
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaHgd { get; set; }

    public string SodienthoaiHgd { get; set; } = null!;

    public string ChuHo { get; set; } = null!;

    public string DiaChiHgd { get; set; } = null!;

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();

    public virtual ICollection<PhanCongBacSi> PhanCongBacSis { get; set; } = new List<PhanCongBacSi>();
}
