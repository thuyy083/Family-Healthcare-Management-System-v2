using HTQL_DichVuYTeGiaDinh.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace HTQL_DichVuYTeGiaDinh.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.BacSis.Any())
            {
                ChuyenKhoa NoiTongQuat = new ChuyenKhoa { TenChuyenKhoa = "Nội Tổng Quát" };
                ChuyenKhoa NgoaiTongQuat = new ChuyenKhoa { TenChuyenKhoa = "Ngoại Tổng Quát" };
                //ChuyenKhoa NhiKhoa = new ChuyenKhoa { TenChuyenKhoa = "Nhi Khoa" };
                //ChuyenKhoa DaLieu = new ChuyenKhoa { TenChuyenKhoa = "Da Liễu" };
                QuyenTruyCap BacSi = new QuyenTruyCap { TenQuyen = "Bác Sĩ" };

            }
        }
    }
}
