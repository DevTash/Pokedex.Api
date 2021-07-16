using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pokedex.Api.Clients
{
    public class TranslatorApiClient : ITranslatorApiClient
    {
        private readonly HttpClient _client;
        private readonly string _translationFormat = "json";
        private readonly string _yoda = "yoda";
        private readonly string _shakespeare = "shakespeare";

        public TranslatorApiClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));   
            
            _client.BaseAddress = new Uri("https://api.funtranslations.com/translate/");
        }

        public Task<string> GetShakespeareTranslationFor(string content) => GetTranslation(content, _shakespeare);

        public Task<string> GetYodaTranslationFor(string content) => GetTranslation(content, _yoda);

        private async Task<string> GetTranslation(string content, string type = null)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                type = _shakespeare;
            }

            // TODO: Should probably implement caching here as we may have this translation already!
            var res = await _client.GetAsync($"{type}.{_translationFormat}?text={GetUrlSafeText(content)}");

            res.EnsureSuccessStatusCode();

            var payload = await res.Content.ReadAsStringAsync();

            var translation = JsonSerializer.Deserialize<Translation>(payload, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            });
            
            return translation.Contents.Translated;
        }

        private string GetUrlSafeText(string content) => Uri.EscapeUriString(Regex.Replace(content, @"\t|\n|\r", " "));
    }
}