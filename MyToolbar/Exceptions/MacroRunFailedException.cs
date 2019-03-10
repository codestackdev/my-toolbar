//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using SolidWorks.Interop.swconst;

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