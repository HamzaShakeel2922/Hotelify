using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class ReservationGuest
{
    public int ReservationGuestId { get; set; }

    public int? ReservationId { get; set; }

    public int? GuestId { get; set; }

    public virtual Guest? Guest { get; set; }

    public virtual Reservation? Reservation { get; set; }
}
