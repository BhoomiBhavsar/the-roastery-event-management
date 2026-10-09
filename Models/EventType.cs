using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class EventType
{
    public int EventTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<EventBooking> EventBookings { get; set; } = new List<EventBooking>();

    public virtual ICollection<EventPackage> EventPackages { get; set; } = new List<EventPackage>();
}
