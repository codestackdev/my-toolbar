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

        private Dictionary<Triggers_e, CommandMacroInfo[]> m_Triggers;

        private IDocumentsHandler<DocumentHandler> m_DocHandler;

        public override bool OnConnect()
        {
            try
            {
                if (Dispatcher.CurrentDispatcher != null)
                {
                    Dispatcher.CurrentDispatcher.UnhandledException += OnDispatcherUnhandledException;
                }

                AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
                m_Services = new ServicesContainer(App, Logger);

                CustomToolbarInfo toolbarInfo = null;
                ExceptionHelper.ExecuteUserCommand(() => toolbarInfo = LoadUserToolbar(),
                    e => "Failed to load toolbar specification");

                ExceptionHelper.ExecuteUserCommand(() => LoadTriggers(toolbarInfo),
                    e => "Failed to load toolbar specification");

                AddCommandGroup<Commands_e>(OnButtonClick);

                m_DocHandler = CreateDocumentsHandler();
                m_DocHandler.HandlerCreated += OnDocumentHandlerCreated;

                InvokeTrigger(Triggers_e.ApplicationStart);

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
            InvokeTrigger(Triggers_e.ApplicationClose);

            m_DocHandler.HandlerCreated -= OnDocumentHandlerCreated;

            return true;
        }

        private void OnDocumentHandlerCreated(DocumentHandler doc)
        {
            if (doc.Model == App.IActiveDoc2)
            {
                InvokeTrigger(Triggers_e.DocumentOpen);
            }

            foreach (var trigger in m_Triggers.Keys)
            {
                switch (trigger)
                {
                    case Triggers_e.DocumentSave:
                        doc.Save += OnSave;
                        break;
                    case Triggers_e.NewSelection:
                        doc.Selection += OnSelection;
                        break;
                    case Triggers_e.ConfigurationChange:
                        doc.ConfigurationChange += OnConfigurationChange;
                        break;
                    case Triggers_e.Rebuild:
                        doc.Rebuild += OnRebuild;
                        break;
                }
            }

            doc.Destroyed += OnDestroyed;
        }

        private bool OnSave(DocumentHandler docHandler, string fileName, SwEx.AddIn.Enums.SaveState_e state)
        {
            if (state == SwEx.AddIn.Enums.SaveState_e.PreSave)
            {
                InvokeTrigger(Triggers_e.DocumentSave);
            }

            return true;
        }

        private bool OnSelection(DocumentHandler docHandler, SolidWorks.Interop.swconst.swSelectType_e selType, SwEx.AddIn.Enums.SelectionState_e state)
        {
            if (state == SwEx.AddIn.Enums.SelectionState_e.NewSelection)
            {
                InvokeTrigger(Triggers_e.NewSelection);
            }

            return true;
        }

        private void OnConfigurationChange(DocumentHandler docHandler, SwEx.AddIn.Enums.ConfigurationChangeState_e state, string confName)
        {
            if (state == SwEx.AddIn.Enums.ConfigurationChangeState_e.PostActivate)
            {
                InvokeTrigger(Triggers_e.ConfigurationChange);
            }
        }

        private bool OnRebuild(DocumentHandler docHandler, SwEx.AddIn.Enums.RebuildState_e state)
        {
            if (state == SwEx.AddIn.Enums.RebuildState_e.PostRebuild)
            {
                InvokeTrigger(Triggers_e.Rebuild);
            }

            return true;
        }

        private void OnDestroyed(DocumentHandler docHandler)
        {
            InvokeTrigger(Triggers_e.DocumentClose);

            docHandler.Save -= OnSave;
            docHandler.Selection -= OnSelection;
            docHandler.ConfigurationChange -= OnConfigurationChange;
            docHandler.Rebuild -= OnRebuild;
            docHandler.Destroyed -= OnDestroyed;
        }

        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log(e.ExceptionObject as Exception);
            MessageService.ShowMessage("Unknown domain error", MessageType_e.Error);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Logger.Log(e.Exception);
            MessageService.ShowMessage("Unknown dispatcher error", MessageType_e.Error);
        }

        private CustomToolbarInfo LoadUserToolbar()
        {
            bool isReadOnly;
            var toolbarInfo = ToolbarProvider.GetToolbar(out isReadOnly,
                ToolbarSpecificationFile);

            if (toolbarInfo?.Groups != null)
            {
                foreach (var grp in toolbarInfo.Groups
                    .Where(g => g.Commands?.Any(c => c.Triggers.HasFlag(Triggers_e.Button)) == true))
                {
                    var cmdGrp = new CommandGroupInfoSpec(grp, App);
                    cmdGrp.MacroCommandClick += OnMacroCommandClick;

                    Logger.Log($"Adding command group: {cmdGrp.Title} [{cmdGrp.Id}]. Commands: {string.Join(", ", cmdGrp.Commands.Select(c => $"{c.Title} [{c.UserId}]").ToArray())}");

                    AddCommandGroup(cmdGrp);
                }
            }

            return toolbarInfo;
        }

        private void LoadTriggers(CustomToolbarInfo toolbarInfo)
        {
            m_Triggers = new Dictionary<Triggers_e, CommandMacroInfo[]>();

            var allCmds = toolbarInfo.Groups?.SelectMany(g => g.Commands);

            if (allCmds == null)
            {
                return;
            }

            var triggers = EnumHelper.GetFlags(typeof(Triggers_e)).Where(e => !e.Equals(Triggers_e.Button));

            foreach (Triggers_e trigger in triggers)
            {
                var cmds = allCmds.Where(c => c.Scope.HasFlag(trigger));

                if (cmds.Any())
                {
                    m_Triggers.Add(trigger, cmds.ToArray());
                }
            }
        }

        private void OnMacroCommandClick(CommandMacroInfo cmd)
        {
            RunMacroCommand(cmd);
        }

        private void InvokeTrigger(Triggers_e trigger)
        {
            CommandMacroInfo[] cmds;

            if (m_Triggers.TryGetValue(trigger, out cmds))
            {
                cmds = cmds.Where(c => c.Scope.IsInScope(App)).ToArray();

                if (cmds != null && cmds.Any())
                {
                    Logger.Log($"Invoking {cmds.Length} command(s) for the trigger {trigger}");

                    foreach (var cmd in cmds)
                    {
                        RunMacroCommand(cmd);
                    }
                }
            }
        }

        private void RunMacroCommand(CommandMacroInfo cmd)
        {
            ExceptionHelper.ExecuteUserCommand(() => m_Services.GetService<IMacroRunner>().RunMacro(cmd.MacroPath, cmd.EntryPoint),
                e => "Failed to run macro");
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
                        ExceptionHelper.ExecuteUserCommand(() =>
                        {
                            UpdatedToolbarConfiguration(vm.Settings, vm.ToolbarInfo, vm.IsEditable);
                        }, e => "Failed to save toolbar specification");
                    }
                    break;

                case Commands_e.About:
                    m_Services.GetService<IAboutApplicationService>().ShowAboutForm();
                    break;
            }
        }

        private void UpdatedToolbarConfiguration(ToolbarSettings toolbarSets, CustomToolbarInfo toolbarConf, bool isEditable)
        {
            bool isToolbarChanged;

            SaveSettingChanges(toolbarSets, toolbarConf, isEditable, out isToolbarChanged);

            if (isToolbarChanged)
            {
                MessageService.ShowMessage("Toolbar specification has changed. Please restart SOLIDWORKS",
                    MessageType_e.Info);
            }
        }

        private void SaveSettingChanges(ToolbarSettings toolbarSets, CustomToolbarInfo toolbarConf,
            bool isEditable, out bool isToolbarChanged)
        {
            isToolbarChanged = false;

            var settsProvider = m_Services.GetService<ISettingsProvider>();
            var oldToolbarSetts = settsProvider.GetSettings();

            if (!DeepCompare(toolbarSets, oldToolbarSetts))
            {
                settsProvider.SaveSettings(toolbarSets);
            }

            var toolbarConfProvider = ToolbarProvider;

            bool isReadOnly;

            var oldToolbarConf = toolbarConfProvider
                .GetToolbar(out isReadOnly, oldToolbarSetts.SpecificationFile);

            isToolbarChanged = !DeepCompare(toolbarConf, oldToolbarConf);

            if (isToolbarChanged)
            {
                if (isEditable)
                {
                    toolbarConfProvider.SaveToolbar(toolbarConf, toolbarSets.SpecificationFile);
                }
                else
                {
                    Logger.Log("Skipped saving of read-only toolbar settings");
                }
            }
        }

        private bool DeepCompare(object obj1, object obj2)
        {
            return JToken.DeepEquals(JToken.FromObject(obj1), JToken.FromObject(obj2));
        }

        private IToolbarConfigurationProvider ToolbarProvider
        {
            get
            {
                return m_Services.GetService<IToolbarConfigurationProvider>();
            }
        }

        private string ToolbarSpecificationFile
        {
            get
            {
                return m_Services.GetService<ISettingsProvider>().GetSettings().SpecificationFile;
            }
        }

        private IMessageService MessageService
        {
            get
            {
                return m_Services.GetService<IMessageService>();
            }
        }
    }
}