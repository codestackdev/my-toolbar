using System;
using System.Runtime.InteropServices;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using CodeStack.Sw.MyToolbar.Preferences;
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

            var toolbarInfo = ToolbarProvider.GetToolbar(out bool isReadOnly,
                ToolbarSpecificationFile);

            if (toolbarInfo?.Groups != null)
            {
                foreach (var grp in toolbarInfo.Groups)
                {
                    AddCommandGroup(new CommandGroupInfoSpec(grp));
                }
            }

            AddCommandGroup<Commands_e>(OnButtonClick);
            
            return true;
        }

        private void OnButtonClick(Commands_e cmd)
        {
            switch (cmd)
            {
                case Commands_e.Configuration:

                    var settsProvider = m_Services.GetService<ISettingsProvider>();

                    var vm = new CommandManagerVM(ToolbarProvider, settsProvider);
                    
                    if (new CommandManagerForm(vm, 
                        new IntPtr(App.IFrameObject().GetHWnd())).ShowDialog() == true)
                    {
                        var isSettsChanged = true;

                        if (isSettsChanged)
                        {
                            settsProvider.SaveSettings(vm.Settings);
                        }

                        //TODO: compare if changed
                        var isToolbarChanged = true;

                        if (isToolbarChanged)
                        {
                            ToolbarProvider.SaveToolbar(vm.ToolbarInfo, vm.Settings.SpecificationFile);
                            //TODO: show message to reload toolbar
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
    }
}
