using CodeStack.Sw.MyToolbar.Exceptions;
using CodeStack.Sw.MyToolbar.Structs;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Services
{
    public interface IMacroRunner
    {
        void RunMacro(string macroPath, MacroEntryPoint entryPoint);
    }

    public class MacroRunner : IMacroRunner
    {
        private readonly ISldWorks m_App;

        public MacroRunner(ISldWorks app)
        {
            m_App = app;
        }

        public void RunMacro(string macroPath, MacroEntryPoint entryPoint)
        {
            int err;

            if (!m_App.RunMacro2(macroPath, entryPoint.ModuleName, entryPoint.SubName, 0, out err))
            {
                throw new MacroRunFailedException((swRunMacroError_e)err);
            }
        }
    }
}
