//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.SwEx.AddIn.Core;
using System;

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