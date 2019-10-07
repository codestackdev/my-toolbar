//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.UI.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CodeStack.Sw.MyToolbar.UI.Controls
{
    public class EnumValueToHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumVal = value as Enum;
            return enumVal.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EnumComboBoxItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Item { get; set; }
        public DataTemplate Header { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var elem = container as FrameworkElement;

            if (elem?.TemplatedParent is EnumComboBox)
            {
                return Header;
            }
            else
            {
                return Item;
            }
        }
    }

    public class EnumComboBox : ComboBox
    {
        public enum EnumItemType_e
        {
            Default,
            Combined,
            None
        }

        public class EnumComboBoxItem : NotifyPropertyChanged
        {
            private readonly EnumComboBox m_Parent;
            private readonly Enum m_Value;
            private readonly Enum[] m_AffectedFlags;

            internal EnumComboBoxItem(EnumComboBox parent, Enum value, Enum[] affectedFlags)
            {
                m_Parent = parent;
                m_Parent.ValueChanged += OnValueChanged;
                m_Value = value;
                m_AffectedFlags = affectedFlags;

                if (m_AffectedFlags.Length > 1)
                {
                    Type = EnumItemType_e.Combined;
                }
                else if (m_AffectedFlags.Length == 0)
                {
                    Type = EnumItemType_e.None;
                }
                else
                {
                    Type = EnumItemType_e.Default;
                }
            }

            public EnumItemType_e Type { get; private set; }

            public bool IsSelected
            {
                get
                {
                    if (Type == EnumItemType_e.None)
                    {
                        return IsNone(m_Parent.Value);
                    }
                    else
                    {
                        return m_Parent.Value.HasFlag(m_Value);
                    }
                }
                set
                {
                    if (Type == EnumItemType_e.None)
                    {
                        m_Parent.Value = (Enum)Enum.ToObject(m_Value.GetType(), 0);
                    }
                    else
                    {
                        int val = Convert.ToInt32(m_Parent.Value);

                        if (value)
                        {
                            foreach (var flag in m_AffectedFlags)
                            {
                                if (!m_Parent.Value.HasFlag(flag))
                                {
                                    val += Convert.ToInt32(flag);
                                }
                            }
                        }
                        else
                        {
                            foreach (var flag in m_AffectedFlags)
                            {
                                if (m_Parent.Value.HasFlag(flag))
                                {
                                    val -= Convert.ToInt32(flag);
                                }
                            }
                        }

                        m_Parent.Value = (Enum)Enum.ToObject(m_Value.GetType(), val);
                    }

                    NotifyChanged();
                }
            }

            private void OnValueChanged(Enum value)
            {
                NotifyChanged(nameof(IsSelected));
            }

            private bool IsNone(Enum val)
            {
                return Convert.ToInt32(val) == 0;
            }

            public override string ToString()
            {
                return m_Value.ToString();
            }
        }

        internal event Action<Enum> ValueChanged;

        private Type m_CurBoundType;
        private Enum[] m_CurFlags;

        static EnumComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EnumComboBox), 
                new FrameworkPropertyMetadata(typeof(EnumComboBox)));
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(Enum),
            typeof(EnumComboBox), new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public Enum Value
        {
            get { return (Enum)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cmb = d as EnumComboBox;

            var val = e.NewValue as Enum;

            if (val != null)
            {
                var enumType = val.GetType();

                if (enumType != cmb.m_CurBoundType)
                {
                    cmb.m_CurFlags = GetFlags(enumType);

                    cmb.Items.Clear();

                    cmb.m_CurBoundType = enumType;

                    var items = Enum.GetValues(enumType);

                    foreach (Enum item in items)
                    {
                        cmb.Items.Add(new EnumComboBoxItem(cmb, item,
                            cmb.m_CurFlags.Where(f => item.HasFlag(f)).ToArray()));
                    }

                    UpdateHeader(cmb);
                }
            }

            cmb.ValueChanged?.Invoke(val);
        }

        private static Enum[] GetFlags(Type enumType)
        {
            var flags = new List<Enum>();

            var flag = 0x1;

            foreach (Enum value in Enum.GetValues(enumType))
            {
                var bits = Convert.ToInt32(value);

                if (bits != 0)
                {
                    while (flag < bits)
                    {
                        flag <<= 1;
                    }
                    if (flag == bits)
                    {
                        flags.Add(value);
                    }
                }
            }

            return flags.ToArray();
        }

        private static void UpdateHeader(EnumComboBox cmb)
        {
            if (cmb.Items.Count > 0)
            {
                cmb.SelectedIndex = 0;
            }
        }
    }
}
