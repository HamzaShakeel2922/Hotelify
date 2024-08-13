using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? RoomNumber { get; set; }

    public int? RoomTypeId { get; set; }

    public decimal? Price { get; set; }

    public string? BedType { get; set; }

    public string? ViewType { get; set; }

    public string? Status { get; set; }

    public bool? IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual RoomType? RoomType { get; set; }
}
