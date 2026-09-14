using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PromptStructTool.Models;

namespace PromptStructTool.Services
{
    /// <summary>
    /// Service for file I/O operations
    /// </summary>
    public class FileService
    {
        private readonly JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <summary>
        /// Save a prompt project to a JSON file
        /// </summary>
        public async Task<bool> SaveAsync(PromptData data, string filePath)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, jsonOptions);
                await File.WriteAllTextAsync(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Load a prompt project from a JSON file
        /// </summary>
        public async Task<PromptData> LoadAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"File not found: {filePath}");

                var json = await File.ReadAllTextAsync(filePath);
                var data = JsonSerializer.Deserialize<PromptData>(json, jsonOptions)
                    ?? throw new InvalidOperationException("Failed to deserialize JSON");

                return data;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Export the final prompt as plain text
        /// </summary>
        public async Task<bool> ExportAsTextAsync(string content, string filePath)
        {
            try
            {
                await File.WriteAllTextAsync(filePath, content);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to export file: {ex.Message}", ex);
            }
        }
    }
}
