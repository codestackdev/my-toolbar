using System.Runtime.InteropServices;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xarial.AppLaunchKit.Services.Attributes;
using Xarial.AppLaunchKit.Attributes;
using static CodeStack.Community.Sw.MyToolbar.MyToolbarSwAddin;
using CodeStack.Community.Sw.MyToolbar.Properties;
using CodeStack.Community.Sw.MyToolbar;

[assembly: AssemblyTitle("MyToolbar")]
[assembly: AssemblyDescription("Add-in to manage custom toolbars in SOLIDWORKS")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("CodeStack")]
[assembly: AssemblyProduct("MyToolbar")]
[assembly: AssemblyCopyright("Copyright(C) 2018 www.codestack.net")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("1.1.0.0")]

[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
[assembly: AssemblyFileVersion("1.1.0.0")]
[assembly: ComVisible(false)]

[assembly: ApplicationInfo(typeof(AppInfo),
        nameof(AppInfo.WorkingDir), nameof(AppInfo.Title), nameof(AppInfo.Icon))]

[assembly: UpdatesUrl(typeof(Settings), nameof(Settings.Default) + "." + nameof(Settings.Default.UpgradeUrl))]
[assembly: About(typeof(Resources), nameof(Resources.eula), nameof(Resources.Licenses), nameof(Resources.custom_toolbars_toolbar))]
[assembly: UserSettings("Settings", false, typeof(ToolbarInfoVersionConverter))]