//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Services;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CodeStack.Sw.MyToolbar.UI.Converters
{
    public class MacroPathToEntryPointsConverter : IValueConverter
    {
        private readonly IMacroEntryPointsExtractor m_Extractor;

        public MacroPathToEntryPointsConverter()
            : this(ServicesContainer.Instance.GetService<IMacroEntryPointsExtractor>())
        {
        }

        public MacroPathToEntryPointsConverter(IMacroEntryPointsExtractor extractor)
        {
            m_Extractor = extractor;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return m_Extractor.GetEntryPoints(value as string);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}