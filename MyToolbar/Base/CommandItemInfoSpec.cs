//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Helpers;
using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.SwEx.AddIn.Core;
using CodeStack.SwEx.AddIn.Enums;
using SolidWorks.Interop.sldworks;
using System;

namespace CodeStack.Sw.MyToolbar.Base
{
    internal class CommandItemInfoSpec : CommandSpec
    {
        public event Action<CommandMacroInfo> MacroCommandClick;

        private readonly CommandMacroInfo m_Info;
        private readonly ISldWorks m_App;

        internal CommandItemInfoSpec(CommandMacroInfo info, ISldWorks app)
        {
            m_Info = info;
            m_App = app;

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

        public override CommandItemEnableState_e OnEnable()
        {
            if (m_Info.Scope.IsInScope(m_App))
            {
                return CommandItemEnableState_e.DeselectEnable;
            }
            else
            {
                return CommandItemEnableState_e.DeselectDisable;
            }
        }
    }
}