//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

namespace CodeStack.Sw.MyToolbar.Structs
{
    public abstract class CommandItemInfo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string IconPath { get; set; }
    }
}