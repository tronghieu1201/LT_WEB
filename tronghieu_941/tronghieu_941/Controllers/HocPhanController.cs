using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using tronghieu_941.Models;
using System.Collections.Generic;

namespace tronghieu_941.Controllers
{
    public class HocPhanController : Controller
    {
        private static readonly List<HocPhan> AllHocPhans = new List<HocPhan>()
        {
            new HocPhan { MaHocPhan = "01", TenHocPhan = "Lập trình C", SoTinChi = 3 },
            new HocPhan { MaHocPhan = "02", TenHocPhan = "Cơ sở dữ liệu", SoTinChi = 4 },
            new HocPhan { MaHocPhan = "03", TenHocPhan = "Mạng máy tính", SoTinChi = 3 },
        };

        private bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserMSVV"));
        }

        private List<HocPhan> GetRegisteredCourses()
        {
            var userMSVV = HttpContext.Session.GetString("UserMSVV");
            if (string.IsNullOrEmpty(userMSVV)) return new List<HocPhan>();

            var json = HttpContext.Session.GetString("RegisteredCourses_" + userMSVV);
            return string.IsNullOrEmpty(json)
                ? new List<HocPhan>()
                : JsonSerializer.Deserialize<List<HocPhan>>(json) ?? new List<HocPhan>();
        }

        private void SaveRegisteredCourses(List<HocPhan> courses)
        {
            var userMSVV = HttpContext.Session.GetString("UserMSVV");
            if (string.IsNullOrEmpty(userMSVV)) return;

            var json = JsonSerializer.Serialize(courses);
            HttpContext.Session.SetString("RegisteredCourses_" + userMSVV, json);
        }

        public IActionResult Index()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            return View(AllHocPhans);
        }

        public IActionResult DangKy(string maHocPhan)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var registered = GetRegisteredCourses();
            var hp = AllHocPhans.Find(x => x.MaHocPhan == maHocPhan);

            if (hp != null && !registered.Exists(x => x.MaHocPhan == maHocPhan))
            {
                registered.Add(hp);
                SaveRegisteredCourses(registered);
                TempData["SuccessMessage"] = $"Đăng ký học phần \"{hp.TenHocPhan}\" thành công.";
            }

            return RedirectToAction("DanhSachDangKy");
        }

        public IActionResult DanhSachDangKy()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var registered = GetRegisteredCourses();
            return View(registered);
        }

        public IActionResult XoaHocPhan(string maHocPhan)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var registered = GetRegisteredCourses();
            registered.RemoveAll(x => x.MaHocPhan == maHocPhan);
            SaveRegisteredCourses(registered);

            return RedirectToAction("DanhSachDangKy");
        }

        public IActionResult XoaDangKy()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var userMSVV = HttpContext.Session.GetString("UserMSVV");
            HttpContext.Session.Remove("RegisteredCourses_" + userMSVV);

            return RedirectToAction("DanhSachDangKy");
        }

    }
}
