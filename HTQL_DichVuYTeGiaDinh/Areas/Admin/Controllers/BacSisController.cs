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
    public class BacSisController : Controller
    {
        private readonly DataContext _context;

        public BacSisController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/BacSis
        public async Task<IActionResult> Index(string searchString, int? chuyenKhoaId, int? page)
        {
            int pageSize = 4; 
            int pageNumber = (page ?? 1);

            
            var chuyenKhoas = await _context.ChuyenKhoas.ToListAsync();
            ViewBag.ChuyenKhoaId = new SelectList(chuyenKhoas, "MaChuyenKhoa", "TenChuyenKhoa", chuyenKhoaId);

            
            var bacSisQuery = _context.BacSis
                .Include(b => b.MaChuyenKhoaNavigation)
                .Where(b => b.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bacSisQuery = bacSisQuery.Where(b => b.HoTen.Contains(searchString));
            }

            if (chuyenKhoaId.HasValue && chuyenKhoaId > 0)
            {
                bacSisQuery = bacSisQuery.Where(b => b.MaChuyenKhoa == chuyenKhoaId);
            }

            int totalItems = await bacSisQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString;

            var paginatedList = await bacSisQuery
                .OrderBy(b => b.HoTen)
                .Skip((pageNumber - 1) * pageSize) // Bỏ qua các mục trước đó
                .Take(pageSize)
                .ToListAsync();

            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }

        public async Task<IActionResult> HiddenDoctors(string searchString, int? chuyenKhoaId, int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var chuyenKhoas = await _context.ChuyenKhoas.ToListAsync();
            ViewBag.ChuyenKhoaId = new SelectList(chuyenKhoas, "MaChuyenKhoa", "TenChuyenKhoa", chuyenKhoaId);

            var bacSisQuery = _context.BacSis
                .Include(b => b.MaChuyenKhoaNavigation)
                .Where(b => b.IsDeleted == true) 
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bacSisQuery = bacSisQuery.Where(b => b.HoTen.Contains(searchString));
            }

            if (chuyenKhoaId.HasValue && chuyenKhoaId > 0)
            {
                bacSisQuery = bacSisQuery.Where(b => b.MaChuyenKhoa == chuyenKhoaId);
            }

            int totalItems = await bacSisQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString;

            var paginatedList = await bacSisQuery
                .OrderBy(b => b.HoTen)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(int id)
        {
            // Tìm bác sĩ theo id
            var bacSi = await _context.BacSis.FirstOrDefaultAsync(b => b.MaTk == id && !b.IsDeleted);
            if (bacSi == null)
            {
                return NotFound();
            }

            // Cập nhật isDelete thành true cho bác sĩ
            bacSi.IsDeleted = true;

            // Cập nhật isDelete thành true trong bảng TaiKhoan
            var taiKhoan = await _context.TaiKhoans.FirstOrDefaultAsync(t => t.MaTk == id);
            if (taiKhoan != null)
            {
                taiKhoan.IsDeleted = true;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Bác sĩ đã được ẩn thành công.";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var bacSi = await _context.BacSis.FirstOrDefaultAsync(b => b.MaTk == id && b.IsDeleted);
            if (bacSi == null)
            {
                return NotFound();
            }

            // Cập nhật isDelete thành false
            bacSi.IsDeleted = false;

            // Cập nhật trạng thái trong bảng Tài Khoản
            var taiKhoan = await _context.TaiKhoans.FirstOrDefaultAsync(t => t.MaTk == id);
            if (taiKhoan != null)
            {
                taiKhoan.IsDeleted = false;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Bác sĩ đã được hiện lại thành công.";
            return RedirectToAction(nameof(HiddenDoctors));
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Lấy thông tin bác sĩ
            var bacSi = await _context.BacSis
                .Include(bs => bs.MaChuyenKhoaNavigation)
                .Include(bs => bs.PhanCongBacSis)
                    .ThenInclude(pc => pc.MaHgdNavigation)
                        .ThenInclude(hgd => hgd.KhachHangs)
                .FirstOrDefaultAsync(bs => bs.MaTk == id);

            if (bacSi == null)
            {
                return NotFound("Không tìm thấy thông tin bác sĩ.");
            }

            // Lấy danh sách khách hàng thuộc hộ gia đình mà bác sĩ được phân công
            var hoGiaDinhKhachHangs = bacSi.PhanCongBacSis
                .SelectMany(pc => pc.MaHgdNavigation.KhachHangs)
                .ToList();

            // Truyền dữ liệu vào ViewModel
            var viewModel = new BacSiDetailsViewModel
            {
                BacSi = bacSi,
                KhachHangs = hoGiaDinhKhachHangs
            };

            return View(viewModel);
        }

        // Action để xóa bác sĩ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var bacSi = await _context.BacSis.FindAsync(id);
            if (bacSi == null)
            {
                return NotFound("Không tìm thấy thông tin bác sĩ để xóa.");
            }

            _context.BacSis.Remove(bacSi);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "BacSi");
        }


        // GET: Admin/BacSis/Create
        public async Task<IActionResult> Create()
        {
            var chuyenKhoas = await _context.ChuyenKhoas.ToListAsync();

            var viewModel = new CreateBacSiViewModel
            {
                NgaySinh = DateTime.Now,
                ChuyenKhoas = chuyenKhoas
            };

            return View(viewModel);
        }

        // POST: Admin/BacSis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBacSiViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Lỗi tại trường {state.Key}: {error.ErrorMessage}");
                    }
                }
                viewModel.ChuyenKhoas = await _context.ChuyenKhoas.ToListAsync();
                return View(viewModel);
            }

            // Kiểm tra số điện thoại và số CCCD
            var isPhoneExists = await _context.TaiKhoans.AnyAsync(tk => tk.SoDienThoai == viewModel.SoDienThoai);
            var isCccdExists = await _context.TaiKhoans.AnyAsync(tk => tk.SoCccd == viewModel.SoCccd);

            if (isPhoneExists)
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại đã tồn tại trong hệ thống.");
            }

            if (isCccdExists)
            {
                ModelState.AddModelError("SoCccd", "Số CCCD đã tồn tại trong hệ thống.");
            }

            if (isPhoneExists || isCccdExists)
            {
                viewModel.ChuyenKhoas = await _context.ChuyenKhoas.ToListAsync();
                return View(viewModel);
            }

            if (viewModel.HinhAnh != null && viewModel.HinhAnh.Length > 0)
            {
                var extension = Path.GetExtension(viewModel.HinhAnh.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận các định dạng file: .jpg, .jpeg, .png, .gif");
                    viewModel.ChuyenKhoas = await _context.ChuyenKhoas.ToListAsync();
                    return View(viewModel);
                }

                var newFileName = Guid.NewGuid().ToString() + extension;
                var imagePath = Path.Combine("wwwroot/images/bacsi", newFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await viewModel.HinhAnh.CopyToAsync(stream);
                }

                var taiKhoan = new TaiKhoan
                {
                    HoTen = viewModel.HoTen,
                    NgaySinh = viewModel.NgaySinh,
                    SoCccd = viewModel.SoCccd,
                    GioiTinh = viewModel.GioiTinh,
                    DiaChi = viewModel.DiaChi,
                    SoDienThoai = viewModel.SoDienThoai,
                    MatKhau = viewModel.MatKhau,
                    HinhAnh = newFileName,
                    DiaChiEmail = viewModel.DiaChiEmail,
                    MaQuyen = 2
                };

                _context.TaiKhoans.Add(taiKhoan);
                await _context.SaveChangesAsync();

                var bacSi = new BacSi
                {
                    MaTk = taiKhoan.MaTk,
                    HoTen = viewModel.HoTen,
                    NgaySinh = viewModel.NgaySinh,
                    SoCccd = viewModel.SoCccd,
                    GioiTinh = viewModel.GioiTinh,
                    DiaChi = viewModel.DiaChi,
                    SoDienThoai = viewModel.SoDienThoai,
                    MatKhau = viewModel.MatKhau,
                    HinhAnh = newFileName,
                    DiaChiEmail = viewModel.DiaChiEmail,
                    MaQuyen = 2,
                    MaChuyenKhoa = viewModel.MaChuyenKhoa,
                    TrinhDoHocVan = viewModel.TrinhDoHocVan
                };

                _context.BacSis.Add(bacSi);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("HinhAnh", "Vui lòng chọn file hình ảnh.");
            viewModel.ChuyenKhoas = await _context.ChuyenKhoas.ToListAsync();
            return View(viewModel);
        }


        // GET: BacSi/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var bacSi = _context.BacSis.Find(id);
            if (bacSi == null)
            {
                return NotFound();
            }

            // Chuyển đổi từ BacSi sang ViewModel để hiển thị trong view
            var viewModel = new CreateBacSiViewModel
            {
                MaTk = bacSi.MaTk,
                HoTen = bacSi.HoTen,
                NgaySinh = bacSi.NgaySinh,
                SoCccd = bacSi.SoCccd,
                GioiTinh = bacSi.GioiTinh,
                DiaChi = bacSi.DiaChi,
                SoDienThoai = bacSi.SoDienThoai,
                DiaChiEmail = bacSi.DiaChiEmail,
                TrinhDoHocVan = bacSi.TrinhDoHocVan,
                MaChuyenKhoa = bacSi.MaChuyenKhoa,
                ChuyenKhoas = _context.ChuyenKhoas.ToList() // Lấy danh sách chuyên khoa
            };

            return View(viewModel);
        }

        // POST: Admin/BacSis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CreateBacSiViewModel viewModel)
        {
            if (id != viewModel.MaTk)
            {
                return NotFound();
            }

            // Kiểm tra xem dữ liệu hợp lệ không
            if (ModelState.IsValid)
            {
                // Lấy bản ghi bác sĩ từ cơ sở dữ liệu
                var bacSi = _context.BacSis.Find(id);
                if (bacSi == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin nhưng giữ lại hình ảnh, mật khẩu, mã tài khoản và mã quyền
                bacSi.HoTen = viewModel.HoTen;
                bacSi.NgaySinh = viewModel.NgaySinh;
                bacSi.SoCccd = viewModel.SoCccd;
                bacSi.GioiTinh = viewModel.GioiTinh;
                bacSi.DiaChi = viewModel.DiaChi;
                bacSi.SoDienThoai = viewModel.SoDienThoai;
                bacSi.DiaChiEmail = viewModel.DiaChiEmail;
                bacSi.TrinhDoHocVan = viewModel.TrinhDoHocVan;
                bacSi.MaChuyenKhoa = viewModel.MaChuyenKhoa;

                try
                {
                    // Lưu các thay đổi vào cơ sở dữ liệu
                    _context.Update(bacSi);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.BacSis.Any(e => e.MaTk == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Sau khi cập nhật thành công, chuyển hướng về trang danh sách bác sĩ hoặc trang thông báo thành công
                return RedirectToAction(nameof(Index));
            }

            // Nếu dữ liệu không hợp lệ, load lại danh sách chuyên khoa và trả lại view
            viewModel.ChuyenKhoas = _context.ChuyenKhoas.ToList();

            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Lỗi tại trường {state.Key}: {error.ErrorMessage}");
                }
            }

            return View(viewModel);
        }
        // GET: Admin/BacSis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BacSis == null)
            {
                return NotFound();
            }

            var bacSi = await _context.BacSis
                .Include(b => b.MaChuyenKhoaNavigation)
                .Include(b => b.MaTkNavigation)
                .FirstOrDefaultAsync(m => m.MaTk == id);
            if (bacSi == null)
            {
                return NotFound();
            }

            return View(bacSi);
        }

        // POST: Admin/BacSis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BacSis == null)
            {
                return Problem("Entity set 'DataContext.BacSis'  is null.");
            }
            var bacSi = await _context.BacSis.FindAsync(id);
            if (bacSi != null)
            {
                _context.BacSis.Remove(bacSi);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BacSiExists(int id)
        {
          return (_context.BacSis?.Any(e => e.MaTk == id)).GetValueOrDefault();
        }
    }
}
