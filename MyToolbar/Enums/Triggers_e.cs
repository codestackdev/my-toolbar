using CodeStack.SwEx.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Enums
{
    [Flags]
    public enum Triggers_e
    {
        [Summary("Disabled command")]
        None = 0,

        [Summary("Invoked by clicking button in the toolbar")]
        Button = 1 << 0,

        ApplicationStart = 1 << 1,
        ApplicationClose = 1 << 2,
        DocumentNew = 1 << 3,
        DocumentOpen = 1 << 4,
        DocumentSave = 1 << 5,
        DocumentClose = 1 << 6,
        NewSelection = 1 << 7,
        ConfigurationChange = 1 << 8,
        Rebuild = 1 << 9,
    }
}
