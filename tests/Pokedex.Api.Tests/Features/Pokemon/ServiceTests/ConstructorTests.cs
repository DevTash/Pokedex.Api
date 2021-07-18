using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Pokedex.Api.Clients;
using Pokedex.Api.Clients.PokeApi;
using Pokedex.Api.Clients.TranslatorApi;
using Pokedex.Api.Features.Pokemon;
using Xunit;

namespace Pokedex.Api.Tests.Features.Pokemon.ServiceTests
{
    /// <summary>
    ///     PokemonService constructor tests.
    /// </summary>
    public class ConstructorTests
    {
        [Theory]
        [MemberData(nameof(InvalidDependencies))]
        public void Given_Missing_Dependency_Then_ArgumentNullException_Is_Thrown(
            ILogger<PokemonService> logger,
            IApiClientFactory<IPokeApiClientWrapper> pokeApiClientFactory,
            IApiClientFactory<ITranslatorApiClient> translatorApiClientFactory
        )
        {
            // Arrange
            // Act
            Action construct = () => new PokemonService(logger, pokeApiClientFactory, translatorApiClientFactory); 

            // Assert
            construct.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_Dependencies_Then_ArgumentNullException_Is_Not_Thrown()
        {
            // Arrange
            var logger = Substitute.For<ILogger<PokemonService>>();
            var pokeApiClientFactory = Substitute.For<IApiClientFactory<IPokeApiClientWrapper>>();
            var translatorApiClientFactory = Substitute.For<IApiClientFactory<ITranslatorApiClient>>();

            // Act
            Action construct = () => new PokemonService(logger, pokeApiClientFactory, translatorApiClientFactory);

            // Assert
            construct.Should().NotThrow();
        }

        public static IEnumerable<object[]> InvalidDependencies()
        {
            yield return new object[]
            {
                null,
                Substitute.For<IApiClientFactory<IPokeApiClientWrapper>>(),
                Substitute.For<IApiClientFactory<ITranslatorApiClient>>()
            };

            yield return new object[]
            {
                Substitute.For<ILogger<PokemonService>>(),
                null,
                Substitute.For<IApiClientFactory<ITranslatorApiClient>>()
            };

            yield return new object[]
            {
                Substitute.For<ILogger<PokemonService>>(),
                Substitute.For<IApiClientFactory<IPokeApiClientWrapper>>(),
                null
            };
        }
    }
}