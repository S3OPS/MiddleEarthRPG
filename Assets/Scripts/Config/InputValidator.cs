using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MiddleEarth.Config
{
    /// <summary>
    /// Input validation utilities for security.
    /// Implements the "Inspect the Ranks" principle - auditing for hidden flaws.
    /// </summary>
    public static class InputValidator
    {
        /// <summary>
        /// Validates a string is not null, empty, or contains invalid characters.
        /// </summary>
        /// <param name="input">The string to validate.</param>
        /// <param name="maxLength">Maximum allowed length.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidString(string input, int maxLength = 256)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            
            if (input.Length > maxLength)
                return false;
            
            // Check for potentially dangerous characters
            foreach (char c in input)
            {
                if (char.IsControl(c) && c != '\n' && c != '\r' && c != '\t')
                    return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Clamps a float value within safe bounds.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>The clamped value.</returns>
        public static float ClampFloat(float value, float min, float max)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
                return min;
            
            return Mathf.Clamp(value, min, max);
        }
        
        /// <summary>
        /// Clamps an integer value within safe bounds.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>The clamped value.</returns>
        public static int ClampInt(int value, int min, int max)
        {
            return Mathf.Clamp(value, min, max);
        }
        
        /// <summary>
        /// Validates JSON data size is within acceptable limits.
        /// </summary>
        /// <param name="jsonData">The JSON string to validate.</param>
        /// <returns>True if the data is within limits.</returns>
        public static bool IsValidJsonSize(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData))
                return false;
            
            // Check byte size
            int byteSize = Encoding.UTF8.GetByteCount(jsonData);
            return byteSize <= GameConstants.MAX_SAVE_FILE_SIZE;
        }
        
        /// <summary>
        /// Generates a checksum for save file integrity verification.
        /// </summary>
        /// <param name="data">The data to generate a checksum for.</param>
        /// <returns>The checksum string.</returns>
        public static string GenerateChecksum(string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;
            
            using (SHA256 sha256 = SHA256.Create())
            {
                string saltedData = data + GameConstants.CHECKSUM_SALT;
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedData));
                
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        
        /// <summary>
        /// Verifies a checksum matches the data.
        /// </summary>
        /// <param name="data">The data to verify.</param>
        /// <param name="expectedChecksum">The expected checksum.</param>
        /// <returns>True if the checksum matches.</returns>
        public static bool VerifyChecksum(string data, string expectedChecksum)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(expectedChecksum))
                return false;
            
            string actualChecksum = GenerateChecksum(data);
            return string.Equals(actualChecksum, expectedChecksum, StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// Safely parses a float from a string with a fallback value.
        /// </summary>
        /// <param name="input">The string to parse.</param>
        /// <param name="fallback">The fallback value if parsing fails.</param>
        /// <returns>The parsed value or fallback.</returns>
        public static float SafeParseFloat(string input, float fallback)
        {
            if (float.TryParse(input, out float result))
            {
                if (float.IsNaN(result) || float.IsInfinity(result))
                    return fallback;
                return result;
            }
            return fallback;
        }
        
        /// <summary>
        /// Safely parses an integer from a string with a fallback value.
        /// </summary>
        /// <param name="input">The string to parse.</param>
        /// <param name="fallback">The fallback value if parsing fails.</param>
        /// <returns>The parsed value or fallback.</returns>
        public static int SafeParseInt(string input, int fallback)
        {
            if (int.TryParse(input, out int result))
            {
                return result;
            }
            return fallback;
        }
    }
}
