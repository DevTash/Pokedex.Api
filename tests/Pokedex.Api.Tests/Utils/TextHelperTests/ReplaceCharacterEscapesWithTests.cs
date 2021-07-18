using System.Collections.Generic;
using FluentAssertions;
using Pokedex.Api.Utils;
using Xunit;

namespace Pokedex.Api.Tests.Utils.TextHelperTests
{
    public class ReplaceCharacterEscapesWithTests
    {
        [Theory]
        [MemberData(nameof(InputWithCharacterEscapes))]
        public void Given_Input_With_Character_Escapes_Then_Character_Escapes_Are_Replaced_With_Whitespace(string input, string expected)
        {
            // Arrange
            // Act
            var result = TextHelper.ReplaceCharacterEscapesWith(input);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Given_Input_With_No_NewLine_Characters_Then_Input_Is_Unchanged()
        {
            // Arrange
            var input = "This content has no new line characters";

            // Act
            var result = TextHelper.ReplaceCharacterEscapesWith(input);

            // Assert
            result.Should().Be(input);
        }

        public static IEnumerable<object[]> InputWithCharacterEscapes()
        {
            yield return new object[]
            {
                "This content contain\nnew line characters\nthat should be replaced!",
                "This content contain new line characters that should be replaced!"
            };

            yield return new object[]
            {
                "This content contain\tnew line characters\tthat should be replaced!",
                "This content contain new line characters that should be replaced!"
            };

            yield return new object[]
            {
                "This content contain\rnew line characters\rthat should be replaced!",
                "This content contain new line characters that should be replaced!"
            };

            yield return new object[]
            {
                "This content contain\fnew line characters\fthat should be replaced!",
                "This content contain new line characters that should be replaced!"
            };

            yield return new object[]
            {
                "This content contain\\enew line characters\\ethat should be replaced!",
                "This content contain new line characters that should be replaced!"
            };

            yield return new object[]
            {
                "This content contain\vnew line characters\vthat should be replaced!",
                "This content contain new line characters that should be replaced!"
            };

            yield return new object[]
            {
                "This content contain\anew line characters\athat should be replaced!",
                "This content contain new line characters that should be replaced!"
            };
        }
    }
}