using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Pokedex.Api.Clients.TranslatorApi;
using Xunit;

namespace Pokedex.Api.Tests.Clients.FunTranslationsApiClientTests
{
    public class GetShakespeareTranslationForTests : ClientTestBase
    {
        [Fact]
        public async Task Given_Content_Then_GetTranslation_Is_Called_With_Type_Shakespeare()
        {
            // Arrange
            // Act
            await Sut.GetShakespeareTranslationFor("test content");

            // Assert
            MockHttpMessageHandler.Uri.Should().Contain("shakespeare");

        }
    }
}