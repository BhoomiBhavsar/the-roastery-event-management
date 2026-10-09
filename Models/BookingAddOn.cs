using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class BookingAddOn
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int AddOnId { get; set; }

    public int Quantity { get; set; }

    public virtual AddOn AddOn { get; set; } = null!;

    public virtual EventBooking Booking { get; set; } = null!;
}
