using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Api.Tests.Features.Pokemon.ServiceTests
{
    public class GetBasicInfoByNameAsyncTests : ServiceTestBase
    {
        [Theory]
        public async Task Given_Invalid_Pokemon_Name_Then_Null_Is_Returned()
        {

        }

        [Fact]
        public async Task Given_PokeApiClient_Is_Null_Then_Null_Is_Returned()
        {
            
        }

        [Fact]
        public async Task Given_PokeApiClient_Is_Null_Then_Critical_Message_Is_Logged()
        {

        }

        [Fact]
        public async Task Given_PokeApiClient_Then_Data_Gather_Api_Calls_Are_Made()
        {

        }

        [Fact]
        public async Task Given_PokeApiClient_Throws_Exception_With_NotFound_Then_Null_Is_Returned()
        {

        }

        [Fact]
        public async Task Given_PokeApiClient_Throws_Socket_Exception_Then_Critical_Message_Is_Logged()
        {

        }

        [Fact]
        public async Task Given_PokeApiClient_Throws_Socket_Exception_Then_Exception_Rethrows()
        {

        }

        [Fact]
        public async Task Given_Pokemon_FlavorTexts_Only_FlavorText_With_Langage_EN_Are_Used()
        {

        }

        [Fact]
        public async Task Given_Happy_Path_Then_Pokemon_Info_Is_Correctly_Mapped_To_BasicPokemonInformation()
        {
            
        }

        [Fact]
        public async Task Given_Happy_Path_Then_BasicPokemonInformation_Description_Is_Randomly_Chosen()
        {

        }
    }
}