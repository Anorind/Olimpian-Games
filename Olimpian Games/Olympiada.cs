using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Olimpian_Games
{
    public class Olympiada
    {
        [Key]
        public int OlympiadId { get; set; }
        public int Year { get; set; }
        public string Season { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int ParticipantsCount { get; set; }
        public ICollection<Participant> Participants { get; set; }

    }
}
