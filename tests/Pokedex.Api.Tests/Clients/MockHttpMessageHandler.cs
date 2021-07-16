using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Api.Tests.Clients
{
    /// <summary>
    ///     Mock http message handler for HttpClient construction.
    /// </summary>
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        /// <summary>
        ///     Allows inspection of request Uri.
        /// </summary>
        /// <value></value>
        public string Uri { get; private set; }

        /// <summary>
        ///     Fake SendAsync. Updates Uri prop for inspection.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Uri = request.RequestUri.AbsoluteUri;

            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{ \"contents\": { \"translated\": \"translated text\" } }")
            };
        }
    }
}