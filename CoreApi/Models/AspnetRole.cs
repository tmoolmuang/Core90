﻿using System;
using System.Collections.Generic;

namespace CoreApi.Models;

public partial class AspnetRole
{
    public Guid ApplicationId { get; set; }

    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string LoweredRoleName { get; set; } = null!;

    public string? Description { get; set; }
}
