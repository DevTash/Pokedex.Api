using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Pokedex.Api.Tests.Clients.FunTranslationsApiClientTests
{
    public class GetYodaTranslationForTests : ClientTestBase
    {
        [Fact]
        public async Task Given_Content_Then_GetTranslation_Is_Called_With_Type_Yoda()
        {
            // Arrange
            // Act
            await Sut.GetYodaTranslationFor("test content");

            // Assert
            MockHttpMessageHandler.Uri.Should().Contain("yoda");

        }
    }
}