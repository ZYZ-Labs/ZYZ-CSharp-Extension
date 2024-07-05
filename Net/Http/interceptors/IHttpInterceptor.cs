using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Net.Http.interceptors
{
    public interface IHttpInterceptor
    {
        Task<HttpResponseMessage> InterceptAsync(HttpRequestMessage request, Func<HttpRequestMessage, Task<HttpResponseMessage>> next, CancellationToken cancellationToken);
    }
}
