using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class ChuongTrinhKhuyenMai
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaKhuyenMai { get; set; }

    public string TenKhuyenMai { get; set; } = null!;

    [Range(0, 100)]
    public float PhanTramGiamGia { get; set; }

    public DateTime NgayBatDauKm { get; set; }

    public DateTime NgayKetThucKm { get; set; }

    public string? MoTa { get; set; }

    public virtual ICollection<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; } = new List<ChiTietKhuyenMai>();
}
