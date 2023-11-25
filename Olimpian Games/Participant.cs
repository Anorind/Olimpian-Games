using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olimpian_Games
{
    public class Participant
    {
        public int ParticipantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public int SportId { get; set; }
        public Sport Sport { get; set; }
        public string? Medal { get; set; }
        public int OlympiadId { get; set; } 
        public Olympiada Olympiad { get; set; } 

    }
}
