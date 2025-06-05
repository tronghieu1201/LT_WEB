using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using tronghieu_941.Models;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace tronghieu_941.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<SinhVienController> _logger;

        public SinhVienController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<SinhVienController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // Hiển thị danh sách sinh viên
        public async Task<IActionResult> Index()
        {
            try
            {
                var sinhViens = await _context.SinhViens.Include(sv => sv.Nganh).ToListAsync();
                return View(sinhViens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading student list");
                return StatusCode(500, "Lỗi khi tải danh sách sinh viên");
            }
        }

        // GET: Tạo mới sinh viên
        public IActionResult Create()
        {
            ViewBag.Nganhs = new SelectList(_context.NganhHocs, "Id", "TenNganh");
            return View();
        }

        // POST: Tạo mới sinh viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SinhVien sinhVien, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid: {Errors}", string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                ViewBag.Nganhs = new SelectList(_context.NganhHocs, "Id", "TenNganh", sinhVien.MaNganh);
                return View(sinhVien);
            }

            try
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var folder = Path.Combine(_webHostEnvironment.WebRootPath, "ImageSinhVien");
                    Directory.CreateDirectory(folder);
                    var fileName = System.Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    sinhVien.Hinh = "/ImageSinhVien/" + fileName;
                }

                _context.Add(sinhVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student");
                ModelState.AddModelError("", "Lỗi khi thêm sinh viên. Vui lòng thử lại.");
                ViewBag.Nganhs = new SelectList(_context.NganhHocs, "Id", "TenNganh", sinhVien.MaNganh);
                return View(sinhVien);
            }
        }

        // GET: Chi tiết sinh viên
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var sinhVien = await _context.SinhViens
                    .Include(sv => sv.Nganh)
                    .FirstOrDefaultAsync(sv => sv.Id == id);

                if (sinhVien == null) return NotFound();

                return View(sinhVien);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading student details for ID {Id}", id);
                return StatusCode(500, "Lỗi khi tải chi tiết sinh viên");
            }
        }

        // GET: Chỉnh sửa sinh viên
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var sinhVien = await _context.SinhViens.FindAsync(id);
                if (sinhVien == null) return NotFound();

                ViewBag.Nganhs = new SelectList(_context.NganhHocs, "Id", "TenNganh", sinhVien.MaNganh);
                return View(sinhVien);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit page for student ID {Id}", id);
                return StatusCode(500, "Lỗi khi tải trang chỉnh sửa");
            }
        }

        // POST: Chỉnh sửa sinh viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SinhVien sinhVien, IFormFile? ImageFile)
        {
            if (id != sinhVien.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid: {Errors}", string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                ViewBag.Nganhs = new SelectList(_context.NganhHocs, "Id", "TenNganh", sinhVien.MaNganh);
                return View(sinhVien);
            }

            try
            {
                var existing = await _context.SinhViens.AsNoTracking().FirstOrDefaultAsync(sv => sv.Id == id);
                if (existing == null) return NotFound();

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Xóa file ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(existing.Hinh))
                    {
                        var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, existing.Hinh.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    var folder = Path.Combine(_webHostEnvironment.WebRootPath, "ImageSinhVien");
                    Directory.CreateDirectory(folder);
                    var fileName = System.Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    sinhVien.Hinh = "/ImageSinhVien/" + fileName;
                }
                else
                {
                    sinhVien.Hinh = existing.Hinh;
                }

                _context.Update(sinhVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.SinhViens.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student ID {Id}", id);
                ModelState.AddModelError("", "Lỗi khi cập nhật sinh viên. Vui lòng thử lại.");
                ViewBag.Nganhs = new SelectList(_context.NganhHocs, "Id", "TenNganh", sinhVien.MaNganh);
                return View(sinhVien);
            }
        }

        // GET: Xác nhận xóa
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var sinhVien = await _context.SinhViens
                    .Include(sv => sv.Nganh)
                    .FirstOrDefaultAsync(sv => sv.Id == id);

                if (sinhVien == null) return NotFound();

                return View(sinhVien);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete confirmation for student ID {Id}", id);
                return StatusCode(500, "Lỗi khi tải trang xác nhận xóa");
            }
        }

        // POST: Xác nhận xóa sinh viên
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var sinhVien = await _context.SinhViens.FindAsync(id);
                if (sinhVien != null)
                {
                    if (!string.IsNullOrEmpty(sinhVien.Hinh))
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, sinhVien.Hinh.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    _context.SinhViens.Remove(sinhVien);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student ID {Id}", id);
                return StatusCode(500, "Lỗi khi xóa sinh viên");
            }
        }
    }
}