using System;
using Microsoft.Extensions.DependencyInjection;

namespace Pokedex.Api.Clients
{
    public class TranslatorApiClientFactory : IApiClientFactory<ITranslatorApiClient>
    {
        private readonly IServiceProvider _serviceProvider;

        public TranslatorApiClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        
        public ITranslatorApiClient GetInstance() => _serviceProvider.GetRequiredService<ITranslatorApiClient>();
    }
}