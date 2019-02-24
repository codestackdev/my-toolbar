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
            m_Services = new ServicesContainer(App, Logger);
            
            try
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
            catch(Exception ex)
            {
                MessageService.ShowMessage("Failed to load toolbar specification", MessageType_e.Warning);
                Logger.Log(ex);
            }
            
            AddCommandGroup<Commands_e>(OnButtonClick);
            
            return true;
        }

        private void OnMacroCommandClick(CommandMacroInfo cmd)
        {
            try
            {
                m_Services.GetService<IMacroRunner>().RunMacro(cmd.MacroPath, cmd.EntryPoint);
            }
            catch(Exception ex)
            {
                Logger.Log(ex);
                MessageService.ShowMessage("Failed to run macro", MessageType_e.Error);
            }
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
                        try
                        {
                            var isSettsChanged = true;

                            if (isSettsChanged)
                            {
                                var settsProvider = m_Services.GetService<ISettingsProvider>();
                                settsProvider.SaveSettings(vm.Settings);
                            }

                            //TODO: compare if changed
                            var isToolbarChanged = true;

                            if (isToolbarChanged)
                            {
                                ToolbarProvider.SaveToolbar(vm.ToolbarInfo, vm.Settings.SpecificationFile);
                                MessageService.ShowMessage("Toolbar specification has changed. Please restart SOLIDWORKS",
                                    MessageType_e.Info);
                            }
                        }
                        catch(Exception ex)
                        {
                            Logger.Log(ex);
                            MessageService.ShowMessage("Failed to save toolbar specification", MessageType_e.Error);
                        }
                    }
                    break;

                case Commands_e.About:
                    m_Services.GetService<IAboutApplicationService>().ShowAboutForm();
                    break;
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
