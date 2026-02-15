using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalmaAI.Model
{
    public class ExpenseSplit
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseSplitId { get; set; }//
        public int Amount { get; set; }//المبلغ الذي تم دفعه من قبل هذا الشخص
        public int ExpenseId { get; set; }//FK يجيب رقم الفاتوره
        public Expense Expense { get; set; }//NV 
        
        public int PaidForParticipantId { get; set; }//عن مين دفع 
        public Participant PaidParticipant { get; set; }


        public int ParticipantId { get; set; }//FK ومين دفع 
        public Participant Participant { get; set; }
    }
}
