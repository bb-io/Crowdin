using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.Crowdin.Utils
{
    public static class FileOperationWrapper
    {
        public static void ValidateFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new PluginMisconfigurationException("File name cannot be null or empty.");
            }

            if (fileName.Contains("\n") || fileName.Contains("\r"))
            {
                throw new PluginMisconfigurationException("File name must not contain new-line characters. Please check the input");
            }

            var invalidCharacters = new[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

            if (fileName.IndexOfAny(invalidCharacters) != -1)
            {
                throw new PluginMisconfigurationException(
                    $"File name '{fileName}' contains invalid characters. " +
                    "It cannot include any of the following: \\ / : * ? \" < > |"
                );
            }
        }

        public static bool IsOnlyAscii(string input)
        {
            return input.All(c => c <= 127);
        }

        public static async Task<T> ExecuteFileOperation<T>(Func<Task<T>> operation, Stream fileStream, string fileName = "File")
        {
            if (fileStream == null)
            {
                throw new PluginMisconfigurationException($"{fileName} is null. Please check the input file");
            }
            if (fileStream.Length == 0)
            {
                throw new PluginMisconfigurationException($"{fileName} is empty. Please check the input file");
            }

            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                throw new PluginApplicationException($"Error executing file operation for {fileName}: {ex.Message}", ex);
            }
        }

        public static async Task ExecuteFileOperation(Func<Task> operation, Stream fileStream, string fileName = "File")
        {
            if (fileStream == null)
            {
                throw new PluginMisconfigurationException($"{fileName} is null. Please check the input file");
            }
            if (fileStream.Length == 0)
            {
                throw new PluginMisconfigurationException($"{fileName} is empty. Please check the input file");
            }

            try
            {
                await operation();
            }
            catch (Exception ex)
            {
                throw new PluginApplicationException($"Error executing file operation for {fileName}: {ex.Message}", ex);
            }
        }

        public static async Task<Stream> ExecuteFileDownloadOperation(Func<Task<Stream>> operation, string fileName = "File")
        {
            Stream fileStream;
            try
            {
                fileStream = await operation();
            }
            catch (Exception ex)
            {
                throw new PluginApplicationException($"Error downloading {fileName}: {ex.Message}", ex);
            }
            
            return fileStream;
        }
    }
}
