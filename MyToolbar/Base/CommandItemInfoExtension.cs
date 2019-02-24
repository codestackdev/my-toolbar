using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.Sw.MyToolbar.Properties;
using CodeStack.SwEx.AddIn.Icons;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Base
{
    internal static class CommandItemInfoExtension
    {
        internal static CommandGroupIcon GetCommandIcon(this CommandItemInfo info)
        {
            Image icon = null;

            try
            {
                if (File.Exists(info.IconPath))
                {
                    icon = Image.FromFile(info.IconPath);
                }
            }
            catch
            {
            }

            if (icon == null)
            {
                icon = Resources.macro_icon_default;
            }

            return new MasterIcon(icon);
        }
    }
}
