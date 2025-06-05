using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tronghieu_941.Models;

namespace tronghieu_941.Models
{
    public class Nganh
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNganh { get; set; } = string.Empty;
        public ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();
    }
}
