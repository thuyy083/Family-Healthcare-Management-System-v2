using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class DonViTinhThuoc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaDvt { get; set; }

    public string TenDvt { get; set; } = null!;

    public virtual ICollection<Thuoc> Thuocs { get; set; } = new List<Thuoc>();
}
