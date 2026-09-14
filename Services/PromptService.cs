using System;
using PromptStructTool.Models;

namespace PromptStructTool.Services
{
    /// <summary>
    /// Service for prompt construction and formatting
    /// </summary>
    public class PromptService
    {
        /// <summary>
        /// Assemble a prompt from the given components
        /// </summary>
        public string AssemblePrompt(string task, string context, string outputFormat, string constraints)
        {
            task = task.Trim();
            context = context.Trim();
            outputFormat = outputFormat.Trim();
            constraints = constraints.Trim();

            // Auto-bold the first verb in the task if not already bolded
            if (!task.StartsWith("**") && !string.IsNullOrEmpty(task))
            {
                string[] words = task.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (words.Length > 0)
                {
                    string firstWord = words[0];
                    task = task.Replace(firstWord, $"**{firstWord}**", StringComparison.Ordinal);
                }
            }

            string markdown = "## Instruction\r\n" + task + "\r\n\r\n" +
                              "## Context\r\n" + context + "\r\n\r\n" +
                              "## Output Format\r\n" + outputFormat + "\r\n\r\n" +
                              "## Constraints\r\n" + constraints + "\r\n";

            return markdown;
        }

        /// <summary>
        /// Get built-in example for a field
        /// </summary>
        public string GetExample(string fieldName)
        {
            return fieldName switch
            {
                "Task" => "**Summarize** the following {{document_type}} in **no more than {{max_words}} words**, focusing on {{key_topic}}.",
                "Context" => "The audience is a non-technical product team. The document is a 2-page meeting notes about upcoming feature decisions. Prior attempts: a terse bullet list; needs more explanation.",
                "OutputFormat" => "Bullet list with 5 bullets, each 1-2 sentences. Include a 2-sentence summary at the top.",
                "Constraints" => "Use plain English, avoid jargon. Do not reference internal project names. Maximum 300 words.",
                _ => string.Empty
            };
        }
    }
}
