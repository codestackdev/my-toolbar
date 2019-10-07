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
        DocumentOpen = 1 << 3,
        DocumentSave = 1 << 4,
        DocumentClose = 1 << 5,
        NewSelection = 1 << 6,
        ConfigurationChange = 1 << 7,
        Rebuild = 1 << 8,
    }
}
