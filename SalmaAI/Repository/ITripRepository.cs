using SalmaAI.Model;

namespace SalmaAI.Repository
{
    public interface ITripRepository
    {
        Task<List<Trip>> GetAllAsync();
        Task<Trip?> GetByIdAsync(int id);
        Task<Trip> CreateAsync(Trip trip);
    }
}
