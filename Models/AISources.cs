using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchDesk.Models
{
    public class AISource
    {
        public int Id { get; set; }

        [Required]
        public string ToolName { get; set; } = string.Empty;  

        [Required]
        public string QuestionAsked { get; set; } = string.Empty;

        [Required]
        public string AIResponse { get; set; } = string.Empty;

        public DateTime DateUsed { get; set; } = DateTime.Now;

  
        public int AssignmentId { get; set; }       // FOREIGN KEY - links to which assignment this AI log belongs to

        public Assignment? Assignment { get; set; }  // goes back to the assignment mentioned


        [NotMapped]
        public string SnapshotText => $"Q: {QuestionAsked}\nA: {AIResponse}\nDate: {DateUsed}"; // displaying the AI log
    }
}