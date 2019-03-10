using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Exceptions
{
    public class MacroRunFailedException : UserException
    {
        private static string GetError(swRunMacroError_e err)
        {
            const string MACRO_PREFIX = "swRunMacroError_";

            string errorDesc = err.ToString();

            if (errorDesc.StartsWith(MACRO_PREFIX))
            {
                errorDesc = errorDesc.Substring(MACRO_PREFIX.Length);
            }

            return $"Failed to run macro: {errorDesc}";
        }

        public MacroRunFailedException(swRunMacroError_e err)
            : base(GetError(err))
        {
        }
    }
}
