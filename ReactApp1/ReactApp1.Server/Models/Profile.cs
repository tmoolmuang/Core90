using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class Profile
{
    public int ProfileId { get; set; }

    public string UserName { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public bool Active { get; set; }

    public string? Email { get; set; }

    public string? OneHealthcareUuid { get; set; }
}
