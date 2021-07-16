using System.Threading.Tasks;

namespace Pokedex.Api.Clients.TranslatorApi
{
    /// <summary>
    ///     Interface for a class that can perform translations.
    /// </summary>
    public interface ITranslatorApiClient
    {
        /// <summary>
        ///     Translates the given content to Yoda dialect.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<string> GetYodaTranslationFor(string content);

        /// <summary>
        ///     Translates the given content to Shakespeare dialect.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<string> GetShakespeareTranslationFor(string content);
    }
}