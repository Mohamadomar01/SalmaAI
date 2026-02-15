using Microsoft.EntityFrameworkCore;
using SalmaAI.Context;
using SalmaAI.Model;

namespace SalmaAI.Repository
{
    
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly AppDBContext _appDBContext;
        public ParticipantRepository(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        } 
        
        public async Task<List<Participant>> GetAllAsync()
        {
            var get =await _appDBContext.Participants.ToListAsync();
            return get;
            
        }
        public async Task<Participant?> GetByIdAsync(int id)
        {
            var findbyid=await _appDBContext.Participants.FirstOrDefaultAsync(p=>p.ParticipantId == id);
            return findbyid;
        }
        public async Task<Participant> CreateAsync(Participant participant)
        {
            _appDBContext.Participants.Add(participant);
            await _appDBContext.SaveChangesAsync();
            return  participant;
        }

       

    }
}
