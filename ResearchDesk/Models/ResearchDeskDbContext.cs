using Microsoft.EntityFrameworkCore;

namespace ResearchDesk.Models
{

    public class ResearchDeskDbContext : DbContext
    {
        // Constructor - receives database options from Program.cs
        public ResearchDeskDbContext(DbContextOptions<ResearchDeskDbContext> options)
            : base(options) { }

       //creates tables in the databse for each property 
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Framework> Frameworks { get; set; }
        public DbSet<AISource> AISources { get; set; }
        public DbSet<AcademicSource> AcademicSources { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}