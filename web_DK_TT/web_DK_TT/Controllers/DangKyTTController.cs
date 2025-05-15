using Microsoft.AspNetCore.Mvc;
using web_DK_TT.Models;

namespace web_DK_TT.Controllers
{
    public class DangKyTTController : Controller
    {
        private static List<SinhVien> danhSachSV = new List<SinhVien>();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShowKQ(SinhVien sv)
        {
            danhSachSV.Add(sv);
            int soLuong = danhSachSV.Count(x => x.ChuyenNganh == sv.ChuyenNganh);
            ViewBag.SoLuong = soLuong;
            return View(sv);
        }
    }
}
