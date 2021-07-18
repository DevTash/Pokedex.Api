using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Api.Tests
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
        public string CurrentUri { get; private set; } = null;

        /// <summary>
        ///     Allows inspection of request Uri(s).
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        public List<string> UriSink { get; private set; } = new List<string>();

        /// <summary>
        ///     Gets or sets the status code to use for SendAsync response.
        /// </summary>
        /// <value></value>
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        /// <summary>
        ///     Gets or sets the content to use for SendAsync response.
        ///     Defaults to "{ \"contents\": { \"translated\": \"translated text\" } }"
        /// </summary>
        /// <returns></returns>
        public HttpContent Content { get; set; } = new StringContent("{ \"contents\": { \"translated\": \"translated text\" } }");

        /// <summary>
        ///     Fake SendAsync. Updates Uri prop for inspection.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uri = request.RequestUri.AbsoluteUri;
            CurrentUri = uri;
            UriSink.Add(uri);

            return new HttpResponseMessage
            {
                StatusCode = StatusCode,
                Content = Content
            };
        }
    }
}