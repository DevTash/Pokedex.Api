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
    ///     PokemonController GetTranslatedDescription tests.
    /// </summary>
    public class GetTranslatedDescriptionTests : ControllerTestBase
    {
        [Fact]
        public async Task Given_GetBasicInfo_Returns_BadRequest_Then_BadRequest_Is_Returned()
        {
            // Arrange
            var invalidPokemonName = "    ";

            // Act
            var badRequestRes = await Sut.GetBasicInfoWithTranslatedDesc(invalidPokemonName);

            // Assert            
            using (new AssertionScope())
            {
                var model = badRequestRes.Should().BeOfType<BadRequestObjectResult>().Which;
                var message = model.Value.Should().BeOfType<string>().Which;
                           
                message.ToLower().Should().Contain("pokemon name is required");
            }
        }

        [Fact]
        public async Task Given_GetBasicInfo_Returns_Error_Then_StatusCode_500_Is_Returned()
        {
            // Arrange
            var pokemonName = "mewtwo";

            MockPokemonService.GetBasicInfoByNameAsync(Arg.Any<string>())
                              .ThrowsForAnyArgs(new Exception());

            // Act
            var serverErrorRes = await Sut.GetBasicInfoWithTranslatedDesc(pokemonName);

            // Assert            
            using (new AssertionScope())
            {
                serverErrorRes.Should().BeOfType<StatusCodeResult>().Which.StatusCode
                              .Should().Be(StatusCodes.Status500InternalServerError);
            }
        }

        [Fact]
        public async Task Given_GetBasicInfo_Returns_NotFound_Then_NotFound_Is_Returned()
        {
            // Arrange
            var pokemonName = "mewtwo";
            BasicPokemonInformation pokemonInfo = null;

            MockPokemonService.GetBasicInfoByNameAsync(Arg.Any<string>())
                              .ReturnsForAnyArgs(pokemonInfo);

            // Act
            var notFoundRes = await Sut.GetBasicInfoWithTranslatedDesc(pokemonName);

            // Assert            
            notFoundRes.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Given_GetBasicInfo_Returns_Ok_Then_TranslateDescription_Is_Called_With_Pokemon_BasicInfo()
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
            await Sut.GetBasicInfoWithTranslatedDesc(pokemonName);

            // Assert            
            await MockPokemonService.Received().TranslateDescription(Arg.Is(mockPokemonInfo));
        }

        [Fact]
        public async Task Given_TranslateDescription_Completes_Successfully_Then_OkResult_Is_Returned()
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

            MockPokemonService.TranslateDescription(Arg.Any<BasicPokemonInformation>())
                              .ReturnsForAnyArgs(Task.CompletedTask);

            // Act
            var okRes = await Sut.GetBasicInfoWithTranslatedDesc(pokemonName);

            // Assert            
            using (new AssertionScope())
            {
                okRes.Should().BeOfType<OkObjectResult>().Which.Value
                     .Should().BeEquivalentTo(mockPokemonInfo);
            }
        }

        [Fact]
        public async Task Given_TranslateDescription_Throws_Exception_Then_StatusCode_500_Is_Returned()
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

            MockPokemonService.TranslateDescription(Arg.Any<BasicPokemonInformation>())
                              .ThrowsForAnyArgs(new Exception());

            // Act
            var okRes = await Sut.GetBasicInfoWithTranslatedDesc(pokemonName);

            // Assert            
            using (new AssertionScope())
            {
                okRes.Should().BeOfType<StatusCodeResult>().Which.StatusCode
                     .Should().Be(StatusCodes.Status500InternalServerError);
            }
        }
    }
}