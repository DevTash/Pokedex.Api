using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Pokedex.Api.Features.Pokemon;
using Xunit;

namespace Pokedex.Api.Tests.Features.Pokemon.ControllerTests
{
    /// <summary>
    ///     PokemonController GetBasicInfo tests.
    /// </summary>
    public class GetBasicInfoTests : ControllerTestBase
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")] // Single space
        [InlineData("    ")] // 4 Spaces
        [InlineData("   ")] // Tab
        [InlineData(null)]
        public async Task Given_Invalid_Pokemon_Name_Then_BadRequest_Is_Returned(string pokemonName)
        {  
            // Arrange
            // Act
            var badRequestRes = await Sut.GetBasicInfo(pokemonName);

            // Assert
            using (new AssertionScope())
            {
                var model = badRequestRes.Should().BeOfType<BadRequestObjectResult>().Which;
                var message = model.Value.Should().BeOfType<string>().Which;

                message.ToLower().Should().Contain("pokemon name is required");
            }           
        }

        [Fact]
        public async Task Given_Valid_Pokemon_Name_Then_PokemonService_Is_Called_With_Correct_Pokename_Name()
        {
            // Arrange
            var pokemonName = "mewtwo";

            // Act
            await Sut.GetBasicInfo(pokemonName);

            // Assert
            await MockPokemonService.Received().GetBasicInfoByNameAsync(Arg.Is(pokemonName));
        }

        [Fact]
        public async Task Given_PokemonService_Returns_Null_Then_NotFound_Is_Returned()
        {
            // Arrange
            var pokemonName = "mewtwo";
            BasicPokemonInformation mockPokemonInfo = null;

            MockPokemonService.GetBasicInfoByNameAsync(Arg.Any<string>())
                              .ReturnsForAnyArgs(mockPokemonInfo);

            // Act
            var notFoundRes = await Sut.GetBasicInfo(pokemonName);

            // Assert
            notFoundRes.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Given_PokemonService_Returns_BasicPokemon_Info_Then_OkResult_Is_Returned_With_Correct_Data()
        {
            // Arrange
            var pokemonName = "mewtwo";
            var mockPokemonInfo = new BasicPokemonInformation
            {
                Name = pokemonName,
                Description = "Mewtwo desc.",
                Habitat = "rare",
                IsLegendary = true
            };

            MockPokemonService.GetBasicInfoByNameAsync(Arg.Any<string>())
                              .ReturnsForAnyArgs(mockPokemonInfo);

            // Act
            var okResult = await Sut.GetBasicInfo(pokemonName);

            // Assert
            using (new AssertionScope())
            {
                var model = okResult.Should().BeOfType<OkObjectResult>().Which;
                
                model.Value.Should().BeOfType<BasicPokemonInformation>().Which
                           .Should().BeEquivalentTo(mockPokemonInfo);
            }
        }

        [Fact]
        public async Task Given_PokemonService_Throws_Exception_Then_StatusCode_500_Is_Returned()
        {
            // Arrange
            var pokemonName = "mewtwo";

            MockPokemonService.GetBasicInfoByNameAsync(Arg.Any<string>())
                              .ThrowsForAnyArgs(new Exception());

            // Act

            var serverErrorRes = await Sut.GetBasicInfo(pokemonName);

            // Assert
            using (new AssertionScope())                       
            {
                serverErrorRes.Should().BeOfType<StatusCodeResult>().Which.StatusCode
                          .Should().Be(StatusCodes.Status500InternalServerError);
            }
        }
    }
}