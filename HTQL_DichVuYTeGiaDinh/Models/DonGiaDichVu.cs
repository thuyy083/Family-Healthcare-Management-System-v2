using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class DonGiaDichVu
{
    [Key, Column(Order = 0)]
    public int MaDichVu { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập đơn giá.")]
    [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0.")]
    public decimal DonGiaDv { get; set; }

    [Key, Column(Order = 1)]
    public DateTime ThoiDiem { get; set; }

    public virtual DichVuYTe? MaDichVuNavigation { get; set; }
}
