using CodeStack.Sw.MyToolbar.UI.Base;
using CodeStack.Sw.MyToolbar.Preferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;

namespace CodeStack.Sw.MyToolbar.UI.ViewModels
{
    public class CommandVM : NotifyPropertyChanged
    {
        private readonly CommandItemInfo m_Command;

        private ICommand m_BrowseIconCommand;

        internal CommandItemInfo Command
        {
            get
            {
                return m_Command;
            }
        }

        public string Title
        {
            get
            {
                return m_Command.Title;
            }
            set
            {
                m_Command.Title = value;
            }
        }

        public string Description
        {
            get
            {
                return m_Command.Description;
            }
            set
            {
                m_Command.Description = value;
            }
        }

        public string IconPath
        {
            get
            {
                return m_Command.IconPath;
            }
            set
            {
                m_Command.IconPath = value;
            }
        }

        public ICommand BrowseIconCommand
        {
            get
            {
                if (m_BrowseIconCommand == null)
                {
                    m_BrowseIconCommand = new RelayCommand(() => 
                    {
                        var dlg = new OpenFileDialog();
                        
                        dlg.Filter = "Image File (*.jpg;*.png;*.gif;*.bmp)|*.jpg;*.png;*.gif;*.bmp";

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            IconPath = dlg.FileName;
                        }
                    });
                }

                return m_BrowseIconCommand;
            }
        }

        public CommandVM() 
            : this(new CommandItemInfo())
        {
        }

        public CommandVM(CommandItemInfo cmd)
        {
            m_Command = cmd;
        }
    }
}
