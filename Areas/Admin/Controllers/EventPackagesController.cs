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
    public class EventPackagesController : Controller
    {
        private readonly EventManagementContext _context;

        public EventPackagesController(EventManagementContext context)
        {
            _context = context;
        }

        // GET: EventPackages
        public async Task<IActionResult> Index()
        {
            var eventManagementContext = _context.EventPackages.Include(e => e.EventType);
            return View(await eventManagementContext.ToListAsync());
        }

        // GET: EventPackages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPackage = await _context.EventPackages
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.PackageId == id);
            if (eventPackage == null)
            {
                return NotFound();
            }

            return View(eventPackage);
        }

        // GET: EventPackages/Create
       
        public IActionResult Create()
        {
            ViewBag.EventTypeList = _context.EventTypes
                .Select(e => new SelectListItem
                {
                    Value = e.EventTypeId.ToString(),
                    Text = e.Name
                }).ToList();

            return View();
        }

        // POST: EventPackages/Create
      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventPackage eventPackage)
        {
            if (!ModelState.IsValid)
            {
                string errors = "";

                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        errors += item.Key + " : " + error.ErrorMessage + "\n";
                    }
                }

                return Content(errors);
            }

            _context.EventPackages.Add(eventPackage);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: EventPackages/Edit/5
       

        public async Task<IActionResult> Edit(int id)
        {
            var eventPackage = await _context.EventPackages.FindAsync(id);

            if (eventPackage == null)
            {
                return NotFound();
            }

            ViewBag.EventTypeId = _context.EventTypes.ToList();

            return View(eventPackage);
        }


        // POST: EventPackages/Edit/5

       
        [HttpPost]
                public async Task<IActionResult> Edit(
            int PackageId,
            int EventTypeId,
            string PackageName,
            string? Description,
            decimal PricePerGuest,
            string? ImageUrl,
            bool IsActive)
        {
            var eventPackage = await _context.EventPackages
                .FirstOrDefaultAsync(x => x.PackageId == PackageId);

            if (eventPackage == null)
            {
                return NotFound();
            }

            eventPackage.EventTypeId = EventTypeId;
            eventPackage.PackageName = PackageName;
            eventPackage.Description = Description;
            eventPackage.PricePerGuest = PricePerGuest;
            eventPackage.ImageUrl = ImageUrl;
            eventPackage.IsActive = IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: EventPackages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPackage = await _context.EventPackages
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.PackageId == id);
            if (eventPackage == null)
            {
                return NotFound();
            }

            return View(eventPackage);
        }

      
        // POST: EventPackages/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var eventPackage = await _context.EventPackages.FindAsync(id);

            if (eventPackage != null)
            {
                _context.EventPackages.Remove(eventPackage);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventPackageExists(int id)
        {
            return _context.EventPackages.Any(e => e.PackageId == id);
        }
    }
}
