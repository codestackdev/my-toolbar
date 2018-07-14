using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CodeStack.Community.Sw.MyToolbar.Preferences
{
    [DataContract]
    public class CommandGroupInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public IIconList Icons { get; set; }
    }
}
