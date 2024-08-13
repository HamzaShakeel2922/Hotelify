using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class RefundMethod
{
    public int MethodId { get; set; }

    public string? MethodName { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();
}
