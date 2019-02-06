using System.Threading.Tasks;

namespace Api.ImageSearch
{
    public interface IImageSearcher
    {
        Task<string> SearchForMeal(string meal);
    }
}