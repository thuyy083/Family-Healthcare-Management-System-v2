using System;
using System.Collections.Generic;
using HTQL_DichVuYTeGiaDinh.Models;
using Microsoft.EntityFrameworkCore;

namespace HTQL_DichVuYTeGiaDinh.Repository
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        public virtual DbSet<BacSi> BacSis { get; set; }

        public virtual DbSet<BinhLuan> BinhLuans { get; set; }

        public virtual DbSet<ChiTietDonThuoc> ChiTietDonThuocs { get; set; }

        public virtual DbSet<ChiTietKetQuaCanLamSang> ChiTietKetQuaCanLamSangs { get; set; }

        public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        
        public virtual DbSet<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; }

        public virtual DbSet<ChuongTrinhKhuyenMai> ChuongTrinhKhuyenMais { get; set; }

        public virtual DbSet<ChuyenKhoa> ChuyenKhoas { get; set; }

        public virtual DbSet<DichVuYTe> DichVuYTes { get; set; }

        public virtual DbSet<DonGiaDichVu> DonGiaDichVus { get; set; }

        public virtual DbSet<DonGiaThuoc> DonGiaThuocs { get; set; }

        public virtual DbSet<DonThuoc> DonThuocs { get; set; }

        public virtual DbSet<DonViTinhThuoc> DonViTinhThuocs { get; set; }

        public virtual DbSet<HoGiaDinh> HoGiaDinhs { get; set; }

        public virtual DbSet<HoSoYTe> HoSoYTes { get; set; }

        public virtual DbSet<HoaDon> HoaDons { get; set; }

        public virtual DbSet<KhachHang> KhachHangs { get; set; }

        public virtual DbSet<NhanVienYTe> NhanVienYTes { get; set; }

        public virtual DbSet<PhanCongBacSi> PhanCongBacSis { get; set; }

        public virtual DbSet<PhieuNhapThuoc> PhieuNhapThuocs { get; set; }

        public virtual DbSet<PhieuThuTienThuoc> PhieuThuTienThuocs { get; set; }

        public virtual DbSet<PhuongPhapCanLamSang> PhuongPhapCanLamSangs { get; set; }

        public virtual DbSet<QuyenTruyCap> QuyenTruyCaps { get; set; }

        public virtual DbSet<SuDungDichVu> SuDungDichVus { get; set; }

        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

        public virtual DbSet<Thuoc> Thuocs { get; set; }

        public virtual DbSet<TuVan> TuVans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BacSi>(entity =>
            {
                entity.HasKey(e => e.MaTk);

                entity.ToTable("BAC_SI");

                entity.HasIndex(e => e.MaChuyenKhoa, "THUOC_CHUYEN_KHOA_FK");

                entity.Property(e => e.MaTk)
                    .HasColumnName("MA_TK");
                entity.Property(e => e.DiaChi)
                    .HasMaxLength(1024)
                    .HasColumnName("DIA_CHI");
                entity.Property(e => e.DiaChiEmail)
                    .HasMaxLength(250)
                    .HasColumnName("DIA_CHI_EMAIL");
                entity.Property(e => e.GioiTinh).HasColumnName("GIOI_TINH");
                entity.Property(e => e.HinhAnh)
                    .HasMaxLength(250)
                    .HasColumnName("HINH_ANH");
                entity.Property(e => e.HoTen)
                    .HasMaxLength(100)
                    .HasColumnName("HO_TEN");
                entity.Property(e => e.MaChuyenKhoa).HasColumnName("MA_CHUYEN_KHOA");
                entity.Property(e => e.MaQuyen).HasColumnName("MA_QUYEN");
                entity.Property(e => e.MatKhau)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("MAT_KHAU");
                entity.Property(e => e.NgaySinh).HasColumnName("NGAY_SINH");
                entity.Property(e => e.SoCccd)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SO_CCCD");
                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SO_DIEN_THOAI");
                entity.Property(e => e.TrinhDoHocVan)
                    .HasMaxLength(50)
                    .HasColumnName("TRINH_DO_HOC_VAN");
                entity.Property(e => e.IsDeleted)
                    .HasColumnName("IS_DELETED")
                    .HasDefaultValue(false);
                entity.HasOne(d => d.MaChuyenKhoaNavigation).WithMany(p => p.BacSis)
                    .HasForeignKey(d => d.MaChuyenKhoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BAC_SI_THUOC_CHU_CHUYEN_K");

                entity.HasOne(d => d.MaTkNavigation).WithOne(p => p.BacSi)
                    .HasForeignKey<BacSi>(d => d.MaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BAC_SI_CO_TK2_TAI_KHOA");
            });

            modelBuilder.Entity<BinhLuan>(entity =>
            {
                entity.HasKey(e => e.MaBinhLuan);

                entity.ToTable("BINH_LUAN");

                entity.HasIndex(e => e.MaBinhLuan, "BINH_LUAN_PK").IsUnique();

                entity.HasIndex(e => e.MaDichVu, "CO_BINH_LUAN_FK");

                entity.HasIndex(e => e.MaTk, "VIET_BINH_LUAN_FK");

                entity.Property(e => e.MaBinhLuan)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_BINH_LUAN");
                entity.Property(e => e.DiemDanhGia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("DIEM_DANH_GIA");
                entity.Property(e => e.MaDichVu).HasColumnName("MA_DICH_VU");
                entity.Property(e => e.MaTk).HasColumnName("MA_TK");
                entity.Property(e => e.NoiDungBinhLuan)
                    .HasMaxLength(1000)
                    .HasColumnName("NOI_DUNG_BINH_LUAN");

                entity.Property(e => e.ThoiDiemBinhLuan)
                    .HasColumnType("datetime2(7)")
                    .HasColumnName("THOI_DIEM_BINH_LUAN");

                entity.HasOne(d => d.MaDichVuNavigation).WithMany(p => p.BinhLuans)
                    .HasForeignKey(d => d.MaDichVu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BINH_LUA_CO_BINH_L_DICH_VU_");

                entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.BinhLuans)
                    .HasForeignKey(d => d.MaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BINH_LUA_VIET_BINH_KHACH_HA");
            });

            modelBuilder.Entity<ChiTietDonThuoc>(entity =>
            {
                entity.HasKey(e => new { e.MaDonThuoc, e.MaThuoc });

                entity.ToTable("CHI_TIET_DON_THUOC");

                entity.Property(e => e.MaDonThuoc).HasColumnName("MA_DON_THUOC");
                entity.Property(e => e.MaThuoc).HasColumnName("MA_THUOC");
                entity.Property(e => e.HuongDanSuDung)
                    .HasMaxLength(100)
                    .HasColumnName("HUONG_DAN_SU_DUNG");
                entity.Property(e => e.SoLuong).HasColumnName("SO_LUONG");

                entity.HasOne(d => d.MaDonThuocNavigation).WithMany(p => p.ChiTietDonThuocs)
                    .HasForeignKey(d => d.MaDonThuoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHI_TIET_CO_CHI_TI_DON_THUO");

                entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.ChiTietDonThuocs)
                    .HasForeignKey(d => d.MaThuoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHI_TIET_CO_THUOC_THUOC");
            });

            modelBuilder.Entity<ChiTietKetQuaCanLamSang>(entity =>
            {
                entity.HasKey(e => new { e.MaPhuongPhap, e.MaHoSo }).HasName("PK_CHI_TIET_KET_QUA_CAN_LAM_SA");

                entity.ToTable("CHI_TIET_KET_QUA_CAN_LAM_SANG");

                entity.HasIndex(e => e.MaHoSo, "CO_CHI_TIET_KET_QUA_FK");

                entity.HasIndex(e => e.MaPhuongPhap, "CUA_PHUONG_PHAP_CAN_LAM_SANG_FK");

                entity.Property(e => e.MaPhuongPhap).HasColumnName("MA_PHUONG_PHAP");
                entity.Property(e => e.MaHoSo).HasColumnName("MA_HO_SO");
                entity.Property(e => e.HinhAnhKetQua)
                    .HasMaxLength(100)
                    .HasColumnName("HINH_ANH_KET_QUA");

                entity.HasOne(d => d.MaHoSoNavigation).WithMany(p => p.ChiTietKetQuaCanLamSangs)
                    .HasForeignKey(d => d.MaHoSo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHI_TIET_CO_CHI_TI_HO_SO_Y_");

                entity.HasOne(d => d.MaPhuongPhapNavigation).WithMany(p => p.ChiTietKetQuaCanLamSangs)
                    .HasForeignKey(d => d.MaPhuongPhap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHI_TIET_CUA_PHUON_PHUONG_P");
            });

            modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
            {
                entity.HasKey(e => new { e.MaThuoc, e.MaPhieuNhap });

                entity.ToTable("CHI_TIET_PHIEU_NHAP");

                entity.HasIndex(e => e.MaPhieuNhap, "NHAP_CHO_PHIEU_FK");

                entity.HasIndex(e => e.MaThuoc, "NHAP_THUOC_FK");

                entity.Property(e => e.MaThuoc).HasColumnName("MA_THUOC");
                entity.Property(e => e.MaPhieuNhap).HasColumnName("MA_PHIEU_NHAP");
                entity.Property(e => e.DonGiaNhap)
                    .HasColumnType("decimal(18,2)")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("DON_GIA_NHAP");
                entity.Property(e => e.SoLuongNhap)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SO_LUONG_NHAP");

                entity.HasOne(d => d.MaPhieuNhapNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                    .HasForeignKey(d => d.MaPhieuNhap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHI_TIET_NHAP_CHO__PHIEU_NH");

                entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                    .HasForeignKey(d => d.MaThuoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHI_TIET_NHAP_THUO_THUOC");
            });

            modelBuilder.Entity<ChiTietKhuyenMai>(entity =>
            {
                entity.HasKey(e => new { e.MaKhuyenMai, e.MaDichVu });

                entity.ToTable("CHI_TIET_KHUYEN_MAI");

                entity.HasOne(e => e.MaKhuyenMaiNavigation)
                    .WithMany(p => p.ChiTietKhuyenMais)
                    .HasForeignKey(e => e.MaKhuyenMai)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CHI_TIET_KM_CHO__KM"); ;

                entity.HasOne(e => e.MaDichVuNavigation)
                    .WithMany(p => p.ChiTietKhuyenMais)
                    .HasForeignKey(e => e.MaDichVu)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CHI_TIET_KM_DV"); ;

            });

            modelBuilder.Entity<ChuongTrinhKhuyenMai>(entity =>
            {
                entity.HasKey(e => e.MaKhuyenMai);

                entity.ToTable("CHUONG_TRINH_KHUYEN_MAI");

                entity.HasIndex(e => e.MaKhuyenMai, "CHUONG_TRINH_KHUYEN_MAI_PK").IsUnique();

                entity.Property(e => e.MaKhuyenMai)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_KHUYEN_MAI");
                entity.Property(e => e.MoTa)
                    .HasMaxLength(500)
                    .HasColumnName("MO_TA");
                entity.Property(e => e.NgayBatDauKm).HasColumnName("NGAY_BAT_DAU_KM");
                entity.Property(e => e.NgayKetThucKm).HasColumnName("NGAY_KET_THUC_KM");
                entity.Property(e => e.PhanTramGiamGia).HasColumnName("PHAN_TRAM_GIAM_GIA");
                entity.Property(e => e.TenKhuyenMai)
                    .HasMaxLength(100)
                    .HasColumnName("TEN_KHUYEN_MAI");
            });

            modelBuilder.Entity<ChuyenKhoa>(entity =>
            {
                entity.HasKey(e => e.MaChuyenKhoa);

                entity.ToTable("CHUYEN_KHOA");

                entity.HasIndex(e => e.MaChuyenKhoa, "CHUYEN_KHOA_PK").IsUnique();

                entity.Property(e => e.MaChuyenKhoa)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_CHUYEN_KHOA");
                entity.Property(e => e.TenChuyenKhoa)
                    .HasMaxLength(100)
                    .HasColumnName("TEN_CHUYEN_KHOA");
            });

            modelBuilder.Entity<DichVuYTe>(entity =>
            {
                entity.HasKey(e => e.MaDichVu);

                entity.ToTable("DICH_VU_Y_TE");

                entity.HasIndex(e => e.MaDichVu, "DICH_VU_Y_TE_PK").IsUnique();

                entity.Property(e => e.MaDichVu)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_DICH_VU");
                entity.Property(e => e.DonViTinh)
                    .HasMaxLength(50)
                    .HasColumnName("DON_VI_TINH");
                entity.Property(e => e.MoTa)
                    .HasMaxLength(500)
                    .HasColumnName("MO_TA");
                entity.Property(e => e.TenDichVu)
                    .HasMaxLength(100)
                    .HasColumnName("TEN_DICH_VU");
                entity.Property(e => e.IsDelete)
                    .HasColumnName("IS_DELETE")
                    .HasDefaultValue(false);

            });

            modelBuilder.Entity<DonGiaDichVu>(entity =>
            {
                entity.HasKey(e => new { e.MaDichVu, e.ThoiDiem });

                entity.ToTable("DON_GIA_DICH_VU");

                entity.Property(e => e.MaDichVu)
                    .HasColumnName("MA_DICH_VU");
                entity.Property(e => e.DonGiaDv)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("DON_GIA_DV");
                entity.Property(e => e.ThoiDiem)
                    .HasColumnName("THOI_DIEM");

                entity.HasOne(d => d.MaDichVuNavigation)
                    .WithMany(p => p.DonGiaDichVus) // Với một dịch vụ, có nhiều đơn giá
                    .HasForeignKey(d => d.MaDichVu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DON_GIA__CO_DON_GI_DICH_VU_");
            });

            modelBuilder.Entity<DonGiaThuoc>(entity =>
            {
                entity.HasKey(e => new { e.MaThuoc, e.ThoiDiem });

                entity.ToTable("DON_GIA_THUOC");

                entity.Property(e => e.MaThuoc)
                    .ValueGeneratedNever()
                    .HasColumnName("MA_THUOC");
                entity.Property(e => e.DonGiaThuoc1)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("DON_GIA_THUOC");
                entity.Property(e => e.ThoiDiem)
                    .HasColumnName("THOI_DIEM");

                entity.HasOne(d => d.MaThuocNavigation)
                    .WithMany(p => p.DonGiaThuocs)
                    .HasForeignKey(d => d.MaThuoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DON_GIA__CO_DON_GI_THUOC_");
            });

            modelBuilder.Entity<DonThuoc>(entity =>
            {
                entity.HasKey(e => e.MaDonThuoc);

                entity.ToTable("DON_THUOC");

                entity.HasIndex(e => e.MaHoSo, "CO_DON_THUOC_FK");

                entity.HasIndex(e => e.MaDonThuoc, "DON_THUOC_PK").IsUnique();

                entity.Property(e => e.MaDonThuoc)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_DON_THUOC");
                entity.Property(e => e.GhiChuDonThuoc)
                    .HasMaxLength(500)
                    .HasColumnName("GHI_CHU_DON_THUOC");
                entity.Property(e => e.MaHoSo).HasColumnName("MA_HO_SO");
                entity.Property(e => e.NgayKeDon).HasColumnName("NGAY_KE_DON");
                entity.Property(e => e.TrangThaiDonThuoc)
                    .HasMaxLength(50)
                    .HasColumnName("TRANG_THAI_DON_THUOC");

                entity.HasOne(d => d.MaHoSoNavigation).WithMany(p => p.DonThuocs)
                    .HasForeignKey(d => d.MaHoSo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DON_THUO_CO_DON_TH_HO_SO_Y_");
            });

            modelBuilder.Entity<DonViTinhThuoc>(entity =>
            {
                entity.HasKey(e => e.MaDvt);

                entity.ToTable("DON_VI_TINH_THUOC");

                entity.HasIndex(e => e.MaDvt, "DON_VI_TINH_THUOC_PK").IsUnique();

                entity.Property(e => e.MaDvt)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_DVT");
                entity.Property(e => e.TenDvt)
                    .HasMaxLength(50)
                    .HasColumnName("TEN_DVT");
            });

            modelBuilder.Entity<HoGiaDinh>(entity =>
            {
                entity.HasKey(e => e.MaHgd);

                entity.ToTable("HO_GIA_DINH");

                entity.HasIndex(e => e.MaHgd, "HO_GIA_DINH_PK").IsUnique();

                entity.Property(e => e.MaHgd)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_HGD");
                entity.Property(e => e.ChuHo)
                    .HasMaxLength(100)
                    .HasColumnName("CHU_HO");
                entity.Property(e => e.DiaChiHgd)
                    .HasMaxLength(500)
                    .HasColumnName("DIA_CHI_HGD");
                entity.Property(e => e.SodienthoaiHgd)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SODIENTHOAI_HGD");
            });

            modelBuilder.Entity<HoSoYTe>(entity =>
            {
                entity.HasKey(e => e.MaHoSo);

                entity.ToTable("HO_SO_Y_TE");

                entity.HasIndex(e => e.MaTk, "CO_HO_SO_Y_TE_FK");

                entity.HasIndex(e => e.BacMaTk, "GHI_HO_SO_Y_TE_FK");

                entity.HasIndex(e => e.MaHoSo, "HO_SO_Y_TE_PK").IsUnique();

                entity.Property(e => e.MaHoSo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_HO_SO");
                entity.Property(e => e.BacMaTk).HasColumnName("BAC_MA_TK");
                entity.Property(e => e.ChanDoan)
                    .HasMaxLength(500)
                    .HasColumnName("CHAN_DOAN");
                entity.Property(e => e.MaTk).HasColumnName("MA_TK");
                entity.Property(e => e.NgayKhamBenh).HasColumnName("NGAY_KHAM_BENH");
                entity.Property(e => e.NgayTaiKham).HasColumnName("NGAY_TAI_KHAM");
                entity.Property(e => e.ThongTinKhac)
                    .HasMaxLength(500)
                    .HasColumnName("THONG_TIN_KHAC");

                entity.HasOne(d => d.BacMaTkNavigation).WithMany(p => p.HoSoYTes)
                    .HasForeignKey(d => d.BacMaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HO_SO_Y__GHI_HO_SO_BAC_SI");

                entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.HoSoYTes)
                    .HasForeignKey(d => d.MaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HO_SO_Y__CO_HO_SO__KHACH_HA");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.MaHoaDon);

                entity.ToTable("HOA_DON");

                entity.HasIndex(e => e.MaLanSuDung, "CO_HOA_DON_FK");

                entity.HasIndex(e => e.MaTk, "CO_NVYT_FK");

                entity.HasIndex(e => e.MaHoaDon, "HOA_DON_PK").IsUnique();

                entity.Property(e => e.MaHoaDon)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_HOA_DON");
                entity.Property(e => e.MaLanSuDung).HasColumnName("MA_LAN_SU_DUNG");
                entity.Property(e => e.MaTk).HasColumnName("MA_TK");
                entity.Property(e => e.NgayLapHoaDon)
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("NGAY_LAP_HOA_DON");

                entity.Property(e => e.TongTien)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("TONG_TIEN");

                entity.HasOne(d => d.MaLanSuDungNavigation).WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaLanSuDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HOA_DON_CO_HOA_DO_SU_DUNG_");

                entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HOA_DON_CO_NVYT_NHAN_VIE");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.MaTk);

                entity.ToTable("KHACH_HANG");

                entity.HasIndex(e => e.MaHgd, "THUOC_HGD_FK");

                entity.Property(e => e.MaTk)
                    .ValueGeneratedNever()
                    .HasColumnName("MA_TK");
                entity.Property(e => e.DiaChi)
                    .HasMaxLength(1024)
                    .HasColumnName("DIA_CHI");
                entity.Property(e => e.DiaChiEmail)
                    .HasMaxLength(250)
                    .HasColumnName("DIA_CHI_EMAIL");
                entity.Property(e => e.GioiTinh).HasColumnName("GIOI_TINH");
                entity.Property(e => e.HinhAnh)
                    .HasMaxLength(250)
                    .HasColumnName("HINH_ANH");
                entity.Property(e => e.HoTen)
                    .HasMaxLength(100)
                    .HasColumnName("HO_TEN");
                entity.Property(e => e.MaHgd).HasColumnName("MA_HGD");
                entity.Property(e => e.MaQuyen).HasColumnName("MA_QUYEN");
                entity.Property(e => e.MatKhau)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("MAT_KHAU");
                entity.Property(e => e.MoiQuanHeVoiChuHo)
                    .HasMaxLength(250)
                    .HasColumnName("MOI_QUAN_HE_VOI_CHU_HO");
                entity.Property(e => e.NgaySinh).HasColumnName("NGAY_SINH");
                entity.Property(e => e.SdtNguoiLienHeKhanCap)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SDT_NGUOI_LIEN_HE_KHAN_CAP");
                entity.Property(e => e.SoCccd)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SO_CCCD");
                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SO_DIEN_THOAI");
                entity.Property(e => e.TenNguoiLienHeKhanCap)
                    .HasMaxLength(100)
                    .HasColumnName("TEN_NGUOI_LIEN_HE_KHAN_CAP");
                entity.Property(e => e.IsDeleted)
                    .HasColumnName("IS_DELETED")
                    .HasDefaultValue(false);

                entity.HasOne(d => d.MaHgdNavigation).WithMany(p => p.KhachHangs)
                    .HasForeignKey(d => d.MaHgd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KHACH_HA_THUOC_HGD_HO_GIA_D");

                entity.HasOne(d => d.MaTkNavigation).WithOne(p => p.KhachHang)
                    .HasForeignKey<KhachHang>(d => d.MaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KHACH_HA_CO_TK3_TAI_KHOA");
            });

            modelBuilder.Entity<NhanVienYTe>(entity =>
            {
                entity.HasKey(e => e.MaTk);

                entity.ToTable("NHAN_VIEN_Y_TE");

                entity.Property(e => e.MaTk)
                    .ValueGeneratedNever()
                    .HasColumnName("MA_TK");
                entity.Property(e => e.DiaChi)
                    .HasMaxLength(1024)
                    .HasColumnName("DIA_CHI");
                entity.Property(e => e.DiaChiEmail)
                    .HasMaxLength(250)
                    .HasColumnName("DIA_CHI_EMAIL");
                entity.Property(e => e.GioiTinh).HasColumnName("GIOI_TINH");
                entity.Property(e => e.HinhAnh)
                    .HasMaxLength(250)
                    .HasColumnName("HINH_ANH");
                entity.Property(e => e.HoTen)
                    .HasMaxLength(100)
                    .HasColumnName("HO_TEN");
               
                entity.Property(e => e.MaQuyen).HasColumnName("MA_QUYEN");
                entity.Property(e => e.MatKhau)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("MAT_KHAU");
                entity.Property(e => e.NgaySinh).HasColumnName("NGAY_SINH");
                entity.Property(e => e.SoCccd)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SO_CCCD");
                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SO_DIEN_THOAI");
                entity.Property(e => e.IsDeleted)
                    .HasColumnName("IS_DELETED")
                    .HasDefaultValue(false);

                entity.HasOne(d => d.MaTkNavigation).WithOne(p => p.NhanVienYTe)
                    .HasForeignKey<NhanVienYTe>(d => d.MaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NHAN_VIE_CO_TK_TAI_KHOA");
            });

            modelBuilder.Entity<PhanCongBacSi>(entity =>
            {
                entity.HasKey(e => new { e.MaHgd, e.MaTk });

                entity.ToTable("PHAN_CONG_BAC_SI");

                entity.HasIndex(e => e.MaHgd, "CO_BAC_SI_FK");

                entity.HasIndex(e => e.MaTk, "PHAN_CONG_HGD_FK");

                entity.Property(e => e.MaHgd).HasColumnName("MA_HGD");
                entity.Property(e => e.MaTk).HasColumnName("MA_TK");
                entity.Property(e => e.ThoiGianBatDau).HasColumnName("THOI_GIAN_BAT_DAU");
                entity.Property(e => e.ThoiGianKetThuc).HasColumnName("THOI_GIAN_KET_THUC");

                entity.HasOne(d => d.MaHgdNavigation).WithMany(p => p.PhanCongBacSis)
                    .HasForeignKey(d => d.MaHgd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PHAN_CON_CO_BAC_SI_HO_GIA_D");

                entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.PhanCongBacSis)
                    .HasForeignKey(d => d.MaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PHAN_CON_PHAN_CONG_BAC_SI");
            });

            modelBuilder.Entity<PhieuNhapThuoc>(entity =>
            {
                entity.HasKey(e => e.MaPhieuNhap);

                entity.ToTable("PHIEU_NHAP_THUOC");

                entity.HasIndex(e => e.MaTk, "LAP_BOI_NHAN_VIEN_FK");

                entity.HasIndex(e => e.MaPhieuNhap, "PHIEU_NHAP_THUOC_PK").IsUnique();

                entity.Property(e => e.MaPhieuNhap)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_PHIEU_NHAP");
                entity.Property(e => e.MaTk).HasColumnName("MA_TK");
                entity.Property(e => e.NgayLapPhieuNhap)
                    .HasColumnName("NGAY_LAP_PHIEU_NHAP");
                entity.Property(e => e.TenNhaCungCap)
                    .HasMaxLength(100)
                    .HasColumnName("TEN_NHA_CUNG_CAP");
                entity.Property(e => e.TongTienNhap)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("TONG_TIEN_NHAP");

                entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.PhieuNhapThuocs)
                    .HasForeignKey(d => d.MaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PHIEU_NH_LAP_BOI_N_NHAN_VIE");
            });

            modelBuilder.Entity<PhieuThuTienThuoc>(entity =>
            {
                entity.HasKey(e => e.MaPhieuThu);

                entity.ToTable("PHIEU_THU_TIEN_THUOC");

                entity.HasIndex(e => e.MaDonThuoc, "CO_PHIEU_THU_FK");

                entity.HasIndex(e => e.MaTk, "LAP_PHIEU_THU_FK");

                entity.HasIndex(e => e.MaPhieuThu, "PHIEU_THU_TIEN_THUOC_PK").IsUnique();

                entity.Property(e => e.MaPhieuThu)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_PHIEU_THU");
                entity.Property(e => e.MaDonThuoc).HasColumnName("MA_DON_THUOC");
                entity.Property(e => e.MaTk).HasColumnName("MA_TK");
                entity.Property(e => e.NgayLapPhieuThu)
                    .HasColumnName("NGAY_LAP_PHIEU_THU")
                    .IsRequired();
                entity.Property(e => e.TongTien)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("TONG_TIEN");

                entity.HasOne(d => d.MaDonThuocNavigation).WithMany(p => p.PhieuThuTienThuocs)
                    .HasForeignKey(d => d.MaDonThuoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PHIEU_TH_CO_PHIEU__DON_THUO");

                entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.PhieuThuTienThuocs)
                    .HasForeignKey(d => d.MaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PHIEU_TH_LAP_PHIEU_NHAN_VIE");
            });

            modelBuilder.Entity<PhuongPhapCanLamSang>(entity =>
            {
                entity.HasKey(e => e.MaPhuongPhap);

                entity.ToTable("PHUONG_PHAP_CAN_LAM_SANG");

                entity.HasIndex(e => e.MaPhuongPhap, "PHUONG_PHAP_CAN_LAM_SANG_PK").IsUnique();

                entity.Property(e => e.MaPhuongPhap)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_PHUONG_PHAP");
                entity.Property(e => e.ChiPhi)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("CHI_PHI");
                entity.Property(e => e.GhiChu)
                    .HasMaxLength(1024)
                    .HasColumnName("GHI_CHU");
                entity.Property(e => e.TenPhuongPhap)
                    .HasMaxLength(250)
                    .HasColumnName("TEN_PHUONG_PHAP");
                entity.Property(e => e.YeuCauDacBiet)
                    .HasMaxLength(250)
                    .HasColumnName("YEU_CAU_DAC_BIET");
            });

            modelBuilder.Entity<QuyenTruyCap>(entity =>
            {
                entity.HasKey(e => e.MaQuyen);

                entity.ToTable("QUYEN_TRUY_CAP");

                entity.HasIndex(e => e.MaQuyen, "QUYEN_TRUY_CAP_PK").IsUnique();

                entity.Property(e => e.MaQuyen)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_QUYEN");
                entity.Property(e => e.TenQuyen)
                    .HasMaxLength(50)
                    .HasColumnName("TEN_QUYEN");
            });

            modelBuilder.Entity<SuDungDichVu>(entity =>
            {
                entity.HasKey(e => e.MaLanSuDung);

                entity.ToTable("SU_DUNG_DICH_VU");

                entity.HasIndex(e => e.MaDichVu, "CUA_DICH_VU_FK");

                entity.HasIndex(e => e.MaTk, "SU_DUNG_DICH_VU_FK");

                entity.HasIndex(e => e.MaLanSuDung, "SU_DUNG_DICH_VU_PK").IsUnique();

                entity.Property(e => e.MaLanSuDung)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_LAN_SU_DUNG");
                entity.Property(e => e.DiaChiLienLac)
                    .HasMaxLength(500)
                    .HasColumnName("DIA_CHI_LIEN_LAC");
                entity.Property(e => e.MaDichVu).HasColumnName("MA_DICH_VU");
                entity.Property(e => e.MaTk).HasColumnName("MA_TK");
                entity.Property(e => e.MoTaBenhLy)
                    .HasMaxLength(250)
                    .HasColumnName("MO_TA_BENH_LY");
                entity.Property(e => e.NgayBatDau).HasColumnName("NGAY_BAT_DAU");
                entity.Property(e => e.SoDienThoaiLienHe)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SO_DIEN_THOAI_LIEN_HE");
                entity.Property(e => e.TongTien)
                    .HasColumnType("decimal(18,2)");
                entity.Property(e => e.SoLuong).HasColumnName("SO_LUONG");
                entity.Property(e => e.TenNhanVienChiDinh)
                    .HasMaxLength(100)
                    .HasColumnName("TEN_NHAN_VIEN_CHI_DINH");
                entity.Property(e => e.TrangThai).HasColumnName("TRANG_THAI");
                entity.Property(e => e.YeuCauChiDinhNhanVien).HasColumnName("YEU_CAU_CHI_DINH_NHAN_VIEN");

                entity.HasOne(d => d.MaDichVuNavigation).WithMany(p => p.SuDungDichVus)
                    .HasForeignKey(d => d.MaDichVu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SU_DUNG__CUA_DICH__DICH_VU_");

                entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.SuDungDichVus)
                    .HasForeignKey(d => d.MaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SU_DUNG__SU_DUNG_D_KHACH_HA");
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.MaTk);

                entity.ToTable("TAI_KHOAN");

                entity.HasIndex(e => e.MaQuyen, "CO_QTC_FK");

                entity.HasIndex(e => e.MaTk, "TAI_KHOAN_PK").IsUnique();

                entity.Property(e => e.MaTk)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_TK");
                entity.Property(e => e.DiaChi)
                    .HasMaxLength(1024)
                    .HasColumnName("DIA_CHI");
                entity.Property(e => e.DiaChiEmail)
                    .HasMaxLength(250)
                    .HasColumnName("DIA_CHI_EMAIL");
                entity.Property(e => e.GioiTinh).HasColumnName("GIOI_TINH");
                entity.Property(e => e.HinhAnh)
                    .HasMaxLength(250)
                    .HasColumnName("HINH_ANH");
                entity.Property(e => e.HoTen)
                    .HasMaxLength(100)
                    .HasColumnName("HO_TEN");
                entity.Property(e => e.MaQuyen).HasColumnName("MA_QUYEN");
                entity.Property(e => e.MatKhau)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("MAT_KHAU");
                entity.Property(e => e.NgaySinh).HasColumnName("NGAY_SINH");
                entity.Property(e => e.SoCccd)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SO_CCCD");
                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("SO_DIEN_THOAI");
                entity.Property(e => e.IsDeleted)
                    .HasColumnName("IS_DELETED")
                    .HasDefaultValue(false);
                entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.TaiKhoans)
                    .HasForeignKey(d => d.MaQuyen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TAI_KHOA_CO_QTC_QUYEN_TR");
            });

            modelBuilder.Entity<Thuoc>(entity =>
            {
                entity.HasKey(e => e.MaThuoc);

                entity.ToTable("THUOC");

                entity.HasIndex(e => e.MaDvt, "CO_DVT_FK");

                entity.HasIndex(e => e.MaThuoc, "THUOC_PK").IsUnique();

                entity.Property(e => e.MaThuoc)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_THUOC");
                entity.Property(e => e.HanSuDung).HasColumnName("HAN_SU_DUNG");
                entity.Property(e => e.SoLuong).HasColumnName("SO_LUONG");
                entity.Property(e => e.MaDvt).HasColumnName("MA_DVT");
                entity.Property(e => e.MoTaThuoc)
                    .HasMaxLength(250)
                    .HasColumnName("MO_TA_THUOC");
                entity.Property(e => e.TenThuoc)
                    .HasMaxLength(100)
                    .HasColumnName("TEN_THUOC");
                entity.Property(e => e.IsDelete)
                    .HasColumnName("IS_DELETE")
                    .HasDefaultValue(false);

                entity.HasOne(d => d.MaDvtNavigation).WithMany(p => p.Thuocs)
                    .HasForeignKey(d => d.MaDvt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_THUOC_CO_DVT_DON_VI_T");
            });

            modelBuilder.Entity<TuVan>(entity =>
            {
                entity.HasKey(e => e.MaTinNhan);

                entity.ToTable("TU_VAN");

                entity.HasIndex(e => e.BacMaTk, "TU_VAN_CHO_BENH_NHAN_FK");

                entity.HasIndex(e => e.MaTinNhan, "TU_VAN_PK").IsUnique();

                entity.HasIndex(e => e.MaTk, "YEU_CAU_TU_VAN_FK");

                entity.Property(e => e.MaTinNhan)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MA_TIN_NHAN");
                entity.Property(e => e.BacMaTk).HasColumnName("BAC_MA_TK");
                entity.Property(e => e.MaTk).HasColumnName("MA_TK");
                entity.Property(e => e.NoiDungTinNhan)
                    .HasMaxLength(1)
                    .HasColumnName("NOI_DUNG_TIN_NHAN");
                entity.Property(e => e.ThoiDiemNhanTin)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("THOI_DIEM_NHAN_TIN");
                entity.Property(e => e.TrangThaiTinNhan).HasColumnName("TRANG_THAI_TIN_NHAN");

                entity.HasOne(d => d.BacMaTkNavigation).WithMany(p => p.TuVans)
                    .HasForeignKey(d => d.BacMaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TU_VAN_TU_VAN_CH_BAC_SI");

                entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.TuVans)
                    .HasForeignKey(d => d.MaTk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TU_VAN_YEU_CAU_T_KHACH_HA");
            });
            
        }
    }
}
