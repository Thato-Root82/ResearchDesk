using System.ComponentModel.DataAnnotations;

namespace ResearchDesk.Models
{
    public class AcademicSource
    {
        public int Id { get; set; }

        [Required]
        public string Authors { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        // For books
        public string? Publisher { get; set; }

        // For journal articles
        public string? JournalName { get; set; }
        public int? Volume { get; set; }
        public int? Issue { get; set; }
        public string? PageRange { get; set; }

        // Optional URL to the source
        public string? Url { get; set; }

   
        public int AssignmentId { get; set; }

      
        public Assignment? Assignment { get; set; }

        
        // This is called in the View to display the formatted reference
        public string GetHarvardReference()
        {
            // If it's a journal article
            if (!string.IsNullOrEmpty(JournalName))
                return $"{Authors} ({Year}). {Title}. *{JournalName}*, {Volume}({Issue}), pp.{PageRange}.";

            // If it's a book
            else
                return $"{Authors} ({Year}). *{Title}*. {Publisher}.";
        }
    }
}