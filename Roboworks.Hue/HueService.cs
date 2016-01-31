using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Newtonsoft.Json.Linq;

using Roboworks.Hue.Entities;

namespace Roboworks.Hue
{
    public interface IHueService
    {
        HueBridgeInfo HueBridgeInfo { get; }

        Task<HueLightBulb[]> LightBulbsGet();

        Task<bool> LightBulbIsOnSet(string id, bool value);

        Task<bool> LightBulbBrightnessSet(string id, double value);
    }
    
    internal class HueService : IHueService
    {
        private const string HueBridgePageHeader = "hue personal wireless lighting";

        private readonly IHttpClient _httpClient;
        private readonly string _hueApiUserId;

#region Properties

        public HueBridgeInfo HueBridgeInfo { get; }

#endregion

        public HueService(HueBridgeInfo hueBridgeInfo, string hueApiUserId, IHttpClient httpClient)
        {
            if (hueBridgeInfo == null)
            {
                throw new ArgumentNullException(nameof(hueBridgeInfo));
            }

            if (hueApiUserId == null)
            {
                throw new ArgumentNullException(nameof(hueApiUserId));
            }

            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            this.HueBridgeInfo = hueBridgeInfo;
            this._hueApiUserId = hueApiUserId;
            this._httpClient = httpClient;
        }

#region Public Methods
        
        public async Task<HueLightBulb[]> LightBulbsGet()
        {
            var uri = this.HueApiWithUserUriGet() + "/lights";
            var json = await this._httpClient.HttpClientGet(uri);

            var data = JObject.Parse(json);

            var lightBulbs = 
                data.Children()
                    .Cast<JProperty>()
                    .Select(item => HueLightBulb.FromData(item.Name, item.Value.ToString()))
                    .ToArray();

            return lightBulbs;
        }

        public async Task<bool> LightBulbIsOnSet(string id, bool value)
        {
            var uri = this.HueApiWithUserUriGet() + $"/lights/{id}/state";
            var data = new JObject(new JProperty("on", value));

            var json = await this._httpClient.HttpClientPut(uri, data.ToString());

            return
                JArray.Parse(json)
                    .SelectMany(item => item.Children())
                    .Cast<JProperty>()
                    .Any(item => item.Name == "success");
        }

        public async Task<bool> LightBulbBrightnessSet(string id, double value)
        {
            if (value < 0d || value > 1d)
            {
                throw 
                    new ArgumentOutOfRangeException(
                        $"Passed \"{nameof(value)}\" argument value should be between 0 and 1 inclusive."
                    );
            }

            var uri = this.HueApiWithUserUriGet() + $"/lights/{id}/state";
            var data = new JObject(new JProperty("bri", (int)(253 * value) + 1)); // value range is 1 - 254
            
            var json = await this._httpClient.HttpClientPut(uri, data.ToString());

            return
                JArray.Parse(json)
                    .SelectMany(item => item.Children())
                    .Cast<JProperty>()
                    .Any(item => item.Name == "success");
        }

#endregion

#region Private Methods

        private string HueApiUriGet()
        {
            return HueHelper.HueApiUriGet(this.HueBridgeInfo.IpAddress);
        }

        private string HueApiWithUserUriGet()
        {
            return HueHelper.HueApiWithUserUriGet(this.HueBridgeInfo.IpAddress, this._hueApiUserId);
        }

        //private async Task<bool> BridgePing(string ipAddress)
        //{
        //    var requestUri = this.HueApiUriGet(ipAddress);
        //    var data = await this.HttpClientGet(requestUri);

        //    var element = XElement.Parse(data);

        //    var pageTitle = element.Element("html")?.Element("head")?.Element("title")?.Value;
            
        //    return pageTitle == HueService.HueBridgePageHeader;
        //}

#endregion

    }
}    