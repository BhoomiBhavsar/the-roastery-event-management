using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventMVC.Models;

public partial class Area
{
    public int AreaId { get; set; }


    public string AreaName { get; set; } = string.Empty;
    public int Capacity { get; set; }

    public decimal PricePerGuest { get; set; }

    public bool IsActive { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<EventBooking> EventBookings { get; set; } = new List<EventBooking>();
}
