using System.ComponentModel.DataAnnotations;

namespace SalmaAI.Models
{
    public class ExpenseDTO
    {
        [Required]
        public int Amount { get; set; }
        [Required]
        public int PaidById { get; set; }




    
    }
}
