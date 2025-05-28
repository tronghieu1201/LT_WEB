using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace Buoi_5.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId);

            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Lập trình" },
                new Category { Id = 2, Name = "Văn học" },
                new Category { Id = 3, Name = "Khoa học" },
                new Category { Id = 4, Name = "Kinh tế" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Lập Trình C# Cơ Bản",
                    Author = "Nguyễn Văn A",
                    Price = 150000,
                    Description = "Cuốn sách giới thiệu lập trình C#.",
                    ImagePath = "/Content/ImageBooks/th (1).jpg",
                    CategoryId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "Toán Học Cao Cấp",
                    Author = "Trần Thị B",
                    Price = 200000.50m,
                    Description = "Kiến thức nâng cao về toán.",
                    ImagePath = "/Content/ImageBooks/th.jpg",
                    CategoryId = 3
                }
            );
        }
    }
}
