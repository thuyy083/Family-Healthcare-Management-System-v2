using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTQL_DichVuYTeGiaDinh.Migrations
{
    public partial class AddTongTienToSuDungDichVu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TongTien",
                table: "SU_DUNG_DICH_VU",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TongTien",
                table: "SU_DUNG_DICH_VU");
        }
    }
}
