using CodeStack.Sw.MyToolbar.Preferences;
using CodeStack.Sw.MyToolbar.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace CodeStack.Sw.MyToolbar.UI.ViewModels
{
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
                        var dlg = new OpenFileDialog()
                        {
                            Filter = "SOLIDWORKS Macros (*.swp;*.swb;*.dll)|*.swp;*.swb;*.dll"
                        };

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            MacroPath = dlg.FileName;
                        }
                    });
                }

                return m_BrowseMacroPathCommand;
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
