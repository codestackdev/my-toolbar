using System.Runtime.Serialization;

namespace CodeStack.Community.Sw.MyToolbar.Preferences
{
    [DataContract]
    public class BasicIcons : IIconList
    {
        [DataMember]
        public string Size16x16 { get; set; }

        [DataMember]
        public string Size24x24 { get; set; }
    }
}
