using Microsoft.EntityFrameworkCore;
using ResearchDesk.Models;

namespace ResearchDesk
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); // this is a builder that will be configuring the web app

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ResearchDeskDbContext>(options =>
                options.UseSqlite($"Data Source={Path.Combine(AppContext.BaseDirectory, "ResearchDesk.db")}"));

            var app = builder.Build();

            // if there is no database, then its created. Sample info to show the user how everything would look like
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ResearchDeskDbContext>();

                dbContext.Database.EnsureCreated();

                var seedMarkerPath = Path.Combine(AppContext.BaseDirectory, ".seeded");
                if (!File.Exists(seedMarkerPath))
                {
                    if (!dbContext.Assignments.Any())
                    {
                        SeedData(dbContext);
                    }
                    File.Create(seedMarkerPath).Dispose();
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");

            app.Run();
        }

        // SEED DATA
        private static void SeedData(ResearchDeskDbContext dbContext)
        {
            // Create sample frameworks
            var frameworks = new[]
            {
                new Framework
                {
                    Name = "Taylor's Framework (Taylor & Triegaardt, 2018)",
                    Description = "Stages: problem identification, policy formulation, implementation, evaluation.",
                    SourceCitation = "Taylor, J. & Triegaardt, P. (2018). Social policy analysis. Oxford University Press."
                },
                new Framework
                {
                    Name = "Bacchi's WPR Approach",
                    Description = "What's the Problem Represented to be? – deconstructs policy assumptions.",
                    SourceCitation = "Bacchi, C. (2009). Analysing policy: What's the problem represented to be? Pearson."
                },
                new Framework
                {
                    Name = "Kingdon's Multiple Streams",
                    Description = "Problem, policy, politics streams – windows of opportunity.",
                    SourceCitation = "Kingdon, J.W. (2014). Agendas, alternatives, and public policies. Pearson."
                }
            };
            dbContext.Frameworks.AddRange(frameworks);

            // Create sample assignment
            var assignment1 = new Assignment
            {
                Title = "Assignment 01 – Social Welfare Policy Analysis",
                DueDate = new DateTime(2026, 4, 28),
                UniqueNumber = "68868",
                TotalMarks = 100,
                EssayPrompt = "Write an essay of approximately 8-10 pages. Use AI sources, prescribed book, and two peer-reviewed articles. Analyse safe school transport policy using a selected framework.",
                IsCompleted = false
            };
            dbContext.Assignments.Add(assignment1);
            dbContext.SaveChanges();  // saved so that we get the ID (primary key) for each assignment

            // Add AI sources linked to assignment1
            dbContext.AISources.AddRange(
                new AISource
                {
                    ToolName = "DeepSeek",
                    QuestionAsked = "Visually present and discuss two alternative frameworks for social welfare policy analysis, plus Taylor's framework.",
                    AIResponse = "1. Bacchi's WPR: asks 'What's the problem represented to be?' – useful for deconstructing policy assumptions. 2. Kingdon's Multiple Streams: problem, policy, politics streams. 3. Taylor's framework: problem ID, formulation, implementation, evaluation.",
                    DateUsed = new DateTime(2026, 4, 10),
                    AssignmentId = assignment1.Id
                },
                new AISource
                {
                    ToolName = "ChatGPT",
                    QuestionAsked = "Apply all stages of Taylor's framework to the safe school transport issue (minibus taxi crash, 19 Jan 2026).",
                    AIResponse = "Stage 1 – Problem identification: lack of licensed transport. Stage 2 – Policy formulation: propose national learner transport subsidy, mandatory permits for taxis. Stage 3 – Implementation: phased rollout starting in high-risk areas. Stage 4 – Evaluation: monitor accident rates after 12 months.",
                    DateUsed = new DateTime(2026, 4, 15),
                    AssignmentId = assignment1.Id
                });

            // Add academic sources
            dbContext.AcademicSources.AddRange(
                new AcademicSource
                {
                    Authors = "Mkhize, S. & Ndlovu, T.",
                    Year = 2022,
                    Title = "School transport safety in rural South Africa: A policy gap analysis",
                    JournalName = "South African Journal of Social Work",
                    Volume = 38,
                    Issue = 2,
                    PageRange = "45-67",
                    AssignmentId = assignment1.Id
                },
                new AcademicSource
                {
                    Authors = "Van der Berg, R.",
                    Year = 2021,
                    Title = "Policy implementation challenges in learner transport",
                    Publisher = "HSRC Press",
                    AssignmentId = assignment1.Id
                });

            // Add notes
            dbContext.Notes.AddRange(
                new Note
                {
                    Content = "Minibus taxi crash near Vanderbijlpark (19 Jan 2026) killed 14 children. Current policy only covers distances >5km. Expand to all learners, enforce permits.",
                    CreatedAt = new DateTime(2026, 4, 18),
                    AssignmentId = assignment1.Id
                },
                new Note
                {
                    Content = "AI's response on Taylor's framework was generic. Added my own analysis of DoE 2025 report. Prescribed book emphasises stakeholder participation.",
                    CreatedAt = new DateTime(2026, 4, 20),
                    AssignmentId = assignment1.Id
                });

            // Add second assignment
            var assignment2 = new Assignment
            {
                Title = "Assignment 02 – Community Development Project",
                DueDate = new DateTime(2026, 6, 15),
                UniqueNumber = "782134",
                TotalMarks = 80,
                EssayPrompt = "Design a community intervention for youth unemployment. Use participatory frameworks.",
                IsCompleted = false
            };
            dbContext.Assignments.Add(assignment2);
            dbContext.SaveChanges();

            dbContext.Notes.Add(new Note
            {
                Content = "Look up Asset-Based Community Development framework.",
                CreatedAt = DateTime.Now,
                AssignmentId = assignment2.Id
            });

            // Save all changes to database
            dbContext.SaveChanges();
        }
    }
}