//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.UI.ViewModels;
using System;
using System.Windows;

namespace CodeStack.Sw.MyToolbar.UI.Forms
{
    public partial class CommandManagerForm : Window
    {
        public CommandManagerForm(CommandManagerVM vm, IntPtr parent)
        {
            InitializeComponent();
            this.DataContext = vm;
            new System.Windows.Interop.WindowInteropHelper(this).Owner = parent;
        }

        private void OnOk(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}