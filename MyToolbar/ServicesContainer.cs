//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Services;
using CodeStack.Sw.MyToolbar.UI.ViewModels;
using CodeStack.SwEx.Common.Diagnostics;
using SolidWorks.Interop.sldworks;
using System;
using Unity;
using Unity.Lifetime;
using Xarial.AppLaunchKit;
using Xarial.AppLaunchKit.Base.Services;
using Xarial.AppLaunchKit.Services.About;
using Xarial.AppLaunchKit.Services.Updates;
using Xarial.AppLaunchKit.Services.UserSettings;

namespace CodeStack.Sw.MyToolbar
{
    public class ServicesContainer
    {
        public static ServicesContainer Instance
        {
            get;
            private set;
        }

        private readonly UnityContainer m_Container;
        private readonly ServicesManager m_Kit;
        private readonly ILogger m_Logger;

        public ServicesContainer(ISldWorks app, ILogger logger)
        {
            Instance = this;

            m_Logger = logger;

            m_Container = new UnityContainer();

            m_Kit = RegisterServicesManager(app);

            m_Container.RegisterInstance(app);

            m_Container.RegisterType<IMacroEntryPointsExtractor, MacroEntryPointsExtractor>(
                new ContainerControlledLifetimeManager());

            m_Container.RegisterType<IMacroRunner, MacroRunner>(
                new ContainerControlledLifetimeManager());

            m_Container.RegisterType<IToolbarConfigurationProvider, ToolbarConfigurationProvider>(
                new ContainerControlledLifetimeManager());

            m_Container.RegisterType<ISettingsProvider, SettingsProvider>(
                new ContainerControlledLifetimeManager());

            m_Container.RegisterType<IMessageService, MessageService>(
                new ContainerControlledLifetimeManager());

            m_Container.RegisterType<CommandManagerVM>(new TransientLifetimeManager());

            m_Container.RegisterInstance(m_Logger);

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
    }
}