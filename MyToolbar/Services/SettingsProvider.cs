using CodeStack.Sw.MyToolbar.Preferences;
using CodeStack.Sw.MyToolbar.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var setts = m_UserSettsSrv.ReadSettings<ToolbarSettings>(Settings.Default.SettingsFile);
            if (setts == null)
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
            m_UserSettsSrv.StoreSettings(setts, Settings.Default.SettingsFile);
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
