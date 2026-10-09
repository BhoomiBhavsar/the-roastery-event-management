using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class MenuItem
{
    public int MenuItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<BookingMenuItem> BookingMenuItems { get; set; } = new List<BookingMenuItem>();

    public virtual ICollection<PackageMenuItem> PackageMenuItems { get; set; } = new List<PackageMenuItem>();
}
