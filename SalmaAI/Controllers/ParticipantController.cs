using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalmaAI.Context;
using SalmaAI.Model;
using SalmaAI.Models;
using SalmaAI.Repository;

namespace SalmaAI.Controllers
{
    [Route("api/trips/{tripId}/participants")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {
        private readonly AppDBContext _appDBContext;
        private readonly ITripRepository _tripRepository;
        private readonly IParticipantRepository _participantRepository;

        public ParticipantController (AppDBContext appDBContext, IParticipantRepository participantRepository,ITripRepository tripRepository)
        {
            _appDBContext = appDBContext;
            _participantRepository = participantRepository;
            _tripRepository = tripRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int tripId )
        {
            var getbyid=await _participantRepository.GetByIdAsync(tripId);
            if (getbyid == null)
                return NotFound($"the participant not found");
            return Ok(getbyid);
         
        }

        [HttpPost]
        public async Task<IActionResult> Create(int tripId,[FromForm]ParticipantDTO participant) 
        {
            var tripExist = await _tripRepository.GetByIdAsync(tripId);
            if(tripExist == null) { return NotFound(); }//check if tripid exixt 

            var newparticipant = new Participant
            {
                ParticipantName = participant.ParticipantName,
                TripId = tripId,
            };
            return Ok(await _participantRepository.CreateAsync(newparticipant));


      
        }
    }
}

