using FastFood.Models;
using FastFood.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FastFood.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) return RedirectToAction("Login", "Account");
            var userId = claim.Value;

            var carts = _db.Carts
                .Include(c => c.Item)
                .Where(c => c.ApplicationUserId == userId)
                .ToList();

            double total = 0;
            foreach (var cart in carts)
            {
                total += cart.Item!.Price * cart.Count;
            }

            ViewBag.Total = total;
            return View(carts);
        }

        public IActionResult Add(int itemId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) return RedirectToAction("Login", "Account");
            var userId = claim.Value;

            var cart = _db.Carts.FirstOrDefault(c =>
                c.ApplicationUserId == userId && c.ItemId == itemId);

            if (cart == null)
            {
                cart = new Cart
                {
                    ApplicationUserId = userId,
                    ItemId = itemId,
                    Count = 1
                };
                _db.Carts.Add(cart);
            }
            else
            {
                cart.Count++;
                _db.Carts.Update(cart);
            }

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _db.Carts.Find(cartId);
            if (cart == null) return NotFound();

            if (cart.Count == 1)
            {
                _db.Carts.Remove(cart);
            }
            else
            {
                cart.Count--;
                _db.Carts.Update(cart);
            }

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _db.Carts.Find(cartId);
            if (cart == null) return NotFound();
            _db.Carts.Remove(cart);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}