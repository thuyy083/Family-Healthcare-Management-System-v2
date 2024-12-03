using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTQL_DichVuYTeGiaDinh.Migrations
{
    public partial class AddIsDeletedToModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IS_DELETED",
                table: "TAI_KHOAN",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IS_DELETED",
                table: "NHAN_VIEN_Y_TE",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IS_DELETED",
                table: "KHACH_HANG",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "THOI_DIEM_BINH_LUAN",
                table: "BINH_LUAN",
                type: "datetime2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldRowVersion: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_DELETED",
                table: "BAC_SI",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IS_DELETED",
                table: "TAI_KHOAN");

            migrationBuilder.DropColumn(
                name: "IS_DELETED",
                table: "NHAN_VIEN_Y_TE");

            migrationBuilder.DropColumn(
                name: "IS_DELETED",
                table: "KHACH_HANG");

            migrationBuilder.DropColumn(
                name: "IS_DELETED",
                table: "BAC_SI");

            migrationBuilder.AlterColumn<DateTime>(
                name: "THOI_DIEM_BINH_LUAN",
                table: "BINH_LUAN",
                type: "datetime2",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)");
        }
    }
}
