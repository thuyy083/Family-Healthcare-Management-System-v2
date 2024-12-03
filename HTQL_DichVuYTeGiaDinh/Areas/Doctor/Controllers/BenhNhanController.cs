using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using HTQL_DichVuYTeGiaDinh.ViewModels;

namespace HTQL_DichVuYTeGiaDinh.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    public class BenhNhanController : Controller
    {
        private readonly DataContext _context;

        public BenhNhanController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> DanhSachBenhNhan(string searchString, int? page)
        {
            var maTk = HttpContext.Session.GetInt32("MaTk");

            if (maTk == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int pageSize = 4; 
            int pageNumber = page ?? 1; 

            var danhSachHoGiaDinh = _context.PhanCongBacSis
                                            .Where(pc => pc.MaTk == maTk)
                                            .Select(pc => pc.MaHgd)
                                            .ToList();

            var benhNhanQuery = _context.KhachHangs
                                        .Where(kh => danhSachHoGiaDinh.Contains(kh.MaHgd))
                                        .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                benhNhanQuery = benhNhanQuery.Where(kh => kh.HoTen.Contains(searchString));
            }

            int totalItems = await benhNhanQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString;

            var paginatedList = await benhNhanQuery
                                        .OrderBy(kh => kh.HoTen)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benhNhan = await _context.KhachHangs
                .Include(k => k.MaHgdNavigation) 
                .FirstOrDefaultAsync(m => m.MaTk == id);

            if (benhNhan == null)
            {
                return NotFound();
            }

            return View(benhNhan);
        }


    }
}
