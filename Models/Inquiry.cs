namespace EventMVC.Models
{
    public class Inquiry
    {
        public int InquiryId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int EventTypeId { get; set; }

        public DateOnly EventDate { get; set; }

        public int GuestCount { get; set; }

        public string Message { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; }

        public virtual EventType EventType { get; set; }

        public virtual ICollection<InquiryReply> InquiryReplies { get; set; }
            = new List<InquiryReply>();
    }
}
