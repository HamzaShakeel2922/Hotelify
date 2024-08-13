using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class RoomType
{
    public int RoomTypeId { get; set; }

    public string? TypeName { get; set; }

    public string? AccessibilityFeatures { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
}
