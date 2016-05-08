using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Windows.ApplicationModel.Resources;

using Roboworks.Hue;

namespace Roboworks.Band.Tiles.PhilipsHue
{
    public static class ResourceManager
    {
        private static ResourceLoader ResourceLoader { get; }

        static ResourceManager()
        {
            var assemblyName = typeof(ResourceManager).GetTypeInfo().Assembly.GetName().Name;
            ResourceManager.ResourceLoader = new ResourceLoader($"{assemblyName}/Resources");
        }

#region HueSetupView

        public static string TileTitle
        {
            get
            {
                return ResourceManager.ResourceLoader.GetString("TileTitle");
            }
        }

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

        public static string HueSetupView_ConnectingText
        {
            get
            {
                return ResourceManager.ResourceLoader.GetString("HueSetupView_ConnectingText");
            }
        }
        
        public static string HueSetupView_DisconnectingText
        {
            get
            {
                return ResourceManager.ResourceLoader.GetString("HueSetupView_DisconnectingText");
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
