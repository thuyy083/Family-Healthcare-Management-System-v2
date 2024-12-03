using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTQL_DichVuYTeGiaDinh.Migrations
{
    public partial class NavigationNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DON_GIA_THUOC",
                table: "DON_GIA_THUOC");

            migrationBuilder.AlterColumn<int>(
                name: "MA_QUYEN",
                table: "BAC_SI",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DON_GIA_THUOC",
                table: "DON_GIA_THUOC",
                columns: new[] { "MA_THUOC", "THOI_DIEM" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DON_GIA_THUOC",
                table: "DON_GIA_THUOC");

            migrationBuilder.AlterColumn<int>(
                name: "MA_QUYEN",
                table: "BAC_SI",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DON_GIA_THUOC",
                table: "DON_GIA_THUOC",
                column: "MA_THUOC");
        }
    }
}
