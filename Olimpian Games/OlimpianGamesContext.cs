using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olimpian_Games
{
    internal class OlimpianGamesContext : DbContext
    {
        public DbSet<Olympiada> Olympiads { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Participant> Participants { get; set; }


    }
}
