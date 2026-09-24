using System.ComponentModel.DataAnnotations;

namespace ResearchDesk.Models
{
    public class Assignment
    {
        // Primary key (database identifier )
        public int Id { get; set; }

      
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }

        // Optional (can be null)
        public string? UniqueNumber { get; set; }

        public int TotalMarks { get; set; }

        public string? EssayPrompt { get; set; }

        public bool IsCompleted { get; set; }

        // RELATIONSHIPS (one assignment can have many notes and sources )
        public List<Note> Notes { get; set; } = new();
        public List<AISource> AISources { get; set; } = new();
        public List<AcademicSource> AcademicSources { get; set; } = new();
    }
}