using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PromptStructTool.Models;

namespace PromptStructTool.Services
{
    /// <summary>
    /// Service for detecting sensitive information in text
    /// </summary>
    public class SensitiveDetectionService
    {
        private static readonly string[] CommonNames = { "John", "Mary", "Alice", "Bob", "Charlie", "Dana", "Eve", "Frank" };

        /// <summary>
        /// Detect all sensitive items in the given text
        /// </summary>
        public List<SensitiveItem> Detect(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<SensitiveItem>();

            var results = new List<SensitiveItem>();

            // Email detection
            DetectEmails(text, results);

            // IPv4 detection
            DetectIPAddresses(text, results);

            // Phone numbers
            DetectPhoneNumbers(text, results);

            // API Keys / Tokens
            DetectAPIKeys(text, results);

            // Bank accounts / card numbers
            DetectBankAccounts(text, results);

            // Passwords
            DetectPasswords(text, results);

            // Personal names
            DetectNames(text, results);

            // Remove duplicates and update match counts
            var deduplicated = DeduplicateResults(results);

            return deduplicated.OrderBy(x => x.Category).ThenBy(x => x.Value).ToList();
        }

        private void DetectEmails(string text, List<SensitiveItem> results)
        {
            var pattern = @"\b[\w.-]+@[\w.-]+\.[A-Za-z]{2,6}\b";
            foreach (Match m in Regex.Matches(text, pattern))
            {
                results.Add(new SensitiveItem
                {
                    Category = "Email",
                    Value = m.Value
                });
            }
        }

        private void DetectIPAddresses(string text, List<SensitiveItem> results)
        {
            var pattern = @"\b(?:\d{1,3}\.){3}\d{1,3}\b";
            foreach (Match m in Regex.Matches(text, pattern))
            {
                results.Add(new SensitiveItem
                {
                    Category = "IP Address",
                    Value = m.Value
                });
            }
        }

        private void DetectPhoneNumbers(string text, List<SensitiveItem> results)
        {
            var pattern = @"\b(?:\+?\d{1,3}[ -]?)?(?:\(\d+\)[ -]?)?\d{2,4}[ -]?\d{3,4}[ -]?\d{3,4}\b";
            foreach (Match m in Regex.Matches(text, pattern))
            {
                if (m.Value.Length >= 7) // Crude filter to avoid matching years
                {
                    results.Add(new SensitiveItem
                    {
                        Category = "Phone",
                        Value = m.Value
                    });
                }
            }
        }

        private void DetectAPIKeys(string text, List<SensitiveItem> results)
        {
            var pattern = @"\b(?:sk-|key-)?[A-Za-z0-9\-_=]{20,}\b";
            foreach (Match m in Regex.Matches(text, pattern))
            {
                if (m.Value.Length >= 20)
                {
                    results.Add(new SensitiveItem
                    {
                        Category = "API Key",
                        Value = m.Value
                    });
                }
            }
        }

        private void DetectBankAccounts(string text, List<SensitiveItem> results)
        {
            var pattern = @"\b(?:\d[ -]?){12,19}\b";
            foreach (Match m in Regex.Matches(text, pattern))
            {
                string digitsOnly = Regex.Replace(m.Value, "[^0-9]", "");
                if (digitsOnly.Length >= 12 && digitsOnly.Length <= 19)
                {
                    results.Add(new SensitiveItem
                    {
                        Category = "Bank Account",
                        Value = m.Value
                    });
                }
            }
        }

        private void DetectPasswords(string text, List<SensitiveItem> results)
        {
            var pattern = @"(?i)(?:password|pwd|pass)\s*[:=]\s*(\S+)";
            foreach (Match m in Regex.Matches(text, pattern))
            {
                string val = m.Groups[1].Value;
                if (!string.IsNullOrEmpty(val))
                {
                    results.Add(new SensitiveItem
                    {
                        Category = "Password",
                        Value = val
                    });
                }
            }
        }

        private void DetectNames(string text, List<SensitiveItem> results)
        {
            // Pattern: "my name is X", "I am X", etc.
            var pattern = @"(?i)(?:my name is|i am|name is|name:)\s*([A-Z][a-z]{1,24})";
            foreach (Match m in Regex.Matches(text, pattern))
            {
                results.Add(new SensitiveItem
                {
                    Category = "Name",
                    Value = m.Groups[1].Value
                });
            }

            // Also check for common names
            foreach (string name in CommonNames)
            {
                var pattern2 = $@"\b{Regex.Escape(name)}\b";
                foreach (Match m in Regex.Matches(text, pattern2))
                {
                    results.Add(new SensitiveItem
                    {
                        Category = "Name",
                        Value = m.Value
                    });
                }
            }
        }

        private List<SensitiveItem> DeduplicateResults(List<SensitiveItem> items)
        {
            var dict = new Dictionary<string, SensitiveItem>();

            foreach (var item in items)
            {
                string key = $"{item.Category}:{item.Value}";
                if (dict.TryGetValue(key, out var existing))
                {
                    existing.MatchCount++;
                }
                else
                {
                    dict[key] = item;
                }
            }

            return dict.Values.ToList();
        }
    }
}
