using CodeStack.Sw.MyToolbar.Preferences;
using CodeStack.SwEx.AddIn.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Base
{
    internal class CommandItemInfoSpec : CommandSpec
    {
        internal CommandItemInfoSpec(CommandItemInfo info)
        {
            UserId = info.Id;
            Title = info.Title;
            Tooltip = info.Description;
            Icon = info.GetCommandIcon();
        }
    }
}
