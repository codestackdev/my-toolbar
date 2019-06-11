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
    public interface ISettingsProvider
    {
        ToolbarSettings GetSettings();

        void SaveSettings(ToolbarSettings setts);
    }

    public class SettingsProvider : ISettingsProvider
    {
        private readonly IUserSettingsService m_UserSettsSrv;

        public SettingsProvider(IUserSettingsService userSettsSrv)
        {
            m_UserSettsSrv = userSettsSrv;
        }

        public ToolbarSettings GetSettings()
        {
            ToolbarSettings setts;
            try
            {
                setts = m_UserSettsSrv.ReadSettings<ToolbarSettings>(Settings.Default.SettingsStoreName);
            }
            catch
            {
                setts = new ToolbarSettings()
                {
                    SpecificationFile = ToolbarsDefaultSpecFilePath
                };
            }

            return setts;
        }

        public void SaveSettings(ToolbarSettings setts)
        {
            m_UserSettsSrv.StoreSettings(setts, Settings.Default.SettingsStoreName);
        }

        private string ToolbarsDefaultSpecFilePath
        {
            get
            {
                var dataFile = Path.Combine(Locations.AppDirectoryPath,
                    Settings.Default.ToolbarsSpecFile);

                return dataFile;
            }
        }
    }
}