using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Roboworks.Band.Common;

namespace Roboworks.HueManager
{
    public static class ExceptionHandler
    {
        public static void Handle(Exception exception, bool isHandled = false)
        {
            if (!isHandled)
            {
                System.Diagnostics.Debugger.Break();
            }

            MessageService.DialogShow(exception.Message, exception.GetType().Name);
        }
    }
}
