using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roboworks.Hue;
using Windows.ApplicationModel.Resources;

namespace Roboworks.HueManager
{
    public static class ResourceManager
    {
        private static ResourceLoader ResourceLoader { get; } = new ResourceLoader();

#region HueSetupView

        public static string HueSetupView_ConnectFailedTitle
        {
            get
            {
                return ResourceManager.ResourceLoader.GetString("HueSetupView_ConnectFailedTitle");
            }
        }

        public static string HueSetupView_ConnectFailedMessage
        {
            get
            {
                return ResourceManager.ResourceLoader.GetString("HueSetupView_ConnectFailedMessage");
            }
        }

#endregion

#region Extensions

        public static string ErrorMessageGet(HueErrorType hueErrorType)
        {
            string resourceKey;

            switch(hueErrorType)
            {
                case HueErrorType.ResourceIsNotAvailable:
                    resourceKey = "HueErrorType_ResourceIsNotAvailable";
                    break;

                case HueErrorType.LinkButtonNotPressed:
                    resourceKey = "HueErrorType_LinkButtonNotPressed";
                    break;

                default:
                    throw new NotSupportedException();
            }
            
            return ResourceManager.ResourceLoader.GetString(resourceKey);
        }

#endregion

    }
}
