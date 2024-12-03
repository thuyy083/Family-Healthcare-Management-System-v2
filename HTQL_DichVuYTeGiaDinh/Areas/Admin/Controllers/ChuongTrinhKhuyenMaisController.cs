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
    public class ChuongTrinhKhuyenMaisController : Controller
    {
        private readonly DataContext _context;

        public ChuongTrinhKhuyenMaisController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/ChuongTrinhKhuyenMais
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            int pageSize = 4; 
            int pageNumber = (page ?? 1); 

            var chuongTrinhKhuyenMaiQuery = _context.ChuongTrinhKhuyenMais.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                chuongTrinhKhuyenMaiQuery = chuongTrinhKhuyenMaiQuery.Where(km => km.TenKhuyenMai.Contains(searchString));
            }

            int totalItems = await chuongTrinhKhuyenMaiQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString; 

            var paginatedList = await chuongTrinhKhuyenMaiQuery
                .OrderBy(km => km.NgayBatDauKm)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize) 
                .ToListAsync();

            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }


        // GET: Admin/ChuongTrinhKhuyenMais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.ChuongTrinhKhuyenMais
                .Include(km => km.ChiTietKhuyenMais)
                .ThenInclude(ctkm => ctkm.MaDichVuNavigation)
                .FirstOrDefaultAsync(m => m.MaKhuyenMai == id);

            if (khuyenMai == null)
            {
                return NotFound();
            }

            return View(khuyenMai);
        }

        // GET: Admin/ChuongTrinhKhuyenMai/Create
        public IActionResult Create()
        {
            ViewBag.DichVuYTes = new SelectList(
                    _context.DichVuYTes.Where(dv => !dv.IsDelete),
                    "MaDichVu",
                    "TenDichVu"
                ); return View();
        }

        // POST: Admin/ChuongTrinhKhuyenMai/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChuongTrinhKhuyenMai chuongTrinhKhuyenMai, int[] selectedDichVuYTes)
        {
            if (chuongTrinhKhuyenMai.NgayBatDauKm > chuongTrinhKhuyenMai.NgayKetThucKm)
            {
                ModelState.AddModelError("NgayBatDauKm", "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(chuongTrinhKhuyenMai);
                await _context.SaveChangesAsync();

                foreach (var maDichVu in selectedDichVuYTes)
                {
                    var chiTietKhuyenMai = new ChiTietKhuyenMai
                    {
                        MaKhuyenMai = chuongTrinhKhuyenMai.MaKhuyenMai,
                        MaDichVu = maDichVu
                    };
                    _context.ChiTietKhuyenMais.Add(chiTietKhuyenMai);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DichVuYTes = new SelectList(
                    _context.DichVuYTes.Where(dv => !dv.IsDelete),
                    "MaDichVu",
                    "TenDichVu",
                    selectedDichVuYTes
                );
            return View(chuongTrinhKhuyenMai);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var khuyenMai = await _context.ChuongTrinhKhuyenMais
                .Include(km => km.ChiTietKhuyenMais)
                .ThenInclude(ct => ct.MaDichVuNavigation)
                .FirstOrDefaultAsync(km => km.MaKhuyenMai == id);

            if (khuyenMai == null)
            {
                return NotFound();
            }

            // Lấy danh sách tất cả các dịch vụ y tế để hiển thị trong form
            ViewBag.DichVuYTeList = await _context.DichVuYTes
                   .Where(dv => !dv.IsDelete) // Chỉ lấy dịch vụ chưa bị ẩn
                   .ToListAsync();

            return View(khuyenMai);
        }

        // POST: Admin/ChuongTrinhKhuyenMais/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ChuongTrinhKhuyenMai model, List<int> selectedDichVu)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật thông tin cơ bản của chương trình khuyến mãi
                var khuyenMai = await _context.ChuongTrinhKhuyenMais
                    .Include(km => km.ChiTietKhuyenMais)
                    .FirstOrDefaultAsync(km => km.MaKhuyenMai == model.MaKhuyenMai);

                if (khuyenMai == null)
                {
                    return NotFound();
                }

                khuyenMai.TenKhuyenMai = model.TenKhuyenMai;
                khuyenMai.PhanTramGiamGia = model.PhanTramGiamGia;
                khuyenMai.NgayBatDauKm = model.NgayBatDauKm;
                khuyenMai.NgayKetThucKm = model.NgayKetThucKm;
                khuyenMai.MoTa = model.MoTa;

                // Cập nhật danh sách các dịch vụ trong khuyến mãi
                // Xóa các dịch vụ cũ
                _context.ChiTietKhuyenMais.RemoveRange(khuyenMai.ChiTietKhuyenMais);

                // Thêm các dịch vụ mới
                foreach (var dichVuId in selectedDichVu)
                {
                    var chiTietKhuyenMai = new ChiTietKhuyenMai
                    {
                        MaKhuyenMai = model.MaKhuyenMai,
                        MaDichVu = dichVuId
                    };
                    _context.ChiTietKhuyenMais.Add(chiTietKhuyenMai);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = model.MaKhuyenMai });
            }

            // Nếu ModelState không hợp lệ, tải lại danh sách dịch vụ y tế để hiển thị
            ViewBag.DichVuYTeList = await _context.DichVuYTes.ToListAsync();
            return View(model);
        }

        // GET: Admin/ChuongTrinhKhuyenMais/Delete/5
        // Controller: ChuongTrinhKhuyenMaisController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Tìm chương trình khuyến mãi theo ID
            var chuongTrinhKhuyenMai = await _context.ChuongTrinhKhuyenMais
                .Include(ct => ct.ChiTietKhuyenMais)
                .FirstOrDefaultAsync(ctkm => ctkm.MaKhuyenMai == id);

            if (chuongTrinhKhuyenMai == null)
            {
                TempData["ErrorMessage"] = "Chương trình khuyến mãi không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            // Xóa chi tiết khuyến mãi
            _context.ChiTietKhuyenMais.RemoveRange(chuongTrinhKhuyenMai.ChiTietKhuyenMais);

            // Xóa chương trình khuyến mãi
            _context.ChuongTrinhKhuyenMais.Remove(chuongTrinhKhuyenMai);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa chương trình khuyến mãi thành công.";
            return RedirectToAction(nameof(Index));
        }



        private bool ChuongTrinhKhuyenMaiExists(int id)
        {
          return (_context.ChuongTrinhKhuyenMais?.Any(e => e.MaKhuyenMai == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> XoaDichVuKhoiKhuyenMai(int maKhuyenMai, int maDichVu)
        {
            var chiTietKhuyenMai = await _context.ChiTietKhuyenMais
                .FirstOrDefaultAsync(ct => ct.MaKhuyenMai == maKhuyenMai && ct.MaDichVu == maDichVu);

            if (chiTietKhuyenMai == null)
            {
                return NotFound();
            }

            _context.ChiTietKhuyenMais.Remove(chiTietKhuyenMai);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = maKhuyenMai });
        }
    }
}
