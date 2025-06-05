using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tronghieu_941.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace tronghieu_941.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string msvv)
        {
            if (string.IsNullOrEmpty(msvv))
            {
                ModelState.AddModelError("", "Vui lòng nhập mã sinh viên.");
                return View();
            }

            var sv = await _context.SinhViens.FirstOrDefaultAsync(s => s.MSVV == msvv);

            if (sv != null)
            {
                HttpContext.Session.SetString("UserMSVV", sv.MSVV);
                HttpContext.Session.SetString("UserHoTen", sv.HoTen);
                HttpContext.Session.SetInt32("UserId", sv.Id);

                return RedirectToAction("Index", "HocPhan");
            }
            else
            {
                ModelState.AddModelError("", "Mã sinh viên không tồn tại.");
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
