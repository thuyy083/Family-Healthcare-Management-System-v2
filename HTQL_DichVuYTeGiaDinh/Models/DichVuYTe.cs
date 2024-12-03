using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class DichVuYTe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int MaDichVu { get; set; }

    public string TenDichVu { get; set; } = null!;

    public string MoTa { get; set; } = null!;

    public string DonViTinh { get; set; } = null!;

    public bool IsDelete { get; set; } = false;

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual ICollection<DonGiaDichVu> DonGiaDichVus { get; set; } = new List<DonGiaDichVu>();

    public virtual ICollection<SuDungDichVu> SuDungDichVus { get; set; } = new List<SuDungDichVu>();

    public virtual ICollection<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; } = new List<ChiTietKhuyenMai>();
}
