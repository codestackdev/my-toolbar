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
using CodeStack.Sw.MyToolbar.Services;

namespace CodeStack.Sw.MyToolbar.UI.ViewModels
{
    public class CommandManagerVM : NotifyPropertyChanged
    {
        private ICommandVM m_SelectedElement;
        private ICommand m_SelectCommandCommand;

        private readonly CustomToolbarInfo m_ToolbarInfo;
        private readonly CommandsCollection<CommandGroupVM> m_Groups;

        private readonly IToolbarConfigurationProvider m_ConfsProvider;
        private readonly ISettingsProvider m_SettsProvider;
        private readonly ToolbarSettings m_Settings;

        private readonly bool m_IsReadOnly;

        public CommandManagerVM(IToolbarConfigurationProvider confsProvider, ISettingsProvider settsProvider)
        {
            m_ConfsProvider = confsProvider;
            m_SettsProvider = settsProvider;

            m_Settings = m_SettsProvider.GetSettings();

            m_ToolbarInfo = m_ConfsProvider.GetToolbar(out m_IsReadOnly, m_Settings.SpecificationFile);

            m_Groups = new CommandsCollection<CommandGroupVM>(
                (m_ToolbarInfo.Groups ?? new CommandGroupInfo[0])
                .Select(g => new CommandGroupVM(g)));
            
            m_Groups.CommandsChanged += OnGroupsCollectionChanged;
        }

        public bool IsEditable
        {
            get
            {
                return !m_IsReadOnly;
            }
        }

        public CustomToolbarInfo ToolbarInfo
        {
            get
            {
                return m_ToolbarInfo;
            }
        }

        public ToolbarSettings Settings
        {
            get
            {
                return m_Settings;
            }
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
