using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class Amenity
{
    public int AmenityId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();
}
