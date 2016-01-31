using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Roboworks.Hue.Entities;

namespace Roboworks.Hue
{
    public interface IHueServiceProvider
    {
        Task<HueApiUser> HueApiUserCreate(string ipAddress, string appName);

        Task HueApiUserDelete(string ipAddress, string hueApiUserId);

        Task<IHueService> Connect(string ipAddress, string hueApiUserId);
    }

    public class HueServiceProvider : IHueServiceProvider
    {
        private readonly IHttpClient _httpClient;

        public HueServiceProvider() : this(new HttpClient())
        {
        }

        internal HueServiceProvider(IHttpClient httpClient)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            this._httpClient = httpClient;
        }

        public async Task<HueApiUser> HueApiUserCreate(string ipAddress, string appName)
        {
            if (ipAddress == null)
            {
                throw new ArgumentNullException(nameof(ipAddress));
            }

            if (appName == null)
            {
                throw new ArgumentNullException(nameof(appName));
            }

            if (appName.Length == 0 || appName.Length >= 20)
            {
                throw new ArgumentException($"\"{nameof(appName)}\" argument length should be (0..20).");
            }

            var deviceType = this.HueDeviceTypeGenerate(appName);
            var requestContent = new JObject(new JProperty("devicetype", deviceType)).ToString();
            var requestUri = HueHelper.HueApiUriGet(ipAddress);

            var data = await this._httpClient.HttpClientPost(requestUri, requestContent);

            HueHelper.HueApiResponseErrorCheck(data);
            
            return HueApiUser.FromData(data);
        }

        public async Task HueApiUserDelete(string ipAddress, string hueApiUserId)
        {
            var hueApiWithUserUri = HueHelper.HueApiWithUserUriGet(ipAddress, hueApiUserId);
            var resourceLocation = $"/config/whitelist/{hueApiUserId}";
            var requestUri = hueApiWithUserUri + resourceLocation;

            var data = await this._httpClient.HttpClientDelete(requestUri);
            
            HueHelper.HueApiResponseErrorCheck(data);
            HueHelper.HueApiResponseSuccessDeleteCheck(data, resourceLocation);
        }

        public async Task<IHueService> Connect(string ipAddress, string hueApiUserId)
        {
            var requestUri = HueHelper.HueApiWithUserUriGet(ipAddress, hueApiUserId) + "/config";
            var data = await this._httpClient.HttpClientGet(requestUri);

            var hueBridgeInfo = HueBridgeInfo.FromData(data);

            return new HueService(hueBridgeInfo, hueApiUserId, this._httpClient);
        }

#region Private Methods

        private string HueDeviceTypeGenerate(string appName)
        {
            var appUserid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12);
            return string.Format("{0}#{1}", appName, appUserid);
        }

#endregion

    }
}
