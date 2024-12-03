using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HTQL_DichVuYTeGiaDinh.ViewModels;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using DinkToPdf.Contracts;

namespace HTQL_DichVuYTeGiaDinh.Areas.Staff.Controllers
{
    [Area("Staff")]

    public class PhieuThuController : Controller
    {
        private readonly DataContext _context;
        private readonly IConverter _converter;

        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public PhieuThuController(
            DataContext context,
            IConverter converter,
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider)
        {
            _context = context;
            _converter = converter;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
        }
        public async Task<IActionResult> DSDonThuoc(string searchString, int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var donThuocQuery = _context.DonThuocs
                .Include(dt => dt.MaHoSoNavigation) 
                .ThenInclude(hs => hs.MaTkNavigation) 
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                int searchMaDonThuoc;
                if (int.TryParse(searchString, out searchMaDonThuoc))
                {
                    donThuocQuery = donThuocQuery.Where(dt => dt.MaDonThuoc == searchMaDonThuoc);
                }
            }

            int totalItems = await donThuocQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString;

            var paginatedList = await donThuocQuery
                .OrderByDescending(dt => dt.NgayKeDon) 
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize) 
                .ToListAsync();

            ViewBag.StartingIndex = (pageNumber - 1) * pageSize + 1;

            return View(paginatedList);
        }

        // GET: LapPhieuThu
        [HttpGet]
        public async Task<IActionResult> LapPhieuThu(int maDonThuoc)
        {
            var donThuoc = await _context.DonThuocs
                .Include(dt => dt.ChiTietDonThuocs)
                .ThenInclude(ct => ct.MaThuocNavigation)
                .FirstOrDefaultAsync(dt => dt.MaDonThuoc == maDonThuoc);

            if (donThuoc == null)
            {
                return NotFound();
            }

            decimal tongTien = 0;

            foreach (var chiTiet in donThuoc.ChiTietDonThuocs)
            {
                var donGiaThuoc = await _context.DonGiaThuocs
                    .Where(dg => dg.MaThuoc == chiTiet.MaThuoc)
                    .OrderByDescending(dg => dg.ThoiDiem) 
                    .FirstOrDefaultAsync();

                if (donGiaThuoc != null)
                {
                    tongTien += chiTiet.SoLuong * donGiaThuoc.DonGiaThuoc1;
                }
            }

            var viewModel = new LapPhieuThuViewModel
            {
                MaDonThuoc = maDonThuoc,
                DonThuoc = donThuoc,
                TongTien = tongTien
            };

            return View(viewModel); 
        }

        // POST: LapPhieuThu
        [HttpPost]
        public async Task<IActionResult> LapPhieuThuConfirmed(int maDonThuoc)
        {
            var maTk = HttpContext.Session.GetInt32("MaTk");
            if (maTk == null)
            {
                return RedirectToAction("Login", "Account"); 
            }

            var donThuoc = await _context.DonThuocs
                .Include(dt => dt.ChiTietDonThuocs)
                .ThenInclude(ct => ct.MaThuocNavigation)
                .FirstOrDefaultAsync(dt => dt.MaDonThuoc == maDonThuoc);

            if (donThuoc == null)
            {
                return NotFound();
            }

            // Kiểm tra số lượng thuốc trong kho
            foreach (var chiTiet in donThuoc.ChiTietDonThuocs)
            {
                var thuoc = chiTiet.MaThuocNavigation;
                if (thuoc == null || thuoc.SoLuong < chiTiet.SoLuong)
                {
                    TempData["ErrorMessage"] = $"Không đủ số lượng thuốc '{thuoc?.TenThuoc}'.";
                    return RedirectToAction("LapPhieuThu", new { maDonThuoc });
                }
            }


            decimal tongTien = 0;

            foreach (var chiTiet in donThuoc.ChiTietDonThuocs)
            {
                var thuoc = chiTiet.MaThuocNavigation;
                var donGiaThuoc = await _context.DonGiaThuocs
                    .Where(dg => dg.MaThuoc == chiTiet.MaThuoc)
                    .OrderByDescending(dg => dg.ThoiDiem) 
                    .FirstOrDefaultAsync();

                if (thuoc != null && donGiaThuoc != null)
                {
                    tongTien += chiTiet.SoLuong * donGiaThuoc.DonGiaThuoc1;
                    thuoc.SoLuong -= chiTiet.SoLuong;
                }
            }

            var phieuThu = new PhieuThuTienThuoc
            {
                MaDonThuoc = maDonThuoc,
                MaTk = maTk.Value,
                NgayLapPhieuThu = DateTime.Now,
                TongTien = tongTien
            };

            _context.PhieuThuTienThuocs.Add(phieuThu);
            var dThuoc = await _context.DonThuocs.FindAsync(maDonThuoc);
            if (dThuoc != null)
            {
                donThuoc.TrangThaiDonThuoc = "Đã phát thuốc"; 
            }
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Lập phiếu thu thành công!";

            return RedirectToAction("DSDonThuoc", "PhieuThu"); 
        }


        [HttpGet]
        public IActionResult XuatPhieuThu(int maDonThuoc)
        {
            var donThuoc = _context.DonThuocs
                .Include(dt => dt.MaHoSoNavigation)
                .ThenInclude(hs => hs.MaTkNavigation)
                .Include(dt => dt.ChiTietDonThuocs)
                .ThenInclude(ct => ct.MaThuocNavigation)
                .ThenInclude(t => t.DonGiaThuocs)
                .FirstOrDefault(dt => dt.MaDonThuoc == maDonThuoc);

            if (donThuoc == null)
            {
                return NotFound("Không tìm thấy đơn thuốc.");
            }

            var maNhanVien = HttpContext.Session.GetInt32("MaTk"); 
            if (maNhanVien == null)
            {
                return Unauthorized("Bạn cần đăng nhập để thực hiện chức năng này.");
            }

            var nhanVien = _context.NhanVienYTes.FirstOrDefault(nv => nv.MaTk == maNhanVien);
            if (nhanVien == null)
            {
                return NotFound("Không tìm thấy thông tin nhân viên.");
            }

            // Tính tổng tiền từ các chi tiết đơn thuốc
            var tongTien = donThuoc.ChiTietDonThuocs.Sum(ct =>
            {
                var donGiaGanNhat = ct.MaThuocNavigation.DonGiaThuocs
                    .OrderByDescending(dg => dg.ThoiDiem)
                    .FirstOrDefault()?.DonGiaThuoc1 ?? 0;
                return ct.SoLuong * donGiaGanNhat;
            });

            // Tạo ViewModel
            var viewModel = new PhieuThuViewModel
            {
                DonThuoc = donThuoc,
                TongTien = tongTien,
                NhanVien = nhanVien
            };

            // Render view thành HTML
            var htmlContent = RenderViewAsString("XuatPhieuThu", viewModel);

            // Tạo PDF
            var doc = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = { new ObjectSettings { HtmlContent = htmlContent } }
            };

            byte[] pdf = _converter.Convert(doc);

            // Trả PDF về cho người dùng
            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-disposition", "inline; filename=PhieuThuTienThuoc.pdf");
            return File(pdf, "application/pdf");
        }

        // Phương thức phụ để render view thành HTML
        private string RenderViewAsString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext).Wait();
                return writer.GetStringBuilder().ToString();
            }
        }



    }
}
