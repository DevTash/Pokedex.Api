using System;
using Microsoft.Extensions.DependencyInjection;

namespace Pokedex.Api.Clients.TranslatorApi
{
    /// <summary>
    ///     Factory for typed TranslatorApiClient.
    /// </summary>
    public class TranslatorApiClientFactory : IApiClientFactory<ITranslatorApiClient>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        ///     Constructs a new instance of TranslatorApiClientFactory.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public TranslatorApiClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        ///     Returns a new instance of ITranslatorApiClient.
        /// </summary>
        /// <typeparam name="ITranslatorApiClient"></typeparam>
        /// <returns></returns>
        public ITranslatorApiClient GetInstance() => _serviceProvider.GetRequiredService<ITranslatorApiClient>();
    }
}