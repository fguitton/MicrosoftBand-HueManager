using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboworks.Hue.UTests.Mock
{
    public class HttpClientMock : IHttpClient
    {
        public delegate Task<string> HttpClientGetDelegate(string requestUri);
        public delegate Task<string> HttpClientPutDelegate(string requestUri, string content);
        public delegate Task<string> HttpClientPostDelegate(string requestUri, string content);
        public delegate Task<string> HttpClientDeleteDelegate(string requestUri);

        public HttpClientGetDelegate HttpClientGet_Delegate { get; set; } = null;

        public HttpClientPutDelegate HttpClientPut_Delegate { get; set; } = null;

        public HttpClientPostDelegate HttpClientPost_Delegate { get; set; } = null;

        public HttpClientDeleteDelegate HttpClientDelete_Delegate { get; set; } = null;

        public Task<string> HttpClientDelete(string requestUri)
        {
            return this.HttpClientDelete_Delegate?.Invoke(requestUri);
        }

        public Task<string> HttpClientGet(string requestUri)
        {
            return this.HttpClientGet_Delegate?.Invoke(requestUri);
        }

        public Task<string> HttpClientPost(string requestUri, string content)
        {
            return this.HttpClientPost_Delegate.Invoke(requestUri, content);
        }

        public Task<string> HttpClientPut(string requestUri, string content)
        {
            return this.HttpClientPut_Delegate?.Invoke(requestUri, content);
        }
    }
}
