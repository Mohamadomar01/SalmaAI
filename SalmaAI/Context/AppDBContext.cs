using Microsoft.EntityFrameworkCore;
using SalmaAI.Model;

namespace SalmaAI.Context
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public  DbSet<ExpenseSplit> ExpenseSplits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<ExpenseSplit>()
                .HasOne(es=>es.PaidParticipant)
                .WithMany()
                .HasForeignKey(es=>es.PaidForParticipantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExpenseSplit>()
                .HasOne(es => es.Participant)
                .WithMany()
                .HasForeignKey(es => es.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
