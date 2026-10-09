using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class PackageAddOn
{
    public int Id { get; set; }

    public int PackageId { get; set; }

    public int AddOnId { get; set; }

    public virtual AddOn AddOn { get; set; } = null!;

    public virtual EventPackage Package { get; set; } = null!;
}
