using System;
using System.Threading.Tasks;
using PokeApiNet;

namespace Pokedex.Api.Clients.PokeApi
{
    /// <summary>
    ///     Wrapper for PokeApiNet.PokeApiClient.
    /// </summary>
    public class PokeApiClientWrapper : IPokeApiClientWrapper
    {

        private readonly PokeApiClient _client;

        public PokeApiClientWrapper(PokeApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        ///     Wrapper for PokeApiNet.PokeApiClient GetResourceAsync.
        /// </summary>
        /// <param name="input"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<T> GetResourceAsync<T>(string input) where T : NamedApiResource
        {
            return _client.GetResourceAsync<T>(input);
        }

        /// <summary>
        ///     Wrapper for PokeApiNet.PokeApiClient GetResourceAsync.
        /// </summary>
        /// <param name="input"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<T> GetResourceAsync<T>(UrlNavigation<T> input) where T : NamedApiResource
        {
            return _client.GetResourceAsync<T>(input);
        }
    }
}