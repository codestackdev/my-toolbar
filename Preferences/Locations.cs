using CodeStack.Community.Sw.MyToolbar.Properties;
using System;
using System.IO;

namespace CodeStack.Community.Sw.MyToolbar.Preferences
{
    internal static class Locations
    {
        internal static string AppDirectoryPath
        {
            get
            {
                var appDir = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                    Settings.Default.AppRootDir);

                return appDir;
            }
        }

        internal static string ToolbarsSpecFilePath
        {
            get
            {
                var dataFile = Path.Combine(AppDirectoryPath,
                    Settings.Default.ToolbarsSpecFile);

                return dataFile;
            }
        }
    }
}
