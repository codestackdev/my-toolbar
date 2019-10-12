//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Base;
using CodeStack.Sw.MyToolbar.Enums;
using CodeStack.Sw.MyToolbar.Exceptions;
using CodeStack.Sw.MyToolbar.Helpers;
using CodeStack.Sw.MyToolbar.Services;
using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.Sw.MyToolbar.UI.Forms;
using CodeStack.Sw.MyToolbar.UI.ViewModels;
using CodeStack.SwEx.AddIn;
using CodeStack.SwEx.AddIn.Attributes;
using CodeStack.SwEx.AddIn.Base;
using CodeStack.SwEx.AddIn.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using Xarial.AppLaunchKit.Base.Services;

namespace CodeStack.Sw.MyToolbar
{
    [Guid("63496b16-e9ad-4d3a-8473-99d124a1672b"), ComVisible(true)]
    [AutoRegister("MyToolbar", "Add-in for managing custom toolbars", true)]
    public class MyToolbarSwAddin : SwAddInEx
    {
        private ServicesContainer m_Services;
        private IMessageService m_Msg;
        private ICommandsManager m_CmdsMgr;
        private ITriggersManager m_TriggersMgr;

        public override bool OnConnect()
        {
            try
            {
                if (Dispatcher.CurrentDispatcher != null)
                {
                    Dispatcher.CurrentDispatcher.UnhandledException += OnDispatcherUnhandledException;
                }

                AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
                m_Services = new ServicesContainer(App, this);
                m_Msg = m_Services.GetService<IMessageService>();

                m_CmdsMgr = m_Services.GetService<ICommandsManager>();
                m_TriggersMgr = m_Services.GetService<ITriggersManager>();
                
                AddCommandGroup<Commands_e>(OnButtonClick);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                new MessageService().ShowMessage("Critical error while loading add-in", MessageType_e.Error);
                throw;
            }
        }

        public override bool OnDisconnect()
        {
            m_Services.Dispose();
            return true;
        }
        
        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log(e.ExceptionObject as Exception);
            m_Msg.ShowMessage("Unknown domain error", MessageType_e.Error);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Logger.Log(e.Exception);
            m_Msg.ShowMessage("Unknown dispatcher error", MessageType_e.Error);
        }

        private void OnButtonClick(Commands_e cmd)
        {
            switch (cmd)
            {
                case Commands_e.Configuration:

                    var vm = m_Services.GetService<CommandManagerVM>();

                    if (new CommandManagerForm(vm,
                        new IntPtr(App.IFrameObject().GetHWnd())).ShowDialog() == true)
                    {
                        try
                        {
                            m_CmdsMgr.UpdatedToolbarConfiguration(vm.Settings, vm.ToolbarInfo, vm.IsEditable);
                        }
                        catch(Exception ex)
                        {
                            m_Msg.ShowError(ex, "Failed to save toolbar specification");
                        }
                    }
                    break;

                case Commands_e.About:
                    m_Services.GetService<IAboutApplicationService>().ShowAboutForm();
                    break;
            }
        }
    }
}