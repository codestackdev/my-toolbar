using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Structs
{
    public class ToolbarLocalSettings
    {
        public Dictionary<Guid, int> CommandUserIds { get; private set; }

        public ToolbarLocalSettings()
        {
            CommandUserIds = new Dictionary<Guid, int>();
        }
    }
}
