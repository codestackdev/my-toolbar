using CodeStack.Sw.MyToolbar.Structs;
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
        public event Action<CommandMacroInfo> MacroCommandClick;

        private readonly CommandMacroInfo m_Info;

        internal CommandItemInfoSpec(CommandMacroInfo info)
        {
            m_Info = info;
            UserId = info.Id;
            Title = info.Title;
            Tooltip = info.Description;
            Icon = info.GetCommandIcon();
            HasMenu = true;
            HasToolbar = true;
        }

        public override void OnClick()
        {
            MacroCommandClick?.Invoke(m_Info);
        }
    }
}
