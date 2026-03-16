using FastFood.Models;
using FastFood.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CouponController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var coupons = _db.Coupons.ToList();
            return View(coupons);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                _db.Coupons.Add(coupon);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var coupon = _db.Coupons.Find(id);
            if (coupon == null) return NotFound();
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                _db.Coupons.Update(coupon);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var coupon = _db.Coupons.Find(id);
            if (coupon == null) return NotFound();
            return View(coupon);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var coupon = _db.Coupons.Find(id);
            if (coupon == null) return NotFound();
            _db.Coupons.Remove(coupon);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}