using CodeStack.Sw.MyToolbar.UI.Base;
using CodeStack.Sw.MyToolbar.Preferences;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections;
using CodeStack.Sw.MyToolbar.UI.Views;

namespace CodeStack.Sw.MyToolbar.UI.ViewModels
{
    public class CommandManagerVM : NotifyPropertyChanged
    {
        private ICommandVM m_SelectedElement;
        private ICommand m_SelectCommandCommand;

        private readonly CustomToolbarInfo m_ToolbarInfo;
        private readonly CommandsCollection<CommandGroupVM> m_Groups;

        public CommandManagerVM(CustomToolbarInfo toolbarInfo)
        {
            m_ToolbarInfo = toolbarInfo;

            m_Groups = new CommandsCollection<CommandGroupVM>(
                (toolbarInfo.Groups ?? new CommandGroupInfo[0])
                .Select(g => new CommandGroupVM(g)));
            
            m_Groups.CommandsChanged += OnGroupsCollectionChanged;
        }

        public CommandsCollection<CommandGroupVM> Groups
        {
            get
            {
                return m_Groups;
            }
        }
        
        public ICommandVM SelectedElement
        {
            get
            {
                return m_SelectedElement;
            }
            set
            {
                m_SelectedElement = value;
                NotifyChanged();
            }
        }

        public ICommand SelectCommandCommand
        {
            get
            {
                if (m_SelectCommandCommand == null)
                {
                    m_SelectCommandCommand = new RelayCommand<ICommandVM>(cmd =>
                    {
                        SelectedElement = cmd;
                    });
                }

                return m_SelectCommandCommand;
            }
        }

        private void OnGroupsCollectionChanged(IEnumerable<CommandGroupVM> cmds)
        {
            m_ToolbarInfo.Groups = cmds
                .Select(g => g.Command).ToArray();
        }
    }
}
