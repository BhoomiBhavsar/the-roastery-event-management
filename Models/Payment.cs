using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int BookingId { get; set; }

    public decimal AmountPaid { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public DateTime PaidAt { get; set; }

    public virtual EventBooking Booking { get; set; } = null!;
}
