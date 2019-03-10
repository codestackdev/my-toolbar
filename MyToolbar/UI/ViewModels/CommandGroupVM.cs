//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.Sw.MyToolbar.UI.Base;
using System.Collections.Generic;
using System.Linq;

namespace CodeStack.Sw.MyToolbar.UI.ViewModels
{
    public class CommandGroupVM : CommandVM<CommandGroupInfo>
    {
        private readonly CommandGroupInfo m_CmdGrp;
        private readonly CommandsCollection<CommandMacroVM> m_Commands;

        public CommandsCollection<CommandMacroVM> Commands
        {
            get
            {
                return m_Commands;
            }
        }

        public CommandGroupVM()
            : this(new CommandGroupInfo())
        {
        }

        public CommandGroupVM(CommandGroupInfo cmdGrp) : base(cmdGrp)
        {
            m_CmdGrp = cmdGrp;
            m_Commands = new CommandsCollection<CommandMacroVM>(
                (cmdGrp.Commands ?? new CommandMacroInfo[0])
                .Select(c => new CommandMacroVM(c)));

            m_Commands.CommandsChanged += OnCommandsCollectionChanged;
        }

        private void OnCommandsCollectionChanged(IEnumerable<CommandMacroVM> cmds)
        {
            m_CmdGrp.Commands = cmds.Select(c => c.Command).ToArray();
        }
    }
}