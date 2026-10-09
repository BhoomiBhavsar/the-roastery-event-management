namespace EventMVC.Models
{
    public class InquiryReply
    {
        public int ReplyId { get; set; }

        public int InquiryId { get; set; }

        public string ReplyMessage { get; set; }

        public string RepliedBy { get; set; }

        public DateTime RepliedAt { get; set; }

        public virtual Inquiry Inquiry { get; set; }
    }
}
