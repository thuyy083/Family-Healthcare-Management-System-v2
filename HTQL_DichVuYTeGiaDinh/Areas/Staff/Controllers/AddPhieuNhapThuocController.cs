using HTQL_DichVuYTeGiaDinh.Models;
using HTQL_DichVuYTeGiaDinh.Repository;
using HTQL_DichVuYTeGiaDinh.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTQL_DichVuYTeGiaDinh.Areas.Staff.Controllers
{
    [Area("Staff")]

    public class AddPhieuNhapThuocController : Controller
    {
        private readonly DataContext _context;

        public AddPhieuNhapThuocController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AddPhieuNhapThuoc()
        {
            var viewModel = new AddPhieuNhapThuocViewModel
            {
                NgayLapPhieuNhap = DateTime.Now,
                DanhSachThuoc = await _context.Thuocs.ToListAsync() // Lấy danh sách thuốc để hiển thị
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhieuNhapThuoc(AddPhieuNhapThuocViewModel viewModel)
        {
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                // Nếu model không hợp lệ, lấy lại danh sách thuốc và trả về view
                viewModel.DanhSachThuoc = await _context.Thuocs.ToListAsync();
                return View(viewModel);
            }

            // Lấy mã tài khoản từ session
            var maTk = HttpContext.Session.GetInt32("MaTk");

            var ngayLapPhieuNhap = viewModel.NgayLapPhieuNhap == default(DateTime) ? DateTime.Now : viewModel.NgayLapPhieuNhap;
            Console.WriteLine($"NgayLapPhieuNhap: {ngayLapPhieuNhap}");

            // Tạo mới phiếu nhập thuốc
            var phieuNhapThuoc = new PhieuNhapThuoc
            {
                MaTk = maTk.Value,
                NgayLapPhieuNhap = ngayLapPhieuNhap,
                TenNhaCungCap = viewModel.TenNhaCungCap,
                TongTienNhap = 0 // Tính sau khi thêm chi tiết
            };

            // Thêm phiếu nhập thuốc vào cơ sở dữ liệu
            _context.PhieuNhapThuocs.Add(phieuNhapThuoc);
            await _context.SaveChangesAsync(); // Lưu phiếu nhập để lấy mã phiếu nhập

            decimal tongTienNhap = 0; // Biến để tính tổng tiền nhập

            // Lặp qua từng chi tiết phiếu nhập và thêm vào cơ sở dữ liệu
            foreach (var chiTiet in viewModel.ChiTietPhieuNhaps)
            {
                if (chiTiet.MaThuoc > 0 && chiTiet.SoLuongNhap > 0 && chiTiet.DonGiaNhap > 0)
                {
                    // Kiểm tra xem thuốc có tồn tại hay không
                    var thuoc = await _context.Thuocs.FindAsync(chiTiet.MaThuoc);
                    if (thuoc == null)
                    {
                        ModelState.AddModelError("ChiTietPhieuNhap", "Thuốc không tồn tại.");
                        continue;
                    }

                    var chiTietPhieuNhap = new ChiTietPhieuNhap
                    {
                        MaPhieuNhap = phieuNhapThuoc.MaPhieuNhap,
                        MaThuoc = chiTiet.MaThuoc,
                        SoLuongNhap = chiTiet.SoLuongNhap,
                        DonGiaNhap = chiTiet.DonGiaNhap
                    };

                    // Thêm chi tiết phiếu nhập vào cơ sở dữ liệu
                    _context.ChiTietPhieuNhaps.Add(chiTietPhieuNhap);

                    // Cập nhật số lượng thuốc trong kho
                    thuoc.SoLuong += chiTiet.SoLuongNhap;

                    // Cộng dồn tổng tiền nhập
                    tongTienNhap += chiTiet.SoLuongNhap * chiTiet.DonGiaNhap;
                }
            }

            // Cập nhật tổng tiền nhập cho phiếu nhập thuốc
            phieuNhapThuoc.TongTienNhap = tongTienNhap;
            _context.PhieuNhapThuocs.Update(phieuNhapThuoc);

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Chuyển hướng đến một trang khác hoặc hiển thị thông báo thành công
            return RedirectToAction("Index", "Thuoc");
        }

    }
}
