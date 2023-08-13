#nullable enable
using System;
using System.Collections.Generic;
using Elephant.UnityLibrary.Logging.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Elephant.UnityLibrary.Logging
{
	/// <summary>
	/// Log to console.
	/// </summary>
	public class ConsoleLogger : IConsoleLogger
	{
		/// <inheritdoc/>
		public bool IsLogEnabled { get; set; }

		/// <inheritdoc/>
		public LogType FilterLogType { get; set; }

		/// <summary>
		/// A higher number means that it is more severe.
		/// </summary>
		private static readonly Dictionary<LogType, int> LogTypeSeverityOrder = new()
		{
			{ LogType.Assert, 0 },
			{ LogType.Log, 1 },
			{ LogType.Warning, 2 },
			{ LogType.Error, 3 },
			{ LogType.Exception, 4 },
		};

		/// <summary>
		/// Raw <see cref="LogTag"/> value.
		/// </summary>
		private string _logTag = string.Empty;

		/// <inheritdoc/>
		public string LogTag
		{
			get => _logTag;
			set => _logTag = string.IsNullOrEmpty(value) ? string.Empty : $"[{value}] ";
		}

		/// <summary>
		/// Is called immediately after this class causes a log to occur.
		/// </summary>
		public event OnLogEventHandler? OnLog = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ConsoleLogger(string logTag = "")
		{
			LogTag = logTag;
		}

		/// <inheritdoc/>
		public virtual void LogFormat(LogType logType, Object? context, string format, params object[] args)
		{
			if (!IsLogTypeAllowed(logType))
				return;

			string formattedMessage = string.Format(format, args);
			Debug.LogFormat(_logTag + "{0} [{1}] {2}", context, logType, formattedMessage);
			OnLog?.Invoke(logType, string.Format("{0}{1}", LogTag, args), context);
		}

		/// <inheritdoc/>
		public virtual void LogFormat(LogType logType, string format, params object[] args)
		{
			LogFormat(logType, null, _logTag + format, args);
		}

		/// <inheritdoc/>
		public virtual void LogException(Exception exception, Object? context)
		{
			if (!IsLogTypeAllowed(LogType.Exception))
				return;

			Debug.LogException(exception, context);
		}

		/// <inheritdoc/>
		public virtual void LogException(Exception exception)
		{
			LogException(exception, null);
		}

		/// <inheritdoc/>
		public virtual bool IsLogTypeAllowed(LogType logType)
		{
			return IsLogEnabled && (LogTypeSeverityOrder[logType] >= LogTypeSeverityOrder[FilterLogType]);
		}

		/// <inheritdoc/>
		public virtual void Log(LogType logType, object message)
		{
			Log(logType, $"{_logTag}{message}", null);
		}

		/// <inheritdoc/>
		public virtual void Log(LogType logType, object message, Object? context)
		{
			if (!IsLogTypeAllowed(logType))
				return;

			Debug.LogFormat(_logTag + "{0} [{1}] {2}", context, logType, message);
			OnLog?.Invoke(logType, string.Format("{0}{1}", LogTag, message), context);
		}

		/// <inheritdoc/>
		public virtual void Log(LogType logType, string tag, object message, Object? context)
		{
			string tagPrefix = string.IsNullOrEmpty(tag) ? string.Empty : $"[{tag}] ";
			Log(logType, $"{tagPrefix}{message}", context);
		}

		/// <inheritdoc/>
		public virtual void Log(object message)
		{
			Log(LogType.Log, $"{_logTag}{message}", null);
		}

		/// <inheritdoc/>
		public virtual void Log(string tag, object message)
		{
			string tagPrefix = string.IsNullOrEmpty(tag) ? string.Empty : $"[{tag}] ";
			Log(LogType.Log, $"{tagPrefix}{message}", null);
		}

		/// <inheritdoc/>
		public virtual void Log(string tag, object message, Object? context)
		{
			string tagPrefix = string.IsNullOrEmpty(tag) ? string.Empty : $"[{tag}] ";
			Log(LogType.Log, $"{tagPrefix}{message}", context);
		}

		/// <inheritdoc/>
		public virtual void LogWarning(object message, Object? context = null)
		{
			Log(LogType.Warning, $"{LogTag}{message}", context);
		}

		/// <inheritdoc/>
		public virtual void LogWarning(string tag, object message, Object? context)
		{
			string tagPrefix = string.IsNullOrEmpty(tag) ? string.Empty : $"[{tag}] ";
			Log(LogType.Warning, $"{tagPrefix}{message}", context);
		}

		/// <inheritdoc/>
		public virtual void LogError(object message, Object? context = null)
		{
			Log(LogType.Error, $"{LogTag}{message}", context);
		}

		/// <inheritdoc/>
		public virtual void LogError(string tag, object message, Object? context)
		{
			string tagPrefix = string.IsNullOrEmpty(tag) ? string.Empty : $"[{tag}] ";
			Log(LogType.Error, $"{tagPrefix}{message}", context);
		}
	}
}
