using System;
using System.ComponentModel.DataAnnotations;
using tronghieu_941.Models;

namespace tronghieu_941.Models
{
    public class SinhVien
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string MSVV { get; set; } = string.Empty;  // Mã SV (khóa chính logic)

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [StringLength(5)]
        public string GioiTinh { get; set; } = string.Empty;

        [Required]
        public DateTime NgaySinh { get; set; }

        public string? Hinh { get; set; }

        [Required]
        public int MaNganh { get; set; }  // khóa ngoại

        public Nganh? Nganh { get; set; }
    }
}
