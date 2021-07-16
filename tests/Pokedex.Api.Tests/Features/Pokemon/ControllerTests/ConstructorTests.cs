using System;
using FluentAssertions;
using NSubstitute;
using Pokedex.Api.Features.Pokemon;
using Xunit;

namespace Pokedex.Api.Tests.Features.Pokemon.ControllerTests
{
    /// <summary>
    ///     PokemonController constructor tests.
    /// </summary>
    public class ConstructorTests
    {
        [Fact]
        public void Given_PokemonService_Dep_Is_Null_Then_ArgumentNullException_Is_Thrown()
        {
            // Arrange
            // Act
            Action construct = () => new PokemonController(null);

            // Assert
            construct.Should().Throw<ArgumentNullException>().Which.Message
                     .Should().Contain("pokemonService");
        }

        [Fact]
        public void Given_PokemonService_Dep_Is_Not_Null_Then_No_ArgumentNullException_Is_Thrown()
        {
            // Arrange
            var pokemonService = Substitute.For<IPokemonService>();

            // Act
            Action construct = () => new PokemonController(pokemonService);

            // Assert
            construct.Should().NotThrow<ArgumentNullException>();
        }
    }
}