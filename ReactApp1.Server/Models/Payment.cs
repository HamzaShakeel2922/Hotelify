using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? ReservationId { get; set; }

    public decimal? Amount { get; set; }

    public int? PaymentBatchId { get; set; }

    public virtual PaymentBatch? PaymentBatch { get; set; }

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();

    public virtual Reservation? Reservation { get; set; }
}
