using PokeApiNet;

namespace Pokedex.Api.Clients
{
    public interface IApiClientFactory<T>
    {
        T GetInstance();
    }
}