//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Services;
using CodeStack.Sw.MyToolbar.UI.ViewModels;
using CodeStack.SwEx.AddIn;
using CodeStack.SwEx.AddIn.Base;
using CodeStack.SwEx.AddIn.Core;
using CodeStack.SwEx.Common.Diagnostics;
using SolidWorks.Interop.sldworks;
using System;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Xarial.AppLaunchKit;
using Xarial.AppLaunchKit.Base.Services;
using Xarial.AppLaunchKit.Services.About;
using Xarial.AppLaunchKit.Services.Updates;
using Xarial.AppLaunchKit.Services.UserSettings;

namespace CodeStack.Sw.MyToolbar
{
    public class ServicesContainer : IDisposable
    {
        internal static ServicesContainer Instance { get; private set; }

        private readonly UnityContainer m_Container;
        private readonly ServicesManager m_Kit;
        private readonly ILogger m_Logger;
        private readonly IToolbarAddIn m_AddIn;

        public ServicesContainer(ISldWorks app, IToolbarAddIn addIn, ILogger logger)
        {
            Instance = this;
            m_AddIn = addIn;

            m_Logger = logger;

            m_Container = new UnityContainer();

            m_Kit = RegisterServicesManager(app);

            m_Container.RegisterInstance(app);
            m_Container.RegisterInstance(m_AddIn);

            m_Container.RegisterType<IMacroEntryPointsExtractor, MacroEntryPointsExtractor>();

            m_Container.RegisterType<IMacroRunner, MacroRunner>();

            m_Container.RegisterType<IToolbarConfigurationProvider, ToolbarConfigurationProvider>();

            m_Container.RegisterType<ISettingsProvider, SettingsProvider>();

            m_Container.RegisterType<ILocalSettingsProvider, LocalSettingsProvider>();

            m_Container.RegisterType<IMessageService, MessageService>();

            m_Container.RegisterSingleton<CommandManagerVM>();

            m_Container.RegisterFactory<IDocumentsHandler<DocumentHandler>>(c => m_AddIn.CreateDocumentsHandler());

            m_Container.RegisterInstance(m_Logger);

            m_Container.RegisterSingleton<ICommandsManager, CommandsManager>();
            m_Container.RegisterSingleton<ITriggersManager, TriggersManager>();

            m_Container.RegisterInstance(m_Kit.GetService<IUserSettingsService>());
            m_Container.RegisterInstance(m_Kit.GetService<IAboutApplicationService>());
        }

        internal TService GetService<TService>()
        {
            return m_Container.Resolve<TService>();
        }

        private ServicesManager RegisterServicesManager(ISldWorks app)
        {
            var srv = new ServicesManager(this.GetType().Assembly, new IntPtr(app.IFrameObject().GetHWnd()),
                typeof(UpdatesService),
                typeof(UserSettingsService),
                typeof(AboutApplicationService));

            srv.HandleError += OnHandleError;

            srv.StartServicesInBackground();

            return srv;
        }

        private bool OnHandleError(Exception ex)
        {
            m_Logger.Log(ex);

            return true;
        }

        public void Dispose()
        {
            m_Container.Dispose();
        }
    }
}