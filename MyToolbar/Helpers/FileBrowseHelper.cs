using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeStack.Sw.MyToolbar.Helpers
{
    public class FileFilterExtensions
    {
        public string[] Extensions { get; private set; }

        public FileFilterExtensions(params string[] extensions)
        {
            Extensions = extensions;
        }
    }

    public class FileFilter : Dictionary<string, FileFilterExtensions>
    {
    }

    public static class FileBrowseHelper
    {
        public static string BrowseFile(string caption,
            FileFilter filters, string initialFile = "", bool includeAllFiles = true)
        {
            if (includeAllFiles)
            {
                filters.Add("All Files", new FileFilterExtensions("*"));
            }

            var filterStr = string.Join("|",
                filters.Select(f => {
                    var exts = string.Join(";", f.Value.Extensions?.Select(e => $"*.{e}").ToArray());
                    return $"{f.Key} ({exts})|{exts}";
                }).ToArray());

            var dlg = new OpenFileDialog()
            {
                Filter = filterStr,
                Title = caption,
                FileName = initialFile
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.FileName;
            }
            else
            {
                return "";
            }
        }
    }
}
