using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PokeApiNet;
using Pokedex.Api.Clients.PokeApi;
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

            PokeApiClientWrapper pokeApiClient = null;
            MockPokeApiClientFactory.GetInstance().ReturnsForAnyArgs(pokeApiClient);
            
            // Act
            Func<Task> getBasicInfo = async () => await Sut.GetBasicInfoByNameAsync(pokemonName);
            
            // Assert
            using (new AssertionScope())
            {
                getBasicInfo.Should().Throw<NullReferenceException>().Which.Message
                            .Should().Contain("PokeApiClientWrapper");
            }
        }

        [Fact]
        public void Given_ApiClient_Is_Null_Then_Critical_Message_Is_Logged()
        {
            // Arrange
            var pokemonName = "mewtwo";

            PokeApiClientWrapper pokeApiClient = null;
            MockPokeApiClientFactory.GetInstance().ReturnsForAnyArgs(pokeApiClient);

            // Act
            Func<Task> getBasicInfo = async () => await Sut.GetBasicInfoByNameAsync(pokemonName);

            // Assert
            using (new AssertionScope())
            {
                getBasicInfo.Should().Throw<NullReferenceException>();

                LoggerSink.LogEntries.Should().Contain(entry => entry.Message.Contains("PokeApiClientWrapper is null"));
            }
        }

        [Fact]
        public async Task Given_PokeApiClient_Then_Data_Gather_Api_Calls_Are_Made()
        {
            // Arrange
            var pokemonName = "mewtwo";
            var pokeApiClient = Substitute.For<IPokeApiClientWrapper>();

            pokeApiClient.GetResourceAsync<PokeApiNet.Pokemon>(Arg.Any<string>())
                         .Returns(CreateTestApiPokemon());

            pokeApiClient.GetResourceAsync(Arg.Any<NamedApiResource<PokemonSpecies>>())
                         .Returns(CreateTestPokemonSpecies());

            MockPokeApiClientFactory.GetInstance().ReturnsForAnyArgs(pokeApiClient);
            
            // Act
            await Sut.GetBasicInfoByNameAsync(pokemonName);
            
            // Assert
            await pokeApiClient.Received(1).GetResourceAsync<PokeApiNet.Pokemon>(Arg.Is(pokemonName));
            await pokeApiClient.Received(1).GetResourceAsync<PokemonSpecies>(Arg.Any<NamedApiResource<PokemonSpecies>>());            
        }

        [Fact]
        public async Task Given_PokeApiClient_Throws_Exception_With_NotFound_Then_Null_Is_Returned()
        {
            // Arrange
            var pokemonName = "mewtwo";
            var pokeApiClient = Substitute.For<IPokeApiClientWrapper>();
            var notFoundException = new HttpRequestException("NOT FOUND", null, HttpStatusCode.NotFound);

            pokeApiClient.GetResourceAsync<PokeApiNet.Pokemon>(Arg.Any<string>())
                         .ThrowsForAnyArgs(notFoundException);

            MockPokeApiClientFactory.GetInstance().ReturnsForAnyArgs(pokeApiClient);

            // Act
            var basicInfo = await Sut.GetBasicInfoByNameAsync(pokemonName);

            // Assert
            basicInfo.Should().BeNull();
        }

        [Fact]
        public void Given_PokeApiClient_Throws_Socket_Exception_Then_Critical_Message_Is_Logged()
        {
            // Arrange
            var pokemonName = "mewtwo";
            var pokeApiClient = Substitute.For<IPokeApiClientWrapper>();
            var socketException = new HttpRequestException("NETWORK ERROR", new SocketException());

            pokeApiClient.GetResourceAsync<PokeApiNet.Pokemon>(Arg.Any<string>())
                         .ThrowsForAnyArgs(socketException);

            MockPokeApiClientFactory.GetInstance().ReturnsForAnyArgs(pokeApiClient);

            // Act
            Func<Task> getBasicInfo = () => Sut.GetBasicInfoByNameAsync(pokemonName);

            // Assert
            getBasicInfo.Should().Throw<HttpRequestException>();
            
            LoggerSink.LogEntries.Should().Contain(
                entry => entry.Message.Contains("A call to external pokemon api failed. The service maybe down")
            );
        }

        [Fact]
        public void Given_PokeApiClient_Throws_Socket_Exception_Then_Exception_Rethrows()
        {
            // Arrange
            var pokemonName = "mewtwo";
            var pokeApiClient = Substitute.For<IPokeApiClientWrapper>();
            var socketException = new HttpRequestException("NETWORK ERROR", new SocketException());

            pokeApiClient.GetResourceAsync<PokeApiNet.Pokemon>(Arg.Any<string>())
                         .ThrowsForAnyArgs(socketException);

            MockPokeApiClientFactory.GetInstance().ReturnsForAnyArgs(pokeApiClient);

            // Act
            Func<Task> getBasicInfo = () => Sut.GetBasicInfoByNameAsync(pokemonName);

            // Assert
            using (new AssertionScope())
            {
                getBasicInfo.Should().Throw<HttpRequestException>().Which.InnerException
                            .Should().BeOfType<SocketException>();
            }
        }

        [Fact]
        public async Task Given_Pokemon_FlavorTexts_Only_FlavorText_With_Langage_EN_Are_Used()
        {
            // Arrange
            var pokemonName = "mewtwo";
            var pokeApiClient = Substitute.For<IPokeApiClientWrapper>();

            pokeApiClient.GetResourceAsync<PokeApiNet.Pokemon>(Arg.Any<string>())
                         .Returns(CreateTestApiPokemon());

            pokeApiClient.GetResourceAsync(Arg.Any<NamedApiResource<PokemonSpecies>>())
                         .Returns(CreateTestPokemonSpecies());

            MockPokeApiClientFactory.GetInstance().ReturnsForAnyArgs(pokeApiClient);

            // Act
            var basicInfo = await Sut.GetBasicInfoByNameAsync(pokemonName);

            // Assert
            using (new AssertionScope())
            {
                basicInfo.Description.Should().NotBeNullOrWhiteSpace();
                basicInfo.Description.Should().NotBe("Non English text 1");
            }
        }

        [Fact]
        public async Task Given_Happy_Path_Then_Pokemon_Info_Is_Correctly_Mapped_To_BasicPokemonInformation()
        {
            // Arrange
            var pokemonName = "mewtwo";
            var pokeApiClient = Substitute.For<IPokeApiClientWrapper>();

            pokeApiClient.GetResourceAsync<PokeApiNet.Pokemon>(Arg.Any<string>())
                         .Returns(CreateTestApiPokemon());

            pokeApiClient.GetResourceAsync(Arg.Any<NamedApiResource<PokemonSpecies>>())
                         .Returns(CreateTestPokemonSpecies());

            MockPokeApiClientFactory.GetInstance().ReturnsForAnyArgs(pokeApiClient);

            // Act
            var basicInfo = await Sut.GetBasicInfoByNameAsync(pokemonName);

            // Assert
            using (new AssertionScope())
            {
                basicInfo.Name.Should().Be("mewtwo");
                basicInfo.Description.Should().NotBeNullOrWhiteSpace();
                basicInfo.Habitat.Should().Be("rare");
                basicInfo.IsLegendary.Should().BeTrue();
            }
        }

        private PokeApiNet.Pokemon CreateTestApiPokemon()
        {
            return new PokeApiNet.Pokemon
            {
                Species = new NamedApiResource<PokemonSpecies>
                {
                    Name = "mewtwo",
                    Url = "resource-uri"
                }
            };
        }

        private PokemonSpecies CreateTestPokemonSpecies()
        {
            return new PokemonSpecies
            {
                Name = "mewtwo",
                FlavorTextEntries = new List<PokemonSpeciesFlavorTexts>
                {
                    new PokemonSpeciesFlavorTexts
                    {
                        Language = new NamedApiResource<Language>
                        {
                            Name = "en"
                        },
                        FlavorText = "English text 1",
                    },
                    new PokemonSpeciesFlavorTexts
                    {
                        Language = new NamedApiResource<Language>
                        {
                            Name = "en"
                        },
                        FlavorText = "English text 2",
                    },
                    new PokemonSpeciesFlavorTexts
                    {
                        Language = new NamedApiResource<Language>
                        {
                            Name = "en"
                        },
                        FlavorText = "English text 3",
                    },
                    new PokemonSpeciesFlavorTexts
                    {
                        Language = new NamedApiResource<Language>
                        {
                            Name = "es"
                        },
                        FlavorText = "Non English text 1",
                    }
                },
                Habitat = new NamedApiResource<PokemonHabitat>
                {
                    Name = "rare",
                    Url = "resource-uri"
                },
                IsLegendary = true
            };
        }
    }
}