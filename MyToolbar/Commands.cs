using CodeStack.SwEx.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar
{
    [SwEx.AddIn.Attributes.CommandGroupInfo(999)]
    [Title("MyToolbar")]
    [Description("Custom toolbar")]
    public enum Commands_e
    {
        Configuration,
        About
    }
}
