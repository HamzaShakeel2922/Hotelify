using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public string? CountryCode { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
