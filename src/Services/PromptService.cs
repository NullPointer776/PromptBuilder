using System;
using System.Collections.Generic;
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
        /// Get built-in templates for a field, keyed by template name.
        /// </summary>
        public IReadOnlyDictionary<string, string> GetTemplates(string fieldName)
        {
            return fieldName switch
            {
                "Task" => new Dictionary<string, string>
                {
                    ["Summarize a document"] = "**Summarize** the following {{document_type}} in **no more than {{max_words}} words**, focusing on {{key_topic}}.",
                    ["Write code"] = "**Write** a {{language}} function that {{function_purpose}}, handling {{edge_cases}}.",
                    ["Explain a concept"] = "**Explain** {{concept}} to a {{audience_level}} audience, using {{analogy_or_example}}.",
                    ["Translate text"] = "**Translate** the following text from {{source_language}} to {{target_language}}, preserving {{tone_or_style}}.",
                    ["Review & improve"] = "**Review** the following {{artifact_type}} and suggest improvements for {{quality_goal}}."
                },
                "Context" => new Dictionary<string, string>
                {
                    ["Product team audience"] = "The audience is a non-technical product team. The document is a 2-page meeting notes about upcoming feature decisions. Prior attempts: a terse bullet list; needs more explanation.",
                    ["Technical audience"] = "The audience is experienced {{role}} engineers. They are familiar with {{domain}} but new to {{specific_technology}}.",
                    ["Executive summary"] = "The audience is senior leadership who have limited time. They care about {{business_impact}} and {{key_decision}}.",
                    ["Customer-facing"] = "The audience is existing customers with mixed technical backgrounds. Tone should be {{tone}} and avoid internal jargon.",
                    ["Academic / research"] = "The audience is researchers in {{field}}. Assume familiarity with {{background_theory}} but not with {{specific_method}}."
                },
                "OutputFormat" => new Dictionary<string, string>
                {
                    ["Bullet list"] = "Bullet list with 5 bullets, each 1-2 sentences. Include a 2-sentence summary at the top.",
                    ["Step-by-step guide"] = "Numbered steps, each with a short imperative title followed by 1-2 sentences of detail.",
                    ["Table"] = "A markdown table with columns: {{column_1}}, {{column_2}}, {{column_3}}. Keep cells under 15 words.",
                    ["JSON"] = "Valid JSON only, no prose. Use keys: {{key_1}}, {{key_2}}, {{key_3}}.",
                    ["Code + explanation"] = "A single code block, followed by a short paragraph explaining the key decisions."
                },
                "Constraints" => new Dictionary<string, string>
                {
                    ["Plain English"] = "Use plain English, avoid jargon. Do not reference internal project names. Maximum 300 words.",
                    ["Strict length"] = "Respond in no more than {{word_count}} words. Do not add greetings or sign-offs.",
                    ["No speculation"] = "Only use the information provided. If something is unknown, say so explicitly instead of guessing.",
                    ["Formatting rules"] = "Do not use emojis or exclamation marks. Use headings only at the top level.",
                    ["Compliance"] = "Do not include any personal data, credentials, or confidential identifiers."
                },
                _ => new Dictionary<string, string>()
            };
        }

        /// <summary>
        /// Get built-in example for a field (first template).
        /// </summary>
        public string GetExample(string fieldName)
        {
            foreach (var template in GetTemplates(fieldName).Values)
            {
                return template;
            }
            return string.Empty;
        }
    }
}
