using Microsoft.EntityFrameworkCore;
using tronghieu_941.Models;
using System;
using tronghieu_941.Models;

namespace tronghieu_941.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Nganh> NganhHocs { get; set; }
        public DbSet<DangKyHocPhan> DangKyHocPhans { get; set; }

        public DbSet<SinhVien> SinhViens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quan hệ 1-n giữa NganhHoc và SinhVien
            modelBuilder.Entity<SinhVien>()
                .HasOne(sv => sv.Nganh)
                .WithMany(ng => ng.SinhViens)
                .HasForeignKey(sv => sv.MaNganh)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data cho NganhHoc
            modelBuilder.Entity<Nganh>().HasData(
                new Nganh { Id = 1, TenNganh = "Công nghệ thông tin" },
                new Nganh { Id = 2, TenNganh = "Quản trị kinh doanh" },
                new Nganh { Id = 3, TenNganh = "Du lịch" }
            );

            // Seed data cho SinhVien
            modelBuilder.Entity<SinhVien>().HasData(
                new SinhVien
                {
                    Id = 1,
                    MSVV = "SV001",
                    HoTen = "Nguyễn Văn A",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(2002, 5, 10),
                    Hinh = "/ImageSinhVien/th.jpg",
                    MaNganh = 1
                },
                new SinhVien
                {
                    Id = 2,
                    MSVV = "SV002",
                    HoTen = "Trần Thị B",
                    GioiTinh = "Nữ",
                    NgaySinh = new DateTime(2001, 8, 20),
                    Hinh = "/ImageSinhVien/default.jpg",
                    MaNganh = 2
                }
            );
        }
    }
}
