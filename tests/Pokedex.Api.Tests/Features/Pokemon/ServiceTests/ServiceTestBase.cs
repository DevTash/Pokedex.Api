using Microsoft.Extensions.Logging;
using NSubstitute;
using PokeApiNet;
using Pokedex.Api.Clients;
using Pokedex.Api.Clients.TranslatorApi;
using Pokedex.Api.Features.Pokemon;

namespace Pokedex.Api.Tests.Features.Pokemon.ServiceTests
{
    /// <summary>
    ///     PokemonService test base.
    /// </summary>
    public class ServiceTestBase
    {
        /// <summary>
        ///     Useful for mocking/inspecting logger.
        /// </summary>
        protected readonly ILogger<PokemonService> MockLogger;

        /// <summary>
        ///     Useful for mocking/inspecting PokeApiClientFactory.
        /// </summary>
        protected readonly IApiClientFactory<PokeApiClient> MockPokeApiClientFactory;

        /// <summary>
        ///     Useful for mocking/inspecting TranslatorApiClientFactory.
        /// </summary>
        protected readonly IApiClientFactory<ITranslatorApiClient> MockTranslatorApiClientFactory;

        /// <summary>
        ///     System/Subject under test.
        /// </summary>
        protected readonly PokemonService Sut;

        /// <summary>
        ///     Constructs a new instance of PokemonService ServiceTesBase.
        /// </summary>
        public ServiceTestBase()
        {
            MockLogger = Substitute.For<ILogger<PokemonService>>();
            MockPokeApiClientFactory = Substitute.For<IApiClientFactory<PokeApiClient>>();
            MockTranslatorApiClientFactory = Substitute.For<IApiClientFactory<ITranslatorApiClient>>();
            Sut = new PokemonService(MockLogger, MockPokeApiClientFactory, MockTranslatorApiClientFactory);
        }
    }
}