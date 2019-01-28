using CodeStack.Sw.MyToolbar.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CodeStack.Sw.MyToolbar.UI.Base
{
    public class CommandDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NewCommandTemplate { get; set; }
        public DataTemplate CommandTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is NewCommandPlaceholderVM)
            {
                return NewCommandTemplate;
            }
            else
            {
                return CommandTemplate;
            }
        }
    }
}
