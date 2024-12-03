using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HTQL_DichVuYTeGiaDinh.ViewModels;


namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class BaoCaoController : Controller
    {
        private readonly DataContext _context;

        public BaoCaoController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> BaoCaoDoanhThu(int? thang, int? nam)
        {
            thang ??= DateTime.Now.Month;
            nam ??= DateTime.Now.Year;

            var doanhThuTheoDichVu = await _context.SuDungDichVus
                .Where(s => s.TrangThai == 1 &&
                            s.NgayBatDau.Month == thang &&
                            s.NgayBatDau.Year == nam)
                .GroupBy(s => s.MaDichVuNavigation.TenDichVu)
                .Select(g => new
                {
                    TenDichVu = g.Key,
                    TongDoanhThu = g.Sum(s => s.TongTien)
                })
                .ToListAsync();

            var viewModel = new BaoCaoDoanhThuViewModel
            {
                Thang = thang.Value,
                Nam = nam.Value,
                DoanhThuTheoDichVu = doanhThuTheoDichVu
                    .OrderByDescending(d => d.TongDoanhThu)
                    .Select(d => new DoanhThuDichVuViewModel
                    {
                        TenDichVu = d.TenDichVu,
                        TongDoanhThu = d.TongDoanhThu
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BaoCaoDoanhThuChiPhi(int? thang, int? nam)
        {
            thang ??= DateTime.Now.Month;
            nam ??= DateTime.Now.Year;

            var doanhThu = await _context.PhieuThuTienThuocs
                .Where(pt => pt.NgayLapPhieuThu.Month == thang && pt.NgayLapPhieuThu.Year == nam)
                .SumAsync(pt => pt.TongTien);

            var chiPhi = await _context.PhieuNhapThuocs
                .Where(pn => pn.NgayLapPhieuNhap.Month == thang && pn.NgayLapPhieuNhap.Year == nam)
                .SumAsync(pn => pn.TongTienNhap);

            var viewModel = new BaoCaoDoanhThuChiPhiViewModel
            {
                Thang = thang.Value,
                Nam = nam.Value,
                DoanhThu = doanhThu,
                ChiPhi = chiPhi
            };

            return View(viewModel);
        }


    }
}
