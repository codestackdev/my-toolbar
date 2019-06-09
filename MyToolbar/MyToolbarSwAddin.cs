//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Base;
using CodeStack.Sw.MyToolbar.Exceptions;
using CodeStack.Sw.MyToolbar.Helpers;
using CodeStack.Sw.MyToolbar.Services;
using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.Sw.MyToolbar.UI.Forms;
using CodeStack.Sw.MyToolbar.UI.ViewModels;
using CodeStack.SwEx.AddIn;
using CodeStack.SwEx.AddIn.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using Xarial.AppLaunchKit.Base.Services;

namespace CodeStack.Sw.MyToolbar
{
    [Guid("63496b16-e9ad-4d3a-8473-99d124a1672b"), ComVisible(true)]
    [AutoRegister("MyToolbar", "Add-in for managing custom toolbars", true)]
    public class MyToolbarSwAddin : SwAddInEx
    {
        private ServicesContainer m_Services;

        public override bool OnConnect()
        {
            try
            {
                if (Dispatcher.CurrentDispatcher != null)
                {
                    Dispatcher.CurrentDispatcher.UnhandledException += OnDispatcherUnhandledException;
                }

                AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
                m_Services = new ServicesContainer(App, Logger);

                ExceptionHelper.ExecuteUserCommand(LoadUserToolbar, e => "Failed to load toolbar specification");

                AddCommandGroup<Commands_e>(OnButtonClick);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                new MessageService().ShowMessage("Critical error while loading add-in", MessageType_e.Error);
                throw;
            }
        }

        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log(e.ExceptionObject as Exception);
            MessageService.ShowMessage("Unknown domain error", MessageType_e.Error);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Logger.Log(e.Exception);
            MessageService.ShowMessage("Unknown dispatcher error", MessageType_e.Error);
        }

        private void LoadUserToolbar()
        {
            bool isReadOnly;
            var toolbarInfo = ToolbarProvider.GetToolbar(out isReadOnly,
                ToolbarSpecificationFile);

            if (toolbarInfo?.Groups != null)
            {
                foreach (var grp in toolbarInfo.Groups)
                {
                    var cmdGrp = new CommandGroupInfoSpec(grp);
                    cmdGrp.MacroCommandClick += OnMacroCommandClick;
                    AddCommandGroup(cmdGrp);
                }
            }
        }

        private void OnMacroCommandClick(CommandMacroInfo cmd)
        {
            RunMacroCommand(cmd);
        }

        private void RunMacroCommand(CommandMacroInfo cmd)
        {
            ExceptionHelper.ExecuteUserCommand(() => m_Services.GetService<IMacroRunner>().RunMacro(cmd.MacroPath, cmd.EntryPoint),
                e => "Failed to run macro");
        }

        private void OnButtonClick(Commands_e cmd)
        {
            switch (cmd)
            {
                case Commands_e.Configuration:

                    var vm = m_Services.GetService<CommandManagerVM>();

                    if (new CommandManagerForm(vm,
                        new IntPtr(App.IFrameObject().GetHWnd())).ShowDialog() == true)
                    {
                        ExceptionHelper.ExecuteUserCommand(() =>
                        {
                            UpdatedToolbarConfiguration(vm.Settings, vm.ToolbarInfo);
                        }, e => "Failed to save toolbar specification");
                    }
                    break;

                case Commands_e.About:
                    m_Services.GetService<IAboutApplicationService>().ShowAboutForm();
                    break;
            }
        }

        private void UpdatedToolbarConfiguration(ToolbarSettings toolbarSets, CustomToolbarInfo toolbarConf)
        {
            bool isToolbarChanged;

            SaveSettingChanges(toolbarSets, toolbarConf, out isToolbarChanged);

            if (isToolbarChanged)
            {
                MessageService.ShowMessage("Toolbar specification has changed. Please restart SOLIDWORKS",
                    MessageType_e.Info);
            }
        }

        private void SaveSettingChanges(ToolbarSettings toolbarSets, CustomToolbarInfo toolbarConf, out bool isToolbarChanged)
        {
            isToolbarChanged = false;
            
            var settsProvider = m_Services.GetService<ISettingsProvider>();
            var oldToolbarSetts = settsProvider.GetSettings();

            if (!DeepCompare(toolbarSets, oldToolbarSetts))
            {
                settsProvider.SaveSettings(toolbarSets);
            }

            var toolbarConfProvider = ToolbarProvider;

            bool isReadOnly;

            var oldToolbarConf = toolbarConfProvider
                .GetToolbar(out isReadOnly, oldToolbarSetts.SpecificationFile);

            isToolbarChanged = !DeepCompare(toolbarConf, oldToolbarConf);

            if (isToolbarChanged)
            {
                if (!isReadOnly)
                {
                    toolbarConfProvider.SaveToolbar(toolbarConf, toolbarSets.SpecificationFile);
                }
                else
                {
                    throw new ToolbarConfigurationReadOnlyException();
                }
            }
        }

        private bool DeepCompare(object obj1, object obj2)
        {
            return JToken.DeepEquals(JToken.FromObject(obj1), JToken.FromObject(obj2));
        }

        private IToolbarConfigurationProvider ToolbarProvider
        {
            get
            {
                return m_Services.GetService<IToolbarConfigurationProvider>();
            }
        }

        private string ToolbarSpecificationFile
        {
            get
            {
                return m_Services.GetService<ISettingsProvider>().GetSettings().SpecificationFile;
            }
        }

        private IMessageService MessageService
        {
            get
            {
                return m_Services.GetService<IMessageService>();
            }
        }
    }
}