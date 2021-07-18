using System.Threading.Tasks;
using PokeApiNet;

namespace Pokedex.Api.Clients.PokeApi
{
    /// <summary>
    ///  Interface for PokeApiNet.PokeApiClient to ease testing.
    /// </summary>
    public interface IPokeApiClientWrapper
    {
        /// <summary>
        ///     Calls GetResource on PokeApiClient.
        /// </summary>
        /// <param name="input"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetResourceAsync<T>(string input) where T : NamedApiResource;

        /// <summary>
        ///     Calls GetResource on PokeApiClient.
        /// </summary>
        /// <param name="input"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetResourceAsync<T>(UrlNavigation<T> input) where T : NamedApiResource;
    }
}