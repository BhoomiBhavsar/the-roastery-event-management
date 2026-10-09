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
    public class DecorationsController : Controller
    {
        private readonly EventManagementContext _context;

        public DecorationsController(EventManagementContext context)
        {
            _context = context;
        }

        // GET: Decorations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Decorations.ToListAsync());
        }

        // GET: Decorations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var decoration = await _context.Decorations
                .FirstOrDefaultAsync(m => m.DecorationId == id);
            if (decoration == null)
            {
                return NotFound();
            }

            return View(decoration);
        }

        // GET: Decorations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Decorations/Create
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Decoration decoration)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(decoration);
                }

                _context.Decorations.Add(decoration);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(decoration);
            }
        }

        // GET: Decorations/Edit/5

        public async Task<IActionResult> Edit(int id)
        {
            var decoration = await _context.Decorations
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DecorationId == id);

            if (decoration == null)
            {
                return NotFound();
            }

            return View(decoration);
        }



        // POST: Decorations/Edit/5
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
             int DecorationId,
             string DecorationName,
             decimal Price,
             string? ImageUrl,
             bool IsActive)
        {
            var decoration = await _context.Decorations.FindAsync(DecorationId);

            if (decoration == null)
            {
                return NotFound();
            }

            decoration.DecorationName = DecorationName;
            decoration.Price = Price;
            decoration.ImageUrl = ImageUrl;
            decoration.IsActive = IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: Decorations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var decoration = await _context.Decorations
                .FirstOrDefaultAsync(m => m.DecorationId == id);
            if (decoration == null)
            {
                return NotFound();
            }

            return View(decoration);
        }

        
        // POST: Decorations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var decoration = await _context.Decorations.FindAsync(id);

            if (decoration != null)
            {
                _context.Decorations.Remove(decoration);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DecorationExists(int id)
        {
            return _context.Decorations.Any(e => e.DecorationId == id);
        }
    }
}
