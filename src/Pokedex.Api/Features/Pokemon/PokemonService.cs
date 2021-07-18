using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using Pokedex.Api.Clients;
using Pokedex.Api.Clients.PokeApi;
using Pokedex.Api.Clients.TranslatorApi;
using Pokedex.Api.Utils;

namespace Pokedex.Api.Features.Pokemon
{
    /// <summary>
    ///     Implementation of IPokemonService.
    /// </summary>
    public class PokemonService : IPokemonService
    {
        private readonly ILogger<PokemonService> _logger;
        private readonly IApiClientFactory<IPokeApiClientWrapper> _pokeApiClientFactory;
        private readonly IApiClientFactory<ITranslatorApiClient> _translatorApiClientFactory;
        private readonly static Random _random = new Random();

        /// <summary>
        ///     Constructs a new instance of PokemonService.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pokeApiClientFactory"></param>
        /// <param name="translatorApiClientFactory"></param>
        public PokemonService(
            ILogger<PokemonService> logger, 
            IApiClientFactory<IPokeApiClientWrapper> pokeApiClientFactory,
            IApiClientFactory<ITranslatorApiClient> translatorApiClientFactory
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pokeApiClientFactory = pokeApiClientFactory ?? throw new ArgumentNullException(nameof(pokeApiClientFactory));
            _translatorApiClientFactory = translatorApiClientFactory ?? throw new ArgumentNullException(nameof(translatorApiClientFactory));
        }

        /// <inheritdoc />
        public async Task<BasicPokemonInformation> GetBasicInfoByNameAsync(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                return null;
            }

            PokeApiNet.Pokemon pokemon = null;
            PokemonSpecies pokemonInfo = null;

            var apiClient = _pokeApiClientFactory.GetInstance();
            
            EnsureApiClientInstance(apiClient);

            try
            {
                pokemon = await apiClient.GetResourceAsync<PokeApiNet.Pokemon>(pokemonName);
                pokemonInfo = await apiClient.GetResourceAsync<PokemonSpecies>(pokemon.Species);

            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                _logger.LogCritical(ex, "A call to external pokemon api failed. The service maybe down");
                throw;
            }

            var useableFlavorTexts = pokemonInfo.FlavorTextEntries.Where(ft => ft.Language.Name.ToLower() == "en").ToList();

            return new BasicPokemonInformation
            {
                Name = pokemonInfo.Name,
                Description = TextHelper.ReplaceCharacterEscapesWith(GetRandomDescription(useableFlavorTexts)),
                Habitat = pokemonInfo.Habitat.Name,
                IsLegendary = pokemonInfo.IsLegendary
            };
        }

        /// <inheritdoc />
        public async Task TranslateDescription(BasicPokemonInformation pokemonInfo)
        {
            if (pokemonInfo == null)
            {
                return;
            }

            var originalDesc = pokemonInfo.Description;
            var apiClient = _translatorApiClientFactory.GetInstance();

            EnsureApiClientInstance(apiClient);

            try
            {
                if (pokemonInfo.Habitat.ToLower() == "cave" || pokemonInfo.IsLegendary)
                {
                    pokemonInfo.Description = await apiClient.GetYodaTranslationFor(pokemonInfo.Description);
                }
                else
                {
                    pokemonInfo.Description = await apiClient.GetShakespeareTranslationFor(pokemonInfo.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error occurred translating description for Pokemon: {PokemonName}. Using original desc.", pokemonInfo.Name);

                pokemonInfo.Description = originalDesc;
            }

            return;
        }

        /// <summary>
        ///     Selects a random description from a list of descriptions.
        /// </summary>
        /// <param name="descriptions"></param>
        /// <returns></returns>
        private string GetRandomDescription(IList<PokemonSpeciesFlavorTexts> descriptions) => descriptions[_random.Next(descriptions.Count())].FlavorText;

        /// <summary>
        ///     Checks if the given API client is null and throws if true.
        /// </summary>
        /// <param name="apiClient"></param>
        /// <typeparam name="T"></typeparam>
        private void EnsureApiClientInstance<T>(T apiClient)
        {
            if (apiClient == null)
            {
                var errorMsg = $"{typeof(T).Name} is null";

                _logger.LogCritical(errorMsg);
                throw new NullReferenceException(errorMsg);
            }
        }
    }
}