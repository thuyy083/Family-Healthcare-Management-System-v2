using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using HTQL_DichVuYTeGiaDinh.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HTQL_DichVuYTeGiaDinh.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;



        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult TrangChu()
        {
            return View();
        }

        public IActionResult GioiThieu()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> DichVu()
        {
            // Lấy danh sách dịch vụ khuyến mãi
            var dichVuKhuyenMaiRaw = await _context.ChiTietKhuyenMais
                .Include(ct => ct.MaDichVuNavigation)
                .Include(ct => ct.MaKhuyenMaiNavigation)
                .Where(ct => ct.MaKhuyenMaiNavigation != null &&
                             ct.MaKhuyenMaiNavigation.NgayBatDauKm <= DateTime.Now &&
                             ct.MaKhuyenMaiNavigation.NgayKetThucKm >= DateTime.Now)
                .ToListAsync();

            // Xử lý tính giá khuyến mãi trong mã C#
            var dichVuKhuyenMai = dichVuKhuyenMaiRaw.Select(ct => new DichVuKhuyenMaiViewModel
            {
                MaDichVu = ct.MaDichVuNavigation.MaDichVu,
                TenDichVu = ct.MaDichVuNavigation.TenDichVu,
                MoTa = ct.MaDichVuNavigation.MoTa,
                DonViTinh = ct.MaDichVuNavigation.DonViTinh,
                GiaGoc = _context.DonGiaDichVus
                    .Where(dg => dg.MaDichVu == ct.MaDichVu)
                    .OrderByDescending(dg => dg.ThoiDiem)
                    .Select(dg => dg.DonGiaDv)
                    .FirstOrDefault(),
                GiaKhuyenMai = _context.DonGiaDichVus
                    .Where(dg => dg.MaDichVu == ct.MaDichVu)
                    .OrderByDescending(dg => dg.ThoiDiem)
                    .Select(dg => dg.DonGiaDv)
                    .FirstOrDefault() * (1 - (decimal)(ct.MaKhuyenMaiNavigation.PhanTramGiamGia / 100))
            }).ToList();

            // Lấy danh sách dịch vụ không khuyến mãi
            var maDichVuKhuyenMai = dichVuKhuyenMai.Select(dv => dv.MaDichVu).ToList();
            var dichVuKhongKhuyenMai = await _context.DichVuYTes
                .Include(dv => dv.DonGiaDichVus)
                .Where(dv => !maDichVuKhuyenMai.Contains(dv.MaDichVu))
                .Select(dv => new DichVuViewModel
                {
                    MaDichVu = dv.MaDichVu,
                    TenDichVu = dv.TenDichVu,
                    MoTa = dv.MoTa,
                    DonViTinh = dv.DonViTinh,
                    DonGiaDv = dv.DonGiaDichVus
                        .OrderByDescending(dg => dg.ThoiDiem)
                        .Select(dg => dg.DonGiaDv)
                        .FirstOrDefault()
                })
                .ToListAsync();

            // Chuẩn bị ViewModel chính để truyền vào View
            var viewModel = new DanhSachDichVuViewModel
            {
                DichVuKhuyenMai = dichVuKhuyenMai,
                DichVuKhongKhuyenMai = dichVuKhongKhuyenMai
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BinhLuan(int id)
        {
            // Lấy thông tin dịch vụ
            var dichVu = await _context.DichVuYTes
                .Include(dv => dv.BinhLuans)
                    .ThenInclude(bl => bl.MaTkNavigation) // Bao gồm thông tin người dùng
                .FirstOrDefaultAsync(dv => dv.MaDichVu == id);

            if (dichVu == null)
            {
                return NotFound();
            }

            // Tính điểm đánh giá trung bình
            var diemTrungBinh = dichVu.BinhLuans.Any()
                ? dichVu.BinhLuans.Average(bl => bl.DiemDanhGia)
                : 0;

            // Chuẩn bị danh sách bình luận
            var binhLuans = dichVu.BinhLuans
                .OrderByDescending(bl => bl.ThoiDiemBinhLuan)
                .Select(bl => new BinhLuanViewModel
                {
                    TenNguoiBinhLuan = bl.MaTkNavigation?.HoTen,
                    DiemDanhGia = bl.DiemDanhGia,
                    ThoiDiemBinhLuan = bl.ThoiDiemBinhLuan,
                    NoiDungBinhLuan = bl.NoiDungBinhLuan
                }).ToList();

            // Chuẩn bị ViewModel
            var viewModel = new ChiTietDichVuViewModel
            {
                MaDichVu = dichVu.MaDichVu,
                TenDichVu = dichVu.TenDichVu,
                MoTa = dichVu.MoTa,
                DonViTinh = dichVu.DonViTinh,
                DiemTrungBinh = (double)diemTrungBinh,
                BinhLuans = binhLuans
            };

            return View(viewModel);
        }



    }
}
