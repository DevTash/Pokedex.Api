using System.Threading.Tasks;

namespace Pokedex.Api.Features.Pokemon
{
    /// <summary>
    ///     Interface for a service that suppports Pokemon interactions.
    /// </summary>
    public interface IPokemonService
    {
        /// <summary>
        ///     Gets the basic information for the given Pokemon name.
        /// </summary>
        /// <param name="pokemonName"></param>
        /// <returns></returns>
        Task<BasicPokemonInformation> GetBasicInfoByNameAsync(string pokemonName);

        /// <summary>
        ///     Translates the description of the given BasicPokemonInformation using Yoda or Shakespeare.
        /// </summary>
        /// <param name="pokemonInfo"></param>
        /// <returns></returns>
        Task TranslateDescription(BasicPokemonInformation pokemonInfo);
    }
}