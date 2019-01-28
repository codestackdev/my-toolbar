using CodeStack.Sw.MyToolbar.Preferences;
using CodeStack.Sw.MyToolbar.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar
{
    internal static class AppInfo
    {
        internal static string WorkingDir
        {
            get
            {
                return Path.Combine(Locations.AppDirectoryPath, Settings.Default.SystemDir);
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
