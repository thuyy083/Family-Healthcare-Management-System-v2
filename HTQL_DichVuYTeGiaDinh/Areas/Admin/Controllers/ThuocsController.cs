using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using HTQL_DichVuYTeGiaDinh.ViewModels;

namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThuocsController : Controller
    {
        private readonly DataContext _context;

        public ThuocsController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/Thuocs
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


        // GET: Admin/Thuocs/Details/5
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


        // GET: Admin/Thuocs/Create
        public async Task<IActionResult> Create()
        {
            // Lấy danh sách đơn vị tính thuốc để hiển thị trong dropdown
            var donViTinhs = await _context.DonViTinhThuocs.ToListAsync();
            ViewBag.DonViTinhs = donViTinhs;
            return View();
        }


        // POST: Admin/Thuocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Thuoc thuoc)
        {
            if (ModelState.IsValid)
            {
                thuoc.SoLuong = 0;
                _context.Add(thuoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu model không hợp lệ, lấy lại danh sách đơn vị tính thuốc
            ViewBag.DonViTinhs = await _context.DonViTinhThuocs.ToListAsync();
            return View(thuoc);
        }


        // GET: Admin/Thuocs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var thuoc = await _context.Thuocs.Include(t => t.MaDvtNavigation)
                .FirstOrDefaultAsync(t => t.MaThuoc == id);

            if (thuoc == null)
            {
                return NotFound();
            }

            // Lấy danh sách đơn vị tính để hiển thị
            var donViTinhs = await _context.DonViTinhThuocs.ToListAsync();

            // Tạo ViewModel để truyền dữ liệu vào View
            var viewModel = new EditThuocViewModel
            {
                MaThuoc = thuoc.MaThuoc,
                TenThuoc = thuoc.TenThuoc,
                MoTaThuoc = thuoc.MoTaThuoc,
                HanSuDung = thuoc.HanSuDung,
                SoLuong = thuoc.SoLuong,
                MaDvt = thuoc.MaDvt,
                DonViTinhs = donViTinhs
            };

            return View(viewModel);
        }
        // POST: Admin/Thuocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditThuocViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.DonViTinhs = await _context.DonViTinhThuocs.ToListAsync();
                // In ra lỗi cụ thể từ ModelState
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Lỗi tại trường {state.Key}: {error.ErrorMessage}");
                    }
                }
                viewModel.DonViTinhs = await _context.DonViTinhThuocs.ToListAsync();
                return View(viewModel);
            }
            else
            {
                var thuoc = await _context.Thuocs.FindAsync(viewModel.MaThuoc);
                if (thuoc == null)
                {
                    return NotFound();
                }

                thuoc.TenThuoc = viewModel.TenThuoc;
                thuoc.MoTaThuoc = viewModel.MoTaThuoc;
                thuoc.HanSuDung = viewModel.HanSuDung;
                thuoc.SoLuong = viewModel.SoLuong;
                thuoc.MaDvt = viewModel.MaDvt;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = viewModel.MaThuoc });
            }


        }

    // GET: Admin/Thuocs/Delete/5
    public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Thuocs == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuocs
                .Include(t => t.MaDvtNavigation)
                .FirstOrDefaultAsync(m => m.MaThuoc == id);
            if (thuoc == null)
            {
                return NotFound();
            }

            return View(thuoc);
        }

        // POST: Admin/Thuocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Thuocs == null)
            {
                return Problem("Entity set 'DataContext.Thuocs'  is null.");
            }
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc != null)
            {
                _context.Thuocs.Remove(thuoc);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThuocExists(int id)
        {
          return (_context.Thuocs?.Any(e => e.MaThuoc == id)).GetValueOrDefault();
        }

        [HttpGet]
        public IActionResult CreateDonGia(int id)
        {
            var model = new DonGiaThuoc
            {
                MaThuoc = id,
                ThoiDiem = DateTime.Now // Gán thời điểm hiện tại khi tạo đơn giá mới
            };
            return View(model);
        }



        // POST: Thêm đơn giá mới cho dịch vụ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDonGia(DonGiaThuoc donGiaThuoc)
        {
            if (!ModelState.IsValid)
            {
                // In ra lỗi cụ thể từ ModelState
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Lỗi tại trường {state.Key}: {error.ErrorMessage}");
                    }
                }
                return View(donGiaThuoc);
            }

            // Kiểm tra xem dịch vụ có tồn tại không
            var thuoc = await _context.Thuocs.FindAsync(donGiaThuoc.MaThuoc);
            if (thuoc == null)
            {
                return NotFound("Thuốc không tồn tại.");
            }

            // Gán dịch vụ cho thuộc tính điều hướng
            donGiaThuoc.MaThuocNavigation = thuoc;

            // Thêm đơn giá vào cơ sở dữ liệu
            _context.DonGiaThuocs.Add(donGiaThuoc);
            await _context.SaveChangesAsync();

            // Chuyển hướng về trang danh sách dịch vụ hoặc chi tiết dịch vụ
            return RedirectToAction("Details", "Thuocs", new { id = donGiaThuoc.MaThuoc });
        }

    }
}
