using PokeApiNet;

namespace Pokedex.Api.Clients
{
    /// <summary>
    ///     Interface for a TypedClient factory.
    ///     Useful when consuming transient clients within a singleton service.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApiClientFactory<T>
    {
        /// <summary>
        ///     Returns a new (transient) instance of TypedClient (T).
        /// </summary>
        /// <returns></returns>
        T GetInstance();
    }
}