using System;
using System.Threading.Tasks;
using NuGet.Common;

namespace Duality.Editor.PackageManagement
{
	internal class PackageManagerLogger : ILogger
	{
		public void LogDebug(string data)
		{
			Logs.Editor.Write(data);
		}

		public void LogVerbose(string data)
		{
			Logs.Editor.Write(data);
		}

		public void LogInformation(string data)
		{
			Logs.Editor.Write(data);
		}

		public void LogMinimal(string data)
		{
			Logs.Editor.Write(data);
		}

		public void LogWarning(string data)
		{
			Logs.Editor.WriteWarning(data);
		}

		public void LogError(string data)
		{
			Logs.Editor.WriteError(data);
		}

		public void LogInformationSummary(string data)
		{
			Logs.Editor.Write(data);
		}

		public void Log(LogLevel level, string data)
		{
			switch (level)
			{
				case LogLevel.Debug:
					this.LogDebug(data);
					break;
				case LogLevel.Verbose:
					this.LogVerbose(data);
					break;
				case LogLevel.Information:
					this.LogInformation(data);
					break;
				case LogLevel.Minimal:
					this.LogMinimal(data);
					break;
				case LogLevel.Warning:
					this.LogWarning(data);
					break;
				case LogLevel.Error:
					this.LogError(data);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(level), level, null);
			}
		}

		public async Task LogAsync(LogLevel level, string data)
		{
			this.Log(level, data);
			await Task.Yield();
		}

		public void Log(ILogMessage message)
		{
			this.Log(message.Level, message.Message);
		}

		public async Task LogAsync(ILogMessage message)
		{
			await this.LogAsync(message.Level, message.Message);
		}
	}
}
