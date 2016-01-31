using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Popups;

namespace Roboworks.HueManager
{
    public static class ExceptionHandler
    {
        public static void Handle(Exception exception)
        {
            System.Diagnostics.Debugger.Break();

            new MessageDialog(exception.Message, exception.GetType().Name).ShowAsync().Forget();
        }
    }
}
