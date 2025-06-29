using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class LuPermission
{
    public short PermissionId { get; set; }

    public string PermissionTitle { get; set; } = null!;

    public string PermissionDescription { get; set; } = null!;

    public Guid? ApplicationId { get; set; }
}
