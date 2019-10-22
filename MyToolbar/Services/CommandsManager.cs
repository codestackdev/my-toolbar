//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Base;
using CodeStack.Sw.MyToolbar.Enums;
using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.SwEx.AddIn;
using CodeStack.SwEx.AddIn.Base;
using CodeStack.SwEx.Common.Diagnostics;
using Newtonsoft.Json.Linq;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Services
{
    public interface ICommandsManager : IDisposable
    {
        CustomToolbarInfo ToolbarInfo { get; }
        void UpdatedToolbarConfiguration(ToolbarSettings toolbarSets, CustomToolbarInfo toolbarConf, bool isEditable);
        void RunMacroCommand(CommandMacroInfo cmd);
    }

    public class CommandsManager : ICommandsManager
    {
        private readonly IToolbarAddIn m_AddIn;
        private readonly ISldWorks m_App;
        private readonly IMacroRunner m_MacroRunner;
        private readonly IMessageService m_Msg;
        private readonly ISettingsProvider m_SettsProvider;
        private readonly IToolbarConfigurationProvider m_ToolbarConfProvider;
        private readonly ILogger m_Logger;

        public CustomToolbarInfo ToolbarInfo { get; }

        public CommandsManager(IToolbarAddIn addIn, ISldWorks app,
            IMacroRunner macroRunner,
            IMessageService msg, ISettingsProvider settsProvider,
            IToolbarConfigurationProvider toolbarConfProvider,
            ILogger logger)
        {
            m_AddIn = addIn;
            m_App = app;
            m_MacroRunner = macroRunner;
            m_Msg = msg;
            m_SettsProvider = settsProvider;
            m_ToolbarConfProvider = toolbarConfProvider;
            m_Logger = logger;

            try
            {
                ToolbarInfo = LoadUserToolbar();
            }
            catch(Exception ex)
            {
                m_Msg.ShowError(ex, "Failed to load toolbar specification");
            }
        }

        public void RunMacroCommand(CommandMacroInfo cmd)
        {
            try
            {
                m_MacroRunner.RunMacro(cmd.MacroPath, cmd.EntryPoint, false);
            }
            catch (Exception ex)
            {
                m_Logger.Log(ex);
                m_Msg.ShowError(ex, "Failed to run macro");
            }
        }

        public void UpdatedToolbarConfiguration(ToolbarSettings toolbarSets, CustomToolbarInfo toolbarConf, bool isEditable)
        {
            bool isToolbarChanged;

            SaveSettingChanges(toolbarSets, toolbarConf, isEditable, out isToolbarChanged);

            if (isToolbarChanged)
            {
                m_Msg.ShowMessage("Toolbar specification has changed. Please restart SOLIDWORKS",
                    MessageType_e.Info);
            }
        }

        private CustomToolbarInfo LoadUserToolbar()
        {
            bool isReadOnly;
            var toolbarInfo = m_ToolbarConfProvider.GetToolbar(out isReadOnly,
                ToolbarSpecificationFile);

            if (toolbarInfo?.Groups != null)
            {
                foreach (var grp in toolbarInfo.Groups
                    .Where(g => g.Commands?.Any(c => c.Triggers.HasFlag(Triggers_e.Button)) == true))
                {
                    var cmdGrp = new CommandGroupInfoSpec(grp, m_App);
                    cmdGrp.MacroCommandClick += OnMacroCommandClick;

                    m_Logger.Log($"Adding command group: {cmdGrp.Title} [{cmdGrp.Id}]. Commands: {string.Join(", ", cmdGrp.Commands.Select(c => $"{c.Title} [{c.UserId}]").ToArray())}");

                    m_AddIn.AddCommandGroup(cmdGrp);
                }
            }

            return toolbarInfo;
        }

        private void OnMacroCommandClick(CommandMacroInfo cmd)
        {
            RunMacroCommand(cmd);
        }

        private void SaveSettingChanges(ToolbarSettings toolbarSets, CustomToolbarInfo toolbarConf,
            bool isEditable, out bool isToolbarChanged)
        {
            isToolbarChanged = false;

            var oldToolbarSetts = m_SettsProvider.GetSettings();

            if (!DeepCompare(toolbarSets, oldToolbarSetts))
            {
                m_SettsProvider.SaveSettings(toolbarSets);
            }
            
            bool isReadOnly;

            var oldToolbarConf = m_ToolbarConfProvider
                .GetToolbar(out isReadOnly, oldToolbarSetts.SpecificationFile);

            isToolbarChanged = !DeepCompare(toolbarConf, oldToolbarConf);

            if (isToolbarChanged)
            {
                if (isEditable)
                {
                    m_ToolbarConfProvider.SaveToolbar(toolbarConf, toolbarSets.SpecificationFile);
                }
                else
                {
                    m_Logger.Log("Skipped saving of read-only toolbar settings");
                }
            }
        }

        private bool DeepCompare(object obj1, object obj2)
        {
            return JToken.DeepEquals(JToken.FromObject(obj1), JToken.FromObject(obj2));
        }
        
        private string ToolbarSpecificationFile
        {
            get
            {
                return m_SettsProvider.GetSettings().SpecificationFile;
            }
        }

        public void Dispose()
        {
        }
    }
}
