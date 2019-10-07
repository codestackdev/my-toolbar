using CodeStack.SwEx.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Enums
{
    [Flags]
    public enum MacroScope_e
    {
        [Summary("No Open Documents")]
        Application = 1,

        Part = 2,
        Assembly = 4,
        Drawing = 8,

        [Title("All Documents")]
        [Summary("All Documents (Part, Assembly, Drawing)")]
        AllDocuments = Part | Assembly | Drawing,

        All = Application | AllDocuments
    }
}
