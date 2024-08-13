using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class PaymentBatch
{
    public int PaymentBatchId { get; set; }

    public int? UserId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
