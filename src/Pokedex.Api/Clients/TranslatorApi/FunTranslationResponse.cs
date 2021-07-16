namespace Pokedex.Api.Clients.TranslatorApi
{
    /// <summary>
    ///     Represents FunTranslation's translate response.
    /// </summary>
    public class FunTranslationResponse
    {
        /// <summary>
        ///     Gets or sets the contents of this translation response.
        /// </summary>
        public FunTranslationContents Contents { get; set; }
    }
}