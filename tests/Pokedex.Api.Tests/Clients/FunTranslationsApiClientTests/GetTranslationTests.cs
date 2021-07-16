using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Pokedex.Api.Clients.TranslatorApi;
using Xunit;

namespace Pokedex.Api.Tests.Clients.FunTranslationsApiClientTests
{
    public class GetTranslationTests : ClientTestBase
    {
        [Theory]
        [MemberData(nameof(InvalidContent))]
        public async Task Given_Invalid_Content_Then_Empty_String_Is_Returned(string content)
        {
            // Arrange
            // Act
            var translation = await Sut.GetTranslation(content, TranslationsType.Yoda);

            // Assert
            translation.Should().Be("");
        }

        [Theory]
        [MemberData(nameof(InvalidContent))]
        public async Task Given_Invalid_Content_Then_No_Api_Call_Is_Made(string content)
        {
            // Arrange
            // Act
            await Sut.GetTranslation(content, TranslationsType.Yoda);

            // Assert
            MockHttpMessageHandler.Uri.Should().BeNull();
        }

        [Fact]
        public async Task Given_Valid_Content_Then_Api_Call_Is_Made()
        {
            // Arrange
            // Act 
            await Sut.GetTranslation("Sample text to translate", TranslationsType.Yoda);

            // Assert
            MockHttpMessageHandler.Uri.Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData(TranslationsType.Yoda)]
        [InlineData(TranslationsType.Shakespeare)]
        public async Task Given_Valid_Arguments_Then_Request_Uri_Contains_Translation_Type(TranslationsType type)
        {
            // Arrange
            // Act
            await Sut.GetTranslation("Sample text to translate", type);

            // Assert
            MockHttpMessageHandler.Uri.Should().Contain(type.ToString().ToLower());
        }

        [Fact]
        public async Task Given_Valid_Arguments_Then_Request_Uri_Contains_Format_Json()
        {
            // Arrange
            // Act
            await Sut.GetTranslation("Sample text to translate", TranslationsType.Yoda);

            // Assert
            MockHttpMessageHandler.Uri.Should().Contain(".json");
        }

        [Fact]
        public async Task Given_Valid_Arguments_Then_Request_Uri_Contains_Text_Query_Param()
        {
            // Arrange
            // Act
            await Sut.GetTranslation("Sample text to translate", TranslationsType.Yoda);

            // Assert
            MockHttpMessageHandler.Uri.Should().Contain("?text=");
        }

        [Fact]
        public async Task Given_Valid_Arguments_Then_Request_Uri_Contains_Escaped_Text()
        {
            // Arrange
            // Act
            await Sut.GetTranslation("Sample text to translate", TranslationsType.Yoda);

            // Assert
            MockHttpMessageHandler.Uri.Should().Contain("Sample%20text%20to%20translate");
        }

        [Fact]
        public void Given_Translation_Response_Is_Not_Success_Then_Exception_Is_Thrown()
        {
            // Arrange

            // Act

            // Assert
        }

        public static IEnumerable<object[]> InvalidContent()
        {
            yield return new object[] { "" };
            yield return new object[] { " " }; // Single space
            yield return new object[] { "    " }; // 4 Spaces
            yield return new object[] { "  " }; // Tab
            yield return new object[] { null };
        }
    }
}