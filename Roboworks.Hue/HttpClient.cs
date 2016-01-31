using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Roboworks.Hue
{
    internal interface IHttpClient
    {
        Task<string> HttpClientGet(string requestUri);

        Task<string> HttpClientPut(string requestUri, string content);

        Task<string> HttpClientPost(string requestUri, string content);

        Task<string> HttpClientDelete(string requestUri);
    }

    internal class HttpClient : IHttpClient
    {
        public async Task<string> HttpClientGet(string requestUri)
        {
            string data;

            using (var httpClient = new System.Net.Http.HttpClient())
            using (var message = await httpClient.GetAsync(requestUri))
            {
                message.EnsureSuccessStatusCode();

                data = await message.Content.ReadAsStringAsync();
            }

            return data;
        }

        public async Task<string> HttpClientPut(string requestUri, string content)
        {
            string data;
            
            using (var httpClient = new System.Net.Http.HttpClient())
            using (var httpContent = new StringContent(content))
            using (var message = await httpClient.PutAsync(requestUri, httpContent))
            {
                message.EnsureSuccessStatusCode();

                data = await message.Content.ReadAsStringAsync();
            }

            return data;
        }

        public async Task<string> HttpClientPost(string requestUri, string content)
        {
            string data;
            
            using (var httpClient = new System.Net.Http.HttpClient())
            using (var httpContent = new StringContent(content))
            using (var message = await httpClient.PostAsync(requestUri, httpContent))
            {
                message.EnsureSuccessStatusCode();

                data = await message.Content.ReadAsStringAsync();
            }

            return data;
        }

        public async Task<string> HttpClientDelete(string requestUri)
        {
            string data;
            
            using (var httpClient = new System.Net.Http.HttpClient())
            using (var message = await httpClient.DeleteAsync(requestUri))
            {
                message.EnsureSuccessStatusCode();

                data = await message.Content.ReadAsStringAsync();
            }

            return data;
        }
    }
}
