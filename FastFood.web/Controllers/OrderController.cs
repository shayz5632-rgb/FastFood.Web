using FastFood.Models;
using FastFood.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FastFood.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
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

            var orderHeader = new OrderHeader
            {
                ApplicationUserId = userId,
                OrderDate = DateTime.Now,
                OrderTotalOriginal = total,
                OrderTotal = total,
                Status = "Pending",
                PaymentStatus = "Pending"
            };

            ViewBag.Carts = carts;
            ViewBag.Total = total;
            return View(orderHeader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(OrderHeader orderHeader)
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

            orderHeader.ApplicationUserId = userId;
            orderHeader.OrderDate = DateTime.Now;
            orderHeader.OrderTotalOriginal = total;
            orderHeader.OrderTotal = total;
            orderHeader.Status = "Pending";
            orderHeader.PaymentStatus = "Pending";

            _db.OrderHeaders.Add(orderHeader);
            _db.SaveChanges();

            foreach (var cart in carts)
            {
                var orderDetails = new OrderDetails
                {
                    OrderHeaderId = orderHeader.Id,
                    ItemId = cart.ItemId,
                    Count = cart.Count,
                    Name = cart.Item!.Title,
                    Price = cart.Item.Price
                };
                _db.OrderDetails.Add(orderDetails);
            }

            _db.Carts.RemoveRange(carts);
            _db.SaveChanges();

            return RedirectToAction("Confirmation",
                new { id = orderHeader.Id });
        }

        public IActionResult Confirmation(int id)
        {
            var orderHeader = _db.OrderHeaders.Find(id);
            return View(orderHeader);
        }

        public IActionResult MyOrders()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) return RedirectToAction("Login", "Account");
            var userId = claim.Value;

            var orders = _db.OrderHeaders
                .Where(o => o.ApplicationUserId == userId)
                .ToList();

            return View(orders);
        }
    }
}