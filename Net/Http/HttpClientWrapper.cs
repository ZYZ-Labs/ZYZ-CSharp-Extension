using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ZYZ_CSharp_Extension.Net.Http.interceptors;

namespace ZYZ_CSharp_Extension.Net.Http
{
    public class HttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        private static readonly List<IHttpInterceptor> interceptors = new List<IHttpInterceptor>();

        private static readonly X509Certificate2 certificate = null;

        private HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public static HttpClientWrapper Create()
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            if (certificate != null)
            {
                httpClientHandler.ClientCertificates.Add(certificate);
                httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            }
            HttpMessageHandler handler = httpClientHandler;
            if (interceptors.Any())
            {
                handler = new InterceptorHandler(handler, interceptors);
            }

            var httpClient = new HttpClient(handler);
            return new HttpClientWrapper(httpClient);
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetAsync(requestUri, cancellationToken);
        }

        public async Task<HttpResponseMessage> PostJsonAsync(string requestUri,object data, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAsJsonAsync(requestUri, data, cancellationToken);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await _httpClient.SendAsync(request);
        }
    }
}
