//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using System;

namespace CodeStack.Sw.MyToolbar.Structs
{
    public class MacroEntryPoint
    {
        public string ModuleName { get; set; }
        public string SubName { get; set; }

        public static bool operator ==(MacroEntryPoint x, MacroEntryPoint y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null))
            {
                return false;
            }
            if (ReferenceEquals(y, null))
            {
                return false;
            }

            return string.Equals(x.ModuleName, y.ModuleName, StringComparison.CurrentCultureIgnoreCase)
                && string.Equals(x.SubName, y.SubName, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool operator !=(MacroEntryPoint x, MacroEntryPoint y)
        {
            return !(x == y);
        }

        public override bool Equals(object obj)
        {
            if (obj is MacroEntryPoint)
            {
                return this == obj as MacroEntryPoint;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(ModuleName))
            {
                return $"{ModuleName}.{SubName}";
            }
            else
            {
                return SubName;
            }
        }
    }
}