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
    public interface ICommandVM
    {
        string Title { get; set; }
        string Description { get; set; }
        string IconPath { get; set; }
        ICommand BrowseIconCommand { get; }
    }

    public abstract class CommandVM<TCmdInfo> : NotifyPropertyChanged, ICommandVM
        where TCmdInfo : CommandItemInfo
    {
        private readonly TCmdInfo m_Command;

        private ICommand m_BrowseIconCommand;

        internal TCmdInfo Command
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
                NotifyChanged();
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
                NotifyChanged();
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
                NotifyChanged();
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
        
        protected CommandVM(TCmdInfo cmd)
        {
            m_Command = cmd;
        }
    }
}
