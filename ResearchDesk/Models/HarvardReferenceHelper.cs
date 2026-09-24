using System;
using System.Text;

namespace ResearchDesk.Models
{
 
    public static class HarvardReferenceHelper
    {
     
        public static string GetReference(AISource ai)
        {
            if (ai == null) return string.Empty;

            // Extract year from date used
            var year = ai.DateUsed.Year;

            // Tool name (e.g., "DeepSeek", "ChatGPT")
            var toolName = ai.ToolName ?? "AI Tool";

            // Question asked (truncate to 100 chars if too long)
            var question = Truncate(ai.QuestionAsked ?? "Query", 100);

            // Build the reference string
            var reference = $"{toolName} ({year}). \"{question}\".";

            return reference;
        }

        /// <summary>
        /// Generate Harvard reference from Academic Source
        /// Handles both journal articles and books
        /// 
        /// For Journal Articles:
        /// Authors (Year). 'Title of article'. Journal Name, Volume(Issue), pp. pages.
        /// Example: Mkhize, S. & Ndlovu, T. (2022). 'School transport safety in rural South Africa'. 
        /// South African Journal of Social Work, 38(2), pp. 45-67.
        /// 
        /// For Books:
        /// Authors (Year). Title of Book. Publisher.
        /// Example: Bacchi, C. (2012). Analysing policy: What's the problem represented to be? Pearson.
        /// </summary>
        public static string GetReference(AcademicSource src)
        {
            if (src == null) return string.Empty;

            var reference = new StringBuilder();

            // Authors
            if (!string.IsNullOrEmpty(src.Authors))
                reference.Append($"{src.Authors} ");

            // Year
            reference.Append($"({src.Year}). ");

            // Title
            if (!string.IsNullOrEmpty(src.Title))
            {
                // If it's a journal article (has journal name), put title in single quotes
                if (!string.IsNullOrEmpty(src.JournalName))
                    reference.Append($"'{src.Title}'. ");
                else
                    reference.Append($"{src.Title}. ");
            }

            // Journal Name (if exists - means it's a journal article)
            if (!string.IsNullOrEmpty(src.JournalName))
            {
                reference.Append($"{src.JournalName}");

                // Volume
                if (src.Volume > 0)
                {
                    reference.Append($", {src.Volume}");

                    // Issue (if provided)
                    if (src.Issue.HasValue && src.Issue.Value != 0)
                        reference.Append($"({src.Issue.Value})");
                }

                // Pages
                if (!string.IsNullOrEmpty(src.PageRange))
                    reference.Append($", pp. {src.PageRange}");

                reference.Append(".");
            }
            else if (!string.IsNullOrEmpty(src.Publisher))
            {
                // Book format
                reference.Append($"{src.Publisher}.");
            }

            // URL (if available)
            if (!string.IsNullOrEmpty(src.Url))
                reference.Append($" Available at: {src.Url}");

            return reference.ToString();
        }

        /// <summary>
        /// Generate Harvard reference from Framework
        /// Format: Framework Name. Description. Source Citation.
        /// 
        /// Example:
        /// Bacchi's WPR Framework (What's the Problem Represented to be?). 
        /// A methodology for analyzing how policy issues are conceptualized. 
        /// Bacchi, C. (2012). Analysing policy: What's the problem represented to be? Pearson.
        /// </summary>
        public static string GetReference(Framework framework)
        {
            if (framework == null) return string.Empty;

            var reference = new StringBuilder();

            // Framework name
            if (!string.IsNullOrEmpty(framework.Name))
                reference.Append($"{framework.Name}. ");

            // Description
            if (!string.IsNullOrEmpty(framework.Description))
                reference.Append($"{framework.Description}. ");

            // Source citation (how to cite this framework)
            if (!string.IsNullOrEmpty(framework.SourceCitation))
                reference.Append(framework.SourceCitation);

            return reference.ToString();
        }

        /// <summary>
        /// Helper: Truncate string to max length with "..." if needed
        /// Example: "This is a very long question..." (if > maxLength)
        /// </summary>
        private static string Truncate(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
        }
    }
}