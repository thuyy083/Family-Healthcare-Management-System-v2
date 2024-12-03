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
    public class DichVuYTesController : Controller
    {
        private readonly DataContext _context;

        public DichVuYTesController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/DichVuYTes
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            int pageSize = 4; // Kích thước trang
            int pageNumber = (page ?? 1); // Số trang hiện tại

            // Tạo câu truy vấn
            var listDVQuery = _context.DichVuYTes
                .Where(dv => !dv.IsDelete)
                .AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                listDVQuery = listDVQuery.Where(s => s.TenDichVu.Contains(searchString));
            }

            // Lấy danh sách dịch vụ cùng với đơn giá gần nhất
            var paginatedList = await listDVQuery
                .Select(dichVu => new
                {
                    DichVu = dichVu,
                    DonGiaCuoi = _context.DonGiaDichVus
                        .Where(dg => dg.MaDichVu == dichVu.MaDichVu)
                        .OrderByDescending(dg => dg.ThoiDiem)
                        .FirstOrDefault()
                })
                .OrderBy(s => s.DichVu.TenDichVu)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Lấy thông tin khuyến mãi
            var today = DateTime.Today;
            var khuyenMaiList = await _context.ChuongTrinhKhuyenMais
                .Where(km => km.NgayBatDauKm <= today && km.NgayKetThucKm >= today)
                .Select(km => new
                {
                    km.MaKhuyenMai,
                    km.TenKhuyenMai,
                    km.PhanTramGiamGia,
                    km.ChiTietKhuyenMais
                })
                .ToListAsync();


            // Tạo danh sách để hiển thị trong View
            var viewModel = paginatedList.Select(x =>
            {
                var khuyenMaiHienHanh = khuyenMaiList
                    .Where(km => km.ChiTietKhuyenMais.Any(ct => ct.MaDichVu == x.DichVu.MaDichVu))
                    .OrderByDescending(km => km.PhanTramGiamGia)
                    .FirstOrDefault();

                decimal? donGiaKhuyenMai = null;

                // Tính giá khuyến mãi nếu có khuyến mãi hiện hành
                if (khuyenMaiHienHanh != null && x.DonGiaCuoi != null)
                {
                    // Kiểm tra và lấy giá từ DonGiaCuoi
                    decimal donGia = x.DonGiaCuoi != null ? x.DonGiaCuoi.DonGiaDv : 0; // Đảm bảo x.DonGiaCuoi không null
                    decimal phanTramGiamGia = (decimal)khuyenMaiHienHanh.PhanTramGiamGia; // Chuyển đổi sang decimal

                    donGiaKhuyenMai = donGia * (1 - (phanTramGiamGia / 100));
                }

                return new DichVuViewModel
                {
                    MaDichVu = x.DichVu.MaDichVu,
                    TenDichVu = x.DichVu.TenDichVu,
                    MoTa = x.DichVu.MoTa,
                    DonViTinh = x.DichVu.DonViTinh,
                    DonGiaDv = x.DonGiaCuoi != null ? x.DonGiaCuoi.DonGiaDv : 0, // Hoặc giá trị mặc định nào đó
                    DonGiaKhuyenMai = donGiaKhuyenMai // Đơn giá khuyến mãi
                };
            }).ToList();

            // Lấy tổng số trang
            int totalItems = await listDVQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString; // Giữ lại giá trị tìm kiếm

            // Đánh số thứ tự cho dữ liệu
            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var dichVu = await _context.DichVuYTes.FindAsync(id);
            if (dichVu == null)
            {
                TempData["ErrorMessage"] = "Dịch vụ y tế không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            dichVu.IsDelete = true; // Đặt trạng thái là ẩn
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Dịch vụ y tế đã được ẩn.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> HiddenServices(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var hiddenDichVuQuery = _context.DichVuYTes
                .Where(dv => dv.IsDelete) // Lấy dịch vụ bị ẩn
                .OrderBy(dv => dv.TenDichVu);

            int totalItems = await hiddenDichVuQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;

            var paginatedList = await hiddenDichVuQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unhide(int id)
        {
            var dichVu = await _context.DichVuYTes.FindAsync(id);
            if (dichVu == null)
            {
                TempData["ErrorMessage"] = "Dịch vụ y tế không tồn tại.";
                return RedirectToAction(nameof(HiddenServices));
            }

            dichVu.IsDelete = false; // Hiển thị lại
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Dịch vụ y tế đã được hiển thị lại.";
            return RedirectToAction(nameof(HiddenServices));
        }


        // GET: Admin/DichVuYTes/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            var dichVuYTe = _context.DichVuYTes
                .Include(dv => dv.DonGiaDichVus) 
                .FirstOrDefault(dv => dv.MaDichVu == id);

            //.FirstOrDefault(dv => dv.MaDichVu == id && dv.DaXoa == false); // Chỉ hiển thị dịch vụ chưa bị xóa mềm

            if (dichVuYTe == null)
            {
                return NotFound();
            }

            return View(dichVuYTe);
        }
        // GET: Admin/DichVuYTes/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/DichVuYTes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenDichVu,MoTa,DonViTinh")] DichVuYTe dichVuYTe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dichVuYTe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = dichVuYTe.MaDichVu });
            }
            return View(dichVuYTe);
        }

        // GET: Admin/DichVuYTes/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DichVuYTes == null)
            {
                return NotFound();
            }

            var dichVuYTe = await _context.DichVuYTes.FindAsync(id);
            if (dichVuYTe == null)
            {
                return NotFound();
            }
            return View(dichVuYTe);
        }

        // POST: Admin/DichVuYTes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDichVu,TenDichVu,MoTa,DonViTinh")] DichVuYTe dichVuYTe)
        {
            if (id != dichVuYTe.MaDichVu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dichVuYTe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DichVuYTeExists(dichVuYTe.MaDichVu))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = dichVuYTe.MaDichVu });
            }
            return View(dichVuYTe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var dichVuYTe = await _context.DichVuYTes.FindAsync(id);
            if (dichVuYTe == null)
            {
                return NotFound();
            }

            _context.DichVuYTes.Remove(dichVuYTe);
            await _context.SaveChangesAsync();

            return Ok(); // Trả về 200 OK nếu thành công
        }


        private bool DichVuYTeExists(int id)
        {
          return (_context.DichVuYTes?.Any(e => e.MaDichVu == id)).GetValueOrDefault();
        }

        /// GET: Tạo view để nhập đơn giá mới
        [HttpGet]
        public IActionResult CreateDonGia(int id)
        {
            var model = new DonGiaDichVu
            {
                MaDichVu = id,
                ThoiDiem = DateTime.Now // Gán thời điểm hiện tại khi tạo đơn giá mới
            };
            return View(model);
        }



        // POST: Thêm đơn giá mới cho dịch vụ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDonGia(DonGiaDichVu donGiaDichVu)
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
                return View(donGiaDichVu);
            }

            // Kiểm tra xem dịch vụ có tồn tại không
            var dichVu = await _context.DichVuYTes.FindAsync(donGiaDichVu.MaDichVu);
            if (dichVu == null)
            {
                return NotFound("Dịch vụ không tồn tại.");
            }

            // Gán dịch vụ cho thuộc tính điều hướng
            donGiaDichVu.MaDichVuNavigation = dichVu;

            // Thêm đơn giá vào cơ sở dữ liệu
            _context.DonGiaDichVus.Add(donGiaDichVu);
            await _context.SaveChangesAsync();

            // Chuyển hướng về trang danh sách dịch vụ hoặc chi tiết dịch vụ
            return RedirectToAction("Details", "DichVuYTes", new { id = donGiaDichVu.MaDichVu });
        }


    }
}
