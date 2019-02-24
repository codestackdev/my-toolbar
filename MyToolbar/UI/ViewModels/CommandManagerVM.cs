using CodeStack.Sw.MyToolbar.UI.Base;
using CodeStack.Sw.MyToolbar.Structs;
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
using System.Windows.Forms;
using CodeStack.Sw.MyToolbar.Helpers;

namespace CodeStack.Sw.MyToolbar.UI.ViewModels
{
    public class CommandManagerVM : NotifyPropertyChanged
    {
        private ICommandVM m_SelectedElement;
        private ICommand m_SelectCommandCommand;
        private ICommand m_BrowseToolbarSpecificationCommand;

        private readonly CustomToolbarInfo m_ToolbarInfo;
        private readonly CommandsCollection<CommandGroupVM> m_Groups;

        private readonly IToolbarConfigurationProvider m_ConfsProvider;
        private readonly ISettingsProvider m_SettsProvider;
        private readonly IMessageService m_MsgService;

        private readonly ToolbarSettings m_Settings;

        private bool m_IsReadOnly;

        public CommandManagerVM(IToolbarConfigurationProvider confsProvider,
            ISettingsProvider settsProvider, IMessageService msgService)
        {
            m_ConfsProvider = confsProvider;
            m_SettsProvider = settsProvider;
            m_MsgService = msgService;

            m_Settings = m_SettsProvider.GetSettings();

            try
            {
                m_ToolbarInfo = m_ConfsProvider.GetToolbar(out m_IsReadOnly, ToolbarSpecificationPath);
            }
            catch
            {
                m_IsReadOnly = true;
                msgService.ShowMessage("Failed to load the toolbar from the specification file. Make sure that you have access to the specification file",
                    MessageType_e.Error);
            }

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
            set
            {
                m_IsReadOnly = !value;
                NotifyChanged();
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

        public string ToolbarSpecificationPath
        {
            get
            {
                return Settings.SpecificationFile;
            }
            set
            {
                if (!string.Equals(value, Settings.SpecificationFile, StringComparison.CurrentCultureIgnoreCase))
                {
                    Settings.SpecificationFile = value;
                    NotifyChanged();
                    //TODO: reload toolbar
                }
            }
        }

        public ICommand BrowseToolbarSpecificationCommand
        {
            get
            {
                if (m_BrowseToolbarSpecificationCommand == null)
                {
                    m_BrowseToolbarSpecificationCommand = new RelayCommand(() =>
                    {
                        var specFile = FileBrowseHelper.BrowseFile("Select toolbar specification file",
                            new FileFilter()
                            {
                                { "Toolbar Specification File", new FileFilterExtensions("json") }
                            }, ToolbarSpecificationPath);
                        
                        if (!string.IsNullOrEmpty(specFile))
                        {
                            ToolbarSpecificationPath = specFile;
                        }
                    });
                }

                return m_BrowseToolbarSpecificationCommand;
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
