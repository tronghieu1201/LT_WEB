using System.Diagnostics;
using Buoi_4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Buoi_4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Trang chủ"; 
            return View();
        }

        
        public IActionResult About()
        {
            ViewData["Title"] = "Giới thiệu";
            return View();
        }

       
        public IActionResult Cycle()
        {
            ViewData["Title"] = "Mẫu xe đạp";
            return View();
        }

        
        public IActionResult Shop()
        {
            ViewData["Title"] = "Cửa hàng";
            return View();
        }

        public IActionResult News()
        {
            ViewData["Title"] = "Tin tức";
            return View();
        }

       
        public IActionResult Contact()
        {
            ViewData["Title"] = "Liên hệ";
            return View();
        }

       
        [HttpPost]
        public IActionResult ProcessContact(string Name, string Email, string Phone, string Massage)
        {
            
            _logger.LogInformation($"Contact Form Submitted: Name={Name}, Email={Email}, Phone={Phone}, Message={Massage}");

            
            TempData["SuccessMessage"] = "Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm nhất.";
            return RedirectToAction("Contact");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}