using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using HTQL_DichVuYTeGiaDinh.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HTQL_DichVuYTeGiaDinh.Areas.Doctor.Controllers
{
    [Area("Doctor")]

    public class HoSoYTeController : Controller
    {
        private readonly DataContext _context;
        private readonly IConverter _converter;
        private readonly EmailService _emailService;


        //public HoSoYTeController(DataContext context, IConverter converter)
        //{
        //    _context = context;
        //    _converter = converter;
        //}


        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public HoSoYTeController(
            DataContext context,
            IConverter converter,
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            EmailService emailService)
        {
            _context = context;
            _converter = converter;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _emailService = emailService;
        }

        // GET: Doctor/HoSoYTe/Create
        [HttpGet]
        public IActionResult Create(int maTk)
        {
            // Tạo đối tượng HoSoYTe mới và thiết lập MaTk là mã tài khoản của bệnh nhân
            var hoSoYTe = new HoSoYTe
            {
                MaTk = maTk,
                NgayKhamBenh = DateTime.Now // Ngày khám bệnh là ngày hiện tại
            };

            return View(hoSoYTe);
        }

        // POST: Doctor/HoSoYTe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HoSoYTe hoSoYTe)
        {
            if (ModelState.IsValid)
            {
                // Lấy mã tài khoản bác sĩ từ session
                var bacMaTk = HttpContext.Session.GetInt32("MaTk");
                if (bacMaTk == null)
                {
                    return RedirectToAction("Index", "BenhNhan");
                }

                // Thiết lập BacMaTk và NgayKhamBenh cho hồ sơ y tế
                hoSoYTe.BacMaTk = bacMaTk.Value;
                hoSoYTe.NgayKhamBenh = DateTime.Now;

                // Thêm hồ sơ y tế mới vào cơ sở dữ liệu
                _context.HoSoYTes.Add(hoSoYTe);
                await _context.SaveChangesAsync();

                // Chuyển hướng về trang chi tiết bệnh nhân
                return RedirectToAction("Details", "BenhNhan", new { id = hoSoYTe.MaTk });
            }

            // Nếu có lỗi trong quá trình xác thực dữ liệu, trả lại view để sửa lỗi
            return View(hoSoYTe);
        }

        public async Task<IActionResult> Index(int maTk, int? month, int? year)
        {
            // Lấy tất cả hồ sơ y tế của bệnh nhân
            var hoSoYTeList = await _context.HoSoYTes
                .Include(h => h.MaTkNavigation)
                .Where(h => h.MaTk == maTk)
                .ToListAsync();

            // Lọc theo tháng và năm nếu có
            if (month.HasValue && year.HasValue)
            {
                hoSoYTeList = hoSoYTeList
                    .Where(h => h.NgayKhamBenh.Month == month && h.NgayKhamBenh.Year == year)
                    .ToList();
            }

            // Nhóm theo tháng và năm
            var groupedHoSoYTe = hoSoYTeList
                .GroupBy(h => new
                {
                    Month = h.NgayKhamBenh.Month,
                    Year = h.NgayKhamBenh.Year
                })
                .Select(g => new
                {
                    g.Key.Month,
                    g.Key.Year,
                    HoSoYTes = g.ToList() // Lấy danh sách hồ sơ y tế trong nhóm
                })
                .OrderByDescending(g => g.Year).ThenByDescending(g => g.Month) // Sắp xếp theo năm và tháng
                .ToList();

            // Trả về View
            ViewBag.MaTk = maTk; // Truyền mã tài khoản để sử dụng trong view
            return View(groupedHoSoYTe);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Lấy hồ sơ y tế theo id cùng với kết quả cận lâm sàng, đơn thuốc và chi tiết đơn thuốc
            var hoSoYTe = await _context.HoSoYTes
                .Include(h => h.BacMaTkNavigation)
                .Include(h => h.ChiTietKetQuaCanLamSangs)
                    .ThenInclude(ck => ck.MaPhuongPhapNavigation) // Lấy thông tin phương pháp cận lâm sàng
                .Include(h => h.DonThuocs)
                    .ThenInclude(dt => dt.ChiTietDonThuocs) // Lấy chi tiết đơn thuốc
                    .ThenInclude(ct => ct.MaThuocNavigation) // Lấy thông tin thuốc
                .FirstOrDefaultAsync(h => h.MaHoSo == id);

            if (hoSoYTe == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu đến view
            return View(hoSoYTe);
        }

        [HttpGet]
        public async Task<IActionResult> AddCanLamSang(int hoSoId)
        {
            // Lấy danh sách các phương pháp cận lâm sàng để bác sĩ chọn
            var phuongPhapCanLamSangs = await _context.PhuongPhapCanLamSangs.ToListAsync();

            // Tạo view model
            var viewModel = new AddCanLamSangViewModel
            {
                HoSoId = hoSoId,
                PhuongPhapCanLamSangs = phuongPhapCanLamSangs
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddCanLamSang(AddCanLamSangViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Nếu model không hợp lệ, tải lại danh sách các phương pháp cận lâm sàng để hiển thị trong dropdown
                viewModel.PhuongPhapCanLamSangs = await _context.PhuongPhapCanLamSangs.ToListAsync();
                return View(viewModel);
            }

            string newFileName = null;
            if (viewModel.HinhAnhKetQua != null && viewModel.HinhAnhKetQua.Length > 0)
            {
                // Lấy phần mở rộng của file
                var extension = Path.GetExtension(viewModel.HinhAnhKetQua.FileName).ToLower();

                // Các phần mở rộng file được cho phép
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                // Kiểm tra xem phần mở rộng có hợp lệ không
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("HinhAnhKetQua", "Chỉ chấp nhận các định dạng file: .jpg, .jpeg, .png, .gif");
                    viewModel.PhuongPhapCanLamSangs = await _context.PhuongPhapCanLamSangs.ToListAsync();
                    return View(viewModel);
                }

                // Tạo tên file duy nhất bằng cách sử dụng GUID và phần mở rộng gốc của file
                newFileName = Guid.NewGuid().ToString() + extension;

                // Đường dẫn nơi file sẽ được lưu
                var imagePath = Path.Combine("wwwroot/images/kqcanlamsang", newFileName);

                // Lưu file vào thư mục wwwroot/images/canlamsang
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await viewModel.HinhAnhKetQua.CopyToAsync(stream);
                }
                // Tạo đối tượng ChiTietKetQuaCanLamSang mới để lưu vào cơ sở dữ liệu
                var chiTietKetQua = new ChiTietKetQuaCanLamSang
                {
                    MaHoSo = viewModel.HoSoId,
                    MaPhuongPhap = viewModel.SelectedPhuongPhap,
                    HinhAnhKetQua = newFileName // Lưu tên file hình ảnh đã đổi tên
                };
                // Thêm vào cơ sở dữ liệu
                _context.ChiTietKetQuaCanLamSangs.Add(chiTietKetQua);
                await _context.SaveChangesAsync();

                // Chuyển hướng về trang chi tiết hồ sơ y tế sau khi thêm thành công
                return RedirectToAction("Details", new { id = viewModel.HoSoId });

            }


            ModelState.AddModelError("HinhAnh", "Vui lòng chọn file hình ảnh.");
            return View(viewModel);
            
        }


        // Action để hiển thị form kê đơn thuốc
        [HttpGet]
        public async Task<IActionResult> AddDonThuoc(int hoSoId)
        {
            var viewModel = new AddDonThuocViewModel
            {
                MaHoSo = hoSoId,
                NgayKeDon = DateTime.Now,
                TrangThaiDonThuoc = "Chưa phát thuốc",
                DanhSachThuoc = await _context.Thuocs.ToListAsync() 
            };
            ViewBag.HuongDanSuDungSuggestions = await _context.ChiTietDonThuocs
                   .Where(ct => !string.IsNullOrEmpty(ct.HuongDanSuDung))
                   .Select(ct => ct.HuongDanSuDung)
                   .Distinct()
                   .ToListAsync();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddDonThuoc(AddDonThuocViewModel viewModel)
        {
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                // Nếu model không hợp lệ, lấy lại danh sách thuốc và trả về view
                viewModel.DanhSachThuoc = await _context.Thuocs.ToListAsync();
                return View(viewModel);
            }

            bool hasError = false; // Biến đánh dấu nếu có lỗi

            // Kiểm tra từng chi tiết đơn thuốc
            foreach (var chiTiet in viewModel.ChiTietDonThuocs)
            {
                if (chiTiet.MaThuoc > 0 && chiTiet.SoLuong > 0)
                {
                    // Kiểm tra số lượng thuốc trong kho
                    var thuoc = await _context.Thuocs.FindAsync(chiTiet.MaThuoc);
                    if (thuoc == null)
                    {
                        ModelState.AddModelError("ChiTietDonThuoc", "Thuốc không tồn tại.");
                        hasError = true; // Đánh dấu có lỗi
                        continue;
                    }

                    if (thuoc.SoLuong < chiTiet.SoLuong)
                    {
                        ModelState.AddModelError("ChiTietDonThuoc", $"Không đủ thuốc: {thuoc.TenThuoc} chỉ còn {thuoc.SoLuong} trong kho.");
                        hasError = true; // Đánh dấu có lỗi
                        continue;
                    }
                }
            }

            if (hasError)
            {
                // Nếu có lỗi, lấy lại danh sách thuốc và trả về view
                viewModel.DanhSachThuoc = await _context.Thuocs.ToListAsync();
                return View(viewModel);
            }

            // Tạo mới đơn thuốc
            var donThuoc = new DonThuoc
            {
                MaHoSo = viewModel.MaHoSo,
                NgayKeDon = DateTime.Now,
                TrangThaiDonThuoc = "Chưa phát thuốc", // Trạng thái mặc định
                GhiChuDonThuoc = viewModel.GhiChuDonThuoc
            };

            // Thêm đơn thuốc vào cơ sở dữ liệu
            _context.DonThuocs.Add(donThuoc);
            await _context.SaveChangesAsync(); // Lưu đơn thuốc để lấy mã đơn thuốc

            // Lặp qua từng chi tiết đơn thuốc và thêm vào cơ sở dữ liệu
            foreach (var chiTiet in viewModel.ChiTietDonThuocs)
            {
                if (chiTiet.MaThuoc > 0 && chiTiet.SoLuong > 0)
                {
                    var thuoc = await _context.Thuocs.FindAsync(chiTiet.MaThuoc);
                    if (thuoc != null)
                    {
                        var chiTietDonThuoc = new ChiTietDonThuoc
                        {
                            MaDonThuoc = donThuoc.MaDonThuoc,
                            MaThuoc = chiTiet.MaThuoc,
                            SoLuong = chiTiet.SoLuong,
                            HuongDanSuDung = chiTiet.HuongDanSuDung
                        };

                        _context.ChiTietDonThuocs.Add(chiTietDonThuoc);
                        //thuoc.SoLuong -= chiTiet.SoLuong; // Giảm số lượng thuốc trong kho
                    }
                }
            }

            // Lưu các chi tiết đơn thuốc vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Gửi email cho khách hàng
            var hoSo = await _context.HoSoYTes
                .Include(h => h.MaTkNavigation) // Lấy thông tin khách hàng
                .FirstOrDefaultAsync(h => h.MaHoSo == viewModel.MaHoSo);

            if (hoSo?.MaTkNavigation != null)
            {
                // Lấy email và thông tin khách hàng
                string recipientEmail = hoSo.MaTkNavigation.DiaChiEmail;
                string customerName = hoSo.MaTkNavigation.HoTen;

                // Chuẩn bị nội dung email
                string subject = "Thông tin đơn thuốc của bạn";
                string body = GenerateEmailBody(customerName, hoSo, donThuoc);

                // Gửi email
                await _emailService.SendEmailAsync(recipientEmail, subject, body);
            }

            // Chuyển hướng đến một trang khác hoặc hiển thị thông báo thành công
            return RedirectToAction("Details", new { id = viewModel.MaHoSo });
        }

        // Hàm tạo nội dung email
        private string GenerateEmailBody(string customerName, HoSoYTe hoSo, DonThuoc donThuoc)
        {
            var body = $@"
        <h3>Xin chào {customerName},</h3>
        <p>Bạn đã được kê đơn thuốc mới vào ngày {donThuoc.NgayKeDon:dd/MM/yyyy}.</p>
        <p><strong>Chẩn đoán:</strong> {hoSo.ChanDoan}</p>
        <p><strong>Ghi chú:</strong> {donThuoc.GhiChuDonThuoc ?? "Không có"}</p>
        <h4>Chi tiết đơn thuốc:</h4>
        <ul>";

            foreach (var chiTiet in donThuoc.ChiTietDonThuocs)
            {
                var thuoc = _context.Thuocs.FirstOrDefault(t => t.MaThuoc == chiTiet.MaThuoc);
                if (thuoc != null)
                {
                    body += $"<li>{thuoc.TenThuoc} - Số lượng: {chiTiet.SoLuong} - Hướng dẫn: {chiTiet.HuongDanSuDung}</li>";
                }
            }

            body += "</ul><p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>";
            return body;
        }



        //sử dụng thư viện DinkToPdf với wkhtmltox.dll

        public IActionResult XuatDonThuocPdf(int maHoSo)
        {

            try
            {
                var context = new CustomAssemblyLoadContext();
                context.LoadUnmanagedLibrary(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libwkhtmltox.dll"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading library: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }

            // Lấy dữ liệu từ cơ sở dữ liệu theo `maHoSo`
            var hoSo = _context.HoSoYTes
                .Include(h => h.MaTkNavigation)
                .Include(h => h.DonThuocs)
                    .ThenInclude(d => d.ChiTietDonThuocs)
                        .ThenInclude(c => c.MaThuocNavigation)
                            .ThenInclude(t => t.MaDvtNavigation)
                .FirstOrDefault(h => h.MaHoSo == maHoSo);

            if (hoSo == null) return NotFound("Không tìm thấy hồ sơ y tế");

            // Render view thành HTML
            var htmlContent = RenderViewAsString("XuatDonThuocPdf", hoSo);

            // Cấu hình cho DinkToPdf để tạo PDF từ HTML
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects = { new ObjectSettings { HtmlContent = htmlContent } }
            };

            byte[] pdf = _converter.Convert(doc);

            // Trả PDF về cho người dùng
            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-disposition", "inline; filename=DonThuoc.pdf");
            return File(pdf, "application/pdf");
        }

        // Phương thức phụ để render view thành chuỗi HTML
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
        /**
        public IActionResult XuatDonThuocPdf(int maHoSo)
        {
            // Lấy dữ liệu đơn thuốc từ hồ sơ y tế
            var hoSoYTe = _context.HoSoYTes
                .Include(h => h.MaTkNavigation)   // Lấy thông tin khách hàng
                .Include(h => h.BacMaTkNavigation)  // Lấy thông tin bác sĩ
                .Include(h => h.DonThuocs)         // Lấy thông tin đơn thuốc
                    .ThenInclude(dt => dt.ChiTietDonThuocs)
                    .ThenInclude(ct => ct.MaThuocNavigation)
                    .ThenInclude(t => t.MaDvtNavigation) // Lấy thông tin đơn vị tính của thuốc
                .FirstOrDefault(h => h.MaHoSo == maHoSo);

            if (hoSoYTe == null) return NotFound();

            using (var stream = new MemoryStream())
            {
                // Tạo tài liệu PDF
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Tiêu đề
                document.Add(new Paragraph("Đơn Thuốc").SetFontSize(18).SetBold());
                document.Add(new Paragraph($"Họ Tên Bệnh Nhân: {hoSoYTe.MaTkNavigation.HoTen}"));
                document.Add(new Paragraph($"Ngày Sinh: {hoSoYTe.MaTkNavigation.NgaySinh.ToString("dd/MM/yyyy")}"));
                document.Add(new Paragraph($"Giới Tính: {hoSoYTe.MaTkNavigation.GioiTinh}"));
                document.Add(new Paragraph($"Địa Chỉ: {hoSoYTe.MaTkNavigation.DiaChi}"));
                document.Add(new Paragraph($"Chẩn Đoán: {hoSoYTe.ChanDoan}"));
                document.Add(new Paragraph($"Ngày Khám: {hoSoYTe.NgayKhamBenh.ToString("dd/MM/yyyy")}"));
                document.Add(new Paragraph($"Ngày Tái Khám: {(hoSoYTe.NgayTaiKham.HasValue ? hoSoYTe.NgayTaiKham.Value.ToString("dd/MM/yyyy") : "Không có")}"));
                document.Add(new Paragraph($"Thông Tin Khác: {hoSoYTe.ThongTinKhac ?? "Không có"}"));
                document.Add(new Paragraph("----------------------------"));

                // Thông tin đơn thuốc
                foreach (var donThuoc in hoSoYTe.DonThuocs)
                {
                    document.Add(new Paragraph($"Ngày Kê Đơn: {donThuoc.NgayKeDon.ToString("dd/MM/yyyy")}"));
                    foreach (var chiTiet in donThuoc.ChiTietDonThuocs)
                    {
                        document.Add(new Paragraph($"- {chiTiet.MaThuocNavigation.TenThuoc} ({chiTiet.SoLuong} {chiTiet.MaThuocNavigation.MaDvtNavigation?.TenDvt}) - {chiTiet.HuongDanSuDung}"));
                    }
                    document.Add(new Paragraph("----------------------------"));
                }

                document.Close();

                return File(stream.ToArray(), "application/pdf", "DonThuoc.pdf");
            }
        }

        **/
    }
}
