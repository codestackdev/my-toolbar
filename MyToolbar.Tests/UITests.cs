//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Services;
using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.Sw.MyToolbar.UI.Forms;
using CodeStack.Sw.MyToolbar.UI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

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
                    Commands = new CommandMacroInfo[]
                    {
                        new CommandMacroInfo() { Title = "Command1", Description="Sample command in toolbar which will invoke some macro" },
                        new CommandMacroInfo() { Title = "Command2" },
                        new CommandMacroInfo() { Title = "Command3" }
                    }
                },
                new CommandGroupInfo()
                {
                    Title = "Toolbar2",
                    Commands = new CommandMacroInfo[]
                    {
                        new CommandMacroInfo() { Title = "Command4" },
                        new CommandMacroInfo() { Title = "Command5" },
                        new CommandMacroInfo() { Title = "Command6" },
                        new CommandMacroInfo() { Title = "Command7" },
                        new CommandMacroInfo() { Title = "Command8" },
                        new CommandMacroInfo() { Title = "Command9" },
                        new CommandMacroInfo() { Title = "Command10" },
                        new CommandMacroInfo() { Title = "Command11" },
                        new CommandMacroInfo() { Title = "Command12" },
                        new CommandMacroInfo() { Title = "Command13" }
                    }
                }
            };

            var confProviderMock = new Mock<IToolbarConfigurationProvider>();
            var settsProviderMock = new Mock<ISettingsProvider>();

            confProviderMock.Setup(m => m.GetToolbar(out It.Ref<bool>.IsAny, It.IsAny<string>())).
                Returns(toolbar);

            settsProviderMock.Setup(p => p.GetSettings())
                .Returns(new ToolbarSettings());

            var vm = new CommandManagerVM(confProviderMock.Object, settsProviderMock.Object,
                new Mock<IMessageService>().Object);

            new CommandManagerForm(vm, IntPtr.Zero).ShowDialog();
        }
    }
}