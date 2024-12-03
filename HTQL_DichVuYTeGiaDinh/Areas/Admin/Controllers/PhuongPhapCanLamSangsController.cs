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
    public class PhuongPhapCanLamSangsController : Controller
    {
        private readonly DataContext _context;

        public PhuongPhapCanLamSangsController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/PhuongPhapCanLamSangs
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            int pageSize = 4; // Kích thước trang
            int pageNumber = (page ?? 1); // Số trang hiện tại

            // Tạo câu truy vấn
            var clsQuery = _context.PhuongPhapCanLamSangs.AsQueryable();

            // Tìm kiếm theo tên khuyến mãi
            if (!string.IsNullOrEmpty(searchString))
            {
                clsQuery = clsQuery.Where(km => km.TenPhuongPhap.Contains(searchString));
            }

            // Lấy tổng số mục để tính số trang
            int totalItems = await clsQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString; 

            // Phân trang và lấy dữ liệu
            var paginatedList = await clsQuery
                .OrderBy(km => km.TenPhuongPhap) 
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize) 
                .ToListAsync();

            // Đánh số thứ tự cho dữ liệu
            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }




        // GET: Admin/PhuongPhapCanLamSangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PhuongPhapCanLamSangs == null)
            {
                return NotFound();
            }

            var phuongPhapCanLamSang = await _context.PhuongPhapCanLamSangs
                .FirstOrDefaultAsync(m => m.MaPhuongPhap == id);
            if (phuongPhapCanLamSang == null)
            {
                return NotFound();
            }

            return View(phuongPhapCanLamSang);
        }

        // GET: Admin/PhuongPhapCanLamSangs/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/PhuongPhapCanLamSangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhuongPhapCanLamSang phuongPhap)
        {
            if (ModelState.IsValid)
            {
                _context.PhuongPhapCanLamSangs.Add(phuongPhap);
                _context.SaveChanges();
                return RedirectToAction("Index"); // Chuyển hướng đến trang danh sách sau khi tạo thành công
            }
            return View(phuongPhap); // Nếu có lỗi, quay lại trang Create
        }

        // GET: Admin/PhuongPhapCanLamSangs/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Tìm phương pháp theo id
            var phuongPhap = _context.PhuongPhapCanLamSangs.FirstOrDefault(p => p.MaPhuongPhap == id);
            if (phuongPhap == null)
            {
                return NotFound();
            }

            return View(phuongPhap);
        }
        // POST: PhuongPhapCanLamSang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PhuongPhapCanLamSang phuongPhap)
        {
            if (id != phuongPhap.MaPhuongPhap)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // Cập nhật thông tin phương pháp
                _context.PhuongPhapCanLamSangs.Update(phuongPhap);
                _context.SaveChanges();

                return RedirectToAction(nameof(Details), new { id = phuongPhap.MaPhuongPhap });
            }

            return View(phuongPhap);
        }

        // GET: Admin/PhuongPhapCanLamSangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PhuongPhapCanLamSangs == null)
            {
                return NotFound();
            }

            var phuongPhapCanLamSang = await _context.PhuongPhapCanLamSangs
                .FirstOrDefaultAsync(m => m.MaPhuongPhap == id);
            if (phuongPhapCanLamSang == null)
            {
                return NotFound();
            }

            return View(phuongPhapCanLamSang);
        }

        // POST: Admin/PhuongPhapCanLamSangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PhuongPhapCanLamSangs == null)
            {
                return Problem("Entity set 'DataContext.PhuongPhapCanLamSangs'  is null.");
            }
            var phuongPhapCanLamSang = await _context.PhuongPhapCanLamSangs.FindAsync(id);
            if (phuongPhapCanLamSang != null)
            {
                _context.PhuongPhapCanLamSangs.Remove(phuongPhapCanLamSang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhuongPhapCanLamSangExists(int id)
        {
          return (_context.PhuongPhapCanLamSangs?.Any(e => e.MaPhuongPhap == id)).GetValueOrDefault();
        }
    }
}
