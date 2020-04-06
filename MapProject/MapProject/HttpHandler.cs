using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MapProject
{
    public class HttpHandler : DelegatingHandler
    {
        public HttpHandler()
        {
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            Console.WriteLine(request.RequestUri.ToString());
            var responseAsString = response.Content.ReadAsStringAsync();

            return response;
        }
    }
}