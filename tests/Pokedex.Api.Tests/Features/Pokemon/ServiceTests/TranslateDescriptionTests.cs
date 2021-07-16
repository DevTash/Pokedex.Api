using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Api.Tests.Features.Pokemon.ServiceTests
{
    public class TranslateDescriptionTests : ServiceTestBase
    {
        [Fact]
        public async Task Given_PokemonInfo_Null_Then_Exit_Early()
        {

        }

        [Fact]
        public async Task Given_ApiClient_Is_Null_Then_NullReferenceException_Is_Thrown()
        {

        }

        [Fact]
        public async Task Given_ApiClient_Is_Null_Then_Critical_Message_Is_Logged()
        {

        }

        [Fact]
        public async Task Given_Pokemon_With_Habitat_Cave_Then_Yoda_Translation_Is_Applied()
        {

        }

        [Fact]
        public async Task Given_Pokemon_With_IsLegendary_True_Then_Yoda_Translation_Is_Applied()
        {

        }

        [Fact]
        public async Task Given_Pokemon_Without_Habitat_Cave_Or_IsLegendary_True_Then_Shakespeare_Translation_Is_Applied()
        {

        }

        [Fact]
        public async Task Given_Exception_Thrown_Then_Original_Pokemon_Description_Is_Applied()
        {

        }

        [Fact]
        public async Task Given_Exception_Thrown_Then_Error_Message_Is_Logged()
        {

        }
    }
}