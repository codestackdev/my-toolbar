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
using CodeStack.SwEx.Common.Diagnostics;
using SolidWorks.Interop.sldworks;
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
                    var cmds = allCmds.Where(c => c.Scope.HasFlag(trigger));

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
                            m_MacroRunner.RunMacro(cmd.MacroPath, cmd.EntryPoint);
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
            InvokeTrigger(Triggers_e.ApplicationClose);

            m_DocHandler.HandlerCreated -= OnDocumentHandlerCreated;
        }
    }
}
