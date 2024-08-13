using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class Cancellation
{
    public int CancellationId { get; set; }

    public int? ReservationId { get; set; }

    public DateTime? CancellationDate { get; set; }

    public string? Reason { get; set; }

    public decimal? CancellationFee { get; set; }

    public string? CancellationStatus { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Reservation? Reservation { get; set; }
}
