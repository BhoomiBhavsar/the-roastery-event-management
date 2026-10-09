using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class EventBooking
{
    public int BookingId { get; set; }

    public string UserId { get; set; }
    public IdentityUser User { get; set; }

    public int EventTypeId { get; set; }

    public int? PackageId { get; set; }

    public int AreaId { get; set; }

    public DateOnly EventDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int GuestCount { get; set; }

    public decimal SubTotalAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string BookingMode { get; set; } = null!;

    public string BookingStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual ICollection<BookingAddOn> BookingAddOns { get; set; } = new List<BookingAddOn>();

    public virtual ICollection<BookingDecoration> BookingDecorations { get; set; } = new List<BookingDecoration>();

    public virtual ICollection<BookingMenuItem> BookingMenuItems { get; set; } = new List<BookingMenuItem>();

    public virtual EventType EventType { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual EventPackage? Package { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    //public virtual User User { get; set; } = null!;
}
