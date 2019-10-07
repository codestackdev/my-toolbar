using CodeStack.Sw.MyToolbar.Enums;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Helpers
{
    public static class MacroScopeHelper
    {
        public static bool IsInScope(this MacroScope_e scope, ISldWorks app)
        {
            if (app.IActiveDoc2 == null && scope.HasFlag(MacroScope_e.Application))
            {
                return true;
            }
            else if (app.IActiveDoc2 is IPartDoc && scope.HasFlag(MacroScope_e.Part))
            {
                return true;
            }
            else if (app.IActiveDoc2 is IAssemblyDoc && scope.HasFlag(MacroScope_e.Assembly))
            {
                return true;
            }
            else if (app.IActiveDoc2 is IDrawingDoc && scope.HasFlag(MacroScope_e.Drawing))
            {
                return true;
            }
            
            return false;
        }
    }
}
