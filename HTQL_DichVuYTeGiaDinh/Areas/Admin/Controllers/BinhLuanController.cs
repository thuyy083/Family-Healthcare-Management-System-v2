using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BinhLuanController : Controller
    {
        private readonly DataContext _context;

        public BinhLuanController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? maDichVu, string searchString, int? page)
        {
            int pageSize = 4; 
            int pageNumber = page ?? 1; 

            var dichVuList = await _context.DichVuYTes.ToListAsync();
            ViewBag.DichVuList = new SelectList(dichVuList, "MaDichVu", "TenDichVu", maDichVu);

            var binhLuanQuery = _context.BinhLuans
                .Include(bl => bl.MaDichVuNavigation)
                .Include(bl => bl.MaTkNavigation)
                .AsQueryable();

            if (maDichVu.HasValue && maDichVu > 0)
            {
                binhLuanQuery = binhLuanQuery.Where(bl => bl.MaDichVu == maDichVu);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                binhLuanQuery = binhLuanQuery.Where(bl => bl.MaTkNavigation.HoTen.Contains(searchString));
            }

            int totalItems = await binhLuanQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString;

            var paginatedList = await binhLuanQuery
                .OrderByDescending(bl => bl.ThoiDiemBinhLuan)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }


    }
}
