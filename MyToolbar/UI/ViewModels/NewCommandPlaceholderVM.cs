//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.UI.Base;
using System;
using System.Collections;
using System.Windows.Input;

namespace CodeStack.Sw.MyToolbar.UI.ViewModels
{
    public class NewCommandPlaceholderVM
    {
        private ICommand m_AddNewItemCommand;

        private readonly IList m_Items;
        private readonly Func<object> m_NewItemFunc;

        public NewCommandPlaceholderVM(IList items, Func<object> newItemFunc)
        {
            m_Items = items;
            m_NewItemFunc = newItemFunc;
        }

        public ICommand AddNewItemCommand
        {
            get
            {
                if (m_AddNewItemCommand == null)
                {
                    m_AddNewItemCommand = new RelayCommand(
                        () => m_Items.Add(m_NewItemFunc.Invoke()));
                }

                return m_AddNewItemCommand;
            }
        }
    }
}