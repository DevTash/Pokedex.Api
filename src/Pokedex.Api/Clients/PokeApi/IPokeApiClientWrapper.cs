using System.Threading.Tasks;
using PokeApiNet;

namespace Pokedex.Api.Clients.PokeApi
{
    public interface IPokeApiClientWrapper
    {
        Task<T> GetResourceAsync<T>(string input) where T : NamedApiResource;

        Task<T> GetResourceAsync<T>(UrlNavigation<T> input) where T : NamedApiResource;
    }
}