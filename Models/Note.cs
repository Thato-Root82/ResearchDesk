using System.ComponentModel.DataAnnotations;

namespace ResearchDesk.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;  


        public int AssignmentId { get; set; }

   
        public Assignment? Assignment { get; set; }
    }
}