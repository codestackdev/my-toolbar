//**********************
//MyToolbar - Custom toolbar manager
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestack-net-dev/my-toolbar/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/my-toolbar/
//**********************

using CodeStack.Sw.MyToolbar.Exceptions;
using CodeStack.Sw.MyToolbar.Services;
using CodeStack.SwEx.Common.Diagnostics;
using System;

namespace CodeStack.Sw.MyToolbar.Helpers
{
    internal static class ExceptionHelper
    {
        private static readonly IMessageService s_MsgService
            = ServicesContainer.Instance.GetService<IMessageService>();

        private static readonly ILogger s_Logger
            = ServicesContainer.Instance.GetService<ILogger>();

        internal static void ExecuteUserCommand(Action cmd, Func<Exception, string> unknownErrorDescriptionHandler)
        {
            try
            {
                cmd.Invoke();
            }
            catch (UserException ex)
            {
                s_Logger.Log(ex);
                s_MsgService.ShowMessage(ex.Message, MessageType_e.Error);
            }
            catch (Exception ex)
            {
                var errDesc = unknownErrorDescriptionHandler.Invoke(ex);
                s_Logger.Log(ex);
                s_MsgService.ShowMessage(errDesc, MessageType_e.Error);
            }
        }
    }
}