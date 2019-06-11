//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.UI.Base;
using System;
using System.Windows.Input;

namespace CodeStack.Sw.MyToolbar.UI.ViewModels
{
    public class NewCommandPlaceholderVM
    {
        public event Action AddNewCommand;

        private ICommand m_AddNewItemCommand;

        public ICommand AddNewItemCommand
        {
            get
            {
                if (m_AddNewItemCommand == null)
                {
                    m_AddNewItemCommand = new RelayCommand(
                        () =>
                        {
                            AddNewCommand?.Invoke();
                        });
                }

                return m_AddNewItemCommand;
            }
        }
    }
}