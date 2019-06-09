//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Properties;
using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.SwEx.AddIn.Icons;
using System.Drawing;
using System.IO;

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

            return new MacroButtonIcon(icon);
        }
    }
}