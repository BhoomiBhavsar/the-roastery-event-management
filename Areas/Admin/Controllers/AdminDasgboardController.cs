using EventMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly EventManagementContext _context;

        public AdminDashboardController(EventManagementContext context)
        {
            _context = context;
        }

        // GET: Admin Dashboard
        public async Task<IActionResult> Index()
        {
            try
            {
                // Get statistics
                var eventTypesCount = await _context.EventTypes.CountAsync();
                var areasCount = await _context.Areas.CountAsync();
                var menuItemsCount = await _context.MenuItems.CountAsync();
                var decorationsCount = await _context.Decorations.CountAsync();
                var addOnsCount = await _context.AddOns.CountAsync();
                var packagesCount = await _context.EventPackages.CountAsync();

                // Store counts in ViewBag for dashboard display
                ViewBag.EventTypesCount = eventTypesCount;
                ViewBag.AreasCount = areasCount;
                ViewBag.MenuItemsCount = menuItemsCount;
                ViewBag.DecorationsCount = decorationsCount;
                ViewBag.AddOnsCount = addOnsCount;
                ViewBag.PackagesCount = packagesCount;

                return View();
            }
            catch (Exception ex)
            {
                // Log exception or handle as needed
                ViewBag.ErrorMessage = "An error occurred while loading the dashboard.";
                return View();
            }
        }

        // GET: Admin/Home - Redirect to Dashboard
        public IActionResult Home()
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
