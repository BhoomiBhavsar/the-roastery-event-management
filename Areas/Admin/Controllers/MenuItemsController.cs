using EventMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class MenuItemsController : Controller
    {
        private readonly EventManagementContext _context;

        public MenuItemsController(EventManagementContext context)
        {
            _context = context;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuItems.ToListAsync());
        }

        // GET: MenuItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.MenuItemId == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // GET: MenuItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuItems/Create

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItem menuItem)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(menuItem);
                }

                _context.MenuItems.Add(menuItem);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(menuItem);
            }
        }
        // GET: MenuItems/Edit/5
      


        public async Task<IActionResult> Edit(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);

            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
       

        [HttpPost]
        public async Task<IActionResult> Edit(
            int MenuItemId,
            string ItemName,
            string Category,
            decimal Price,
            string? ImageUrl,
            bool IsActive)
        {
            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(x => x.MenuItemId == MenuItemId);

            if (menuItem == null)
            {
                return NotFound();
            }

            menuItem.ItemName = ItemName;
            menuItem.Category = Category;
            menuItem.Price = Price;
            menuItem.ImageUrl = ImageUrl;
            menuItem.IsActive = IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


      
        // GET: Admin/MenuItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.MenuItemId == id);

            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: Admin/MenuItems/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);

            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.MenuItemId == id);
        }
    }
}
