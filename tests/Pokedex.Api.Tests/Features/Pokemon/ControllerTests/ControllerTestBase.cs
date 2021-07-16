using NSubstitute;
using Pokedex.Api.Features.Pokemon;

namespace Pokedex.Api.Tests.Features.Pokemon.ControllerTests
{
    /// <summary>
    ///     PokemonController test base.
    /// </summary>
    public class ControllerTestBase
    {
        /// <summary>
        ///     Useful for mocking IPokemonService calls.
        /// </summary>
        protected readonly IPokemonService MockPokemonService;

        /// <summary>
        ///     System/Subject under test
        /// </summary>
        protected readonly PokemonController Sut;  


        /// <summary>
        ///     Constructs an instance of PokemonController ControllerTestBase.
        /// </summary>
        public ControllerTestBase()
        {
            MockPokemonService = Substitute.For<IPokemonService>();
            Sut = new PokemonController(MockPokemonService);
        } 
    }
}