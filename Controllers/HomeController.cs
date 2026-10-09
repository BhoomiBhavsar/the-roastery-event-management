using EventMVC.Attributes;
using EventMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EventMVC.Controllers
{
    [UserOrAnonymousOnly]
    public class HomeController : Controller
    {
        private readonly EventManagementContext _context;

        public HomeController(EventManagementContext context)
        {
            _context = context;
        }

       

        public async Task<IActionResult> Index()
        {
            ViewBag.EventTypes = await _context.EventTypes
                .Where(e => e.IsActive)
                .Take(6)
                .ToListAsync();

            ViewBag.EventPackages = await _context.EventPackages
                .Include(p => p.EventType)
                .Where(p => p.IsActive)
                .Take(6)
                .ToListAsync();

            //ViewBag.MenuItems = await _context.MenuItems
            //    .Where(m => m.IsActive)
            //    .Take(6)
            //    .ToListAsync();


            ViewBag.Areas = await _context.Areas
            .Where(a => a.IsActive)
            .OrderByDescending(a => a.Capacity)
            .Take(6)
            .ToListAsync();

            ViewBag.MenuItems = await _context.MenuItems
            .Where(m => m.IsActive)
            .GroupBy(m => m.Category)
            .Select(g => g.OrderByDescending(x => x.MenuItemId).First())
            .Take(6)
            .ToListAsync();

            ViewBag.Decorations = await _context.Decorations
                .Where(d => d.IsActive)
                .Take(6)
                .ToListAsync();

            ViewBag.AddOns = await _context.AddOns
                .Where(a => a.IsActive)
                .Take(6)
                .ToListAsync();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
