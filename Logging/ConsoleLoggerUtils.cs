#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Elephant.UnityLibrary.Logging
{
	/// <summary>
	/// Log to console.
	/// </summary>
	public static class ConsoleLoggerUtils
	{
		/// <summary>
		/// If true, logging is possible; otherwise, all log calls will be ignored.
		/// </summary>
		/// <remarks>
		/// Logging may still be stopped depending on the <see cref="FilterLogType"/> value.
		/// Debug.unityLogger.logEnabled must also be enabled if you want to see it in the console.
		/// </remarks>
		public static bool LogEnabled { get; set; } = true;

		/// <summary>
		/// Determines the minimum (inclusive) <see cref="UnityEngine.LogType"/> that is required for a
		/// log to be logged. Logs that have a lower <see cref="UnityEngine.LogType"/> than this will
		/// not be logged.
		/// The order of importance is: <see cref="LogType.Log"/>, <see cref="LogType.Warning"/>,
		/// <see cref="LogType.Assert"/>, <see cref="LogType.Error"/>, <see cref="LogType.Exception"/>.
		/// </summary>
		/// <remarks>
		/// Debug.unityLogger.filterLogType must also have the proper value if you want to see it in the console.
		/// I recommend to leave Debug.unityLogger.filterLogType at <see cref="UnityEngine.LogType.Log"/>.
		/// </remarks>
		public static LogType FilterLogType { get; set; } = LogType.Log;

		/// <summary>
		/// A higher number means that it is more severe.
		/// </summary>
		private static readonly Dictionary<LogType, int> LogTypeSeverityOrder = new()
		{
			{ LogType.Log, 0 },
			{ LogType.Warning, 1 },
			{ LogType.Assert, 2 }, // Note that in Unity, an assertion (LogType.Assert) is a special kind of log. When an assertion fails (or when you manually log with LogType.Assert), Unity displays it as an error in the console.
			{ LogType.Error, 3 },
			{ LogType.Exception, 4 },
		};

		/// <summary>
		/// Raw <see cref="LogTag"/> value.
		/// </summary>
		private static string _logTag = string.Empty;

		/// <summary>
		/// Optional tag that, if it's not empty, is prefixed to all log message as follows:
		/// [Tag_value] Log_message.
		/// </summary>
		public static string LogTag
		{
			get => _logTag;
			set => _logTag = string.IsNullOrEmpty(value) ? string.Empty : $"[{value}] ";
		}

		/// <summary>
		/// Is called immediately after this class causes a log to occur.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		public delegate void OnLogEventHandler(LogType logType, string message, Object? context);

		/// <summary>
		/// Is called immediately after this class causes a log to occur.
		/// </summary>
		public static event OnLogEventHandler? OnLog = null;

		/// <summary>
		/// Log by using a formatted string.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		/// <param name="context">If you pass a GameObject or Component as the optional context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		/// <param name="format">Must contain one or more format items, like {0}, {1}, etc. Each format item corresponds to an additional argument provided to the method.</param>
		/// <param name="args">Zero or more arguments that will be formatted and inserted into the format string.</param>
		public static void LogFormat(LogType logType, Object? context, string format, params object[] args)
		{
			if (!IsLogTypeAllowed(logType))
				return;

			string formattedMessage = string.Format(format, args);
			Debug.LogFormat(logType, LogOption.None, context, "{0}{1}", _logTag, formattedMessage);
			OnLog?.Invoke(logType, $"{LogTag}{args}", context);
		}

		/// <summary>
		/// Log by using a formatted string.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		/// <param name="format">Must contain one or more format items, like {0}, {1}, etc. Each format item corresponds to an additional argument provided to the method.</param>
		/// <param name="args">Zero or more arguments that will be formatted and inserted into the format string.</param>
		public static void LogFormat(LogType logType, string format, params object[] args)
		{
			LogFormat(logType, null, _logTag + format, args);
		}

		/// <summary>
		/// Log an <see cref="System.Exception"/>.
		/// </summary>
		/// <param name="exception"><see cref="System.Exception"/> to log.</param>
		/// <param name="context">If you pass a GameObject or Component as the optional context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		public static void LogException(Exception exception, Object? context)
		{
			if (!IsLogTypeAllowed(LogType.Exception))
				return;

			Debug.LogException(exception, context);
		}

		/// <summary>
		/// Log an <see cref="System.Exception"/>.
		/// </summary>
		/// <param name="exception"><see cref="System.Exception"/> to log.</param>
		public static void LogException(Exception exception)
		{
			LogException(exception, null);
		}

		/// <summary>
		/// Returns true if the specified <paramref name="logType"/> is currently allowed to be logged.
		/// Whether or not it is allowed depends on the <see cref="FilterLogType"/> value.
		/// Note that  must also meet these conditions if you want to see it in the console.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		public static bool IsLogTypeAllowed(LogType logType)
		{
			return LogEnabled && (LogTypeSeverityOrder[logType] >= LogTypeSeverityOrder[FilterLogType]);
		}

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		/// <param name="message">Log message.</param>
		public static void Log(LogType logType, object message)
		{
			Log(logType, $"{_logTag}{message}", null);
		}

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the optional context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		public static void Log(LogType logType, object message, Object? context)
		{
			if (!IsLogTypeAllowed(logType))
				return;

			Debug.LogFormat(logType, LogOption.None, context, "{0}", message);
			OnLog?.Invoke(logType, $"{LogTag}{message}", context);
		}

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		/// <param name="tag">Log prefix between []. If you don't need it then use another overload instead.</param>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the optional context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		public static void Log(LogType logType, string tag, object message, Object? context)
		{
			string tagPrefix = string.IsNullOrEmpty(tag) ? string.Empty : $"[{tag}] ";
			Log(logType, $"[{tagPrefix}{message}", context);
		}

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="message">Log message.</param>
		public static void Log(object message)
		{
			Log(LogType.Log, $"{_logTag}{message}", null);
		}

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="tag">Log prefix between []. If you don't need it then use another overload instead.</param>
		/// <param name="message">Log message.</param>
		public static void Log(string tag, object message)
		{
			string tagPrefix = string.IsNullOrEmpty(tag) ? string.Empty : $"[{tag}] ";
			Log(LogType.Log, $"{tagPrefix}{message}", null);
		}

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="tag">Log prefix between []. If you don't need it then use another overload instead.</param>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the optional context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		public static void Log(string tag, object message, Object? context)
		{
			string tagPrefix = string.IsNullOrEmpty(tag) ? string.Empty : $"[{tag}] ";
			Log(LogType.Log, $"{tagPrefix}{message}", context);
		}

		/// <summary>
		/// Log a warning.
		/// </summary>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the optional context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		public static void LogWarning(object message, Object? context = null)
		{
			Log(LogType.Warning, $"{LogTag}{message}", context);
		}

		/// <summary>
		/// Log a warning.
		/// </summary>
		/// <param name="tag">Log prefix between []. If you don't need it then use another overload instead.</param>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the optional context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		public static void LogWarning(string tag, object message, Object? context)
		{
			string tagPrefix = string.IsNullOrEmpty(tag) ? string.Empty : $"[{tag}] ";
			Log(LogType.Warning, $"{tagPrefix}{message}", context);
		}

		/// <summary>
		/// Log an error.
		/// </summary>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the optional context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		public static void LogError(object message, Object? context = null)
		{
			Log(LogType.Error, $"{LogTag}{message}", context);
		}

		/// <summary>
		/// Log an error.
		/// </summary>
		/// <param name="tag">Log prefix between []. If you don't need it then use another overload instead.</param>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the optional context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		public static void LogError(string tag, object message, Object? context)
		{
			string tagPrefix = string.IsNullOrEmpty(tag) ? string.Empty : $"[{tag}] ";
			Log(LogType.Error, $"{tagPrefix}{message}", context);
		}

		/// <summary>
		/// Log an assert.
		/// </summary>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the optional context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		public static void LogAssert(object message, Object? context = null)
		{
			Log(LogType.Assert, $"{LogTag}{message}", context);
		}
	}
}
