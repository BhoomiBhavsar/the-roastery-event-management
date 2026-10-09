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
    public class AddOnsController : Controller
    {
        private readonly EventManagementContext _context;

        public AddOnsController(EventManagementContext context)
        {
            _context = context;
        }

        // GET: AddOns
        public async Task<IActionResult> Index()
        {
            return View(await _context.AddOns.ToListAsync());
        }

        // GET: AddOns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addOn = await _context.AddOns
                .FirstOrDefaultAsync(m => m.AddOnId == id);
            if (addOn == null)
            {
                return NotFound();
            }

            return View(addOn);
        }

        // GET: AddOns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AddOns/Create

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddOn addOn)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(addOn);
                }

                _context.AddOns.Add(addOn);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(addOn);
            }
        }

        // GET: AddOns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addOn = await _context.AddOns.FindAsync(id);
            if (addOn == null)
            {
                return NotFound();
            }
            return View(addOn);
        }

        // POST: AddOns/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AddOnId,AddOnName,Description,Price,IsActive,ImageUrl")] AddOn addOn)
        {
            if (id != addOn.AddOnId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(addOn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddOnExists(addOn.AddOnId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(addOn);
        }

        // GET: AddOns/Delete/5
       


     

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var addOn = await _context.AddOns.FindAsync(id);

            if (addOn == null)
                return NotFound();

            return View(addOn);
        }

      
        [Area("Admin")]
        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            var addOn = await _context.AddOns.FindAsync(id);

            if (addOn == null)
                return NotFound();

            _context.AddOns.Remove(addOn);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        private bool AddOnExists(int id)
        {
            return _context.AddOns.Any(e => e.AddOnId == id);
        }
    }
}
