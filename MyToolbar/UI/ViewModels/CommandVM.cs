//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Helpers;
using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.Sw.MyToolbar.UI.Base;
using System.Windows.Input;

namespace CodeStack.Sw.MyToolbar.UI.ViewModels
{
    public interface ICommandVM
    {
        string Title { get; set; }
        string Description { get; set; }
        string IconPath { get; set; }
        ICommand BrowseIconCommand { get; }
        CommandItemInfo Command { get; }
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
                        var imgFile = FileBrowseHelper.BrowseFile("Select image file for icon",
                            new FileFilter()
                            {
                                { "Image File", new FileFilterExtensions("jpg", "jpeg", "png", "gif", "bmp") }
                            }, IconPath);

                        if (!string.IsNullOrEmpty(imgFile))
                        {
                            IconPath = imgFile;
                        }
                    });
                }

                return m_BrowseIconCommand;
            }
        }

        CommandItemInfo ICommandVM.Command
        {
            get
            {
                return Command;
            }
        }

        protected CommandVM(TCmdInfo cmd)
        {
            m_Command = cmd;
        }
    }
}