

using EventMVC.Attributes;
using EventMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventMVC.Controllers
{
    public class InquiriesController : Controller
    {
        private readonly EventManagementContext _context;

        public InquiriesController(EventManagementContext context)
        {
            _context = context;
        }
        [UserOrAnonymousOnly]
        // GET: Inquiries
        public async Task<IActionResult> Index()
        {
            var eventTypes = await _context.EventTypes
                .Where(e => e.IsActive)
                .ToListAsync();

            ViewBag.EventTypes = eventTypes;

            return View();
        }

        // GET: My Inquiries
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyInquiries()
        {
            var inquiries = await _context.Inquiries
                .Include(i => i.EventType)
                .Include(i => i.InquiryReplies)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return View(inquiries);
        }
        [Authorize(Roles = "User")]
        // POST: Submit Inquiry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(
            string name,
            string email,
            string phoneNumber,
            int eventTypeId,
            DateOnly eventDate,
            int guestCount,
            string message)
        {
            Inquiry inquiry = new Inquiry
            {
                UserId = 1, // Keep as it is for now
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,

                // FIXED
                EventTypeId = eventTypeId,

                EventDate = eventDate,
                GuestCount = guestCount,
                Message = message,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.Inquiries.Add(inquiry);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Inquiry submitted successfully.";

            return RedirectToAction("Index", "Inquiries");
        }
    }
}