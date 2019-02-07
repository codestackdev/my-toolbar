using CodeStack.Sw.MyToolbar.Preferences;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Services
{
    public interface IMacroEntryPointsExtractor
    {
        MacroEntryPoint[] GetEntryPoints(string macroPath);
    }

    public class MacroEntryPointsExtractor : IMacroEntryPointsExtractor
    {
        private readonly ISldWorks m_App;

        public MacroEntryPointsExtractor(ISldWorks app)
        {
            m_App = app;
        }

        public MacroEntryPoint[] GetEntryPoints(string macroPath)
        {
            //TODO: Implement

            m_App.GetMacroMethods(macroPath, -1);

            return null;
        }
    }
}
