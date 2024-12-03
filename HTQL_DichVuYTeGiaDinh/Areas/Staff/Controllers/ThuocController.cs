using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTQL_DichVuYTeGiaDinh.Areas.Staff.Controllers
{
    [Area("Staff")]

    public class ThuocController : Controller
    {
        private readonly DataContext _context;

        public ThuocController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? page)
        {
            int pageSize = 4; // Kích thước trang
            int pageNumber = (page ?? 1); // Số trang hiện tại

            var thuocsQuery = _context.Thuocs.Include(t => t.MaDvtNavigation).AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                thuocsQuery = thuocsQuery.Where(t => t.TenThuoc.Contains(searchString));
            }

            thuocsQuery = thuocsQuery.OrderBy(t => t.TenThuoc);

            // Lấy tổng số lượng thuốc để tính số trang
            int totalItems = await thuocsQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString; // Giữ lại giá trị tìm kiếm

            // Phân trang
            var paginatedList = await thuocsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Đánh số thứ tự
            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy thông tin chi tiết của thuốc
            var thuoc = await _context.Thuocs
                .Include(t => t.MaDvtNavigation) // Đơn vị tính thuốc
                .Include(t => t.DonGiaThuocs) // Đơn giá thuốc
                .Include(t => t.ChiTietPhieuNhaps) // Chi tiết phiếu nhập
                    .ThenInclude(ct => ct.MaPhieuNhapNavigation) // Phiếu nhập liên quan
                .FirstOrDefaultAsync(t => t.MaThuoc == id);

            if (thuoc == null)
            {
                return NotFound();
            }

            return View(thuoc);
        }
    }
}
