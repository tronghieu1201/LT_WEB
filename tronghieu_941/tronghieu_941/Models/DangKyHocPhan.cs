using System;
using System.Collections.Generic;

namespace tronghieu_941.Models
{
    public class DangKyHocPhan
    {
        public int Id { get; set; }

        public string MSVV { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string Nganh { get; set; }

        public string HocPhansJson { get; set; } // Lưu các học phần dạng JSON
        public DateTime ThoiGianDangKy { get; set; } = DateTime.Now;
    }
}
