using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SalmaAI.Model
{
    public class Expense
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseId { get; set; }

        [Required]
        public int Amount { get; set; }
        [Required]
        public int PaidById { get; set; }

        
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
