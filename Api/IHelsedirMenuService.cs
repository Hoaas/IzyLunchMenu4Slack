using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public interface IHelsedirMenuService
    {
        Task<Dictionary<string, List<string>>> FetchMenu();
        Task<string> FetchEntireMenuAsText();
    }
}