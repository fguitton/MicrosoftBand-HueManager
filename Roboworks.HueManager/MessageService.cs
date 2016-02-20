using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Popups;

namespace Roboworks.HueManager
{
    public class MessageService
    {
        public static void DialogShow(string title, string message)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            new MessageDialog(message, title).ShowAsync().Forget();
        }
    }
}
