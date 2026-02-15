using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalmaAI.Model
{
    public class Trip
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TripId { get; set; }
        public string TripName { get; set; }

        public ICollection<Participant> Participants { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
