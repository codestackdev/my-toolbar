using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.Sw.MyToolbar.Properties;
using CodeStack.SwEx.AddIn.Core;
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
    internal class CommandGroupInfoSpec : CommandGroupSpec
    {
        public event Action<CommandMacroInfo> MacroCommandClick;

        internal CommandGroupInfoSpec(CommandGroupInfo info)
        {
            Id = info.Id;
            Title = info.Title;
            Tooltip = info.Description;
            Icon = info.GetCommandIcon();

            if (info.Commands != null)
            {
                Commands = info.Commands.Select(
                    c => 
                    {
                        var spec = new CommandItemInfoSpec(c);
                        spec.MacroCommandClick += OnMacroCommandClick;
                        return spec;
                    }).ToArray();
            }
            else
            {
                Commands = new CommandItemInfoSpec[0];
            }
        }

        private void OnMacroCommandClick(CommandMacroInfo cmd)
        {
            MacroCommandClick?.Invoke(cmd);
        }
    }
}
