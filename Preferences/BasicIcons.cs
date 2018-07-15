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
    public class BasicIcons : IIconList
    {
        [DataMember]
        public string Size16x16 { get; set; }

        [DataMember]
        public string Size24x24 { get; set; }
    }
}
