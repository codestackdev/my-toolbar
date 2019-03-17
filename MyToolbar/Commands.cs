//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Properties;
using CodeStack.SwEx.Common.Attributes;
using System.ComponentModel;

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