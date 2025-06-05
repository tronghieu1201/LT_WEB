using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace tronghieu_941.Migrations
{
    /// <inheritdoc />
    public partial class taomoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NganhHocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNganh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NganhHocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SinhViens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MSVV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNganh = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhViens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SinhViens_NganhHocs_MaNganh",
                        column: x => x.MaNganh,
                        principalTable: "NganhHocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NganhHocs",
                columns: new[] { "Id", "TenNganh" },
                values: new object[,]
                {
                    { 1, "Công nghệ thông tin" },
                    { 2, "Quản trị kinh doanh" },
                    { 3, "Du lịch" }
                });

            migrationBuilder.InsertData(
                table: "SinhViens",
                columns: new[] { "Id", "GioiTinh", "Hinh", "HoTen", "MSVV", "MaNganh", "NgaySinh" },
                values: new object[,]
                {
                    { 1, "Nam", "/ImageSinhVien/default.jpg", "Nguyễn Văn A", "SV001", 1, new DateTime(2002, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Nữ", "/ImageSinhVien/default.jpg", "Trần Thị B", "SV002", 2, new DateTime(2001, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaNganh",
                table: "SinhViens",
                column: "MaNganh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SinhViens");

            migrationBuilder.DropTable(
                name: "NganhHocs");
        }
    }
}
