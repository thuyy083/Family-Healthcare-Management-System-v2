using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;

namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoGiaDinhsController : Controller
    {
        private readonly DataContext _context;

        public HoGiaDinhsController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/HoGiaDinhs
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            int pageSize = 4; 
            int pageNumber = (page ?? 1); 

            var clsQuery = _context.HoGiaDinhs.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                clsQuery = clsQuery.Where(km => km.ChuHo.Contains(searchString));
            }

            int totalItems = await clsQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString;

            var paginatedList = await clsQuery
                .OrderBy(km => km.ChuHo)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }

        // GET: Admin/HoGiaDinhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoGiaDinh = await _context.HoGiaDinhs
                .Include(hgd => hgd.KhachHangs) 
                .Include(hgd => hgd.PhanCongBacSis)
                    .ThenInclude(pc => pc.MaTkNavigation) 
                        .ThenInclude(bs => bs.MaChuyenKhoaNavigation)
                .FirstOrDefaultAsync(hgd => hgd.MaHgd == id);

            if (hoGiaDinh == null)
            {
                return NotFound();
            }

            return View(hoGiaDinh);
        }

        // GET: Admin/HoGiaDinhs/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SodienthoaiHgd,ChuHo,DiaChiHgd")] HoGiaDinh hoGiaDinh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoGiaDinh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = hoGiaDinh.MaHgd });
            }
            return View(hoGiaDinh);
        }

        // GET: Admin/HoGiaDinhs/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HoGiaDinhs == null)
            {
                return NotFound();
            }

            var hoGiaDinh = await _context.HoGiaDinhs.FindAsync(id);
            if (hoGiaDinh == null)
            {
                return NotFound();
            }
            return View(hoGiaDinh);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHgd,SodienthoaiHgd,ChuHo,DiaChiHgd")] HoGiaDinh hoGiaDinh)
        {
            if (id != hoGiaDinh.MaHgd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoGiaDinh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoGiaDinhExists(hoGiaDinh.MaHgd))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = hoGiaDinh.MaHgd });
            }
            return View(hoGiaDinh);
        }
        // GET: Admin/HoGiaDinhs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HoGiaDinhs == null)
            {
                return NotFound();
            }

            var hoGiaDinh = await _context.HoGiaDinhs
                .FirstOrDefaultAsync(m => m.MaHgd == id);
            if (hoGiaDinh == null)
            {
                return NotFound();
            }

            return View(hoGiaDinh);
        }

        // POST: Admin/HoGiaDinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HoGiaDinhs == null)
            {
                return Problem("Entity set 'DataContext.HoGiaDinhs'  is null.");
            }
            var hoGiaDinh = await _context.HoGiaDinhs.FindAsync(id);
            if (hoGiaDinh != null)
            {
                _context.HoGiaDinhs.Remove(hoGiaDinh);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoGiaDinhExists(int id)
        {
          return (_context.HoGiaDinhs?.Any(e => e.MaHgd == id)).GetValueOrDefault();
        }


        [HttpGet]
        public async Task<IActionResult> ThemPhanCong(int id) // id ở đây là MaHgd
        {
            var hoGiaDinh = await _context.HoGiaDinhs.FindAsync(id);
            if (hoGiaDinh == null)
            {
                return NotFound();
            }

            // Lấy danh sách bác sĩ để chọn từ dropdown
            var bacSiList = await _context.BacSis.ToListAsync();
            ViewBag.BacSiList = new SelectList(bacSiList, "MaTk", "HoTen");

            // Tạo ViewModel để truyền dữ liệu vào view
            var phanCongViewModel = new PhanCongBacSi
            {
                MaHgd = id, // Gán MaHgd cho phân công
                ThoiGianBatDau = DateTime.Now, // Đặt mặc định thời gian bắt đầu là hôm nay
                ThoiGianKetThuc = DateTime.Now // Đặt mặc định thời gian kết thúc là hôm nay
            };

            return View(phanCongViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemPhanCong(PhanCongBacSi phanCongBacSi)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem bác sĩ đã được phân công cho hộ gia đình này hay chưa
                var isAlreadyAssigned = await _context.PhanCongBacSis
                    .AnyAsync(pc => pc.MaHgd == phanCongBacSi.MaHgd && pc.MaTk == phanCongBacSi.MaTk);

                if (isAlreadyAssigned)
                {
                    // Thông báo lỗi nếu bác sĩ đã được phân công
                    ModelState.AddModelError("", "Bác sĩ này đã được phân công cho hộ gia đình.");
                    var List = await _context.BacSis.ToListAsync();
                    ViewBag.BacSiList = new SelectList(List, "MaTk", "HoTen", phanCongBacSi.MaTk);
                    return View(phanCongBacSi);
                }

                // Lưu phân công bác sĩ vào cơ sở dữ liệu
                _context.PhanCongBacSis.Add(phanCongBacSi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = phanCongBacSi.MaHgd });
            }

            // Nếu dữ liệu không hợp lệ, hiển thị lại form
            var bacSiList = await _context.BacSis.ToListAsync();
            ViewBag.BacSiList = new SelectList(bacSiList, "MaTk", "HoTen", phanCongBacSi.MaTk);
            return View(phanCongBacSi);
        }


    }
}
