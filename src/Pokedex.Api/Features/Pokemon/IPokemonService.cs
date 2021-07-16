using System.Threading.Tasks;

namespace Pokedex.Api.Features.Pokemon
{
    public interface IPokemonService
    {
        Task<BasicPokemonInformation> GetBasicInfoByNameAsync(string pokemonName);
        Task TranslateDescription(BasicPokemonInformation pokemonInfo);
    }
}