using CodeStack.Community.Sw.MyToolbar.Preferences;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xarial.AppLaunchKit.Services.UserSettings.Data;

namespace CodeStack.Community.Sw.MyToolbar
{
    public class ToolbarInfoVersionConverter : BaseUserSettingsVersionsTransformer<CustomToolbarInfo>
    {
        public ToolbarInfoVersionConverter()
        {
            Add(new Version(), new Version("1.0"),
                t =>
                {
                    var oldFile = Path.Combine(Locations.AppDirectoryPath, "data.json");

                    if (File.Exists(oldFile))
                    {
                        var token = JToken.Parse(File.ReadAllText(oldFile));

                        foreach (var grp in token["Groups"].Children())
                        {
                            var prop = grp.Children<JProperty>().FirstOrDefault(p => p.Name == "Icons");
                            if (prop != null)
                            {
                                var iconPath = prop.Children().FirstOrDefault()?["IconPath"]?.ToString();
                                prop.Replace(new JProperty("IconPath", iconPath));
                            }
                        }
                        
                        return token;
                    }

                    return null;
                });
        }
    }
}
