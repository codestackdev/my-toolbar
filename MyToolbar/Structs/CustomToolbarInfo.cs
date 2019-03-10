//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using Xarial.AppLaunchKit.Services.UserSettings.Attributes;

namespace CodeStack.Sw.MyToolbar.Structs
{
    [UserSettingVersion("1.0")]
    public class CustomToolbarInfo
    {
        public CommandGroupInfo[] Groups { get; set; }

        public CustomToolbarInfo()
        {
            Groups = new CommandGroupInfo[]
            {
                new CommandGroupInfo()
                {
                    Id = 0,
                    Title = "CodeStack Toolbar",
                    Description = "Customized commands library toolbar",
                }
            };
        }
    }
}