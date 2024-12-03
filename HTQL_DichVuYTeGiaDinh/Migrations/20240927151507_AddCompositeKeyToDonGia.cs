using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTQL_DichVuYTeGiaDinh.Migrations
{
    public partial class AddCompositeKeyToDonGia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DON_GIA__CO_DON_GI_THUOC",
                table: "DON_GIA_THUOC");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DON_GIA_DICH_VU",
                table: "DON_GIA_DICH_VU");

            migrationBuilder.AlterColumn<DateTime>(
                name: "THOI_DIEM",
                table: "DON_GIA_THUOC",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldRowVersion: true)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "MA_THUOC",
                table: "DON_GIA_THUOC",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "THOI_DIEM",
                table: "DON_GIA_DICH_VU",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldRowVersion: true)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "MA_DICH_VU",
                table: "DON_GIA_DICH_VU",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DON_GIA_DICH_VU",
                table: "DON_GIA_DICH_VU",
                columns: new[] { "MA_DICH_VU", "THOI_DIEM" });

            migrationBuilder.AddForeignKey(
                name: "FK_DON_GIA__CO_DON_GI_THUOC_",
                table: "DON_GIA_THUOC",
                column: "MA_THUOC",
                principalTable: "THUOC",
                principalColumn: "MA_THUOC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DON_GIA__CO_DON_GI_THUOC_",
                table: "DON_GIA_THUOC");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DON_GIA_DICH_VU",
                table: "DON_GIA_DICH_VU");

            migrationBuilder.AlterColumn<DateTime>(
                name: "THOI_DIEM",
                table: "DON_GIA_THUOC",
                type: "datetime2",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "MA_THUOC",
                table: "DON_GIA_THUOC",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "THOI_DIEM",
                table: "DON_GIA_DICH_VU",
                type: "datetime2",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "MA_DICH_VU",
                table: "DON_GIA_DICH_VU",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DON_GIA_DICH_VU",
                table: "DON_GIA_DICH_VU",
                column: "MA_DICH_VU");

            migrationBuilder.AddForeignKey(
                name: "FK_DON_GIA__CO_DON_GI_THUOC",
                table: "DON_GIA_THUOC",
                column: "MA_THUOC",
                principalTable: "THUOC",
                principalColumn: "MA_THUOC");
        }
    }
}
