using CodeStack.Sw.MyToolbar.Properties;
using System;
using System.IO;

namespace CodeStack.Sw.MyToolbar.Structs
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
    }
}
