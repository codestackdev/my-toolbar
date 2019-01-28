using CodeStack.Sw.MyToolbar.UI.Base;
using CodeStack.Sw.MyToolbar.UI.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
