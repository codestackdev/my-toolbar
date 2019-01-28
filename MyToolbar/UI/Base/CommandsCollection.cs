using CodeStack.Sw.MyToolbar.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CodeStack.Sw.MyToolbar.UI.Base
{
    public class CommandsCollection<TCommandVM> : CompositeCollection
            where TCommandVM : CommandVM, new()
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
