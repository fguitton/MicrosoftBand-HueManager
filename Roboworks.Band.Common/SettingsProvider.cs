using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

namespace Roboworks.Band.Common
{
    public interface ISettingsProvider
    {
        string HueApiUserId { get; set; }

        string HueBridgeIpAddress { get; set; }
    }

    public class SettingsProvider : ISettingsProvider
    {
        private const string DataKey_HueApiUserId = "HueApiUserId";
        private const string DataKey_HueBridgeIpAddress = "HueBridgeIpAddress";

        private ApplicationDataContainer _dataContainer;

        public string HueApiUserId
        {
            get
            {
                return (string)this._dataContainer.Values[SettingsProvider.DataKey_HueApiUserId];
            }
            set
            {
                this._dataContainer.Values[SettingsProvider.DataKey_HueApiUserId] = value;
            }
        }

        public string HueBridgeIpAddress
        {
            get
            {
                return (string)this._dataContainer.Values[SettingsProvider.DataKey_HueBridgeIpAddress];
            }
            set
            {
                this._dataContainer.Values[SettingsProvider.DataKey_HueBridgeIpAddress] = value;
            }
        }

        public SettingsProvider()
        {
            this._dataContainer = ApplicationData.Current.LocalSettings;
        }
    }
}
