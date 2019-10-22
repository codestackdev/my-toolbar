//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

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
