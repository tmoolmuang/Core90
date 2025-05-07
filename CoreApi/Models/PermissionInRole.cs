using System;
using System.Collections.Generic;

namespace CoreApi.Models;

public partial class PermissionInRole
{
    public short PermissionInRoleId { get; set; }

    public Guid RoleId { get; set; }

    public short PermissionId { get; set; }
}
