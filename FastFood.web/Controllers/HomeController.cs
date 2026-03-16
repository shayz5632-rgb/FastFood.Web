using FastFood.Models;
using FastFood.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var items = _db.Items
                .Include(i => i.Category)
                .Include(i => i.SubCategory)
                .ToList();
            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}