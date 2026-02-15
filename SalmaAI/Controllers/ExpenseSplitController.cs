using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalmaAI.Context;
using SalmaAI.DTO;
using SalmaAI.Model;

namespace SalmaAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseSplitController : ControllerBase
    {
        private readonly AppDBContext _appDBContext;
        
        public ExpenseSplitController (AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        [HttpGet]

        public async Task<IActionResult> ViewSplit()
        {
            var viewsplit = await _appDBContext.ExpenseSplits.ToListAsync();

            return Ok(viewsplit);
        }
        [HttpPost]

        public async Task<IActionResult> CreateSplit([FromForm]ExpensesSplitDTO DTO)
        {
            var expenseid=await _appDBContext.Expenses.SingleOrDefaultAsync(e=>e.ExpenseId==DTO.ExpenseId);
            if (expenseid == null) 
            {
                return NotFound($"the expenses id {DTO.ExpenseId}not exist"  );
            
            }

            var participant= await _appDBContext.Participants.SingleOrDefaultAsync(p=>p.ParticipantId==DTO.ParticipantId);
            if (participant == null)
            {
                return NotFound($"the participant id {DTO.ParticipantId}not found ");
            }

            var createSplit = new ExpenseSplit
            {
                ExpenseId = DTO.ExpenseId,
                Amount=DTO.Amount,
                ParticipantId = DTO.ParticipantId,
                PaidForParticipantId=DTO.PaidForParticipantId

            };
            _appDBContext.ExpenseSplits.Add(createSplit);

           
            await _appDBContext.SaveChangesAsync();
            return Ok(createSplit);

        }
     
        
    }
}
