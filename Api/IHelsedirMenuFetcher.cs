using System.Threading.Tasks;
using Api.Models.Workplace;

namespace Api
{
    public interface IHelsedirMenuFetcher
    {
        Task<WorkplaceResponse> ReadMenu();
    }
}