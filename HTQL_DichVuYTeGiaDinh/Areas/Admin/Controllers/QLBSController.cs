using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HTQL_DichVuYTeGiaDinh.Repository;
using X.PagedList;
using HTQL_DichVuYTeGiaDinh.Models;
using System.Linq;
using X.PagedList.Extensions;

namespace HTQL_DichVuYTeGiaDinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QLBSController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;


        public QLBSController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnviroment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET: Admin/QLBS


        public IActionResult getListBS(string searchString, int? page)
        {
            var listBS = from s in _context.BacSis.Include(b => b.MaChuyenKhoaNavigation) select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                listBS = listBS.Where(s => s.HoTen.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;
            listBS = listBS.OrderBy(s => s.HoTen);

            int pageSize = 4;
            int pageNumber = (page ?? 1);

            return View(listBS.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public IActionResult createBS()
        {
            ViewBag.ChuyenKhoa = new SelectList(_context.ChuyenKhoas, "MaChuyenKhoa", "TenChuyenKhoa");
            var model = new BacSi
            {
                NgaySinh = DateTime.Now // Ngày hiện tại
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> createBS(BacSi BS)
        {
            ViewBag.ChuyenKhoa = new SelectList(_context.ChuyenKhoas, "MaChuyenKhoa", "TenChuyenKhoa", BS.MaChuyenKhoa);
            try
            {
                if (ModelState.IsValid)
                {
                    var existingTaiKhoan = await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.SoCccd == BS.SoCccd);
                    if (existingTaiKhoan != null)
                    {
                        ModelState.AddModelError("So_CCCD", "Số CCCD đã tồn tại.");
                        return View(BS);
                    }

                    else return View(BS);
                    // Tạo một tài khoản mới
                    TaiKhoan TK = new TaiKhoan
                    {
                        MaQuyen = 2,
                        HoTen = BS.HoTen,
                        NgaySinh = BS.NgaySinh,
                        SoCccd = BS.SoCccd,
                        GioiTinh = BS.GioiTinh,
                        DiaChi = BS.DiaChi,
                        SoDienThoai = BS.SoDienThoai,
                        MatKhau = BS.MatKhau,
                        HinhAnh = BS.HinhAnh
                    };
                    _context.Add(TK);
                    await _context.SaveChangesAsync();

                    int newMaTK = TK.MaTk;

                    // Tạo một bác sĩ mới
                    BacSi BacSi = new BacSi
                    {
                        MaTk = newMaTK,
                        MaQuyen = 2,
                        HoTen = BS.HoTen,
                        NgaySinh = BS.NgaySinh,
                        SoCccd = BS.SoCccd,
                        GioiTinh = BS.GioiTinh,
                        DiaChi = BS.DiaChi,
                        SoDienThoai = BS.SoDienThoai,
                        MatKhau = BS.MatKhau,
                        MaChuyenKhoa = BS.MaChuyenKhoa,
                        TrinhDoHocVan = BS.TrinhDoHocVan,
                        HinhAnh = BS.HinhAnh
                    };
                    _context.Add(BacSi);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("getListBS");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Lỗi lưu dữ liệu");
            }

            return View(BS);

        }
    }
}
