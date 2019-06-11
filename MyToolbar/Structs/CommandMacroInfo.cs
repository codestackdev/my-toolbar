﻿//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

namespace CodeStack.Sw.MyToolbar.Structs
{
    public class CommandMacroInfo : CommandItemInfo
    {
        public string MacroPath { get; set; }
        public MacroEntryPoint EntryPoint { get; set; }
    }
}