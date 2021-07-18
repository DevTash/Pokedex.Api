using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Pokedex.Api.Tests.Clients.FunTranslationsApiClientTests
{
    /// <summary>
    ///     FunTranslationsApiClient GetYodaTranslationFor tests.
    /// </summary>
    public class GetYodaTranslationForTests : ClientTestBase
    {
        [Fact]
        public async Task Given_Content_Then_GetTranslation_Is_Called_With_Type_Yoda()
        {
            // Arrange
            // Act
            await Sut.GetYodaTranslationFor("test content");

            // Assert
            MockHttpMessageHandler.CurrentUri.Should().Contain("yoda");

        }
    }
}