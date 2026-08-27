using System.ComponentModel.DataAnnotations;

namespace ResearchDesk.Models
{
    public class Framework
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? VisualReference { get; set; } 

        public string? SourceCitation { get; set; }
    }
}