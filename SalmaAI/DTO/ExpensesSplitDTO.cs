using SalmaAI.Model;

namespace SalmaAI.DTO
{
    public class ExpensesSplitDTO
    {
        public int Amount { get; set; }
        public int ExpenseId { get; set; }//FK
 

        public int PaidForParticipantId { get; set; }


        public int ParticipantId { get; set; }//FK
    }
}
