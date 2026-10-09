using EventMVC.Attributes;
using EventMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventMVC.Controllers
{
    [UserOrAnonymousOnly]
    public class EventTypesController : Controller
    {
        private readonly EventManagementContext _context;

        public EventTypesController(EventManagementContext context)
        {
            _context = context;
        }

        // Display all event types
        public async Task<IActionResult> Index()
        {
            var eventTypes = await _context.EventTypes
                .Where(e => e.IsActive)
                .ToListAsync();
            return View(eventTypes);
        }

        // Display packages for selected event type
        public async Task<IActionResult> Packages(int id)
        {
            var eventType = await _context.EventTypes
                .FirstOrDefaultAsync(e => e.EventTypeId == id && e.IsActive);

            if (eventType == null)
            {
                return NotFound();
            }

            var packages = await _context.EventPackages
                .Include(p => p.EventType)
                .Where(p => p.EventTypeId == id && p.IsActive)
                .ToListAsync();

            ViewBag.EventTypeName = eventType.Name;
            ViewBag.EventTypeId = id;
            return View(packages);
        }

        // Display package details including decorations, menu items, and addons
        public async Task<IActionResult> PackageDetails(int id)
        {
            var package = await _context.EventPackages
                .Include(p => p.EventType)
                .Include(p => p.PackageMenuItems)
                    .ThenInclude(pm => pm.MenuItem)
                .Include(p => p.PackageDecorations)
                    .ThenInclude(pd => pd.Decoration)
                .Include(p => p.PackageAddOns)
                    .ThenInclude(pa => pa.AddOn)
                .FirstOrDefaultAsync(p => p.PackageId == id);

            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }
    }
}
