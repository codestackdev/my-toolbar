using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Exceptions
{
    public class UserException : Exception
    {
        public UserException(string userMessage) : base(userMessage)
        {
        }
    }
}
