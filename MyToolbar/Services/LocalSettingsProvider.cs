//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Properties;
using CodeStack.Sw.MyToolbar.Structs;
using System.IO;
using Xarial.AppLaunchKit.Base.Services;

namespace CodeStack.Sw.MyToolbar.Services
{
    public interface ILocalSettingsProvider
    {
        ToolbarLocalSettings GetLocalSettings();
        void SaveLocalSettings(ToolbarLocalSettings localSetts);
    }

    public class LocalSettingsProvider : ILocalSettingsProvider
    {
        private readonly IUserSettingsService m_UserSettsSrv;

        public LocalSettingsProvider(IUserSettingsService userSettsSrv)
        {
            m_UserSettsSrv = userSettsSrv;
        }

        public ToolbarLocalSettings GetLocalSettings()
        {
            ToolbarLocalSettings localSetts;

            try
            {
                localSetts = m_UserSettsSrv.ReadSettings<ToolbarLocalSettings>(Settings.Default.SettingsStoreName);
            }
            catch
            {
                localSetts = new ToolbarLocalSettings();
            }

            return localSetts;
        }

        public void SaveLocalSettings(ToolbarLocalSettings localSetts)
        {
            m_UserSettsSrv.StoreSettings(localSetts, Settings.Default.SettingsStoreName);
        }
    }
}