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
    public class CommandPropertiesDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CommandMacroTemplate { get; set; }
        public DataTemplate CommandGroupTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item?.GetType() == typeof(CommandMacroVM))
            {
                return CommandMacroTemplate;
            }
            if (item?.GetType() == typeof(CommandGroupVM))
            {
                return CommandGroupTemplate;
            }
            else
            {
                return DefaultTemplate;
            }
        }
    }
}