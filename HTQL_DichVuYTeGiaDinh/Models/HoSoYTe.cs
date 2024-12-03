using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

public partial class HoSoYTe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaHoSo { get; set; }

    public int MaTk { get; set; }

    public int BacMaTk { get; set; }

    public DateTime NgayKhamBenh { get; set; }

    public string ChanDoan { get; set; } = null!;

    public DateTime? NgayTaiKham { get; set; }

    public string? ThongTinKhac { get; set; }

    public virtual BacSi? BacMaTkNavigation { get; set; }

    public virtual ICollection<ChiTietKetQuaCanLamSang> ChiTietKetQuaCanLamSangs { get; set; } = new List<ChiTietKetQuaCanLamSang>();

    public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();

    public virtual KhachHang? MaTkNavigation { get; set; }
}
