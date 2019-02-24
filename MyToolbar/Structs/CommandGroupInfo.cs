using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CodeStack.Sw.MyToolbar.Structs
{
    public class CommandGroupInfo : CommandItemInfo
    {
        public CommandMacroInfo[] Commands { get; set; }
    }
}
