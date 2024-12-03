using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTQL_DichVuYTeGiaDinh.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HoSoYTeController : Controller
    {
        private readonly DataContext _context;

        public HoSoYTeController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> DanhSachHoSo(int? page)
        {
            int pageSize = 5; // Số hồ sơ trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là trang 1

            // Lấy mã tài khoản từ Session
            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Truy vấn danh sách hồ sơ y tế
            var danhSachHoSoQuery = _context.HoSoYTes
                .Include(hs => hs.BacMaTkNavigation)
                .Where(hs => hs.MaTk == maTk)
                .OrderByDescending(hs => hs.NgayKhamBenh)
                .AsQueryable();

            // Tổng số hồ sơ để tính tổng số trang
            int totalItems = await danhSachHoSoQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;

            // Lấy danh sách hồ sơ cho trang hiện tại
            var paginatedList = await danhSachHoSoQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Đánh số thứ tự bắt đầu
            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }

        public async Task<IActionResult> ChiTietHoSo(int id)
        {
            // Lấy thông tin hồ sơ y tế
            var hoSo = await _context.HoSoYTes
                .Include(hs => hs.BacMaTkNavigation)
                .Include(hs => hs.ChiTietKetQuaCanLamSangs)
                    .ThenInclude(ct => ct.MaPhuongPhapNavigation)
                .Include(hs => hs.DonThuocs)
                    .ThenInclude(dt => dt.ChiTietDonThuocs)
                        .ThenInclude(ct => ct.MaThuocNavigation)
                .FirstOrDefaultAsync(hs => hs.MaHoSo == id);

            if (hoSo == null)
            {
                return NotFound("Không tìm thấy hồ sơ y tế.");
            }

            return View(hoSo);
        }
    }
}
