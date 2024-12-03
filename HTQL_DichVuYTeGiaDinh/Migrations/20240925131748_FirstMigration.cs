using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTQL_DichVuYTeGiaDinh.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CHUONG_TRINH_KHUYEN_MAI",
                columns: table => new
                {
                    MA_KHUYEN_MAI = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TEN_KHUYEN_MAI = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PHAN_TRAM_GIAM_GIA = table.Column<float>(type: "real", nullable: false),
                    NGAY_BAT_DAU_KM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NGAY_KET_THUC_KM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MO_TA = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHUONG_TRINH_KHUYEN_MAI", x => x.MA_KHUYEN_MAI);
                });

            migrationBuilder.CreateTable(
                name: "CHUYEN_KHOA",
                columns: table => new
                {
                    MA_CHUYEN_KHOA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TEN_CHUYEN_KHOA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHUYEN_KHOA", x => x.MA_CHUYEN_KHOA);
                });

            migrationBuilder.CreateTable(
                name: "DICH_VU_Y_TE",
                columns: table => new
                {
                    MA_DICH_VU = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TEN_DICH_VU = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MO_TA = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DON_VI_TINH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DICH_VU_Y_TE", x => x.MA_DICH_VU);
                });

            migrationBuilder.CreateTable(
                name: "DON_VI_TINH_THUOC",
                columns: table => new
                {
                    MA_DVT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TEN_DVT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DON_VI_TINH_THUOC", x => x.MA_DVT);
                });

            migrationBuilder.CreateTable(
                name: "HO_GIA_DINH",
                columns: table => new
                {
                    MA_HGD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SODIENTHOAI_HGD = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    CHU_HO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DIA_CHI_HGD = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HO_GIA_DINH", x => x.MA_HGD);
                });

            migrationBuilder.CreateTable(
                name: "PHUONG_PHAP_CAN_LAM_SANG",
                columns: table => new
                {
                    MA_PHUONG_PHAP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TEN_PHUONG_PHAP = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    YEU_CAU_DAC_BIET = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CHI_PHI = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    GHI_CHU = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHUONG_PHAP_CAN_LAM_SANG", x => x.MA_PHUONG_PHAP);
                });

            migrationBuilder.CreateTable(
                name: "QUYEN_TRUY_CAP",
                columns: table => new
                {
                    MA_QUYEN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TEN_QUYEN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QUYEN_TRUY_CAP", x => x.MA_QUYEN);
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_KHUYEN_MAI",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<int>(type: "int", nullable: false),
                    MaDichVu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHI_TIET_KHUYEN_MAI", x => new { x.MaKhuyenMai, x.MaDichVu });
                    table.ForeignKey(
                        name: "FK_CHI_TIET_KM_CHO__KM",
                        column: x => x.MaKhuyenMai,
                        principalTable: "CHUONG_TRINH_KHUYEN_MAI",
                        principalColumn: "MA_KHUYEN_MAI",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CHI_TIET_KM_DV",
                        column: x => x.MaDichVu,
                        principalTable: "DICH_VU_Y_TE",
                        principalColumn: "MA_DICH_VU",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DON_GIA_DICH_VU",
                columns: table => new
                {
                    MA_DICH_VU = table.Column<int>(type: "int", nullable: false),
                    DON_GIA_DV = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    THOI_DIEM = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DON_GIA_DICH_VU", x => x.MA_DICH_VU);
                    table.ForeignKey(
                        name: "FK_DON_GIA__CO_DON_GI_DICH_VU_",
                        column: x => x.MA_DICH_VU,
                        principalTable: "DICH_VU_Y_TE",
                        principalColumn: "MA_DICH_VU");
                });

            migrationBuilder.CreateTable(
                name: "THUOC",
                columns: table => new
                {
                    MA_THUOC = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MA_DVT = table.Column<int>(type: "int", nullable: false),
                    TEN_THUOC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MO_TA_THUOC = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    HAN_SU_DUNG = table.Column<int>(type: "int", nullable: false),
                    SO_LUONG = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THUOC", x => x.MA_THUOC);
                    table.ForeignKey(
                        name: "FK_THUOC_CO_DVT_DON_VI_T",
                        column: x => x.MA_DVT,
                        principalTable: "DON_VI_TINH_THUOC",
                        principalColumn: "MA_DVT");
                });

            migrationBuilder.CreateTable(
                name: "TAI_KHOAN",
                columns: table => new
                {
                    MA_TK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MA_QUYEN = table.Column<int>(type: "int", nullable: false),
                    HO_TEN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NGAY_SINH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SO_CCCD = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: false),
                    GIOI_TINH = table.Column<short>(type: "smallint", nullable: false),
                    DIA_CHI = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    SO_DIEN_THOAI = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    MAT_KHAU = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    HINH_ANH = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DIA_CHI_EMAIL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAI_KHOAN", x => x.MA_TK);
                    table.ForeignKey(
                        name: "FK_TAI_KHOA_CO_QTC_QUYEN_TR",
                        column: x => x.MA_QUYEN,
                        principalTable: "QUYEN_TRUY_CAP",
                        principalColumn: "MA_QUYEN");
                });

            migrationBuilder.CreateTable(
                name: "DON_GIA_THUOC",
                columns: table => new
                {
                    MA_THUOC = table.Column<int>(type: "int", nullable: false),
                    DON_GIA_THUOC = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    THOI_DIEM = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DON_GIA_THUOC", x => x.MA_THUOC);
                    table.ForeignKey(
                        name: "FK_DON_GIA__CO_DON_GI_THUOC",
                        column: x => x.MA_THUOC,
                        principalTable: "THUOC",
                        principalColumn: "MA_THUOC");
                });

            migrationBuilder.CreateTable(
                name: "BAC_SI",
                columns: table => new
                {
                    MA_TK = table.Column<int>(type: "int", nullable: false),
                    MA_CHUYEN_KHOA = table.Column<int>(type: "int", nullable: false),
                    MA_QUYEN = table.Column<int>(type: "int", nullable: true),
                    HO_TEN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NGAY_SINH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SO_CCCD = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: false),
                    GIOI_TINH = table.Column<short>(type: "smallint", nullable: false),
                    DIA_CHI = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    SO_DIEN_THOAI = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    MAT_KHAU = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    HINH_ANH = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DIA_CHI_EMAIL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TRINH_DO_HOC_VAN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BAC_SI", x => x.MA_TK);
                    table.ForeignKey(
                        name: "FK_BAC_SI_CO_TK2_TAI_KHOA",
                        column: x => x.MA_TK,
                        principalTable: "TAI_KHOAN",
                        principalColumn: "MA_TK");
                    table.ForeignKey(
                        name: "FK_BAC_SI_THUOC_CHU_CHUYEN_K",
                        column: x => x.MA_CHUYEN_KHOA,
                        principalTable: "CHUYEN_KHOA",
                        principalColumn: "MA_CHUYEN_KHOA");
                });

            migrationBuilder.CreateTable(
                name: "KHACH_HANG",
                columns: table => new
                {
                    MA_TK = table.Column<int>(type: "int", nullable: false),
                    MA_HGD = table.Column<int>(type: "int", nullable: false),
                    MA_QUYEN = table.Column<int>(type: "int", nullable: true),
                    HO_TEN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NGAY_SINH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SO_CCCD = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: false),
                    GIOI_TINH = table.Column<short>(type: "smallint", nullable: false),
                    DIA_CHI = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    SO_DIEN_THOAI = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    MAT_KHAU = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    HINH_ANH = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DIA_CHI_EMAIL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MOI_QUAN_HE_VOI_CHU_HO = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TEN_NGUOI_LIEN_HE_KHAN_CAP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDT_NGUOI_LIEN_HE_KHAN_CAP = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHACH_HANG", x => x.MA_TK);
                    table.ForeignKey(
                        name: "FK_KHACH_HA_CO_TK3_TAI_KHOA",
                        column: x => x.MA_TK,
                        principalTable: "TAI_KHOAN",
                        principalColumn: "MA_TK");
                    table.ForeignKey(
                        name: "FK_KHACH_HA_THUOC_HGD_HO_GIA_D",
                        column: x => x.MA_HGD,
                        principalTable: "HO_GIA_DINH",
                        principalColumn: "MA_HGD");
                });

            migrationBuilder.CreateTable(
                name: "NHAN_VIEN_Y_TE",
                columns: table => new
                {
                    MA_TK = table.Column<int>(type: "int", nullable: false),
                    MA_QUYEN = table.Column<int>(type: "int", nullable: true),
                    HO_TEN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NGAY_SINH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SO_CCCD = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: false),
                    GIOI_TINH = table.Column<short>(type: "smallint", nullable: false),
                    DIA_CHI = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    SO_DIEN_THOAI = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    MAT_KHAU = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    HINH_ANH = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DIA_CHI_EMAIL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHAN_VIEN_Y_TE", x => x.MA_TK);
                    table.ForeignKey(
                        name: "FK_NHAN_VIE_CO_TK_TAI_KHOA",
                        column: x => x.MA_TK,
                        principalTable: "TAI_KHOAN",
                        principalColumn: "MA_TK");
                });

            migrationBuilder.CreateTable(
                name: "PHAN_CONG_BAC_SI",
                columns: table => new
                {
                    MA_HGD = table.Column<int>(type: "int", nullable: false),
                    MA_TK = table.Column<int>(type: "int", nullable: false),
                    THOI_GIAN_BAT_DAU = table.Column<DateTime>(type: "datetime2", nullable: false),
                    THOI_GIAN_KET_THUC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHAN_CONG_BAC_SI", x => new { x.MA_HGD, x.MA_TK });
                    table.ForeignKey(
                        name: "FK_PHAN_CON_CO_BAC_SI_HO_GIA_D",
                        column: x => x.MA_HGD,
                        principalTable: "HO_GIA_DINH",
                        principalColumn: "MA_HGD");
                    table.ForeignKey(
                        name: "FK_PHAN_CON_PHAN_CONG_BAC_SI",
                        column: x => x.MA_TK,
                        principalTable: "BAC_SI",
                        principalColumn: "MA_TK");
                });

            migrationBuilder.CreateTable(
                name: "BINH_LUAN",
                columns: table => new
                {
                    MA_BINH_LUAN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MA_DICH_VU = table.Column<int>(type: "int", nullable: false),
                    MA_TK = table.Column<int>(type: "int", nullable: false),
                    THOI_DIEM_BINH_LUAN = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false),
                    NOI_DUNG_BINH_LUAN = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DIEM_DANH_GIA = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BINH_LUAN", x => x.MA_BINH_LUAN);
                    table.ForeignKey(
                        name: "FK_BINH_LUA_CO_BINH_L_DICH_VU_",
                        column: x => x.MA_DICH_VU,
                        principalTable: "DICH_VU_Y_TE",
                        principalColumn: "MA_DICH_VU");
                    table.ForeignKey(
                        name: "FK_BINH_LUA_VIET_BINH_KHACH_HA",
                        column: x => x.MA_TK,
                        principalTable: "KHACH_HANG",
                        principalColumn: "MA_TK");
                });

            migrationBuilder.CreateTable(
                name: "HO_SO_Y_TE",
                columns: table => new
                {
                    MA_HO_SO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MA_TK = table.Column<int>(type: "int", nullable: false),
                    BAC_MA_TK = table.Column<int>(type: "int", nullable: false),
                    NGAY_KHAM_BENH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CHAN_DOAN = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    NGAY_TAI_KHAM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    THONG_TIN_KHAC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HO_SO_Y_TE", x => x.MA_HO_SO);
                    table.ForeignKey(
                        name: "FK_HO_SO_Y__CO_HO_SO__KHACH_HA",
                        column: x => x.MA_TK,
                        principalTable: "KHACH_HANG",
                        principalColumn: "MA_TK");
                    table.ForeignKey(
                        name: "FK_HO_SO_Y__GHI_HO_SO_BAC_SI",
                        column: x => x.BAC_MA_TK,
                        principalTable: "BAC_SI",
                        principalColumn: "MA_TK");
                });

            migrationBuilder.CreateTable(
                name: "TU_VAN",
                columns: table => new
                {
                    MA_TIN_NHAN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MA_TK = table.Column<int>(type: "int", nullable: false),
                    BAC_MA_TK = table.Column<int>(type: "int", nullable: false),
                    NOI_DUNG_TIN_NHAN = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    THOI_DIEM_NHAN_TIN = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false),
                    TRANG_THAI_TIN_NHAN = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TU_VAN", x => x.MA_TIN_NHAN);
                    table.ForeignKey(
                        name: "FK_TU_VAN_TU_VAN_CH_BAC_SI",
                        column: x => x.BAC_MA_TK,
                        principalTable: "BAC_SI",
                        principalColumn: "MA_TK");
                    table.ForeignKey(
                        name: "FK_TU_VAN_YEU_CAU_T_KHACH_HA",
                        column: x => x.MA_TK,
                        principalTable: "KHACH_HANG",
                        principalColumn: "MA_TK");
                });

            migrationBuilder.CreateTable(
                name: "PHIEU_NHAP_THUOC",
                columns: table => new
                {
                    MA_PHIEU_NHAP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MA_TK = table.Column<int>(type: "int", nullable: false),
                    NGAY_LAP_PHIEU_NHAP = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false),
                    TONG_TIEN_NHAP = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TEN_NHA_CUNG_CAP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHIEU_NHAP_THUOC", x => x.MA_PHIEU_NHAP);
                    table.ForeignKey(
                        name: "FK_PHIEU_NH_LAP_BOI_N_NHAN_VIE",
                        column: x => x.MA_TK,
                        principalTable: "NHAN_VIEN_Y_TE",
                        principalColumn: "MA_TK");
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_KET_QUA_CAN_LAM_SANG",
                columns: table => new
                {
                    MA_PHUONG_PHAP = table.Column<int>(type: "int", nullable: false),
                    MA_HO_SO = table.Column<int>(type: "int", nullable: false),
                    HINH_ANH_KET_QUA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHI_TIET_KET_QUA_CAN_LAM_SA", x => new { x.MA_PHUONG_PHAP, x.MA_HO_SO });
                    table.ForeignKey(
                        name: "FK_CHI_TIET_CO_CHI_TI_HO_SO_Y_",
                        column: x => x.MA_HO_SO,
                        principalTable: "HO_SO_Y_TE",
                        principalColumn: "MA_HO_SO");
                    table.ForeignKey(
                        name: "FK_CHI_TIET_CUA_PHUON_PHUONG_P",
                        column: x => x.MA_PHUONG_PHAP,
                        principalTable: "PHUONG_PHAP_CAN_LAM_SANG",
                        principalColumn: "MA_PHUONG_PHAP");
                });

            migrationBuilder.CreateTable(
                name: "DON_THUOC",
                columns: table => new
                {
                    MA_DON_THUOC = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MA_HO_SO = table.Column<int>(type: "int", nullable: false),
                    NGAY_KE_DON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TRANG_THAI_DON_THUOC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GHI_CHU_DON_THUOC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DON_THUOC", x => x.MA_DON_THUOC);
                    table.ForeignKey(
                        name: "FK_DON_THUO_CO_DON_TH_HO_SO_Y_",
                        column: x => x.MA_HO_SO,
                        principalTable: "HO_SO_Y_TE",
                        principalColumn: "MA_HO_SO");
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_PHIEU_NHAP",
                columns: table => new
                {
                    MA_THUOC = table.Column<int>(type: "int", nullable: false),
                    MA_PHIEU_NHAP = table.Column<int>(type: "int", nullable: false),
                    SO_LUONG_NHAP = table.Column<int>(type: "int", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    DON_GIA_NHAP = table.Column<decimal>(type: "decimal(18,2)", unicode: false, fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHI_TIET_PHIEU_NHAP", x => new { x.MA_THUOC, x.MA_PHIEU_NHAP });
                    table.ForeignKey(
                        name: "FK_CHI_TIET_NHAP_CHO__PHIEU_NH",
                        column: x => x.MA_PHIEU_NHAP,
                        principalTable: "PHIEU_NHAP_THUOC",
                        principalColumn: "MA_PHIEU_NHAP");
                    table.ForeignKey(
                        name: "FK_CHI_TIET_NHAP_THUO_THUOC",
                        column: x => x.MA_THUOC,
                        principalTable: "THUOC",
                        principalColumn: "MA_THUOC");
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_DON_THUOC",
                columns: table => new
                {
                    MA_DON_THUOC = table.Column<int>(type: "int", nullable: false),
                    MA_THUOC = table.Column<int>(type: "int", nullable: false),
                    SO_LUONG = table.Column<int>(type: "int", nullable: false),
                    HUONG_DAN_SU_DUNG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHI_TIET_DON_THUOC", x => new { x.MA_DON_THUOC, x.MA_THUOC });
                    table.ForeignKey(
                        name: "FK_CHI_TIET_CO_CHI_TI_DON_THUO",
                        column: x => x.MA_DON_THUOC,
                        principalTable: "DON_THUOC",
                        principalColumn: "MA_DON_THUOC");
                    table.ForeignKey(
                        name: "FK_CHI_TIET_CO_THUOC_THUOC",
                        column: x => x.MA_THUOC,
                        principalTable: "THUOC",
                        principalColumn: "MA_THUOC");
                });

            migrationBuilder.CreateTable(
                name: "PHIEU_THU_TIEN_THUOC",
                columns: table => new
                {
                    MA_PHIEU_THU = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MA_DON_THUOC = table.Column<int>(type: "int", nullable: false),
                    MA_TK = table.Column<int>(type: "int", nullable: false),
                    NGAY_LAP_PHIEU_THU = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false),
                    TONG_TIEN = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHIEU_THU_TIEN_THUOC", x => x.MA_PHIEU_THU);
                    table.ForeignKey(
                        name: "FK_PHIEU_TH_CO_PHIEU__DON_THUO",
                        column: x => x.MA_DON_THUOC,
                        principalTable: "DON_THUOC",
                        principalColumn: "MA_DON_THUOC");
                    table.ForeignKey(
                        name: "FK_PHIEU_TH_LAP_PHIEU_NHAN_VIE",
                        column: x => x.MA_TK,
                        principalTable: "NHAN_VIEN_Y_TE",
                        principalColumn: "MA_TK");
                });

            migrationBuilder.CreateTable(
                name: "HOA_DON",
                columns: table => new
                {
                    MA_HOA_DON = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MA_LAN_SU_DUNG = table.Column<int>(type: "int", nullable: false),
                    MA_TK = table.Column<int>(type: "int", nullable: false),
                    NGAY_LAP_HOA_DON = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false),
                    TONG_TIEN = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOA_DON", x => x.MA_HOA_DON);
                    table.ForeignKey(
                        name: "FK_HOA_DON_CO_NVYT_NHAN_VIE",
                        column: x => x.MA_TK,
                        principalTable: "NHAN_VIEN_Y_TE",
                        principalColumn: "MA_TK");
                });

            migrationBuilder.CreateTable(
                name: "SU_DUNG_DICH_VU",
                columns: table => new
                {
                    MA_LAN_SU_DUNG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MA_DICH_VU = table.Column<int>(type: "int", nullable: false),
                    MA_TK = table.Column<int>(type: "int", nullable: false),
                    NGAY_BAT_DAU = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SO_LUONG = table.Column<int>(type: "int", nullable: false),
                    SO_DIEN_THOAI_LIEN_HE = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    DIA_CHI_LIEN_LAC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    YEU_CAU_CHI_DINH_NHAN_VIEN = table.Column<short>(type: "smallint", nullable: false),
                    TEN_NHAN_VIEN_CHI_DINH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MO_TA_BENH_LY = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TRANG_THAI = table.Column<short>(type: "smallint", nullable: false),
                    HoaDonMaHoaDon = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SU_DUNG_DICH_VU", x => x.MA_LAN_SU_DUNG);
                    table.ForeignKey(
                        name: "FK_SU_DUNG__CUA_DICH__DICH_VU_",
                        column: x => x.MA_DICH_VU,
                        principalTable: "DICH_VU_Y_TE",
                        principalColumn: "MA_DICH_VU");
                    table.ForeignKey(
                        name: "FK_SU_DUNG__SU_DUNG_D_KHACH_HA",
                        column: x => x.MA_TK,
                        principalTable: "KHACH_HANG",
                        principalColumn: "MA_TK");
                    table.ForeignKey(
                        name: "FK_SU_DUNG_DICH_VU_HOA_DON_HoaDonMaHoaDon",
                        column: x => x.HoaDonMaHoaDon,
                        principalTable: "HOA_DON",
                        principalColumn: "MA_HOA_DON");
                });

            migrationBuilder.CreateIndex(
                name: "THUOC_CHUYEN_KHOA_FK",
                table: "BAC_SI",
                column: "MA_CHUYEN_KHOA");

            migrationBuilder.CreateIndex(
                name: "BINH_LUAN_PK",
                table: "BINH_LUAN",
                column: "MA_BINH_LUAN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CO_BINH_LUAN_FK",
                table: "BINH_LUAN",
                column: "MA_DICH_VU");

            migrationBuilder.CreateIndex(
                name: "VIET_BINH_LUAN_FK",
                table: "BINH_LUAN",
                column: "MA_TK");

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_DON_THUOC_MA_THUOC",
                table: "CHI_TIET_DON_THUOC",
                column: "MA_THUOC");

            migrationBuilder.CreateIndex(
                name: "CO_CHI_TIET_KET_QUA_FK",
                table: "CHI_TIET_KET_QUA_CAN_LAM_SANG",
                column: "MA_HO_SO");

            migrationBuilder.CreateIndex(
                name: "CUA_PHUONG_PHAP_CAN_LAM_SANG_FK",
                table: "CHI_TIET_KET_QUA_CAN_LAM_SANG",
                column: "MA_PHUONG_PHAP");

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_KHUYEN_MAI_MaDichVu",
                table: "CHI_TIET_KHUYEN_MAI",
                column: "MaDichVu");

            migrationBuilder.CreateIndex(
                name: "NHAP_CHO_PHIEU_FK",
                table: "CHI_TIET_PHIEU_NHAP",
                column: "MA_PHIEU_NHAP");

            migrationBuilder.CreateIndex(
                name: "NHAP_THUOC_FK",
                table: "CHI_TIET_PHIEU_NHAP",
                column: "MA_THUOC");

            migrationBuilder.CreateIndex(
                name: "CHUONG_TRINH_KHUYEN_MAI_PK",
                table: "CHUONG_TRINH_KHUYEN_MAI",
                column: "MA_KHUYEN_MAI",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CHUYEN_KHOA_PK",
                table: "CHUYEN_KHOA",
                column: "MA_CHUYEN_KHOA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "DICH_VU_Y_TE_PK",
                table: "DICH_VU_Y_TE",
                column: "MA_DICH_VU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CO_DON_THUOC_FK",
                table: "DON_THUOC",
                column: "MA_HO_SO");

            migrationBuilder.CreateIndex(
                name: "DON_THUOC_PK",
                table: "DON_THUOC",
                column: "MA_DON_THUOC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "DON_VI_TINH_THUOC_PK",
                table: "DON_VI_TINH_THUOC",
                column: "MA_DVT",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "HO_GIA_DINH_PK",
                table: "HO_GIA_DINH",
                column: "MA_HGD",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CO_HO_SO_Y_TE_FK",
                table: "HO_SO_Y_TE",
                column: "MA_TK");

            migrationBuilder.CreateIndex(
                name: "GHI_HO_SO_Y_TE_FK",
                table: "HO_SO_Y_TE",
                column: "BAC_MA_TK");

            migrationBuilder.CreateIndex(
                name: "HO_SO_Y_TE_PK",
                table: "HO_SO_Y_TE",
                column: "MA_HO_SO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CO_HOA_DON_FK",
                table: "HOA_DON",
                column: "MA_LAN_SU_DUNG");

            migrationBuilder.CreateIndex(
                name: "CO_NVYT_FK",
                table: "HOA_DON",
                column: "MA_TK");

            migrationBuilder.CreateIndex(
                name: "HOA_DON_PK",
                table: "HOA_DON",
                column: "MA_HOA_DON",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "THUOC_HGD_FK",
                table: "KHACH_HANG",
                column: "MA_HGD");

            migrationBuilder.CreateIndex(
                name: "CO_BAC_SI_FK",
                table: "PHAN_CONG_BAC_SI",
                column: "MA_HGD");

            migrationBuilder.CreateIndex(
                name: "PHAN_CONG_HGD_FK",
                table: "PHAN_CONG_BAC_SI",
                column: "MA_TK");

            migrationBuilder.CreateIndex(
                name: "LAP_BOI_NHAN_VIEN_FK",
                table: "PHIEU_NHAP_THUOC",
                column: "MA_TK");

            migrationBuilder.CreateIndex(
                name: "PHIEU_NHAP_THUOC_PK",
                table: "PHIEU_NHAP_THUOC",
                column: "MA_PHIEU_NHAP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CO_PHIEU_THU_FK",
                table: "PHIEU_THU_TIEN_THUOC",
                column: "MA_DON_THUOC");

            migrationBuilder.CreateIndex(
                name: "LAP_PHIEU_THU_FK",
                table: "PHIEU_THU_TIEN_THUOC",
                column: "MA_TK");

            migrationBuilder.CreateIndex(
                name: "PHIEU_THU_TIEN_THUOC_PK",
                table: "PHIEU_THU_TIEN_THUOC",
                column: "MA_PHIEU_THU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PHUONG_PHAP_CAN_LAM_SANG_PK",
                table: "PHUONG_PHAP_CAN_LAM_SANG",
                column: "MA_PHUONG_PHAP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "QUYEN_TRUY_CAP_PK",
                table: "QUYEN_TRUY_CAP",
                column: "MA_QUYEN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CUA_DICH_VU_FK",
                table: "SU_DUNG_DICH_VU",
                column: "MA_DICH_VU");

            migrationBuilder.CreateIndex(
                name: "IX_SU_DUNG_DICH_VU_HoaDonMaHoaDon",
                table: "SU_DUNG_DICH_VU",
                column: "HoaDonMaHoaDon");

            migrationBuilder.CreateIndex(
                name: "SU_DUNG_DICH_VU_FK",
                table: "SU_DUNG_DICH_VU",
                column: "MA_TK");

            migrationBuilder.CreateIndex(
                name: "SU_DUNG_DICH_VU_PK",
                table: "SU_DUNG_DICH_VU",
                column: "MA_LAN_SU_DUNG",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CO_QTC_FK",
                table: "TAI_KHOAN",
                column: "MA_QUYEN");

            migrationBuilder.CreateIndex(
                name: "TAI_KHOAN_PK",
                table: "TAI_KHOAN",
                column: "MA_TK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CO_DVT_FK",
                table: "THUOC",
                column: "MA_DVT");

            migrationBuilder.CreateIndex(
                name: "THUOC_PK",
                table: "THUOC",
                column: "MA_THUOC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "TU_VAN_CHO_BENH_NHAN_FK",
                table: "TU_VAN",
                column: "BAC_MA_TK");

            migrationBuilder.CreateIndex(
                name: "TU_VAN_PK",
                table: "TU_VAN",
                column: "MA_TIN_NHAN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "YEU_CAU_TU_VAN_FK",
                table: "TU_VAN",
                column: "MA_TK");

            migrationBuilder.AddForeignKey(
                name: "FK_HOA_DON_CO_HOA_DO_SU_DUNG_",
                table: "HOA_DON",
                column: "MA_LAN_SU_DUNG",
                principalTable: "SU_DUNG_DICH_VU",
                principalColumn: "MA_LAN_SU_DUNG");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KHACH_HA_CO_TK3_TAI_KHOA",
                table: "KHACH_HANG");

            migrationBuilder.DropForeignKey(
                name: "FK_NHAN_VIE_CO_TK_TAI_KHOA",
                table: "NHAN_VIEN_Y_TE");

            migrationBuilder.DropForeignKey(
                name: "FK_SU_DUNG__CUA_DICH__DICH_VU_",
                table: "SU_DUNG_DICH_VU");

            migrationBuilder.DropForeignKey(
                name: "FK_SU_DUNG__SU_DUNG_D_KHACH_HA",
                table: "SU_DUNG_DICH_VU");

            migrationBuilder.DropForeignKey(
                name: "FK_HOA_DON_CO_HOA_DO_SU_DUNG_",
                table: "HOA_DON");

            migrationBuilder.DropTable(
                name: "BINH_LUAN");

            migrationBuilder.DropTable(
                name: "CHI_TIET_DON_THUOC");

            migrationBuilder.DropTable(
                name: "CHI_TIET_KET_QUA_CAN_LAM_SANG");

            migrationBuilder.DropTable(
                name: "CHI_TIET_KHUYEN_MAI");

            migrationBuilder.DropTable(
                name: "CHI_TIET_PHIEU_NHAP");

            migrationBuilder.DropTable(
                name: "DON_GIA_DICH_VU");

            migrationBuilder.DropTable(
                name: "DON_GIA_THUOC");

            migrationBuilder.DropTable(
                name: "PHAN_CONG_BAC_SI");

            migrationBuilder.DropTable(
                name: "PHIEU_THU_TIEN_THUOC");

            migrationBuilder.DropTable(
                name: "TU_VAN");

            migrationBuilder.DropTable(
                name: "PHUONG_PHAP_CAN_LAM_SANG");

            migrationBuilder.DropTable(
                name: "CHUONG_TRINH_KHUYEN_MAI");

            migrationBuilder.DropTable(
                name: "PHIEU_NHAP_THUOC");

            migrationBuilder.DropTable(
                name: "THUOC");

            migrationBuilder.DropTable(
                name: "DON_THUOC");

            migrationBuilder.DropTable(
                name: "DON_VI_TINH_THUOC");

            migrationBuilder.DropTable(
                name: "HO_SO_Y_TE");

            migrationBuilder.DropTable(
                name: "BAC_SI");

            migrationBuilder.DropTable(
                name: "CHUYEN_KHOA");

            migrationBuilder.DropTable(
                name: "TAI_KHOAN");

            migrationBuilder.DropTable(
                name: "QUYEN_TRUY_CAP");

            migrationBuilder.DropTable(
                name: "DICH_VU_Y_TE");

            migrationBuilder.DropTable(
                name: "KHACH_HANG");

            migrationBuilder.DropTable(
                name: "HO_GIA_DINH");

            migrationBuilder.DropTable(
                name: "SU_DUNG_DICH_VU");

            migrationBuilder.DropTable(
                name: "HOA_DON");

            migrationBuilder.DropTable(
                name: "NHAN_VIEN_Y_TE");
        }
    }
}
