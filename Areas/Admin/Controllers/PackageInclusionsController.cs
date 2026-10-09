using EventMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class PackageInclusionsController : Controller
    {
        private readonly EventManagementContext _context;

        public PackageInclusionsController(EventManagementContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Display(int id)
        {
            var package = await _context.EventPackages
                .Include(x => x.EventType)
                .FirstOrDefaultAsync(x => x.PackageId == id);

            ViewBag.Package = package;

            ViewBag.MenuItems = await _context.PackageMenuItems
                .Where(x => x.PackageId == id)
                .Include(x => x.MenuItem)
                .ToListAsync();

            ViewBag.AddOns = await _context.PackageAddOns
                .Where(x => x.PackageId == id)
                .Include(x => x.AddOn)
                .ToListAsync();

            ViewBag.Decorations = await _context.PackageDecorations
                .Where(x => x.PackageId == id)
                .Include(x => x.Decoration)
                .ToListAsync();

            return View();
        }
        // ADD PAGE
        public async Task<IActionResult> Add(int id)
        {
            var package = await _context.EventPackages
                .FirstOrDefaultAsync(x => x.PackageId == id);

            ViewBag.Package = package;

            //ViewBag.MenuItems = await _context.MenuItems
            //    .Where(x => x.IsActive)
            //    .ToListAsync();

            var menuItems = await _context.MenuItems
             .Where(x => x.IsActive)
             .ToListAsync();

            ViewBag.MenuItems = menuItems;

            ViewBag.Categories = menuItems
                .Select(x => x.Category)
                .Distinct()
                .ToList();

            ViewBag.AddOns = await _context.AddOns
                .Where(x => x.IsActive)
                .ToListAsync();

            ViewBag.Decorations = await _context.Decorations
                .Where(x => x.IsActive)
                .ToListAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(
           int PackageId,
           List<int>? MenuItemIds,
           List<int>? AddOnIds,
           List<int>? DecorationIds)
        {
            if (MenuItemIds != null)
            {
                foreach (var item in MenuItemIds)
                {
                    bool exists = await _context.PackageMenuItems
                        .AnyAsync(x => x.PackageId == PackageId && x.MenuItemId == item);

                    if (!exists)
                    {
                        _context.PackageMenuItems.Add(new PackageMenuItem
                        {
                            PackageId = PackageId,
                            MenuItemId = item
                        });
                    }
                }
            }
            if (AddOnIds != null)
            {
                foreach (var item in AddOnIds)
                {
                    bool exists = await _context.PackageAddOns
                        .AnyAsync(x => x.PackageId == PackageId && x.AddOnId == item);

                    if (!exists)
                    {
                        _context.PackageAddOns.Add(new PackageAddOn
                        {
                            PackageId = PackageId,
                            AddOnId = item
                        });
                    }
                }
            }

            if (DecorationIds != null)
            {
                foreach (var item in DecorationIds)
                {
                    bool exists = await _context.PackageDecorations
                        .AnyAsync(x => x.PackageId == PackageId && x.DecorationId == item);

                    if (!exists)
                    {
                        _context.PackageDecorations.Add(new PackageDecoration
                        {
                            PackageId = PackageId,
                            DecorationId = item
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Display", new { id = PackageId });
        }

        // EDIT PAGE
        public async Task<IActionResult> Edit(int id)
        {
            var package = await _context.EventPackages
                .FirstOrDefaultAsync(x => x.PackageId == id);

            ViewBag.Package = package;

            //ViewBag.MenuItems = await _context.MenuItems.ToListAsync();
            var menuItems = await _context.MenuItems
             .Where(x => x.IsActive)
             .ToListAsync();

            ViewBag.MenuItems = menuItems;

            ViewBag.Categories = menuItems
                .Select(x => x.Category)
                .Distinct()
                .ToList();
            ViewBag.AddOns = await _context.AddOns.ToListAsync();
            ViewBag.Decorations = await _context.Decorations.ToListAsync();

            ViewBag.SelectedMenuItems = await _context.PackageMenuItems
                .Where(x => x.PackageId == id)
                .Select(x => x.MenuItemId)
                .ToListAsync();

            ViewBag.SelectedAddOns = await _context.PackageAddOns
                .Where(x => x.PackageId == id)
                .Select(x => x.AddOnId)
                .ToListAsync();

            ViewBag.SelectedDecorations = await _context.PackageDecorations
                .Where(x => x.PackageId == id)
                .Select(x => x.DecorationId)
                .ToListAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(
           int PackageId,
           List<int>? MenuItemIds,
           List<int>? AddOnIds,
           List<int>? DecorationIds)
        {
            var oldMenuItems = _context.PackageMenuItems
                .Where(x => x.PackageId == PackageId);

            _context.PackageMenuItems.RemoveRange(oldMenuItems);

            if (MenuItemIds != null)
            {
                foreach (var item in MenuItemIds)
                {
                    _context.PackageMenuItems.Add(new PackageMenuItem
                    {
                        PackageId = PackageId,
                        MenuItemId = item
                    });
                }
            }

            var oldAddOns = _context.PackageAddOns
                .Where(x => x.PackageId == PackageId);

            _context.PackageAddOns.RemoveRange(oldAddOns);
            if (AddOnIds != null)
            {
                foreach (var item in AddOnIds)
                {
                    _context.PackageAddOns.Add(new PackageAddOn
                    {
                        PackageId = PackageId,
                        AddOnId = item
                    });
                }
            }
            var oldDecorations = _context.PackageDecorations
               .Where(x => x.PackageId == PackageId);

            _context.PackageDecorations.RemoveRange(oldDecorations);

            if (DecorationIds != null)
            {
                foreach (var item in DecorationIds)
                {
                    _context.PackageDecorations.Add(new PackageDecoration
                    {
                        PackageId = PackageId,
                        DecorationId = item
                    });
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Display", new { id = PackageId });
        }
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var item = await _context.PackageMenuItems.FindAsync(id);

            int packageId = item.PackageId;

            _context.PackageMenuItems.Remove(item);

            await _context.SaveChangesAsync();

            return RedirectToAction("Display", new { id = packageId });
        }
        public async Task<IActionResult> DeleteAddOn(int id)
        {
            var item = await _context.PackageAddOns.FindAsync(id);

            int packageId = item.PackageId;

            _context.PackageAddOns.Remove(item);

            await _context.SaveChangesAsync();

            return RedirectToAction("Display", new { id = packageId });
        }
        // DELETE DECORATION
        public async Task<IActionResult> DeleteDecoration(int id)
        {
            var item = await _context.PackageDecorations.FindAsync(id);

            int packageId = item.PackageId;

            _context.PackageDecorations.Remove(item);

            await _context.SaveChangesAsync();

            return RedirectToAction("Display", new { id = packageId });
        }
    }
}
