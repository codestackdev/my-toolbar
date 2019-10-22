//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Enums;
using CodeStack.Sw.MyToolbar.Helpers;
using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.SwEx.AddIn.Base;
using CodeStack.SwEx.AddIn.Core;
using CodeStack.SwEx.AddIn.Enums;
using CodeStack.SwEx.Common.Diagnostics;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeStack.Sw.MyToolbar.Services
{
    public interface ITriggersManager : IDisposable
    {
    }

    public class TriggersManager : ITriggersManager
    {
        private readonly ISldWorks m_App;
        private readonly Dictionary<Triggers_e, CommandMacroInfo[]> m_Triggers;
        private readonly IDocumentsHandler<DocumentHandler> m_DocHandler;
        private readonly IMacroRunner m_MacroRunner;
        private readonly IMessageService m_Msg;
        private readonly ILogger m_Logger;
        private readonly ICommandsManager m_CmdMgr;

        public TriggersManager(ICommandsManager cmdMgr, ISldWorks app, IDocumentsHandler<DocumentHandler> docsHandler, 
            IMacroRunner macroRunner, IMessageService msgSvc, ILogger logger)
        {
            m_CmdMgr = cmdMgr;
            m_App = app;
            //(m_App as SldWorks).DestroyNotify += OnSwAppClose;
            m_DocHandler = docsHandler;
            m_MacroRunner = macroRunner;
            m_Msg = msgSvc;
            m_Logger = logger;

            m_Triggers = LoadTriggers(m_CmdMgr.ToolbarInfo);

            m_DocHandler.HandlerCreated += OnDocumentHandlerCreated;

            InvokeTrigger(Triggers_e.ApplicationStart);
        }

        private Dictionary<Triggers_e, CommandMacroInfo[]> LoadTriggers(CustomToolbarInfo toolbarInfo)
        {
            var triggersCmds = new Dictionary<Triggers_e, CommandMacroInfo[]>();

            var allCmds = toolbarInfo?.Groups?.SelectMany(g => g.Commands);

            if (allCmds?.Any() == true)
            {
                var triggers = EnumHelper.GetFlags(typeof(Triggers_e)).Where(e => !e.Equals(Triggers_e.Button));

                foreach (Triggers_e trigger in triggers)
                {
                    var cmds = allCmds.Where(c => c.Triggers.HasFlag(trigger));

                    if (cmds.Any())
                    {
                        triggersCmds.Add(trigger, cmds.ToArray());
                    }
                }
            }

            return triggersCmds;
        }

        private void OnDocumentHandlerCreated(DocumentHandler doc)
        {
            if (doc.Model == m_App.IActiveDoc2)
            {
                if (!string.IsNullOrEmpty(doc.Model.GetPathName()))
                {
                    InvokeTrigger(Triggers_e.DocumentOpen);
                }
                else
                {
                    InvokeTrigger(Triggers_e.DocumentNew);
                }
            }

            foreach (var trigger in m_Triggers.Keys)
            {
                switch (trigger)
                {
                    case Triggers_e.DocumentActivated:
                        doc.Activated += OnActivated;
                        break;
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

            doc.Destroyed += OnDocumentClosed;
        }

        private bool OnSave(DocumentHandler docHandler, string fileName, SaveState_e state)
        {
            if (state == SaveState_e.PreSave)
            {
                if (docHandler.Model == m_App.IActiveDoc2)
                {
                    InvokeTrigger(Triggers_e.DocumentSave);
                }
            }

            return true;
        }

        private bool OnSelection(DocumentHandler docHandler, swSelectType_e selType, SelectionState_e state)
        {
            if (state == SelectionState_e.NewSelection)
            {
                InvokeTrigger(Triggers_e.NewSelection);
            }

            return true;
        }

        private void OnConfigurationChange(DocumentHandler docHandler, ConfigurationChangeState_e state, string confName)
        {
            if (state == ConfigurationChangeState_e.PostActivate)
            {
                if (docHandler.Model == m_App.IActiveDoc2)
                {
                    InvokeTrigger(Triggers_e.ConfigurationChange);
                }
            }
        }

        private bool OnRebuild(DocumentHandler docHandler, RebuildState_e state)
        {
            if (state == RebuildState_e.PostRebuild)
            {
                if (docHandler.Model == m_App.IActiveDoc2)
                {
                    InvokeTrigger(Triggers_e.Rebuild);
                }
            }

            return true;
        }

        private void OnDocumentClosed(DocumentHandler docHandler)
        {
            if (docHandler.Model.Visible)
            {
                InvokeTrigger(Triggers_e.DocumentClose);

                docHandler.Activated -= OnActivated;
                docHandler.Save -= OnSave;
                docHandler.Selection -= OnSelection;
                docHandler.ConfigurationChange -= OnConfigurationChange;
                docHandler.Rebuild -= OnRebuild;
                docHandler.Destroyed -= OnDocumentClosed;
            }
        }

        private void OnActivated(DocumentHandler docHandler)
        {
            InvokeTrigger(Triggers_e.DocumentActivated);
        }

        //NOTE: running the macro while SW closing causes the issue when SW process hangs and not released
        //suppressing this functionality until resolved
        //private int OnSwAppClose()
        //{
        //    InvokeTrigger(Triggers_e.ApplicationClose);
        //    return 0;
        //}

        private void InvokeTrigger(Triggers_e trigger)
        {
            CommandMacroInfo[] cmds;

            if (m_Triggers.TryGetValue(trigger, out cmds))
            {
                cmds = cmds.Where(c => c.Scope.IsInScope(m_App)).ToArray();

                if (cmds != null && cmds.Any())
                {
                    m_Logger.Log($"Invoking {cmds.Length} command(s) for the trigger {trigger}");

                    foreach (var cmd in cmds)
                    {
                        try
                        {
                            m_MacroRunner.RunMacro(cmd.MacroPath, cmd.EntryPoint, false);
                        }
                        catch(Exception ex)
                        {
                            m_Logger.Log(ex);
                            m_Msg.ShowError(ex, $"Failed to run a macro on trigger: {trigger}");
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            m_DocHandler.HandlerCreated -= OnDocumentHandlerCreated;
            //(m_App as SldWorks).DestroyNotify -= OnSwAppClose;
        }
    }
}
