using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Pokedex.Api.Tests.Clients.FunTranslationsApiClientTests
{
    /// <summary>
    ///     FunTranslationsApiClient GetShakespeareTranslationFor tests.
    /// </summary>
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