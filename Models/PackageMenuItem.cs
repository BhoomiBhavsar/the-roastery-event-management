using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class PackageMenuItem
{
    public int Id { get; set; }

    public int PackageId { get; set; }

    public int MenuItemId { get; set; }

    public virtual MenuItem MenuItem { get; set; } = null!;

    public virtual EventPackage Package { get; set; } = null!;
}
