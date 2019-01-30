using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using CodeStack.Sw.MyToolbar.UI.Views;
using System.Collections.ObjectModel;
using CodeStack.Sw.MyToolbar.UI.ViewModels;
using CodeStack.Sw.MyToolbar.Preferences;
using CodeStack.Sw.MyToolbar.UI.Forms;

namespace MyToolbar.Tests
{
    [TestClass]
    public class UITests
    {
        [TestMethod]
        public void DisplayCommandManagerView()
        {
            var toolbar = new CustomToolbarInfo();
            toolbar.Groups = new CommandGroupInfo[]
            {
                new CommandGroupInfo()
                {
                    Title = "Toolbar1",
                    Commands = new CommandItemInfo[]
                    {
                        new CommandItemInfo() { Title = "Command1", Description="Sample command in toolbar which will invoke some macro" },
                        new CommandItemInfo() { Title = "Command2" },
                        new CommandItemInfo() { Title = "Command3" }
                    }
                },
                new CommandGroupInfo()
                {
                    Title = "Toolbar2",
                    Commands = new CommandItemInfo[]
                    {
                        new CommandItemInfo() { Title = "Command4" },
                        new CommandItemInfo() { Title = "Command5" },
                        new CommandItemInfo() { Title = "Command6" },
                        new CommandItemInfo() { Title = "Command7" },
                        new CommandItemInfo() { Title = "Command8" },
                        new CommandItemInfo() { Title = "Command9" },
                        new CommandItemInfo() { Title = "Command10" },
                        new CommandItemInfo() { Title = "Command11" },
                        new CommandItemInfo() { Title = "Command12" },
                        new CommandItemInfo() { Title = "Command13" }
                    }
                }
            };

            var vm = new CommandManagerVM(toolbar);

            new CommandManagerForm(vm, IntPtr.Zero).ShowDialog();
        }
    }
}
