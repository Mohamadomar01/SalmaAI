using SalmaAI.Model;

namespace SalmaAI.Repository
{
    public interface IParticipantRepository
    {
        Task<List<Participant>> GetAllAsync();
        Task<Participant?> GetByIdAsync(int id);
        Task<Participant> CreateAsync(Participant participant);
    }
}
