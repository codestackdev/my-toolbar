﻿//**********************
//MyToolbar
//Copyright(C) 2018 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using System.Runtime.Serialization;

namespace CodeStack.Community.Sw.MyToolbar.Preferences
{
    [DataContract]
    public class HighResIcons : IIconList
    {
        [DataMember]
        public string Size20x20 { get; set; }

        [DataMember]
        public string Size32x32 { get; set; }

        [DataMember]
        public string Size40x40 { get; set; }

        [DataMember]
        public string Size64x64 { get; set; }

        [DataMember]
        public string Size96x96 { get; set; }

        [DataMember]
        public string Size128x128 { get; set; }
    }
}
