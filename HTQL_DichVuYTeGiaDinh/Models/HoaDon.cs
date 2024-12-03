using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class HoaDon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaHoaDon { get; set; }

    public int MaLanSuDung { get; set; }

    public int MaTk { get; set; }

    public DateTime NgayLapHoaDon { get; set; }

    public decimal TongTien { get; set; }

    public virtual SuDungDichVu? MaLanSuDungNavigation { get; set; }

    public virtual NhanVienYTe? MaTkNavigation { get; set; }

    public virtual ICollection<SuDungDichVu> SuDungDichVus { get; set; } = new List<SuDungDichVu>();
}
