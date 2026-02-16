using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SalmaAI.Context;
using SalmaAI.DTO;
using SalmaAI.Model;
using SalmaAI.Repository;


namespace SalmaAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripRepository _tripRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly AppDBContext _appDBContext;


        public TripController( AppDBContext appDBContext,ITripRepository tripRepository,IParticipantRepository participantRepository)
        {
          
            _tripRepository = tripRepository;
            _participantRepository = participantRepository;
            _appDBContext = appDBContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(int id)
        {
            var getby = await _tripRepository.GetByIdAsync(id);
            return Ok(getby);
          
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TripDTO trip)
        {


            var createdto = new Trip
            {
                TripName = trip.TripName
            };
            var cdto=await _tripRepository.CreateAsync(createdto);
           
            return Ok(cdto);
        }

        [HttpGet("{tripId}/summary/")]
        public async Task<IActionResult> Summary(int tripId, int participantId)
        {
            //الرحله اذا موجوده
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip == null) return NotFound("Trip not found");
            //المشرتك اذا موجود
            var participant = await _participantRepository.GetByIdAsync(participantId);
                            
            if (participant == null) return NotFound("Participant not found");
            //عدد المشتركين في الرلحه 
            var Allparticipant = await _participantRepository.GetAllAsync();
            var contOfParticipant =  Allparticipant.Count(p => p.TripId == tripId);
            if (contOfParticipant == 0) return BadRequest("No participants in this trip");


            var totalPaid = await _appDBContext.Expenses
                                .Where(e => e.TripId == tripId)
                                .SumAsync(e => e.Amount);//مبلع الرحله كله 

            var splitByParticipant = totalPaid / contOfParticipant;//البلغ بالنسبه لكل شخص بالرحله  

            var paidValue = await _appDBContext.ExpenseSplits
                                 .Where(es => es.ParticipantId == participantId)
                                 .Select(es => es.Amount) // اختر العمود الذي تريد
                                 .SumAsync();             //حصه كل شخص 

            var totalsplitbyparticipant = paidValue - splitByParticipant;//




            return Ok(new
            {
                participant.ParticipantId,
                participant.ParticipantName,
                Paid = paidValue,
                Share = splitByParticipant,
                Balance = totalsplitbyparticipant
            });
        }

        [HttpGet("{tripId}/settlement")]
        public async Task<IActionResult> GetTripSettlement(int tripId)
        {
            //  تحقق من وجود الرحلة


            var trip = await _appDBContext.Trips.FirstOrDefaultAsync(t => t.TripId == tripId);
            if (trip == null) return NotFound("Trip not found");

            // تاكد من  المشاركين
            var participants = await _appDBContext.Participants
                .Where(p => p.TripId == tripId)
                .ToListAsync();
            if (!participants.Any()) return NotFound("No participants in this trip");

            int participantCount = participants.Count;

            //   مجموع المصاريف لكل مشترك
            var paidAmounts = await _appDBContext.ExpenseSplits
                    .Where(es => es.Expense.TripId == tripId)
                    .GroupBy(es => es.ParticipantId)
                    .Select(g => new
                    {
                        ParticipantId = g.Key,
                        Paid = g.Sum(x => x.Amount)
                    })
                         .ToListAsync();


            //  اجمع كل المصاريف وحسب نصيب كل شخص
            decimal totalExpenses = paidAmounts.Sum(x => x.Paid);
            decimal sharePerPerson = totalExpenses / participantCount;

            //  احسب الرصيد لكل مشترك
            var balances = participants.Select(p =>
            {
                var paid = paidAmounts.FirstOrDefault(x => x.ParticipantId == p.ParticipantId)?.Paid ?? 0;
                return new
                {
                    p.ParticipantId,
                    p.ParticipantName,
                    Paid = paid,
                    Share = sharePerPerson,
                    Balance = paid - sharePerPerson
                };
            }).ToList();

            //  قسم المشاركين إلى دائنين ومدينين
            var creditors = balances.Where(b => b.Balance > 0).OrderByDescending(b => b.Balance).ToList();
            var debtors = balances.Where(b => b.Balance < 0).OrderBy(b => b.Balance).ToList();

            
            var transfers = new List<object>();

            int i = 0, j = 0;
            while (i < debtors.Count && j < creditors.Count)
            {
                var debtor = debtors[i];
                var creditor = creditors[j];

                decimal amount = Math.Min(-debtor.Balance, creditor.Balance);

                if (amount > 0)
                {
                    transfers.Add(new
                    {
                        From = debtor.ParticipantName,
                        To = creditor.ParticipantName,
                        Amount = amount
                    });

                    debtor = new
                    {
                        debtor.ParticipantId,
                        debtor.ParticipantName,
                        debtor.Paid,
                        debtor.Share,
                        Balance = debtor.Balance + amount
                    };
                    creditor = new
                    {
                        creditor.ParticipantId,
                        creditor.ParticipantName,
                        creditor.Paid,
                        creditor.Share,
                        Balance = creditor.Balance - amount
                    };

                    debtors[i] = debtor;
                    creditors[j] = creditor;

                    if (debtor.Balance == 0) i++;
                    if (creditor.Balance == 0) j++;
                }
                else
                {
                    i++;
                }
            }

            return Ok(new
            {
                TotalExpenses = totalExpenses,
                SharePerPerson = sharePerPerson,
                Balances = balances,
                Transfers = transfers
            });
        }




    }
}

