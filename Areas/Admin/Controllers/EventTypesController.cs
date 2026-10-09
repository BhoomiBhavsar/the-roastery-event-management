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
    public class EventTypesController : Controller
    {
        private readonly EventManagementContext _context;

        public EventTypesController(EventManagementContext context)
        {
            _context = context;
        }

        // GET: EventTypes
        public IActionResult Index()
        {
            var data = _context.EventTypes.ToList();

            return View(data);
        }
        // GET: EventTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventType = await _context.EventTypes
                .FirstOrDefaultAsync(m => m.EventTypeId == id);
            if (eventType == null)
            {
                return NotFound();
            }

            return View(eventType);
        }

        // GET: EventTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventTypes/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventType eventType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(eventType);
                }

                _context.EventTypes.Add(eventType);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(eventType);
            }
        }

        // GET: EventTypes/Edit/5
       


        public async Task<IActionResult> Edit(int id)
        {
            var eventType = await _context.EventTypes.FindAsync(id);

            if (eventType == null)
            {
                return NotFound();
            }

            return View(eventType);
        }

        // POST: EventTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventTypeId,Name,Description,IsActive,ImageUrl")] EventType eventType)
        {
            if (id != eventType.EventTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventTypeExists(eventType.EventTypeId))
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
            return View(eventType);
        }

       
       
        // GET: EventTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventType = await _context.EventTypes
                .FirstOrDefaultAsync(m => m.EventTypeId == id);

            if (eventType == null)
            {
                return NotFound();
            }

            return View(eventType);
        }

        // POST: EventTypes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var eventType = await _context.EventTypes.FindAsync(id);

            if (eventType != null)
            {
                _context.EventTypes.Remove(eventType);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventTypeExists(int id)
        {
            return _context.EventTypes.Any(e => e.EventTypeId == id);
        }
    }
}
