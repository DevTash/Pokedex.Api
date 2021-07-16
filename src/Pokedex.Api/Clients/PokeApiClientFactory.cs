using System;
using Microsoft.Extensions.DependencyInjection;
using PokeApiNet;

namespace Pokedex.Api.Clients
{
    public class PokeApiClientFactory : IApiClientFactory<PokeApiClient>
    {
        private readonly IServiceProvider _serviceProvider;

        public PokeApiClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        public PokeApiClient GetInstance() => _serviceProvider.GetRequiredService<PokeApiClient>();
    }
}