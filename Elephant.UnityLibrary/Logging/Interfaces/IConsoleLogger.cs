#nullable enable
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Elephant.UnityLibrary.Logging.Interfaces
{
	/// <summary>
	/// Is called immediately after this class causes a log to occur.
	/// </summary>
	/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
	/// <param name="message">Log message.</param>
	/// <param name="context">If you pass a GameObject or Component as the context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
	public delegate void OnLogEventHandler(LogType logType, string message, Object? context);

	/// <summary>
	/// Log to console.
	/// </summary>
	public interface IConsoleLogger
	{
		/// <summary>
		/// If true, logging is possible; otherwise, all log calls will be ignored.
		/// </summary>
		/// <remarks>
		/// Logging may still be stopped depending on the <see cref="FilterLogType"/> value.
		/// Debug.unityLogger.logEnabled must also be enabled if you want to see it in the console.
		/// </remarks>
		bool LogEnabled { get; set; }

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
		LogType FilterLogType { get; set; }

		/// <summary>
		/// Optional tag that, if it's not empty, is prefixed to all log message as follows:
		/// [Tag_value] Log_message.
		/// </summary>
		string LogTag { get; set; }

		/// <summary>
		/// Is called immediately after this class causes a log to occur.
		/// </summary>
		event OnLogEventHandler? OnLog;

		/// <summary>
		/// Log by using a formatted string.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		/// <param name="context">If you pass a GameObject or Component as the context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		/// <param name="format">Must contain one or more format items, like {0}, {1}, etc. Each format item corresponds to an additional argument provided to the method.</param>
		/// <param name="args">Zero or more arguments that will be formatted and inserted into the format string.</param>
		void LogFormat(LogType logType, Object? context, string format, params object[] args);

		/// <summary>
		/// Log by using a formatted string.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		/// <param name="format">Must contain one or more format items, like {0}, {1}, etc. Each format item corresponds to an additional argument provided to the method.</param>
		/// <param name="args">Zero or more arguments that will be formatted and inserted into the format string.</param>
		void LogFormat(LogType logType, string format, params object[] args);

		/// <summary>
		/// Log an <see cref="System.Exception"/>.
		/// </summary>
		/// <param name="exception"><see cref="System.Exception"/> to log.</param>
		/// <param name="context">If you pass a GameObject or Component as the context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		void LogException(Exception exception, Object? context);

		/// <summary>
		/// Log an <see cref="System.Exception"/>.
		/// </summary>
		/// <param name="exception"><see cref="System.Exception"/> to log.</param>
		void LogException(Exception exception);

		/// <summary>
		/// Returns true if the specified <paramref name="logType"/> is currently allowed to be logged.
		/// Whether or not it is allowed depends on the <see cref="FilterLogType"/> value.
		/// Note that  must also meet these conditions if you want to see it in the console.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		bool IsLogTypeAllowed(LogType logType);

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		/// <param name="message">Log message.</param>
		void Log(LogType logType, object message);

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		void Log(LogType logType, object message, Object? context);

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="logType">The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback.</param>
		/// <param name="tag">Log prefix between []. If you don't need it then use another overload instead.</param>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		void Log(LogType logType, string tag, object message, Object? context);

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="message">Log message.</param>
		void Log(object message);

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="tag">Log prefix between []. If you don't need it then use another overload instead.</param>
		/// <param name="message">Log message.</param>
		void Log(string tag, object message);

		/// <summary>
		/// Log.
		/// </summary>
		/// <param name="tag">Log prefix between []. If you don't need it then use another overload instead.</param>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		void Log(string tag, object message, Object? context);

		/// <summary>
		/// Log a warning.
		/// </summary>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		void LogWarning(object message, Object? context = null);

		/// <summary>
		/// Log a warning.
		/// </summary>
		/// <param name="tag">Log prefix between []. If you don't need it then use another overload instead.</param>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		void LogWarning(string tag, object message, Object context);

		/// <summary>
		/// Log an error.
		/// </summary>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		void LogError(object message, Object? context = null);

		/// <summary>
		/// Log an error.
		/// </summary>
		/// <param name="tag">Log prefix between []. If you don't need it then use another overload instead.</param>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		void LogError(string tag, object message, Object? context);

		/// <summary>
		/// Log an assert.
		/// </summary>
		/// <param name="message">Log message.</param>
		/// <param name="context">If you pass a GameObject or Component as the optional context parameter, Unity momentarily highlights that object in the Hierarchy window when you click the log message in the Console.</param>
		void LogAssert(object message, Object? context = null);
	}
}