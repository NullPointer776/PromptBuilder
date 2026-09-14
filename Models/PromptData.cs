using System;
using System.Collections.Generic;

namespace PromptStructTool.Models
{
    /// <summary>
    /// Data model for a prompt project
    /// </summary>
    public class PromptData
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        
        // Original fields
        public string Task { get; set; } = string.Empty;
        public string Context { get; set; } = string.Empty;
        public string OutputFormat { get; set; } = string.Empty;
        public string Constraints { get; set; } = string.Empty;
        
        // Generated prompt
        public string DraftPrompt { get; set; } = string.Empty;
        
        // Sensitive information check
        public string SanitizedPrompt { get; set; } = string.Empty;
        public List<SensitiveItem> DetectedItems { get; set; } = new();
        public Dictionary<string, string> ReplacementMap { get; set; } = new();
        
        /// <summary>
        /// Creates a copy of this object for undo/redo support
        /// </summary>
        public PromptData Clone()
        {
            return new PromptData
            {
                Id = this.Id,
                CreatedAt = this.CreatedAt,
                ModifiedAt = DateTime.Now,
                Task = this.Task,
                Context = this.Context,
                OutputFormat = this.OutputFormat,
                Constraints = this.Constraints,
                DraftPrompt = this.DraftPrompt,
                SanitizedPrompt = this.SanitizedPrompt,
                DetectedItems = new List<SensitiveItem>(this.DetectedItems),
                ReplacementMap = new Dictionary<string, string>(this.ReplacementMap)
            };
        }
    }

    /// <summary>
    /// Represents a detected sensitive item
    /// </summary>
    public class SensitiveItem
    {
        public string Category { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int MatchCount { get; set; } = 1;
        public List<int> Positions { get; set; } = new(); // Positions in text where this item appears
    }
}
