using System.Runtime.Serialization;

namespace CodeStack.Community.Sw.MyToolbar.Preferences
{
    [DataContract]
    [KnownType(typeof(BasicIcons))]
    [KnownType(typeof(HighResIcons))]
    [KnownType(typeof(MasterIcons))]
    public class CustomToolbarInfo
    {
        [DataMember]
        public CommandGroupInfo[] Groups { get; set; }
    }
}
