//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Properties;
using CodeStack.Sw.MyToolbar.Structs;
using System.Drawing;

namespace CodeStack.Sw.MyToolbar
{
    internal static class AppInfo
    {
        internal static string WorkingDir
        {
            get
            {
                return Locations.AppDirectoryPath;
            }
        }

        internal static string Title
        {
            get
            {
                return Resources.AppTitle;
            }
        }

        internal static Icon Icon
        {
            get
            {
                return Resources.custom_toolbars_icon;
            }
        }
    }
}