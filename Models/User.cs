using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class User
{
    public string UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<EventBooking> EventBookings { get; set; } = new List<EventBooking>();
}
