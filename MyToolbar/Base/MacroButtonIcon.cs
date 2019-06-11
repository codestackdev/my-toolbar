using CodeStack.SwEx.AddIn.Icons;
using CodeStack.SwEx.Common.Icons;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStack.Sw.MyToolbar.Base
{
    public class MacroButtonIcon : CommandGroupIcon
    {
        private readonly Image m_Icon;

        //TODO: once MasterIcon constructor in SwEx.AddIn is made to protected internal call it from here and remove all overloads
        internal MacroButtonIcon(Image icon)
        {
            m_Icon = icon;
        }

        public override IEnumerable<IconSizeInfo> GetHighResolutionIconSizes()
        {
            yield return new IconSizeInfo(m_Icon, new Size(20, 20));
            yield return new IconSizeInfo(m_Icon, new Size(32, 32));
            yield return new IconSizeInfo(m_Icon, new Size(40, 40));
            yield return new IconSizeInfo(m_Icon, new Size(64, 64));
            yield return new IconSizeInfo(m_Icon, new Size(96, 96));
            yield return new IconSizeInfo(m_Icon, new Size(128, 128));
        }

        public override IEnumerable<IconSizeInfo> GetIconSizes()
        {
            yield return new IconSizeInfo(m_Icon, new Size(16, 16));
            yield return new IconSizeInfo(m_Icon, new Size(24, 24));
        }
    }
}
