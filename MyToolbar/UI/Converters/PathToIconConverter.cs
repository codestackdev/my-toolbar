//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CodeStack.Sw.MyToolbar.UI.Converters
{
    public class PathToIconConverter : IValueConverter
    {
        private static readonly BitmapImage m_DefaultIcon;

        static PathToIconConverter()
        {
            m_DefaultIcon = ImageToBitmapImage(Resources.macro_icon_default);
        }

        private static BitmapImage ImageToBitmapImage(System.Drawing.Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iconPath = value as string;

            BitmapImage icon = null;

            if (!string.IsNullOrEmpty(iconPath) && File.Exists(iconPath))
            {
                try
                {
                    icon = new BitmapImage(new Uri(iconPath));
                }
                catch
                {
                }
            }

            if (icon == null)
            {
                icon = m_DefaultIcon;
            }

            return icon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}