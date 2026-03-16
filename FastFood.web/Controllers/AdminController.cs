using FastFood.Models;
using FastFood.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        // All Orders
        public IActionResult Orders()
        {
            var orders = _db.OrderHeaders
                .Include(o => o.ApplicationUser)
                .ToList();
            return View(orders);
        }

        // Order Details
        public IActionResult OrderDetails(int id)
        {
            var orderHeader = _db.OrderHeaders
                .Include(o => o.ApplicationUser)
                .FirstOrDefault(o => o.Id == id);

            var orderDetails = _db.OrderDetails
                .Include(o => o.Item)
                .Where(o => o.OrderHeaderId == id)
                .ToList();

            ViewBag.OrderDetails = orderDetails;
            return View(orderHeader);
        }

        // Update Status
        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _db.OrderHeaders.Find(id);
            if (order == null) return NotFound();
            order.Status = status;
            _db.SaveChanges();
            return RedirectToAction(nameof(Orders));
        }

        // Dashboard
        public IActionResult Dashboard()
        {
            ViewBag.TotalOrders = _db.OrderHeaders.Count();
            ViewBag.TotalItems = _db.Items.Count();
            ViewBag.TotalUsers = _db.ApplicationUsers.Count();
            ViewBag.TotalRevenue = _db.OrderHeaders
                .Sum(o => o.OrderTotal);
            return View();
        }
    }
}
