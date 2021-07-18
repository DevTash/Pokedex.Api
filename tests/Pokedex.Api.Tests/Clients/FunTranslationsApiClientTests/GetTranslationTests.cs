using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Pokedex.Api.Clients.TranslatorApi;
using Xunit;

namespace Pokedex.Api.Tests.Clients.FunTranslationsApiClientTests
{
    /// <summary>
    ///     FunTranslationsApiClient GetTranslation tests.
    /// </summary>
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
            MockHttpMessageHandler.CurrentUri.Should().BeNull();
        }

        [Fact]
        public async Task Given_Valid_Content_Then_Api_Call_Is_Made()
        {
            // Arrange
            // Act 
            await Sut.GetTranslation("Sample text to translate", TranslationsType.Yoda);

            // Assert
            MockHttpMessageHandler.CurrentUri.Should().NotBeNullOrWhiteSpace();
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
            MockHttpMessageHandler.CurrentUri.Should().Contain(type.ToString().ToLower());
        }

        [Fact]
        public async Task Given_Valid_Arguments_Then_Request_Uri_Contains_Format_Json()
        {
            // Arrange
            // Act
            await Sut.GetTranslation("Sample text to translate", TranslationsType.Yoda);

            // Assert
            MockHttpMessageHandler.CurrentUri.Should().Contain(".json");
        }

        [Fact]
        public async Task Given_Valid_Arguments_Then_Request_Uri_Contains_Text_Query_Param()
        {
            // Arrange
            // Act
            await Sut.GetTranslation("Sample text to translate", TranslationsType.Yoda);

            // Assert
            MockHttpMessageHandler.CurrentUri.Should().Contain("?text=");
        }

        [Fact]
        public async Task Given_Valid_Arguments_Then_Request_Uri_Contains_Escaped_Text()
        {
            // Arrange
            // Act
            await Sut.GetTranslation("Sample text to translate", TranslationsType.Yoda);

            // Assert
            MockHttpMessageHandler.CurrentUri.Should().Contain("Sample%20text%20to%20translate");
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        public void Given_Translation_Response_Is_Not_Success_Then_Exception_Is_Thrown(HttpStatusCode statusCode)
        {
            // Arrange
            MockHttpMessageHandler.StatusCode = statusCode;

            // Act
            Func<Task> translate = () => Sut.GetTranslation("Sample text to translate", TranslationsType.Yoda);

            // Assert
            translate.Should().Throw<HttpRequestException>();
        }

        [Fact]
        public async Task Given_Translation_Response_Is_Success_Then_Translated_Content_Is_Returned()
        {
            // Arrange
            MockHttpMessageHandler.StatusCode = HttpStatusCode.OK;
            MockHttpMessageHandler.Content = new StringContent("{ \"Contents\": { \"Translated\": \"Translated text from API\" } }");

            // Act
            var translatedText = await Sut.GetTranslation("Some text to translate", TranslationsType.Yoda);            
            
            // Assert
            translatedText.Should().Be("Translated text from API");
            
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