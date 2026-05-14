using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Enums
{
    /// <summary>
    /// Permissions bitmask for menu and page actions
    /// Each value represents a single permission and can be combined using OR (|).
    /// </summary>
    [Flags]  // This attribute enables bitwise operations on the enum
    public enum Permission
    {
        [Description("None")]
        None = 0,           // 0000

        [Description("View")]
        View = 1 << 0,      // 0001

        [Description("Create")]
        Create = 1 << 1,    // 0010

        [Description("Update")]
        Update = 1 << 2,    // 0100

        [Description("Delete")]
        Delete = 1 << 3,    // 1000

        [Description("All")]
        All = View | Create | Update | Delete // 1111
    }
}
