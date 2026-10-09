using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class Decoration
{
    public int DecorationId { get; set; }

    public string DecorationName { get; set; } = null!;

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<BookingDecoration> BookingDecorations { get; set; } = new List<BookingDecoration>();

    public virtual ICollection<PackageDecoration> PackageDecorations { get; set; } = new List<PackageDecoration>();
}
