//using EventMVC.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace EventMVC.Controllers
//{
//    [Authorize]
//    public class AdminBookingsController : Controller
//    {
//        private readonly EventManagementContext _context;

//        public AdminBookingsController(EventManagementContext context)
//        {
//            _context = context;
//        }

//        // Show all bookings
//        public async Task<IActionResult> Index()
//        {
//            var bookings = await _context.EventBookings
//                .Include(b => b.User)
//                .Include(b => b.EventType)
//                .Include(b => b.Package)
//                .Include(b => b.Area)
//                .OrderByDescending(b => b.CreatedAt)
//                .ToListAsync();

//            return View(bookings);
//        }

//        // Approve Booking
//        public async Task<IActionResult> Approve(int id)
//        {
//            var booking = await _context.EventBookings
//                .FirstOrDefaultAsync(b => b.BookingId == id);

//            if (booking == null)
//            {
//                return NotFound();
//            }

//            booking.BookingStatus = "Approved";

//            await _context.SaveChangesAsync();

//            // Generate invoice only after approval
//            var existingInvoice = await _context.Invoices
//                .FirstOrDefaultAsync(i => i.BookingId == booking.BookingId);

//            if (existingInvoice == null)
//            {
//                var invoice = new Invoice
//                {
//                    BookingId = booking.BookingId,
//                    TotalAmount = booking.TotalAmount,
//                    InvoiceDate = DateTime.Now
//                };

//                _context.Invoices.Add(invoice);
//                await _context.SaveChangesAsync();
//            }

//            TempData["Success"] = "Booking approved successfully.";

//            return RedirectToAction(nameof(Index));
//        }

//        // Reject Booking
//        public async Task<IActionResult> Reject(int id)
//        {
//            var booking = await _context.EventBookings
//                .FirstOrDefaultAsync(b => b.BookingId == id);

//            if (booking == null)
//            {
//                return NotFound();
//            }

//            booking.BookingStatus = "Rejected";

//            await _context.SaveChangesAsync();

//            TempData["Error"] = "Booking rejected.";

//            return RedirectToAction(nameof(Index));
//        }
//    }
//}


using EventMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminBookingsController : Controller
    {
        private readonly EventManagementContext _context;

        public AdminBookingsController(EventManagementContext context)
        {
            _context = context;
        }

        // Show all bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.EventBookings
                .Include(b => b.User)
                .Include(b => b.EventType)
                .Include(b => b.Package)
                .Include(b => b.Area)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        // Approve Booking
        //        public async Task<IActionResult> Approve(int id)
        //        {
        //            var booking = await _context.EventBookings
        //                .FirstOrDefaultAsync(b => b.BookingId == id);

        //            if (booking == null)
        //            {
        //                return NotFound();
        //            }

        //            booking.BookingStatus = "Approved";

        //            await _context.SaveChangesAsync();

        //            // Generate invoice only after approval
        //            var existingInvoice = await _context.Invoices
        //                .FirstOrDefaultAsync(i => i.BookingId == booking.BookingId);

        //            if (existingInvoice == null)
        //            {
        //                var invoice = new Invoice
        //                {
        //                    BookingId = booking.BookingId,
        //                    TotalAmount = booking.TotalAmount,
        //                    InvoiceDate = DateTime.Now
        //                };

        //                _context.Invoices.Add(invoice);
        //                await _context.SaveChangesAsync();
        //            }

        //            //TempData["Success"] = "Booking approved successfully.";

        //            //return RedirectToAction(nameof(Index));
        //            TempData["Success"] = "Booking approved successfully.";

        //            return RedirectToAction("Index", "AdminBookings", new { area = "Admin" });
        //        }

        //        // Reject Booking
        //        public async Task<IActionResult> Reject(int id)
        //        {
        //            var booking = await _context.EventBookings
        //                .FirstOrDefaultAsync(b => b.BookingId == id);

        //            if (booking == null)
        //            {
        //                return NotFound();
        //            }

        //            booking.BookingStatus = "Rejected";

        //            await _context.SaveChangesAsync();

        //            //TempData["Error"] = "Booking rejected.";

        //            //return RedirectToAction(nameof(Index));
        //            TempData["Error"] = "Booking rejected.";

        //return RedirectToAction("Index", "AdminBookings", new { area = "Admin" });
        //        }
        //[HttpPost]
        //public async Task<IActionResult> Approve(int id)
        //{

        //    var booking = await _context.EventBookings
        //        .FirstOrDefaultAsync(b => b.BookingId == id);

        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }

        //    booking.BookingStatus = "Approved";

        //    await _context.SaveChangesAsync();

        //    var existingInvoice = await _context.Invoices
        //        .FirstOrDefaultAsync(i => i.BookingId == booking.BookingId);

        //    if (existingInvoice == null)
        //    {
        //        var invoice = new Invoice
        //        {
        //            BookingId = booking.BookingId,
        //            TotalAmount = booking.TotalAmount,
        //            InvoiceDate = DateTime.Now
        //        };

        //        _context.Invoices.Add(invoice);
        //        await _context.SaveChangesAsync();
        //    }

        //    TempData["Success"] = "Booking approved successfully.";

        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //public async Task<IActionResult> Reject(int id)
        //{
        //    var booking = await _context.EventBookings
        //        .FirstOrDefaultAsync(b => b.BookingId == id);

        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }

        //    booking.BookingStatus = "Rejected";

        //    await _context.SaveChangesAsync();

        //    TempData["Error"] = "Booking rejected.";

        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var booking = await _context.EventBookings
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                TempData["Error"] = "Booking not found";
                return RedirectToAction(nameof(Index));
            }

            booking.BookingStatus = "Approved";

            _context.EventBookings.Update(booking);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Booking approved successfully";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var booking = await _context.EventBookings
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                TempData["Error"] = "Booking not found";
                return RedirectToAction(nameof(Index));
            }

            booking.BookingStatus = "Rejected";

            _context.EventBookings.Update(booking);

            await _context.SaveChangesAsync();

            TempData["Error"] = "Booking rejected";

            return RedirectToAction(nameof(Index));
        }
    }
}