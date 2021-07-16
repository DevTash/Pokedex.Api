using System.Collections.Generic;
using FluentAssertions;
using Pokedex.Api.Utils;
using Xunit;

namespace Pokedex.Api.Tests.Utils.TextHelperTests
{
    public class ReplaceNewLinesWithTests
    {
        [Theory]
        [MemberData(nameof(InputWithNewLineCharacters))]
        public void Given_Input_With_NewLine_Characters_Then_NewLine_Characters_Are_Replaced_With_Whitespace(string input, string expected)
        {
            // Arrange
            // Act
            var result = TextHelper.ReplaceNewLinesWith(input);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Given_Input_With_No_NewLine_Characters_Then_Input_Is_Unchanged()
        {
            // Arrange
            var input = "This content has no new line characters";

            // Act
            var result = TextHelper.ReplaceNewLinesWith(input);

            // Assert
            result.Should().Be(input);
        }

        public static IEnumerable<object[]> InputWithNewLineCharacters()
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
        }
    }
}