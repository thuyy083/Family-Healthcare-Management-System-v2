using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQL_DichVuYTeGiaDinh.Models;

    public partial class ChiTietKhuyenMai
    {
        public int MaKhuyenMai { get; set; }

        public int MaDichVu { get; set; }

        // Quan hệ với ChuongTrinhKhuyenMai
        public virtual ChuongTrinhKhuyenMai? MaKhuyenMaiNavigation { get; set; }
        // Quan hệ với DichVuYTe
        public virtual DichVuYTe? MaDichVuNavigation { get; set; }
}

