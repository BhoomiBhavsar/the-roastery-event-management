using EventMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class InquiriesController : Controller
    {
        private readonly EventManagementContext _context;

        public InquiriesController(
            EventManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var inquiries =
                await _context.Inquiries
                .Include(i => i.EventType)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return View(inquiries);
        }

        //public async Task<IActionResult> Reply(int id)
        //{
        //    var inquiry =
        //        await _context.Inquiries
        //        .Include(i => i.EventType)
        //        .FirstOrDefaultAsync(i => i.InquiryId == id);

        //    return View(inquiry);
        //}


        public async Task<IActionResult> Reply(int id)
        {
            var inquiry =
                await _context.Inquiries
                .Include(i => i.EventType)
                .Include(i => i.InquiryReplies)
                .FirstOrDefaultAsync(i => i.InquiryId == id);

            if (inquiry == null)
                return NotFound();

            return View(inquiry);
        }

        [HttpPost]
        public async Task<IActionResult> Reply(
            int inquiryId,
            string replyMessage)
        {
            InquiryReply reply =
                new InquiryReply
                {
                    InquiryId = inquiryId,
                    ReplyMessage = replyMessage,
                    RepliedBy = "Admin",
                    RepliedAt = DateTime.Now
                };

            _context.InquiryReplies.Add(reply);

            var inquiry =
                await _context.Inquiries
                .FirstOrDefaultAsync(
                    i => i.InquiryId == inquiryId);

            inquiry.Status = "Answered";

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
