//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.UI.ViewModels;
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