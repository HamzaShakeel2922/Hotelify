using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class State
{
    public int StateId { get; set; }

    public string? StateName { get; set; }

    public int? CountryId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();
}
