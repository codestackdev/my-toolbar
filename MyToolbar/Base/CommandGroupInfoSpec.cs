//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.SwEx.AddIn.Core;
using System;
using System.Linq;

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