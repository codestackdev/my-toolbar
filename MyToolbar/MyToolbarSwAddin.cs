using System;
using System.Runtime.InteropServices;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using CodeStack.Sw.MyToolbar.Structs;
using System.IO;
using Newtonsoft.Json;
using CodeStack.Sw.MyToolbar.Properties;
using System.Linq;
using SolidWorks.Interop.swconst;
using System.Drawing;
using CodeStack.SwEx.AddIn;
using CodeStack.SwEx.AddIn.Attributes;
using Xarial.AppLaunchKit;
using Xarial.AppLaunchKit.Services.Updates;
using Xarial.AppLaunchKit.Services.About;
using Xarial.AppLaunchKit.Services.UserSettings;
using Xarial.AppLaunchKit.Base.Services;
using CodeStack.SwEx.AddIn.Core;
using CodeStack.Sw.MyToolbar.Base;
using CodeStack.Sw.MyToolbar.UI.Forms;
using CodeStack.Sw.MyToolbar.UI.ViewModels;
using CodeStack.Sw.MyToolbar.Services;
using Newtonsoft.Json.Linq;
using CodeStack.Sw.MyToolbar.Exceptions;
using System.Windows.Threading;
using System.Threading;

namespace CodeStack.Sw.MyToolbar
{
    [Guid("63496b16-e9ad-4d3a-8473-99d124a1672b"), ComVisible(true)]
#if DEBUG
    [AutoRegister("MyToolbar", "Add-in for managing custom toolbars", true)]
#endif
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

                ExecuteUserCommand(LoadUserToolbar, e => "Failed to load toolbar specification");

                AddCommandGroup<Commands_e>(OnButtonClick);

                return true;
            }
            catch(Exception ex)
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
            ExecuteUserCommand(() => m_Services.GetService<IMacroRunner>().RunMacro(cmd.MacroPath, cmd.EntryPoint),
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
                        UpdatedToolbarConfiguration(vm.Settings, vm.ToolbarInfo);
                    }
                    break;

                case Commands_e.About:
                    m_Services.GetService<IAboutApplicationService>().ShowAboutForm();
                    break;
            }
        }

        private void UpdatedToolbarConfiguration(ToolbarSettings toolbarSets, CustomToolbarInfo toolbarConf)
        {
            ExecuteUserCommand(() =>
            {
                var isSettsChanged = true;

                if (isSettsChanged)
                {
                    var settsProvider = m_Services.GetService<ISettingsProvider>();
                    settsProvider.SaveSettings(toolbarSets);
                }

                bool isToolbarChanged;

                SaveSettingChanges(toolbarSets, toolbarConf, out isToolbarChanged);

                if (isToolbarChanged)
                {
                    MessageService.ShowMessage("Toolbar specification has changed. Please restart SOLIDWORKS",
                        MessageType_e.Info);
                }
            }, e => "Failed to save toolbar specification");
        }

        private void SaveSettingChanges(ToolbarSettings toolbarSets, CustomToolbarInfo toolbarConf, out bool isToolbarChanged)
        {
            isToolbarChanged = false;

            var settsProvider = m_Services.GetService<ISettingsProvider>();
            var oldToolbarSetts = settsProvider.GetSettings();

            if (!JToken.DeepEquals(JToken.FromObject(toolbarSets), JToken.FromObject(oldToolbarSetts)))
            {
                settsProvider.SaveSettings(toolbarSets);
            }

            var toolbarConfProvider = ToolbarProvider;

            bool isReadOnly;

            var oldToolbarConf = toolbarConfProvider
                .GetToolbar(out isReadOnly, oldToolbarSetts.SpecificationFile);

            isToolbarChanged = !JToken.DeepEquals(JToken.FromObject(toolbarConf), JToken.FromObject(oldToolbarConf));

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

        private void ExecuteUserCommand(Action cmd, Func<Exception, string> unknownErrorDescriptionHandler)
        {
            try
            {
                cmd.Invoke();
            }
            catch (UserException ex)
            {
                Logger.Log(ex);
                MessageService.ShowMessage(ex.Message, MessageType_e.Error);
            }
            catch (Exception ex)
            {
                var errDesc = unknownErrorDescriptionHandler.Invoke(ex);
                Logger.Log(ex);
                MessageService.ShowMessage(errDesc, MessageType_e.Error);
            }
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
