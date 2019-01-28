using CodeStack.Sw.MyToolbar.Preferences;
using CodeStack.Sw.MyToolbar.UI.Base;
using CodeStack.Sw.MyToolbar.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.UI.ViewModels
{
    public class CommandGroupVM : CommandVM
    {
        private readonly CommandGroupInfo m_CmdGrp;
        private readonly CommandsCollection<CommandVM> m_Commands;

        internal CommandGroupInfo CommandGroup
        {
            get
            {
                return m_CmdGrp;
            }
        }

        public CommandsCollection<CommandVM> Commands
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

        public CommandGroupVM(CommandGroupInfo cmdGrp)
        {
            m_CmdGrp = cmdGrp;
            m_Commands = new CommandsCollection<CommandVM>(
                (cmdGrp.Commands ?? new CommandItemInfo[0])
                .Select(c => new CommandVM(c)));

            m_Commands.CommandsChanged += OnCommandsCollectionChanged;
        }

        private void OnCommandsCollectionChanged(IEnumerable<CommandVM> cmds)
        {
            m_CmdGrp.Commands = cmds.Select(c => c.Command).ToArray();
        }
    }
}
