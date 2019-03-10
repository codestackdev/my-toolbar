using CodeStack.Sw.MyToolbar.Properties;
using CodeStack.SwEx.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar
{
    [SwEx.AddIn.Attributes.CommandGroupInfo(999)]
    [Title("MyToolbar")]
    [Description("Custom toolbar")]
    [Icon(typeof(Resources), nameof(Resources.toolbar_icon))]
    public enum Commands_e
    {
        [Icon(typeof(Resources), nameof(Resources.configure_icon))]
        [Title("Configure...")]
        [Description("Configure toolbar")]
        Configuration,

        [Icon(typeof(Resources), nameof(Resources.about_icon))]
        [Title("About...")]
        [Description("About MyToolbar")]
        About
    }
}
