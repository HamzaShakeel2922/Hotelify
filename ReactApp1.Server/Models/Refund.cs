using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class Refund
{
    public int RefundId { get; set; }

    public int? PaymentId { get; set; }

    public decimal? RefundAmount { get; set; }

    public DateTime? RefundDate { get; set; }

    public string? RefundReason { get; set; }

    public int? RefundMethodId { get; set; }

    public int? ProcessedByUserId { get; set; }

    public string? RefundStatus { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual User? ProcessedByUser { get; set; }

    public virtual RefundMethod? RefundMethod { get; set; }
}
