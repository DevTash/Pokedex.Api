using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using Pokedex.Api.Clients;

namespace Pokedex.Api.Features.Pokemon
{
    public class PokemonService : IPokemonService
    {
        private readonly ILogger<PokemonService> _logger;
        private readonly IApiClientFactory<PokeApiClient> _pokeApiClientFactory;
        private readonly IApiClientFactory<ITranslatorApiClient> _translatorApiClientFactory;
        private readonly static Random _random = new Random();

        public PokemonService(
            ILogger<PokemonService> logger, 
            IApiClientFactory<PokeApiClient> pokeApiClientFactory,
            IApiClientFactory<ITranslatorApiClient> translatorApiClientFactory
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pokeApiClientFactory = pokeApiClientFactory ?? throw new ArgumentNullException(nameof(pokeApiClientFactory));
            _translatorApiClientFactory = translatorApiClientFactory ?? throw new ArgumentNullException(nameof(translatorApiClientFactory));
        }

        public async Task<BasicPokemonInformation> GetBasicInfoByNameAsync(string pokemonName)
        {
            PokeApiNet.Pokemon pokemon = null;
            PokeApiNet.PokemonSpecies pokemonInfo = null;

            var apiClient = _pokeApiClientFactory.GetInstance();

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
                _logger.LogCritical(ex, "A call using {ApiClientNameType} failed. The service maybe down", apiClient.GetType().Name);
                throw;
            }

            var useableFlavorTexts = pokemonInfo.FlavorTextEntries.Where(ft => ft.Language.Name == "en").ToList();

            return new BasicPokemonInformation
            {
                Name = pokemonInfo.Name,
                Description = GetRandomDescription(useableFlavorTexts),
                Habitat = pokemonInfo.Habitat.Name,
                IsLegendary = pokemonInfo.IsLegendary
            };
        }

        public async Task TranslateDescription(BasicPokemonInformation pokemonInfo)
        {
            if (pokemonInfo == null)
            {
                return;
            }

            var apiClient = _translatorApiClientFactory.GetInstance();
            var originalDesc = pokemonInfo.Description;

            try
            {
                if (pokemonInfo.Habitat?.ToLower() == "cave" || pokemonInfo.IsLegendary)
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

        private string GetRandomDescription(IList<PokemonSpeciesFlavorTexts> descriptions) => descriptions[_random.Next(descriptions.Count())].FlavorText;
    }
}