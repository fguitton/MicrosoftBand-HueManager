using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Roboworks.Hue
{
    public interface IHueService
    {
        Task<HueLightBulb[]> LightBulbsGet();

        Task<bool> LightBulbIsOnSet(string id, bool value);

        Task<bool> LightBulbBrightnessSet(string id, double value);
    }

    // http://192.168.1.100/api/188aea1620420fc73c820a7a3b07cdb/lights

    public class HueService : IHueService
    {
        private const string HueIpAddress = "192.168.1.100";
        private const string HueApiUser = "188aea1620420fc73c820a7a3b07cdb";

        private static readonly string HueHttpApi = 
            $"http://{HueService.HueIpAddress}/api/{HueService.HueApiUser}";

        public async Task<HueLightBulb[]> LightBulbsGet()
        {
            var uri = HueService.HueHttpApi + "/lights";
            var json = await this.HttpClientGet(uri);

            var data = JObject.Parse(json);

            var lightBulbs = 
                data.Children()
                    .Cast<JProperty>()
                    .Select(item => HueLightBulb.FromData(item.Name, item.Value))
                    .ToArray();

            return lightBulbs;
        }

        public async Task<bool> LightBulbIsOnSet(string id, bool value)
        {
            var uri = HueService.HueHttpApi + $"/lights/{id}/state";
            var data = new JObject(new JProperty("on", value));

            var json = await this.HttpClientPut(uri, data.ToString());

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

            var uri = HueService.HueHttpApi + $"/lights/{id}/state";
            var data = new JObject(new JProperty("bri", (int)(253 * value) + 1)); // value range is 1 - 254
            
            var json = await this.HttpClientPut(uri, data.ToString());

            return
                JArray.Parse(json)
                    .SelectMany(item => item.Children())
                    .Cast<JProperty>()
                    .Any(item => item.Name == "success");
        }

#region Private Methods

        protected virtual async Task<string> HttpClientGet(string requestUri)
        {
            string data;

            using (var httpClient = new HttpClient())
            using (var message = await httpClient.GetAsync(requestUri))
            {
                message.EnsureSuccessStatusCode();

                data = await message.Content.ReadAsStringAsync();
            }

            return data;
        }

        protected virtual async Task<string> HttpClientPut(string requestUri, string content)
        {
            string data;
            
            using (var httpClient = new HttpClient())
            using (var httpContent = new StringContent(content))
            using (var message = await httpClient.PutAsync(requestUri, httpContent))
            {
                message.EnsureSuccessStatusCode();

                data = await message.Content.ReadAsStringAsync();
            }

            return data;
        }

#endregion

    }
}    