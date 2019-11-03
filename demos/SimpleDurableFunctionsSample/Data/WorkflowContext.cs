using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleDurableFunctionsSample.Configuration;
using SimpleDurableFunctionsSample.Data.Entities;

namespace SimpleDurableFunctionsSample.Data
{
    public class WorkflowContext : DbContext
    {
        private readonly WorkflowOptions _options;

        public WorkflowContext(IOptions<WorkflowOptions> options)
        {
            _options = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(_options.DbConnectionString);

        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workflow>(entity =>
            {
                entity.ToTable("Workflow");
                entity.HasKey(m => m.Id);
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.ToTable("WorkflowParticipant");
                entity.HasKey(m => m.Id);
                entity.HasOne(m => m.Workflow)
                    .WithMany(m => m.Participants)
                    .HasForeignKey(m => m.WorkflowId)
                    .IsRequired();
            });
        }
    }
}