using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SalmaAI.Model
{
    public class Participant
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; }

        public int TripId { get; set; } //foreign key 
        [JsonIgnore]
        public Trip Trip { get; set; }

       
        
        
    }
}
