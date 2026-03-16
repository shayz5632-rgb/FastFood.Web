using FastFood.Models;
using FastFood.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var subCategories = _db.SubCategories
                .Include(s => s.Category)
                .ToList();
            return View(subCategories);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(
                _db.Categories.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                _db.SubCategories.Add(subCategory);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryList = new SelectList(
                _db.Categories.ToList(), "Id", "Name");
            return View(subCategory);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var subCategory = _db.SubCategories.Find(id);
            if (subCategory == null) return NotFound();
            ViewBag.CategoryList = new SelectList(
                _db.Categories.ToList(), "Id", "Name");
            return View(subCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                _db.SubCategories.Update(subCategory);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryList = new SelectList(
                _db.Categories.ToList(), "Id", "Name");
            return View(subCategory);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var subCategory = _db.SubCategories
                .Include(s => s.Category)
                .FirstOrDefault(s => s.Id == id);
            if (subCategory == null) return NotFound();
            return View(subCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var subCategory = _db.SubCategories.Find(id);
            if (subCategory == null) return NotFound();
            _db.SubCategories.Remove(subCategory);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}