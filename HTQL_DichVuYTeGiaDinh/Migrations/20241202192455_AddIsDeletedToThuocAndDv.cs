using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTQL_DichVuYTeGiaDinh.Migrations
{
    public partial class AddIsDeletedToThuocAndDv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IS_DELETE",
                table: "THUOC",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IS_DELETE",
                table: "DICH_VU_Y_TE",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IS_DELETE",
                table: "THUOC");

            migrationBuilder.DropColumn(
                name: "IS_DELETE",
                table: "DICH_VU_Y_TE");
        }
    }
}
