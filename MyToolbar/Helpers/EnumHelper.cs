using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Helpers
{
    internal static class EnumHelper
    {
        //TODO: move to the framework
        internal static Enum[] GetFlags(Type enumType)
        {
            var flags = new List<Enum>();

            var flag = 0x1;

            foreach (Enum value in Enum.GetValues(enumType))
            {
                var bits = Convert.ToInt32(value);

                if (bits != 0)
                {
                    while (flag < bits)
                    {
                        flag <<= 1;
                    }
                    if (flag == bits)
                    {
                        flags.Add(value);
                    }
                }
            }

            return flags.ToArray();
        }
    }
}
