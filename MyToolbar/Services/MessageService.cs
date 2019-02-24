using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly ISldWorks m_App;
        private readonly string m_Caption;

        public MessageService(ISldWorks app, string caption)
        {
            m_App = app;
            m_Caption = caption;
        }

        public void ShowMessage(string message, MessageType_e type)
        {
            var icon = swMessageBoxIcon_e.swMbInformation; ;

            switch (type)
            {
                case MessageType_e.Info:
                    icon = swMessageBoxIcon_e.swMbInformation;
                    break;

                case MessageType_e.Warning:
                    icon = swMessageBoxIcon_e.swMbWarning;
                    break;

                case MessageType_e.Error:
                    icon = swMessageBoxIcon_e.swMbStop;
                    break;
            }

            m_App.SendMsgToUser2(message, (int)icon, (int)swMessageBoxBtn_e.swMbOk);
        }
    }
}
