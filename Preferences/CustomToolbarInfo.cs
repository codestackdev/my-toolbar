//**********************
//MyToolbar
//Copyright(C) 2018 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

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
