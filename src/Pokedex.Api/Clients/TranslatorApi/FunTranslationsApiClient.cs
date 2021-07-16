using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pokedex.Api.Utils;

namespace Pokedex.Api.Clients.TranslatorApi
{
    /// <summary>
    ///     FunTranslations API Client.
    /// </summary>
    public class FunTranslationsApiClient : ITranslatorApiClient
    {
        private readonly HttpClient _client;
        private readonly string _baseAddress = "https://api.funtranslations.com/translate/";

        /// <summary>
        ///     Constructs a new instance of FunTranslationsApiClient.
        /// </summary>
        /// <param name="client"></param>
        public FunTranslationsApiClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));   
            
            _client.BaseAddress = new Uri(_baseAddress);
        }

        /// <inheritdoc />
        public Task<string> GetShakespeareTranslationFor(string content) => GetTranslation(content, TranslationsType.Shakespeare);

        /// <inheritdoc />
        public Task<string> GetYodaTranslationFor(string content) => GetTranslation(content, TranslationsType.Yoda);

        /// <summary>
        ///     Fetches a translation from FunTranslations API.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="translationType"></param>
        /// <returns></returns>
        public async Task<string> GetTranslation(string content, TranslationsType translationType)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return "";
            }

            // TODO: Should probably implement caching here as we may have this translation already!
            var res = await _client.GetAsync($"{translationType.ToString().ToLower()}.json?text={GetUrlSafeText(content)}");

            res.EnsureSuccessStatusCode();

            var payload = await res.Content.ReadAsStringAsync();

            var translation = JsonSerializer.Deserialize<FunTranslationResponse>(payload, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            });
            
            return translation.Contents.Translated;
        }

        /// <summary>
        ///     Transforms the given content so that it can be used within a url.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetUrlSafeText(string content) => Uri.EscapeUriString(TextHelper.ReplaceNewLinesWith(content));
    }
}