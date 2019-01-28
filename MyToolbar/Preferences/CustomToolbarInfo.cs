using System.Runtime.Serialization;
using Xarial.AppLaunchKit.Services.UserSettings.Attributes;

namespace CodeStack.Sw.MyToolbar.Preferences
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
