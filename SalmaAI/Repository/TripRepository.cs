using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SalmaAI.Context;
using SalmaAI.Model;

namespace SalmaAI.Repository
{
    public class TripRepository : ITripRepository
    {
        private readonly AppDBContext _appDBContext;
        public TripRepository (AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public async Task<Trip> CreateAsync(Trip trip)
        {
            _appDBContext.Trips.Add(trip);
            await _appDBContext.SaveChangesAsync();
            return trip;
        }

        public async Task<List<Trip>> GetAllAsync()
        {
            return await _appDBContext.Trips.ToListAsync();

        }

        public async Task<Trip?> GetByIdAsync(int id)
        {
            var get = await _appDBContext.Trips.FirstOrDefaultAsync(t=>t.TripId==id);
            return  get;
        }
    }
}
