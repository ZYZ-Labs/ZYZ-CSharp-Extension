using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Net.Http.interceptors
{
    public class InterceptorHandler : DelegatingHandler
    {
        private readonly IList<IHttpInterceptor> _interceptors;

        public InterceptorHandler(HttpMessageHandler innerHandler, IList<IHttpInterceptor> interceptors) : base(innerHandler)
        {
            _interceptors = interceptors;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Func<HttpRequestMessage, Task<HttpResponseMessage>> handler = async (HttpRequestMessage req) =>
            {
                return await base.SendAsync(req, cancellationToken);
            };

            foreach (var interceptor in _interceptors.Reverse())
            {
                var next = handler;
                handler = async (req) => await interceptor.InterceptAsync(req, next, cancellationToken);
            }

            return await handler(request);
        }
    }
}
