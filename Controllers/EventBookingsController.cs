using EventMVC.Attributes;
using EventMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EventMVC.Controllers
{
  
    public class EventBookingsController : Controller
    {
        private readonly EventManagementContext _context;

        public EventBookingsController(EventManagementContext context)
        {
            _context = context;
        }

        // Display predefined packages for booking
        [UserOrAnonymousOnly]
        public async Task<IActionResult> PredefinedPackages()
        {
            var packages = await _context.EventPackages
                .Include(p => p.EventType)
                .Where(p => p.IsActive)
                .ToListAsync();

            return View(packages);
        }

        // Book a predefined package
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookPredefined(int packageId)
        {
            var package = await _context.EventPackages
                .Include(p => p.EventType)
                .FirstOrDefaultAsync(p => p.PackageId == packageId && p.IsActive);

            if (package == null)
            {
                return NotFound();
            }

            var areas = await _context.Areas.Where(a => a.IsActive).ToListAsync();
            ViewBag.Areas = areas;

            return View(package);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePredefinedBooking(
            int packageId,
            DateOnly eventDate,
            TimeOnly startTime,
            TimeOnly endTime,
            int guestCount,
            int areaId)
        {
            //var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var package = await _context.EventPackages
                .FirstOrDefaultAsync(p => p.PackageId == packageId);

            var area = await _context.Areas
                .FirstOrDefaultAsync(a => a.AreaId == areaId);

            if (package == null || area == null)
            {
                return BadRequest("Package or Area not found");
            }
            // Validate guest count against area capacity
            if (guestCount > area.Capacity)
            {
                TempData["Error"] = $"Selected area capacity is only {area.Capacity} guests.";
                return RedirectToAction("BookPredefined", new { packageId = packageId });
            }

            decimal packageTotal = package.PricePerGuest * guestCount;
            decimal areaTotal = area.PricePerGuest * guestCount;

            decimal subtotal = packageTotal + areaTotal;
            decimal tax = subtotal * 0.05m;
            decimal total = subtotal + tax;

            var booking = new EventBooking
            {
                UserId = userId,
                EventTypeId = package.EventTypeId,
                PackageId = package.PackageId,
                AreaId = area.AreaId,
                EventDate = eventDate,
                StartTime = startTime,
                EndTime = endTime,
                GuestCount = guestCount,
                SubTotalAmount = subtotal,
                TaxAmount = tax,
                TotalAmount = total,
                BookingMode = "Predefined",
                BookingStatus = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.EventBookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Booking submitted successfully. Waiting for admin approval.";

            return RedirectToAction("BookingPending", new { bookingId = booking.BookingId });
        }



        // Custom event booking - Step 1: Select event type
        [UserOrAnonymousOnly]
        public async Task<IActionResult> CustomEvent()
        {
            var eventTypes = await _context.EventTypes.Where(e => e.IsActive).ToListAsync();
            return View(eventTypes);
        }

        // GET: Custom event customization form

        [Authorize(Roles = "User")]
        public async Task<IActionResult> CustomizeEvent(int eventTypeId)
        {
            var eventType = await _context.EventTypes.FirstOrDefaultAsync(e => e.EventTypeId == eventTypeId && e.IsActive);
            if (eventType == null)
            {
                return NotFound();
            }

            var menuItems = await _context.MenuItems.Where(m => m.IsActive).ToListAsync();
            var decorations = await _context.Decorations.Where(d => d.IsActive).ToListAsync();
            var addOns = await _context.AddOns.Where(a => a.IsActive).ToListAsync();
            var areas = await _context.Areas.Where(a => a.IsActive).ToListAsync();

            ViewBag.MenuItems = menuItems;
            ViewBag.Decorations = decorations;
            ViewBag.AddOns = addOns;
            ViewBag.Areas = areas;
            ViewBag.EventTypeId = eventTypeId;

            return View(eventType);
        }

        // POST: Save custom event booking

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
                public async Task<IActionResult> SaveCustomBooking(
            int eventTypeId,
            DateOnly eventDate,
            TimeOnly startTime,
            TimeOnly endTime,
            int guestCount,
            int areaId,
            string selectedMenuItems,
            string selectedDecorations,
            string selectedAddOns)
        {
            //var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var eventType = await _context.EventTypes
                .FirstOrDefaultAsync(e => e.EventTypeId == eventTypeId);

            var area = await _context.Areas
                .FirstOrDefaultAsync(a => a.AreaId == areaId);

            if (eventType == null || area == null)
            {
                return BadRequest("Event Type or Area not found");
            }
            // Validate guest count against area capacity
            //if (guestCount > area.Capacity)
            //{
            //    TempData["Error"] = $"Selected area capacity is only {area.Capacity} guests.";
            //    return RedirectToAction("CustomizeEvent", new { eventTypeId = eventTypeId });
            //}
            ModelState.AddModelError("", "Selected area capacity is only {area.Capacity} guests.");
            var menuItemIds = string.IsNullOrEmpty(selectedMenuItems)
                ? new List<int>()
                : selectedMenuItems.Split(',').Select(int.Parse).ToList();
            //if (menuItemIds.Count < 5)
            //{
            //    TempData["Error"] = "Please select at least 5 menu items.";

            //    return RedirectToAction("CustomizeEvent",
            //        new { eventTypeId = eventTypeId });
            //}
            ModelState.AddModelError("", "Please select at least 5 menu items.");

            var decorationIds = string.IsNullOrEmpty(selectedDecorations)
                ? new List<int>()
                : selectedDecorations.Split(',').Select(int.Parse).ToList();

            var addOnIds = string.IsNullOrEmpty(selectedAddOns)
                ? new List<int>()
                : selectedAddOns.Split(',').Select(int.Parse).ToList();

            var menuItems = await _context.MenuItems
                .Where(m => menuItemIds.Contains(m.MenuItemId))
                .ToListAsync();

            var decorations = await _context.Decorations
                .Where(d => decorationIds.Contains(d.DecorationId))
                .ToListAsync();

            var addOns = await _context.AddOns
                .Where(a => addOnIds.Contains(a.AddOnId))
                .ToListAsync();

            decimal menuCost = menuItems.Sum(m => m.Price) * guestCount;
            decimal decorationCost = decorations.Sum(d => d.Price);
            decimal addOnCost = addOns.Sum(a => a.Price);
            decimal areaCost = area.PricePerGuest * guestCount;

            decimal subtotal = menuCost + decorationCost + addOnCost + areaCost;
            decimal tax = subtotal * 0.05m;
            decimal total = subtotal + tax;

            var booking = new EventBooking
            {
                UserId = userId,
                EventTypeId = eventTypeId,
                PackageId = null,
                AreaId = areaId,
                EventDate = eventDate,
                StartTime = startTime,
                EndTime = endTime,
                GuestCount = guestCount,
                SubTotalAmount = subtotal,
                TaxAmount = tax,
                TotalAmount = total,
                BookingMode = "Custom",
                BookingStatus = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.EventBookings.Add(booking);
            await _context.SaveChangesAsync();

            foreach (var item in menuItems)
            {
                _context.BookingMenuItems.Add(new BookingMenuItem
                {
                    BookingId = booking.BookingId,
                    MenuItemId = item.MenuItemId,
                    Quantity = 1
                });
            }

            foreach (var decoration in decorations)
            {
                _context.BookingDecorations.Add(new BookingDecoration
                {
                    BookingId = booking.BookingId,
                    DecorationId = decoration.DecorationId
                });
            }

            foreach (var addOn in addOns)
            {
                _context.BookingAddOns.Add(new BookingAddOn
                {
                    BookingId = booking.BookingId,
                    AddOnId = addOn.AddOnId,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Custom booking submitted successfully. Waiting for admin approval.";

            return RedirectToAction("BookingPending", new { bookingId = booking.BookingId });
        }


        // Display booking invoice

        //[Authorize]
        //public async Task<IActionResult> Invoice(int bookingId)
        //{
        //    var booking = await _context.EventBookings
        //        .Include(b => b.User)
        //        .Include(b => b.EventType)
        //        .Include(b => b.Area)
        //        .Include(b => b.Package)
        //        .Include(b => b.BookingMenuItems)
        //            .ThenInclude(bm => bm.MenuItem)
        //        .Include(b => b.BookingDecorations)
        //            .ThenInclude(bd => bd.Decoration)
        //        .Include(b => b.BookingAddOns)
        //            .ThenInclude(ba => ba.AddOn)
        //        .Include(b => b.Invoices)
        //        .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }

        //    if (booking.BookingStatus != "Approved")
        //    {
        //        TempData["Error"] = "Invoice will be available after admin approval.";

        //        return RedirectToAction("MyBookings");
        //    }

        //    return View(booking);
        //}


        [Authorize(Roles = "User")]
        public async Task<IActionResult> Invoice(int bookingId)
        {
            var booking = await _context.EventBookings
    .Include(b => b.User)
    .Include(b => b.EventType)
    .Include(b => b.Area)

    .Include(b => b.Package)
        .ThenInclude(p => p.PackageMenuItems)
            .ThenInclude(pm => pm.MenuItem)

    .Include(b => b.Package)
        .ThenInclude(p => p.PackageDecorations)
            .ThenInclude(pd => pd.Decoration)

    .Include(b => b.Package)
        .ThenInclude(p => p.PackageAddOns)
            .ThenInclude(pa => pa.AddOn)

    .Include(b => b.BookingMenuItems)
        .ThenInclude(bm => bm.MenuItem)

    .Include(b => b.BookingDecorations)
        .ThenInclude(bd => bd.Decoration)

    .Include(b => b.BookingAddOns)
        .ThenInclude(ba => ba.AddOn)

    .FirstOrDefaultAsync(b => b.BookingId == bookingId);
            if (booking == null)
                return NotFound();

            if (booking.BookingStatus != "Approved")
            {
                TempData["Error"] = "Invoice will be available after admin approval.";
                return RedirectToAction(nameof(MyBookings));
            }

            return View(booking);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookingPending(int bookingId)
        {
            var booking = await _context.EventBookings
                .Include(b => b.EventType)
                .Include(b => b.Package)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        // View all user bookings
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyBookings()
        {
            //var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (!int.TryParse(userIdString, out int userId))
            //{
            //    return Unauthorized();
            //}

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var bookings = await _context.EventBookings
                .Include(b => b.EventType)
                .Include(b => b.Package)
                .Include(b => b.Area)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }
    }
}
