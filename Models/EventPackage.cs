using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class EventPackage
{
    public int PackageId { get; set; }

    public int EventTypeId { get; set; }

    public string PackageName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal PricePerGuest { get; set; }

    public bool IsActive { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<EventBooking> EventBookings { get; set; } = new List<EventBooking>();

    public virtual EventType? EventType { get; set; }

    public virtual ICollection<PackageAddOn> PackageAddOns { get; set; } = new List<PackageAddOn>();

    public virtual ICollection<PackageDecoration> PackageDecorations { get; set; } = new List<PackageDecoration>();

    public virtual ICollection<PackageMenuItem> PackageMenuItems { get; set; } = new List<PackageMenuItem>();
}
