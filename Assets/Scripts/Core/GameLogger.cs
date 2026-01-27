using UnityEngine;
using System;
using System.Text;

namespace MiddleEarth.Core
{
    /// <summary>
    /// Enhanced logging system with categories and levels.
    /// Provides better debugging and monitoring capabilities.
    /// </summary>
    public static class GameLogger
    {
        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
            Critical
        }
        
        public enum LogCategory
        {
            General,
            Combat,
            Quest,
            Achievement,
            Audio,
            Effects,
            Performance,
            Save,
            Network
        }
        
        private static LogLevel _minimumLogLevel = LogLevel.Debug;
        private static bool _enableTimestamps = true;
        private static bool _enableCategories = true;
        private static StringBuilder _logBuilder = new StringBuilder(256);
        
        /// <summary>
        /// Set the minimum log level to display.
        /// </summary>
        public static void SetMinimumLogLevel(LogLevel level)
        {
            _minimumLogLevel = level;
        }
        
        /// <summary>
        /// Enable or disable timestamps in log messages.
        /// </summary>
        public static void EnableTimestamps(bool enable)
        {
            _enableTimestamps = enable;
        }
        
        /// <summary>
        /// Enable or disable category tags in log messages.
        /// </summary>
        public static void EnableCategories(bool enable)
        {
            _enableCategories = enable;
        }
        
        /// <summary>
        /// Log a debug message.
        /// </summary>
        public static void Debug(string message, LogCategory category = LogCategory.General)
        {
            Log(message, LogLevel.Debug, category);
        }
        
        /// <summary>
        /// Log an info message.
        /// </summary>
        public static void Info(string message, LogCategory category = LogCategory.General)
        {
            Log(message, LogLevel.Info, category);
        }
        
        /// <summary>
        /// Log a warning message.
        /// </summary>
        public static void Warning(string message, LogCategory category = LogCategory.General)
        {
            Log(message, LogLevel.Warning, category);
        }
        
        /// <summary>
        /// Log an error message.
        /// </summary>
        public static void Error(string message, LogCategory category = LogCategory.General)
        {
            Log(message, LogLevel.Error, category);
        }
        
        /// <summary>
        /// Log a critical error message.
        /// </summary>
        public static void Critical(string message, LogCategory category = LogCategory.General)
        {
            Log(message, LogLevel.Critical, category);
        }
        
        /// <summary>
        /// Log a message with exception details.
        /// </summary>
        public static void LogException(Exception exception, LogCategory category = LogCategory.General)
        {
            if (exception == null) return;
            
            _logBuilder.Clear();
            _logBuilder.Append("EXCEPTION: ");
            _logBuilder.Append(exception.Message);
            _logBuilder.AppendLine();
            _logBuilder.Append("Stack Trace: ");
            _logBuilder.Append(exception.StackTrace);
            
            Log(_logBuilder.ToString(), LogLevel.Critical, category);
        }
        
        private static void Log(string message, LogLevel level, LogCategory category)
        {
            if (level < _minimumLogLevel) return;
            
            _logBuilder.Clear();
            
            // Add timestamp
            if (_enableTimestamps)
            {
                _logBuilder.Append("[");
                _logBuilder.Append(DateTime.Now.ToString("HH:mm:ss"));
                _logBuilder.Append("] ");
            }
            
            // Add category
            if (_enableCategories)
            {
                _logBuilder.Append("[");
                _logBuilder.Append(category.ToString());
                _logBuilder.Append("] ");
            }
            
            // Add message
            _logBuilder.Append(message);
            
            string formattedMessage = _logBuilder.ToString();
            
            // Output based on level
            switch (level)
            {
                case LogLevel.Debug:
                case LogLevel.Info:
                    UnityEngine.Debug.Log(formattedMessage);
                    break;
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(formattedMessage);
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    UnityEngine.Debug.LogError(formattedMessage);
                    break;
            }
        }
        
        /// <summary>
        /// Log performance metrics.
        /// </summary>
        public static void LogPerformance(string operation, float milliseconds)
        {
            _logBuilder.Clear();
            _logBuilder.Append(operation);
            _logBuilder.Append(" took ");
            _logBuilder.Append(milliseconds.ToString("F2"));
            _logBuilder.Append("ms");
            Log(_logBuilder.ToString(), LogLevel.Debug, LogCategory.Performance);
        }
        
        /// <summary>
        /// Create a scoped performance logger that logs on dispose.
        /// Usage: using (var timer = GameLogger.MeasurePerformance("MyOperation")) { ... }
        /// </summary>
        public static PerformanceScope MeasurePerformance(string operation)
        {
            return new PerformanceScope(operation);
        }
    }
    
    /// <summary>
    /// Disposable performance measurement scope.
    /// </summary>
    public struct PerformanceScope : IDisposable
    {
        private string _operation;
        private float _startTime;
        
        public PerformanceScope(string operation)
        {
            _operation = operation;
            _startTime = Time.realtimeSinceStartup;
        }
        
        public void Dispose()
        {
            float elapsed = (Time.realtimeSinceStartup - _startTime) * 1000f;
            GameLogger.LogPerformance(_operation, elapsed);
        }
    }
}
