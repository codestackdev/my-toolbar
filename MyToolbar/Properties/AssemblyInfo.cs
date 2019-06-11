//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar;
using CodeStack.Sw.MyToolbar.Properties;
using System.Reflection;
using System.Runtime.InteropServices;
using Xarial.AppLaunchKit.Attributes;
using Xarial.AppLaunchKit.Services.Attributes;

[assembly: AssemblyTitle("MyToolbar")]
[assembly: AssemblyDescription("Add-in to manage custom toolbars in SOLIDWORKS")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("CodeStack")]
[assembly: AssemblyProduct("MyToolbar")]
[assembly: AssemblyCopyright("Copyright(C) 2019 www.codestack.net")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
[assembly: AssemblyFileVersion("2.0.0.0")]
[assembly: ComVisible(false)]
[assembly: ApplicationInfo(typeof(AppInfo),
        nameof(AppInfo.WorkingDir), nameof(AppInfo.Title), nameof(AppInfo.Icon))]
[assembly: UpdatesUrl(typeof(Settings), nameof(Settings.Default) + "." + nameof(Settings.Default.UpgradeUrl))]
[assembly: About(typeof(Resources), nameof(Resources.eula), nameof(Resources.Licenses), nameof(Resources.custom_toolbars_toolbar))]
[assembly: UserSettings("Settings", true, typeof(ToolbarInfoVersionConverter))]