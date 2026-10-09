using System;
using System.Collections.Generic;

namespace EventMVC.Models;

public partial class PackageDecoration
{
    public int Id { get; set; }

    public int PackageId { get; set; }

    public int DecorationId { get; set; }

    public virtual Decoration Decoration { get; set; } = null!;

    public virtual EventPackage Package { get; set; } = null!;
}
