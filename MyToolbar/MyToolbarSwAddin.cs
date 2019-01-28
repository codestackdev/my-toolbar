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

namespace CodeStack.Sw.MyToolbar
{
    [Guid("63496b16-e9ad-4d3a-8473-99d124a1672b"), ComVisible(true)]
#if DEBUG
    [AutoRegister("MyToolbar", "Add-in for managing custom toolbars", true)]
#endif
    public class MyToolbarSwAddin : SwAddInEx
    {
        private CustomToolbarInfo m_ToolbarInfo;

        private ServicesManager m_Services;

        public override bool OnConnect()
        {
            m_Services = RegisterServicesManager(App);

            m_ToolbarInfo = m_Services.GetService<IUserSettingsService>()
                    .ReadSettings<CustomToolbarInfo>(Locations.ToolbarsSpecFilePath);

            if (m_ToolbarInfo.Groups != null)
            {
                foreach (var grp in m_ToolbarInfo.Groups)
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
                    var vm = new CommandManagerVM(m_ToolbarInfo);
                    if (new CommandManagerForm(vm, IntPtr.Zero).ShowDialog() == true)
                    {
                        //TODO: update settings
                    }
                    break;

                case Commands_e.About:
                    m_Services.GetService<IAboutApplicationService>().ShowAboutForm();
                    break;
            }
        }

        private ServicesManager RegisterServicesManager(ISldWorks app)
        {
            var srv = new ServicesManager(this.GetType().Assembly, new IntPtr(app.IFrameObject().GetHWnd()),
                typeof(UpdatesService),
                typeof(UserSettingsService),
                typeof(AboutApplicationService));

            srv.HandleError += OnHandleError;

            srv.StartServicesInBackground();

            return srv;
        }

        private bool OnHandleError(Exception ex)
        {
            Logger.Log(ex);

            return true;
        }
    }
}
