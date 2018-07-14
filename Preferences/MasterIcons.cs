using System;
using System.Runtime.Serialization;

namespace CodeStack.Community.Sw.MyToolbar.Preferences
{
    [DataContract]
    public class MasterIcons : IIconList
    {
        [DataMember]
        public string IconPath { get; set; }
    }
}
