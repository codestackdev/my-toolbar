using CodeStack.Community.Sw.MyToolbar.Preferences;
using CodeStack.Community.Sw.MyToolbar.Properties;
using CodeStack.SwEx.AddIn.Core;
using CodeStack.SwEx.AddIn.Icons;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Community.Sw.MyToolbar.Base
{
    internal class CommandGroupInfoSpec : CommandGroupSpec
    {
        internal CommandGroupInfoSpec(CommandGroupInfo info)
        {
            Id = info.Id;
            Title = info.Title;
            Tooltip = info.Description;
            Icon = info.GetCommandIcon();

            if (info.Commands != null)
            {
                Commands = info.Commands.Select(
                    c => new CommandItemInfoSpec(c)).ToArray();
            }
            else
            {
                Commands = new CommandItemInfoSpec[0];
            }
        }
    }
}
