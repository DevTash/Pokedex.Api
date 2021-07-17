using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using PokeApiNet;
using Xunit;

namespace Pokedex.Api.Tests.Features.Pokemon.ServiceTests
{
    /// <summary>
    ///     PokemonService GetBasicInfoByNameAsync tests.
    /// </summary>
    public class GetBasicInfoByNameAsyncTests : ServiceTestBase
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")] // Single space
        [InlineData("    ")] // 4 Spaces
        [InlineData("   ")] // Tab
        [InlineData(null)]
        public async Task Given_Invalid_Pokemon_Name_Then_Null_Is_Returned(string pokemonName)
        {
            // Arrange            
            // Act
            var basicInfo = await Sut.GetBasicInfoByNameAsync(pokemonName);
            
            // Assert
            basicInfo.Should().BeNull();
            
        }

        [Fact]
        public void Given_ApiClient_Is_Null_Then_NullReferenceException_Is_Thrown()
        {
            // Arrange
            var pokemonName = "mewtwo";

            PokeApiClient pokeApiClient = null;
            MockPokeApiClientFactory.GetInstance().ReturnsForAnyArgs(pokeApiClient);
            
            // Act
            Func<Task> getBasicInfo = async () => await Sut.GetBasicInfoByNameAsync(pokemonName);
            
            // Assert
            using (new AssertionScope())
            {
                getBasicInfo.Should().Throw<NullReferenceException>().Which.Message
                            .Should().Contain("PokeApiClient");
            }
        }

        [Fact]
        public void Given_ApiClient_Is_Null_Then_Critical_Message_Is_Logged()
        {
            // Arrange
            var pokemonName = "mewtwo";

            PokeApiClient pokeApiClient = null;
            MockPokeApiClientFactory.GetInstance().ReturnsForAnyArgs(pokeApiClient);

            // Act
            Func<Task> getBasicInfo = async () => await Sut.GetBasicInfoByNameAsync(pokemonName);

            // Assert
            using (new AssertionScope())
            {
                getBasicInfo.Should().Throw<NullReferenceException>();

                LoggerSink.LogEntries.Should().Contain(entry => entry.Message.Contains("PokeApiClient is null"));
            }
        }

        [Fact]
        public async Task Given_PokeApiClient_Then_Data_Gather_Api_Calls_Are_Made()
        {

        }

        [Fact]
        public async Task Given_PokeApiClient_Throws_Exception_With_NotFound_Then_Null_Is_Returned()
        {

        }

        [Fact]
        public async Task Given_PokeApiClient_Throws_Socket_Exception_Then_Critical_Message_Is_Logged()
        {

        }

        [Fact]
        public async Task Given_PokeApiClient_Throws_Socket_Exception_Then_Exception_Rethrows()
        {

        }

        [Fact]
        public async Task Given_Pokemon_FlavorTexts_Only_FlavorText_With_Langage_EN_Are_Used()
        {

        }

        [Fact]
        public async Task Given_Happy_Path_Then_Pokemon_Info_Is_Correctly_Mapped_To_BasicPokemonInformation()
        {
            
        }

        [Fact]
        public async Task Given_Happy_Path_Then_BasicPokemonInformation_Description_Is_Randomly_Chosen()
        {

        }
    }
}