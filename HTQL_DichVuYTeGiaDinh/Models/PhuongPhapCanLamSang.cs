using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class PhuongPhapCanLamSang
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaPhuongPhap { get; set; }

    public string TenPhuongPhap { get; set; } = null!;

    public string? YeuCauDacBiet { get; set; }

    public decimal ChiPhi { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietKetQuaCanLamSang> ChiTietKetQuaCanLamSangs { get; set; } = new List<ChiTietKetQuaCanLamSang>();
}
