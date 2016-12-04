using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Duality
{
	/// <summary>
	/// Listens for log entries and writes them to registered <see cref="ILogOutput">ILogOutputs</see>.
	/// </summary>
	public sealed class Log
	{
		private List<ILogOutput> output      = new List<ILogOutput>();
		private string           name        = string.Empty;
		private string           prefix      = string.Empty;
		private ILogOutput[]     syncOutput  = new ILogOutput[0];
		private object           syncObj     = new object();


		/// <summary>
		/// [GET] The Log's name
		/// </summary>
		public string Name
		{
			get { return this.name; }
		}
		/// <summary>
		/// [GET] The Log's prefix, which is automatically determined by its name.
		/// </summary>
		public string Prefix
		{
			get { return this.prefix; }
		}
		/// <summary>
		/// [GET] Enumerates all <see cref="ILogOutput"/> instances that are
		/// subscribed to this <see cref="Log"/>.
		/// </summary>
		public IEnumerable<ILogOutput> Output
		{
			get { return this.syncOutput; }
		}


		/// <summary>
		/// Creates a new Log.
		/// </summary>
		/// <param name="name">The logs display name.</param>
		/// <param name="id">The logs shorthand ID that will be displayed along with its log entries.</param>
		/// <param name="output">It will be initially connected to the specified outputs.</param>
		public Log(string name, string id, params ILogOutput[] output)
		{
			this.name = name;
			this.prefix = string.Format("[{0}]", id).PadRight(7);
			this.output.AddRange(output);
			this.syncOutput = this.output.ToArray();
		}

		/// <summary>
		/// Adds an output to write log entries to.
		/// </summary>
		/// <param name="writer"></param>
		public void AddOutput(ILogOutput writer)
		{
			lock (this.syncObj)
			{
				this.output.Add(writer);
				this.syncOutput = this.output.ToArray();
			}
		}
		/// <summary>
		/// Removes a certain output.
		/// </summary>
		/// <param name="writer"></param>
		public void RemoveOutput(ILogOutput writer)
		{
			lock (this.syncObj)
			{
				this.output.Remove(writer);
				this.syncOutput = this.output.ToArray();
			}
		}

		/// <summary>
		/// Increases the current log entry indent.
		/// </summary>
		public void PushIndent()
		{
			ILogOutput[] localOutput = this.syncOutput;
			foreach (ILogOutput target in localOutput)
				target.PushIndent();
		}
		/// <summary>
		/// Decreases the current log entry indent.
		/// </summary>
		public void PopIndent()
		{
			ILogOutput[] localOutput = this.syncOutput;
			foreach (ILogOutput target in localOutput)
				target.PopIndent();
		}

		private void Write(LogMessageType type, string msg, object context)
		{
			// If a null message is provided, log that. Don't throw an exception, since logging isn't expected to throw.
			if (msg == null) msg = "[null message]";

			// Check whether the message contains null characters. If it does, crop it, because it's probably broken.
			int nullCharIndex = msg.IndexOf('\0');
			if (nullCharIndex != -1)
			{
				msg = msg.Substring(0, Math.Min(nullCharIndex, 50)) + " | Contains '\0' and is likely broken.";
			}

			// Forward the message to all outputs
			Profile.TimeLog.BeginMeasure();
			LogEntry entry = new LogEntry(type, msg);
			ILogOutput[] localOutput = this.syncOutput;
			foreach (ILogOutput target in localOutput)
			{
				try
				{
					target.Write(entry, context, this);
				}
				catch (Exception)
				{
					// Don't allow log outputs to throw unhandled exceptions,
					// because they would result in another log - and more exceptions.
				}
			}
			Profile.TimeLog.EndMeasure();
		}
		private string FormatMessage(string format, object[] obj)
		{
			if (obj == null || obj.Length == 0) return format;
			string msg;
			try
			{
				msg = string.Format(System.Globalization.CultureInfo.InvariantCulture, format, obj);
			}
			catch (Exception e)
			{
				// Don't allow log message formatting to throw unhandled exceptions,
				// because they would result in another log - and probably more exceptions.

				// Instead, embed format, arguments and the exception in the resulting
				// log message, so the user can retrieve all necessary information for
				// fixing his log call.
				msg = format + Environment.NewLine;
				if (obj != null)
				{
					try
					{
						msg += obj.ToString(", ") + Environment.NewLine;
					}
					catch (Exception)
					{
						msg += "(Error in ToString call)" + Environment.NewLine;
					}
				}
				msg += LogFormat.Exception(e);
			}
			return msg;
		}
		private object FindContext(object[] obj)
		{
			if (obj == null || obj.Length == 0) return null;
			for (int i = 0; i < obj.Length; i++)
			{
				if (obj[i] is GameObject || obj[i] is Component || obj[i] is Resource || obj[i] is IContentRef)
					return obj[i];
			}
			return obj[0];
		}

		/// <summary>
		/// Writes a new log entry.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="obj"></param>
		public void Write(string format, params object[] obj)
		{
			this.Write(LogMessageType.Message, this.FormatMessage(format, obj), this.FindContext(obj));
		}
		/// <summary>
		/// Writes a new warning log entry.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="obj"></param>
		public void WriteWarning(string format, params object[] obj)
		{
			this.Write(LogMessageType.Warning, this.FormatMessage(format, obj), this.FindContext(obj));
		}
		/// <summary>
		/// Writes a new error log entry.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="obj"></param>
		public void WriteError(string format, params object[] obj)
		{
			this.Write(LogMessageType.Error, this.FormatMessage(format, obj), this.FindContext(obj));
		}
	}
}
