using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTQL_DichVuYTeGiaDinh.Migrations
{
    public partial class UpdateHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "NGAY_LAP_HOA_DON",
                table: "HOA_DON",
                type: "datetime2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldRowVersion: true);

            migrationBuilder.AlterColumn<string>(
                name: "CHAN_DOAN",
                table: "HO_SO_Y_TE",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "NGAY_LAP_HOA_DON",
                table: "HOA_DON",
                type: "datetime2",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)");

            migrationBuilder.AlterColumn<string>(
                name: "CHAN_DOAN",
                table: "HO_SO_Y_TE",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
