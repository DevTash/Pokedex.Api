using System.Net.Http;
using NSubstitute;
using Pokedex.Api.Clients.TranslatorApi;

namespace Pokedex.Api.Tests.Clients.FunTranslationsApiClientTests
{
    /// <summary>
    ///     Base for FunTranslationsApiClient tests.
    /// </summary>
    public class ClientTestBase
    {

        /// <summary>
        ///     Useful for inspecting mock http calls.
        /// </summary>
        protected readonly MockHttpMessageHandler MockHttpMessageHandler;

        /// <summary>
        ///     System/Subject under test.
        /// </summary>
        protected readonly FunTranslationsApiClient Sut;

        /// <summary>
        ///     Constructs an instance of FunTranslations ClientTestBase.
        /// </summary>
        public ClientTestBase()
        {
            MockHttpMessageHandler = new MockHttpMessageHandler();
            Sut = new FunTranslationsApiClient(new HttpClient(MockHttpMessageHandler));
        }
    }
}