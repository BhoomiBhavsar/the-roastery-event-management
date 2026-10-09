using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int BookingId { get; set; }

    public DateTime InvoiceDate { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual EventBooking Booking { get; set; } = null!;
}
