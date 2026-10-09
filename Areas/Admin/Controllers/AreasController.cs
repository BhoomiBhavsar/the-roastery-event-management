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
    public class AreasController : Controller
    {
        private readonly EventManagementContext _context;

        public AreasController(EventManagementContext context)
        {
            _context = context;
        }

        // GET: Areas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Areas.ToListAsync());
        }

        // GET: Areas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas
                .FirstOrDefaultAsync(m => m.AreaId == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // GET: Areas/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Areas/Create
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
             string AreaName,
             int Capacity,
             decimal PricePerGuest,
             bool IsActive,
             string? ImageUrl)
        {
            Area area = new Area();

            area.AreaName = AreaName;
            area.Capacity = Capacity;
            area.PricePerGuest = PricePerGuest;
            area.IsActive = IsActive;
            area.ImageUrl = ImageUrl;

            _context.Areas.Add(area);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: Areas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            return View(area);
        }

        // POST: Areas/Edit/5
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
             int AreaId,
             string AreaName,
             int Capacity,
             decimal PricePerGuest,
             string? ImageUrl,
             bool IsActive)
        {
            var existingArea = await _context.Areas
                .FirstOrDefaultAsync(x => x.AreaId == AreaId);

            if (existingArea == null)
            {
                return NotFound();
            }

            existingArea.AreaName = AreaName;
            existingArea.Capacity = Capacity;
            existingArea.PricePerGuest = PricePerGuest;
            existingArea.ImageUrl = ImageUrl;
            existingArea.IsActive = IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        // GET: Areas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas
                .FirstOrDefaultAsync(m => m.AreaId == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var area = await _context.Areas.FindAsync(id);

            if (area != null)
            {
                _context.Areas.Remove(area);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        private bool AreaExists(int id)
        {
            return _context.Areas.Any(e => e.AreaId == id);
        }
    }
}
