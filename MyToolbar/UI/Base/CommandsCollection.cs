//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace CodeStack.Sw.MyToolbar.UI.Base
{
    public class CommandsCollection<TCommandVM> : CompositeCollection
            where TCommandVM : ICommandVM, new()
    {
        private readonly ObservableCollection<TCommandVM> m_Commands;

        public event Action<IEnumerable<TCommandVM>> CommandsChanged;

        public CommandsCollection(IEnumerable<TCommandVM> commands)
        {
            m_Commands = new ObservableCollection<TCommandVM>(commands);
            m_Commands.CollectionChanged += OnCommandsCollectionChanged;
            Add(new CollectionContainer() { Collection = m_Commands });
            Add(new NewCommandPlaceholderVM(m_Commands, () => new TCommandVM()));
        }

        private void OnCommandsCollectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CommandsChanged?.Invoke(m_Commands);
        }
    }
}