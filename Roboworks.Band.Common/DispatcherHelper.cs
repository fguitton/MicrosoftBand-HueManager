using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Roboworks.Band.Common
{
    public static class DispatcherHelper
    {
        public static async Task Invoke(Action action)
        {
            await 
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    new DispatchedHandler(action)
                );
        }
    }
}
