using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class Guest
{
    public int GuestId { get; set; }

    public int? UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? AgeGroup { get; set; }

    public string? Address { get; set; }

    public int? CountryId { get; set; }

    public int? StateId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<ReservationGuest> ReservationGuests { get; set; } = new List<ReservationGuest>();

    public virtual State? State { get; set; }

    public virtual User? User { get; set; }
}
