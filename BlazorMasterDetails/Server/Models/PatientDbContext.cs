using BlazorMasterDetails.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Server.Models
{
    public class PatientDbContext:DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext>options) : base(options) { }

        public DbSet<Patient> Patients { get; set; } = default!;
        public DbSet<Test> Tests { get; set; } = default!;
        public DbSet<TestEntry> TestEntries { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>().HasData(
                    new Test { TestId = 1, TestName = "Serological Test" },
                    new Test { TestId = 2, TestName = "Blood Test" },
                    new Test { TestId = 3, TestName = "Pregnancy Test" },
                    new Test { TestId = 4, TestName = "Syphilis Test" },
                    new Test { TestId = 5, TestName = "Toxicology Test" },
                    new Test { TestId = 6, TestName = "Brain Scanning" }
                );
        }
    }
}
