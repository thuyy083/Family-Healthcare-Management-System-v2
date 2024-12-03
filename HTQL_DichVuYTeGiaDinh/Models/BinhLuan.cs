using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class BinhLuan
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaBinhLuan { get; set; }

    public int MaDichVu { get; set; }

    public int MaTk { get; set; }

    public DateTime ThoiDiemBinhLuan { get; set; }

    public string NoiDungBinhLuan { get; set; } = null!;

    public decimal DiemDanhGia { get; set; }

    public virtual DichVuYTe? MaDichVuNavigation { get; set; }

    public virtual KhachHang? MaTkNavigation { get; set; }
}
