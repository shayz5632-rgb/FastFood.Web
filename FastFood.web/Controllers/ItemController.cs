using FastFood.Models;
using FastFood.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemController(ApplicationDbContext db,
            IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var items = _db.Items
                .Include(i => i.Category)
                .Include(i => i.SubCategory)
                .ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(
                _db.Categories.ToList(), "Id", "Name");
            ViewBag.SubCategoryList = new SelectList(
                _db.SubCategories.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Item item, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    string uploadsFolder = Path.Combine(
                        _webHostEnvironment.WebRootPath, "images");
                    Directory.CreateDirectory(uploadsFolder);
                    string fileName = Guid.NewGuid().ToString() +
                        Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using var fileStream = new FileStream(
                        filePath, FileMode.Create);
                    image.CopyTo(fileStream);
                    item.Image = "/images/" + fileName;
                }
                _db.Items.Add(item);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryList = new SelectList(
                _db.Categories.ToList(), "Id", "Name");
            ViewBag.SubCategoryList = new SelectList(
                _db.SubCategories.ToList(), "Id", "Name");
            return View(item);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = _db.Items.Find(id);
            if (item == null) return NotFound();
            ViewBag.CategoryList = new SelectList(
                _db.Categories.ToList(), "Id", "Name");
            ViewBag.SubCategoryList = new SelectList(
                _db.SubCategories.ToList(), "Id", "Name");
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Item item, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    string uploadsFolder = Path.Combine(
                        _webHostEnvironment.WebRootPath, "images");
                    Directory.CreateDirectory(uploadsFolder);
                    string fileName = Guid.NewGuid().ToString() +
                        Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using var fileStream = new FileStream(
                        filePath, FileMode.Create);
                    image.CopyTo(fileStream);
                    item.Image = "/images/" + fileName;
                }
                _db.Items.Update(item);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryList = new SelectList(
                _db.Categories.ToList(), "Id", "Name");
            ViewBag.SubCategoryList = new SelectList(
                _db.SubCategories.ToList(), "Id", "Name");
            return View(item);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = _db.Items
                .Include(i => i.Category)
                .Include(i => i.SubCategory)
                .FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = _db.Items.Find(id);
            if (item == null) return NotFound();
            _db.Items.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}