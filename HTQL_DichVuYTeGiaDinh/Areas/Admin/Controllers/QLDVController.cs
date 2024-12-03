using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QLDVController : Controller
    {
        private readonly DataContext _context;
        public QLDVController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> getListDV(string searchString, int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            // Tạo câu truy vấn
            var listDVQuery = _context.DichVuYTes.AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                listDVQuery = listDVQuery.Where(s => s.TenDichVu.Contains(searchString));
            }

            // Lấy tổng số trang
            int totalItems = await listDVQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;

            // Phân trang và lấy dữ liệu
            var paginatedList = await listDVQuery
                .OrderBy(s => s.TenDichVu)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(paginatedList);
        }

        [HttpGet]
        public IActionResult createDV()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> createDV(DichVuYTe DV)
        {
            if (DV == null)
            {
                // Xử lý trường hợp DV là null
                ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
                return View(DV);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var dichVu = new DichVuYTe
                    {
                        TenDichVu = DV.TenDichVu,
                        MoTa = DV.MoTa,
                        DonViTinh = DV.DonViTinh
                    };
                    _context.DichVuYTes.Add(dichVu);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("getListDV");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Lỗi lưu dữ liệu: " + ex.Message);
                }
            }

            // Nếu ModelState không hợp lệ hoặc có lỗi thì trả về view CreateDV
            return View(DV);
        }

    }
}
