using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return $"{ModuleName}.{SubName}";
        }
    }
}
