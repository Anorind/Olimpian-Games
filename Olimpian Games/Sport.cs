using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olimpian_Games
{
    public class Sport
    {
        public int SportId { get; set; }
        public string Name { get; set; } 
        public int ParticipantsCount { get; set; } 
        public ICollection<Participant> Participants { get; set; }
    }
}
