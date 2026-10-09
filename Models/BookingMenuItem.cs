using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class BookingMenuItem
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int MenuItemId { get; set; }

    public int Quantity { get; set; }

    public virtual EventBooking Booking { get; set; } = null!;

    public virtual MenuItem MenuItem { get; set; } = null!;
}
