//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CodeStack.Sw.MyToolbar.UI.Extensions
{
    public static class UIElementExtension
    {
        public static readonly DependencyProperty SelectListItemOnClickProperty = DependencyProperty.RegisterAttached(
            "SelectListItemOnClick",
            typeof(bool),
            typeof(UIElementExtension),
            new FrameworkPropertyMetadata(false, OnSelectListItemOnClickPropertyChanged));

        public static void SetSelectListItemOnClick(UIElement element, Boolean value)
        {
            element.SetValue(SelectListItemOnClickProperty, value);
        }

        public static bool GetSelectListItemOnClick(UIElement element)
        {
            return (bool)element.GetValue(SelectListItemOnClickProperty);
        }

        private static void OnSelectListItemOnClickPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selectListItemOnClick = (bool)e.NewValue;
            var elem = d as UIElement;

            if (selectListItemOnClick)
            {
                elem.PreviewMouseUp += OnToolbarPreviewMouseDown;
            }
            else
            {
                elem.PreviewMouseUp -= OnToolbarPreviewMouseDown;
            }
        }

        private static void OnToolbarPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var elem = sender as UIElement;
            var item = GetAncestorByType<ListBoxItem>(elem);

            if (item != null)
            {
                item.IsSelected = true;
            }
        }

        public static T GetAncestorByType<T>(DependencyObject element)
            where T : UIElement
        {
            if (element == null)
            {
                return null;
            }

            if (element is T)
            {
                return element as T;
            }
            else
            {
                return GetAncestorByType<T>(VisualTreeHelper.GetParent(element));
            }
        }
    }
}