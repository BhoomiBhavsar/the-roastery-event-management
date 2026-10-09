using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class AddOn
{
    public int AddOnId { get; set; }

    public string AddOnName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<BookingAddOn> BookingAddOns { get; set; } = new List<BookingAddOn>();

    public virtual ICollection<PackageAddOn> PackageAddOns { get; set; } = new List<PackageAddOn>();
}
