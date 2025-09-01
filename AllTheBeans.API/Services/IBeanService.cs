using AllTheBeans.API.Models;

namespace AllTheBeans.API.Services
{
    public interface IBeanService
    {
        // Search
        Task<(IReadOnlyList<Bean> Items, int Total)> SearchAsync(
            string? q, string? country, string? colour,
            decimal? minCost, decimal? maxCost,
            int page = 1, int pageSize = 20, string? sort = null);

        // CRUD
        Task<Bean?> GetByIdAsync(int id);
        Task<Bean> CreateAsync(Bean input);
        Task<Bean?> UpdateAsync(int id, Bean input);
        Task<bool> DeleteAsync(int id);

        // Bean of the Day
        Task<Bean?> GetBeanOfTheDayAsync(DateTime? today = null);
    }
}