﻿using CodeStack.Sw.MyToolbar.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeStack.Sw.MyToolbar.UI.Forms
{
    /// <summary>
    /// Interaction logic for CommandManagerForm.xaml
    /// </summary>
    public partial class CommandManagerForm : Window
    {
        public CommandManagerForm(CommandManagerVM vm, IntPtr parent)
        {
            InitializeComponent();
            this.DataContext = vm;
            new System.Windows.Interop.WindowInteropHelper(this).Owner = parent;
        }
    }
}