using System;
using Microsoft.Extensions.DependencyInjection;
using PokeApiNet;

namespace Pokedex.Api.Clients.PokeApi
{
    /// <summary>
    ///     Factory for typed PokeApiClient. 
    /// </summary>
    public class PokeApiClientFactory : IApiClientFactory<PokeApiClient>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        ///     Constructs a new instance of PokeApiClientFactory.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public PokeApiClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        ///     Returns a new instance of PokeApiClient.
        /// </summary>
        /// <typeparam name="PokeApiClient"></typeparam>
        /// <returns></returns>
        public PokeApiClient GetInstance() => _serviceProvider.GetRequiredService<PokeApiClient>();
    }
}