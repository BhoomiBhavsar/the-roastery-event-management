using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class BookingDecoration
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int DecorationId { get; set; }

    public virtual EventBooking Booking { get; set; } = null!;

    public virtual Decoration Decoration { get; set; } = null!;
}
