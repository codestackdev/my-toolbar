using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Exceptions
{
    public class ToolbarConfigurationReadOnlyException : UserException
    {
        public ToolbarConfigurationReadOnlyException() 
            : base("Failed to store toolbar configuration as target file is readonly")
        {
        }
    }
}
