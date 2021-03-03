
using Microsoft.EntityFrameworkCore;
using VotingSystem.API.Models;

namespace VotingSystem.API.DataAccess
{
    public class VotingContext : DbContext
    {
        public VotingContext(DbContextOptions<VotingContext> options) : base(options)
        { }

        public DbSet<SystemUser> Users { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Election> Elections { get; set; }
    }
}