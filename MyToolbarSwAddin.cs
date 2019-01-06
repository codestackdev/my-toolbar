using System;
using System.Runtime.InteropServices;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using CodeStack.Community.Sw.MyToolbar.Preferences;
using System.IO;
using Newtonsoft.Json;
using CodeStack.Community.Sw.MyToolbar.Properties;
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
using CodeStack.Community.Sw.MyToolbar.Base;

namespace CodeStack.Community.Sw.MyToolbar
{
    [Guid("63496b16-e9ad-4d3a-8473-99d124a1672b"), ComVisible(true)]
#if DEBUG
    [AutoRegister("MyToolbar", "Add-in for managing custom toolbars", true)]
#endif
    public class MyToolbarSwAddin : SwAddInEx
    {
        private CustomToolbarInfo m_ToolbarInfo;

        private ServicesManager m_Kit;

        public override bool OnConnect()
        {
            m_Kit = RegisterServicesManager(App);

            m_ToolbarInfo = m_Kit.GetService<IUserSettingsService>()
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
        }

        private ServicesManager RegisterServicesManager(ISldWorks app)
        {
            var kit = new ServicesManager(this.GetType().Assembly, new IntPtr(app.IFrameObject().GetHWnd()),
                typeof(UpdatesService),
                typeof(UserSettingsService),
                typeof(AboutApplicationService));

            kit.HandleError += OnHandleError;

            kit.StartServicesInBackground();

            return kit;
        }

        private bool OnHandleError(Exception ex)
        {
            Logger.Log(ex);

            return true;
        }
    }
}
