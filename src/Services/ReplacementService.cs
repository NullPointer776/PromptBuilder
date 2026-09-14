using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PromptStructTool.Models;

namespace PromptStructTool.Services
{
    /// <summary>
    /// Service for managing sensitive item replacements
    /// </summary>
    public class ReplacementService
    {
        private readonly List<string> namePool = new() { "Alice", "Bob", "Charlie", "Dana", "Eve", "Frank", "Grace", "Henry" };
        private int namePoolIndex = 0;
        private int emailCounter = 1;
        private int ipLastOctet = 123;

        /// <summary>
        /// Apply all replacements to the given text based on the replacement map
        /// </summary>
        public string ApplyReplacements(string text, Dictionary<string, string> replacementMap)
        {
            if (string.IsNullOrEmpty(text) || replacementMap.Count == 0)
                return text;

            string result = text;
            foreach (var kv in replacementMap)
            {
                result = Regex.Replace(result, Regex.Escape(kv.Key), kv.Value);
            }

            return result;
        }

        /// <summary>
        /// Generate a replacement value based on category
        /// </summary>
        public string SuggestReplacement(string category, string originalValue)
        {
            return category switch
            {
                "Name" => GetNextName(),
                "Email" => GetNextEmail(),
                "Phone" => "021 123 4567",
                "Bank Account" => "12-3456-7890123-00",
                "IP Address" => GetNextFakeIP(),
                "Password" => "••••••••",
                "API Key" => "sk-test-XXXXXXXXXXXXXXXXXXXX",
                "Address" => "123 Queen Street, Auckland 1010, New Zealand",
                _ => "[REDACTED]",
            };
        }

        /// <summary>
        /// Generate replacements for all detected items
        /// </summary>
        public Dictionary<string, string> GenerateReplacements(List<SensitiveItem> detectedItems)
        {
            var map = new Dictionary<string, string>();
            var processed = new HashSet<string>();

            foreach (var item in detectedItems)
            {
                if (!processed.Contains(item.Value))
                {
                    map[item.Value] = SuggestReplacement(item.Category, item.Value);
                    processed.Add(item.Value);
                }
            }

            return map;
        }

        /// <summary>
        /// Reset the replacement service for a new session
        /// </summary>
        public void Reset()
        {
            namePoolIndex = 0;
            emailCounter = 1;
            ipLastOctet = 123;
        }

        private string GetNextName()
        {
            string name = namePool[namePoolIndex % namePool.Count];
            namePoolIndex++;
            return name;
        }

        private string GetNextEmail()
        {
            string email = $"test{emailCounter}@example.com";
            emailCounter++;
            return email;
        }

        private string GetNextFakeIP()
        {
            ipLastOctet++;
            if (ipLastOctet > 254) ipLastOctet = 1;
            return $"192.168.1.{ipLastOctet}";
        }
    }
}
