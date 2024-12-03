using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTQL_DichVuYTeGiaDinh.Migrations
{
    public partial class UpdateNoiDungBinhLuanLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NOI_DUNG_BINH_LUAN",
                table: "BINH_LUAN",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NOI_DUNG_BINH_LUAN",
                table: "BINH_LUAN",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);
        }
    }
}
