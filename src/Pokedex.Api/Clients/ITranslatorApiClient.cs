using System.Threading.Tasks;

namespace Pokedex.Api.Clients
{
    public interface ITranslatorApiClient
    {
        Task<string> GetYodaTranslationFor(string content);
        Task<string> GetShakespeareTranslationFor(string content);
    }
}