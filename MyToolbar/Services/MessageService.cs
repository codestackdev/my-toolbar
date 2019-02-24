using CodeStack.Sw.MyToolbar.Properties;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CodeStack.Sw.MyToolbar.Services
{
    public enum MessageType_e
    {
        Info,
        Warning,
        Error
    }

    public interface IMessageService
    {
        void ShowMessage(string message, MessageType_e type);
    }

    public class MessageService : IMessageService
    {
        public void ShowMessage(string message, MessageType_e type)
        {
            var icon = MessageBoxImage.Information;

            switch (type)
            {
                case MessageType_e.Info:
                    icon = MessageBoxImage.Information;
                    break;

                case MessageType_e.Warning:
                    icon = MessageBoxImage.Warning;
                    break;

                case MessageType_e.Error:
                    icon = MessageBoxImage.Error;
                    break;
            }

            MessageBox.Show(message, Resources.AppTitle, MessageBoxButton.OK, icon);
        }
    }
}
