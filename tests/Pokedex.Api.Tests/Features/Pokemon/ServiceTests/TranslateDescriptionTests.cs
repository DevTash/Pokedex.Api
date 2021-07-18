using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Pokedex.Api.Clients.TranslatorApi;
using Pokedex.Api.Features.Pokemon;
using Xunit;

namespace Pokedex.Api.Tests.Features.Pokemon.ServiceTests
{
    /// <summary>
    ///     PokemonService TranslateDescription tests.
    /// </summary>
    public class TranslateDescriptionTests : ServiceTestBase
    {
        [Fact]
        public async Task Given_PokemonInfo_Null_Then_Exit_Early()
        {
            // Arrange
            // Act
            await Sut.TranslateDescription(null);            
            
            // Assert
            MockTranslatorApiClientFactory.DidNotReceive().GetInstance();
            
        }

        [Fact]
        public void Given_ApiClient_Is_Null_Then_NullReferenceException_Is_Thrown()
        {
            // Arrange
            FunTranslationsApiClient translatorClient = null;
            MockTranslatorApiClientFactory.GetInstance().ReturnsForAnyArgs(translatorClient);

            // Act
            Func<Task> translate = () => Sut.TranslateDescription(new BasicPokemonInformation());

            // Assert
            using (new AssertionScope())
            {
                translate.Should().Throw<NullReferenceException>().Which.Message
                         .Should().Contain("TranslatorApiClient");
            }
        }

        [Fact]
        public void Given_ApiClient_Is_Null_Then_Critical_Message_Is_Logged()
        {
            // Arrange
            FunTranslationsApiClient translatorClient = null;
            MockTranslatorApiClientFactory.GetInstance().ReturnsForAnyArgs(translatorClient);

            // Act
            Func<Task> translate = () => Sut.TranslateDescription(new BasicPokemonInformation());

            // Assert
            using (new AssertionScope())
            {
                translate.Should().Throw<NullReferenceException>();

                LoggerSink.LogEntries.Should().Contain(entry => entry.Message.Contains("TranslatorApiClient is null"));
            }
        }

        [Fact]
        public async Task Given_Pokemon_With_Habitat_Cave_Then_Yoda_Translation_Is_Applied()
        {
            // Arrange
            var apiClient =  Substitute.For<ITranslatorApiClient>();
            var desc = "Original desc";
            var basicInfo = new BasicPokemonInformation
            {
                Description = desc,
                Habitat = "Cave"
            };

            MockTranslatorApiClientFactory.GetInstance().ReturnsForAnyArgs(apiClient);

            // Act
            await Sut.TranslateDescription(basicInfo);

            // Assert
            await apiClient.Received().GetYodaTranslationFor(Arg.Is(desc));
            await apiClient.DidNotReceive().GetShakespeareTranslationFor(Arg.Any<string>());
        }

        [Fact]
        public async Task Given_Pokemon_With_IsLegendary_True_Then_Yoda_Translation_Is_Applied()
        {
            // Arrange
            var apiClient = Substitute.For<ITranslatorApiClient>();
            var desc = "Original desc";
            var basicInfo = new BasicPokemonInformation
            {
                Description = desc,
                IsLegendary = true
            };

            MockTranslatorApiClientFactory.GetInstance().ReturnsForAnyArgs(apiClient);

            // Act
            await Sut.TranslateDescription(basicInfo);

            // Assert
            await apiClient.Received().GetYodaTranslationFor(Arg.Is(desc));
            await apiClient.DidNotReceive().GetShakespeareTranslationFor(Arg.Any<string>());
        }

        [Fact]
        public async Task Given_Pokemon_Without_Habitat_Cave_Or_IsLegendary_True_Then_Shakespeare_Translation_Is_Applied()
        {
            // Arrange
            var apiClient = Substitute.For<ITranslatorApiClient>();
            var desc = "Original desc";
            var basicInfo = new BasicPokemonInformation
            {
                Description = desc,
                IsLegendary = false,
                Habitat = "NotCave"
            };

            MockTranslatorApiClientFactory.GetInstance().ReturnsForAnyArgs(apiClient);

            // Act
            await Sut.TranslateDescription(basicInfo);

            // Assert
            await apiClient.Received().GetShakespeareTranslationFor(Arg.Is(desc));
            await apiClient.DidNotReceive().GetYodaTranslationFor(Arg.Any<string>());
        }

        [Fact]
        public async Task Given_Exception_Thrown_Then_Original_Pokemon_Description_Is_Applied()
        {
            var apiClient = Substitute.For<ITranslatorApiClient>();
            var desc = "Original desc";
            var basicInfo = new BasicPokemonInformation
            {
                Description = desc,
                IsLegendary = false,
                Habitat = "NotCave"
            };

            apiClient.GetShakespeareTranslationFor(Arg.Any<string>())
                     .ThrowsForAnyArgs(new Exception());

            MockTranslatorApiClientFactory.GetInstance().ReturnsForAnyArgs(apiClient);

            // Act
            await Sut.TranslateDescription(basicInfo);

            // Assert
            basicInfo.Description.Should().Be(desc);
        }

        [Fact]
        public async Task Given_Exception_Thrown_Then_Error_Message_Is_Logged()
        {
            var apiClient = Substitute.For<ITranslatorApiClient>();
            var desc = "Original desc";
            var basicInfo = new BasicPokemonInformation
            {
                Name = "mewtwo",
                Description = desc,
                IsLegendary = false,
                Habitat = "NotCave"
            };

            apiClient.GetShakespeareTranslationFor(Arg.Any<string>())
                     .ThrowsForAnyArgs(new Exception());

            MockTranslatorApiClientFactory.GetInstance().ReturnsForAnyArgs(apiClient);

            // Act
            await Sut.TranslateDescription(basicInfo);

            // Assert
            LoggerSink.LogEntries.Should().Contain(
                entry => entry.Message.Contains("Error occurred translating description for Pokemon: mewtwo. Using original desc.")
            );
        }
    }
}