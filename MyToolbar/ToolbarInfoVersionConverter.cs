//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Structs;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using Xarial.AppLaunchKit.Services.UserSettings.Data;

namespace CodeStack.Sw.MyToolbar
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

            Add(new Version("1.0"), new Version("2.0"),
                t =>
                {
                    foreach (var group in t["Groups"])
                    {
                        foreach (var cmd in group["Commands"])
                        {
                            cmd["Scope"] = 1 + 2 + 4 + 8; //all
                            cmd["Triggers"] = 1; //button
                        }
                    }
                    
                    return t;
                });
        }
    }
}