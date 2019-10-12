//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Exceptions;
using CodeStack.Sw.MyToolbar.Properties;
using System;
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
        void ShowError(Exception ex, string baseMsg);
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

        public void ShowError(Exception ex, string baseMsg)
        {
            if(ex is UserException)
            {
                ShowMessage(ex.Message, MessageType_e.Error);
            }
            else
            {
                ShowMessage(baseMsg, MessageType_e.Error);
            }
        }
    }
}