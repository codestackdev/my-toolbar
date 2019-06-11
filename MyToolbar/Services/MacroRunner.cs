//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Exceptions;
using CodeStack.Sw.MyToolbar.Structs;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

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