using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class ChuyenKhoa
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaChuyenKhoa { get; set; }

    public string TenChuyenKhoa { get; set; } = null!;

    public virtual ICollection<BacSi> BacSis { get; set; } = new List<BacSi>();
}
