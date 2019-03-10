//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Structs;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Linq;

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
            var methods = m_App.GetMacroMethods(macroPath,
                (int)swMacroMethods_e.swMethodsWithoutArguments) as string[];

            if (methods != null)
            {
                return methods.Select(m =>
                {
                    var ep = m.Split('.');
                    return new MacroEntryPoint()
                    {
                        ModuleName = ep[0],
                        SubName = ep[1]
                    };
                }).ToArray();
            }

            return null;
        }
    }
}