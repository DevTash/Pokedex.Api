using System;
using System.Net.Http;
using FluentAssertions;
using Pokedex.Api.Clients.TranslatorApi;
using Xunit;

namespace Pokedex.Api.Tests.Clients.FunTranslationsApiClientTests
{
    public class ConstructorTests
    {
        [Fact]
        public void Given_HttpClient_Dep_Is_Null_Then_ArgumentNullException_Is_Thrown()
        {
            // Arrange
            // Act
            Action construct = () => new FunTranslationsApiClient(null);

            // Assert
            construct.Should().Throw<ArgumentNullException>().Which.Message
                     .Should().Contain("client");
        }

        [Fact]
        public void Given_HttpClient_Dep_Is_Not_Null_Then_No_ArgumentNullException_Is_Thrown()
        {
            // Arrange
            var client = new HttpClient();

            // Act
            Action construct = () => new FunTranslationsApiClient(client);

            // Assert
            construct.Should().NotThrow<ArgumentNullException>();
        }
    }
}