namespace Pokedex.Api.Features.Pokemon
{
    /// <summary>
    ///     The minimum data needed to form a Pokemon's basic information.
    /// </summary>
    public class BasicPokemonInformation
    {
        /// <summary>
        ///     Gets or sets the name of the associated Pokemon.
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description of the associated Pokemon.
        /// </summary>
        /// <value></value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the habitat of the associated Pokemon.
        /// </summary>
        /// <value></value>
        public string Habitat { get; set; }

        /// <summary>
        ///     Gets or sets the legendary status of the associated Pokemon.
        /// </summary>
        /// <value></value>
        public bool IsLegendary { get; set; }
    }
}