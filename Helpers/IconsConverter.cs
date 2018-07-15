//**********************
//MyToolbar
//Copyright(C) 2018 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Community.Sw.MyToolbar.Preferences;
using CodeStack.Community.Sw.MyToolbar.Properties;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace CodeStack.Community.Sw.MyToolbar.Helpers
{
    internal static class IconsConverter
    {
        private class IconData
        {
            internal string SourceIconPath { get; set; }
            internal Size TargetSize { get; set; }
            internal string TargetIconPath { get; private set; }

            internal IconData(string sourceIconPath, Size targetSize)
            {
                var destDir = Path.Combine(Locations.AppDirectoryPath, 
                    Settings.Default.IconsCacheFolder);

                SourceIconPath = sourceIconPath;
                TargetSize = targetSize;
                TargetIconPath = Path.Combine(destDir, $"icon_{targetSize.Width}x{targetSize.Height}.bmp");
            }
        }
        
        internal static string[] ConvertIcons(IIconList icons, bool highRes)
        {
            if (icons == null)
            {
                throw new ArgumentException("Icons are not specified");
            }

            IconData[] iconsData;

            if (icons is HighResIcons)
            {
                var highResIcons = icons as HighResIcons;

                if (highRes)
                {
                    iconsData = new IconData[]
                    {
                        new IconData(highResIcons.Size20x20, new Size(20,20)),
                        new IconData(highResIcons.Size32x32, new Size(32,32)),
                        new IconData(highResIcons.Size40x40, new Size(40,40)),
                        new IconData(highResIcons.Size64x64, new Size(64,64)),
                        new IconData(highResIcons.Size96x96, new Size(96,96)),
                        new IconData(highResIcons.Size128x128, new Size(128,128))
                    };
                }
                else
                {
                    iconsData = new IconData[]
                    {
                        new IconData(highResIcons.Size20x20, new Size(16,16)),
                        new IconData(highResIcons.Size32x32, new Size(24,24)),
                    };
                }
            }
            else if (icons is BasicIcons)
            {
                var basicIcons = icons as BasicIcons;

                if (highRes)
                {
                    iconsData = new IconData[]
                    {
                        new IconData(basicIcons.Size16x16, new Size(20,20)),
                        new IconData(basicIcons.Size24x24, new Size(32,32)),
                        new IconData(basicIcons.Size24x24, new Size(40,40)),
                        new IconData(basicIcons.Size24x24, new Size(64,64)),
                        new IconData(basicIcons.Size24x24, new Size(96,96)),
                        new IconData(basicIcons.Size24x24, new Size(128,128))
                    };
                }
                else
                {
                    iconsData = new IconData[]
                    {
                        new IconData(basicIcons.Size16x16, new Size(16,16)),
                        new IconData(basicIcons.Size24x24, new Size(24,24)),
                    };
                }
            }
            else if (icons is MasterIcons)
            {
                var masterIcons = icons as MasterIcons;

                if (highRes)
                {
                    iconsData = new IconData[]
                    {
                        new IconData(masterIcons.IconPath, new Size(20,20)),
                        new IconData(masterIcons.IconPath, new Size(32,32)),
                        new IconData(masterIcons.IconPath, new Size(40,40)),
                        new IconData(masterIcons.IconPath, new Size(64,64)),
                        new IconData(masterIcons.IconPath, new Size(96,96)),
                        new IconData(masterIcons.IconPath, new Size(128,128))
                    };
                }
                else
                {
                    iconsData = new IconData[]
                    {
                        new IconData(masterIcons.IconPath, new Size(16,16)),
                        new IconData(masterIcons.IconPath, new Size(24,24)),
                    };
                }
            }
            else
            {
                throw new NotSupportedException($"Specified icons '{icons.GetType().FullName}' are not supported");
            }

            foreach (var iconData in iconsData)
            {
                CreateBitmap(iconData.SourceIconPath,
                    iconData.TargetIconPath,
                    iconData.TargetSize, Color.FromArgb(192, 192, 192));
            }

            return iconsData.Select(i => i.TargetIconPath).ToArray();
        }

        private static void CreateBitmap(string sourceIcon,
            string targetIcon, Size size, Color background)
        {
            if (File.Exists(sourceIcon))
            {
                using (var srcImg = Image.FromFile(sourceIcon))
                {
                    using (var bmp = new Bitmap(size.Width,
                        size.Height, PixelFormat.Format24bppRgb))
                    {
                        bmp.SetResolution(
                            srcImg.HorizontalResolution,
                            srcImg.VerticalResolution);

                        var widthScale = (double)size.Width / (double)srcImg.Width;
                        var heightScale = (double)size.Height / (double)srcImg.Height;
                        var scale = Math.Min(widthScale, heightScale);

                        int destX = 0;
                        int destY = 0;

                        if (heightScale < widthScale)
                        {
                            destX = (int)(size.Width - srcImg.Width * scale) / 2;
                        }
                        else
                        {
                            destY = (int)(size.Height - srcImg.Height * scale) / 2;
                        }

                        int destWidth = (int)(srcImg.Width * scale);
                        int destHeight = (int)(srcImg.Height * scale);

                        using (var graph = Graphics.FromImage(bmp))
                        {
                            graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graph.SmoothingMode = SmoothingMode.HighQuality;
                            graph.PixelOffsetMode = PixelOffsetMode.HighQuality;

                            using (var brush = new SolidBrush(background))
                            {
                                graph.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);
                            }

                            graph.DrawImage(srcImg,
                                new Rectangle(destX, destY, destWidth, destHeight),
                                new Rectangle(0, 0, srcImg.Width, srcImg.Height),
                                GraphicsUnit.Pixel);
                        }

                        var dir = Path.GetDirectoryName(targetIcon);

                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        bmp.Save(targetIcon);
                    }
                }
            }
            else
            {
                throw new FileNotFoundException($"Specified icon '{sourceIcon}' doesn't exist");
            }
        }
    }
}
