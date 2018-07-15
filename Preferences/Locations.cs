//**********************
//MyToolbar
//Copyright(C) 2018 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

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

        internal static string DataFilePath
        {
            get
            {
                var dataFile = Path.Combine(AppDirectoryPath,
                    Settings.Default.DataFile);

                return dataFile;
            }
        }
    }
}
