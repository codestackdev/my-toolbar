//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Helpers;
using CodeStack.Sw.MyToolbar.Structs;
using CodeStack.Sw.MyToolbar.UI.Base;
using System;
using System.Windows.Input;

namespace CodeStack.Sw.MyToolbar.UI.ViewModels
{
    [Flags]
    public enum MacroScope_e
    {
        None = 0,
        Application = 1,
        Part = 2,
        Assembly = 4,
        Drawing = 8,
        AllDocuments = Part | Assembly | Drawing,
        All = Application | AllDocuments
    }

    public class CommandMacroVM : CommandVM<CommandMacroInfo>
    {
        private ICommand m_BrowseMacroPathCommand;

        public string MacroPath
        {
            get
            {
                return Command.MacroPath;
            }
            set
            {
                Command.MacroPath = value;
                NotifyChanged();
            }
        }

        public MacroEntryPoint EntryPoint
        {
            get
            {
                return Command.EntryPoint;
            }
            set
            {
                Command.EntryPoint = value;
                NotifyChanged();
            }
        }

        public ICommand BrowseMacroPathCommand
        {
            get
            {
                if (m_BrowseMacroPathCommand == null)
                {
                    m_BrowseMacroPathCommand = new RelayCommand(() =>
                    {
                        var macroFile = FileBrowseHelper.BrowseFile("Select macro file",
                            new FileFilter()
                            {
                                { "SOLIDWORKS Macros", new FileFilterExtensions("swp", "swb", "dll") }
                            }, MacroPath);

                        if (!string.IsNullOrEmpty(macroFile))
                        {
                            MacroPath = macroFile;
                        }
                    });
                }

                return m_BrowseMacroPathCommand;
            }
        }

        private MacroScope_e m_Scope = MacroScope_e.Application | MacroScope_e.Part | MacroScope_e.Assembly | MacroScope_e.Drawing;

        public MacroScope_e Scope
        {
            get
            {
                return m_Scope;
            }
            set
            {
                m_Scope = value;
                NotifyChanged();
            }
        }

        public CommandMacroVM() : this(new CommandMacroInfo())
        {
        }

        public CommandMacroVM(CommandMacroInfo cmd) : base(cmd)
        {
        }
    }
}