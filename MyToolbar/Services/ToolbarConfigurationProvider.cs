using CodeStack.Sw.MyToolbar.Preferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xarial.AppLaunchKit.Base.Services;

namespace CodeStack.Sw.MyToolbar.Services
{
    public interface IToolbarConfigurationProvider
    {
        CustomToolbarInfo GetToolbar(out bool isReadOnly, string toolbarSpecFilePath);
        void SaveToolbar(CustomToolbarInfo toolbar, string toolbarSpecFilePath);
    }

    public class ToolbarConfigurationProvider : IToolbarConfigurationProvider
    {
        private readonly IUserSettingsService m_UserSettsSrv;

        public ToolbarConfigurationProvider(IUserSettingsService userSettsSrv)
        {
            m_UserSettsSrv = userSettsSrv;
        }

        public CustomToolbarInfo GetToolbar(out bool isReadOnly, string toolbarSpecFilePath)
        {
            isReadOnly = false;
            return m_UserSettsSrv.ReadSettings<CustomToolbarInfo>(toolbarSpecFilePath);
        }

        public void SaveToolbar(CustomToolbarInfo toolbar, string toolbarSpecFilePath)
        {
            m_UserSettsSrv.StoreSettings(toolbar, toolbarSpecFilePath);
        }
    }
}
