using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalmaAI.Context;
using SalmaAI.Model;
using SalmaAI.Models;

namespace SalmaAI.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly AppDBContext _appDBContext;

        public ExpenseController (AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;

        }
        [HttpGet("{tripId}/expenses")]
        public async Task<IActionResult> Index()
        {
            var get =await  _appDBContext.Expenses.ToListAsync();
            return Ok(get);

        }
        [HttpPost("{tripId}/expenses")]
        public async Task<IActionResult> Create( int tripId ,[FromForm]ExpenseDTO expense1)
        {
            var tripidexist = await _appDBContext.Trips.FirstOrDefaultAsync(t => t.TripId == tripId);
            if (tripidexist == null) 
            {
                return NotFound($"the tripid  {tripId}not found ");
            }
            var paidbyid = await _appDBContext.Participants.FirstOrDefaultAsync(p => p.ParticipantId == expense1.PaidById);
            if (paidbyid == null)
                return NotFound($"participant id {tripId}not found");
            
            var expense = new Expense
            {
                Amount = expense1.Amount,
                PaidById = expense1.PaidById,
                TripId = tripId,

            };
            _appDBContext.Expenses.Add(expense);
            await _appDBContext.SaveChangesAsync();

            return Ok(expense);






        }
    }
}
