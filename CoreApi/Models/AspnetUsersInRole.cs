﻿using System;
using System.Collections.Generic;

namespace CoreApi.Models;

public partial class AspnetUsersInRole
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
}
