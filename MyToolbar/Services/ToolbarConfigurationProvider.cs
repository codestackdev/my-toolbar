//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Structs;
using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
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
            isReadOnly = !IsEditable(toolbarSpecFilePath);
            return m_UserSettsSrv.ReadSettings<CustomToolbarInfo>(toolbarSpecFilePath);
        }

        private bool IsEditable(string filePath)
        {
            if (!new FileInfo(filePath).IsReadOnly)
            {
                var permissionSet = new PermissionSet(PermissionState.None);
                var writePermission = new FileIOPermission(
                    FileIOPermissionAccess.Write, filePath);
                permissionSet.AddPermission(writePermission);

                return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
            }
            else
            {
                return false;
            }
        }

        public void SaveToolbar(CustomToolbarInfo toolbar, string toolbarSpecFilePath)
        {
            m_UserSettsSrv.StoreSettings(toolbar, toolbarSpecFilePath);
        }
    }
}