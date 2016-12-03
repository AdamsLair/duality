using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Microsoft.Build.Execution;

namespace Duality.Editor
{
	public static class BuildHelper
	{
		public static bool BuildSolutionFile(string solutionFile, string configuration)
		{
			try
			{
				return _BuildSolutionFile(solutionFile, configuration);
			}
			catch (FileNotFoundException e)
			{
				// This will be caught when the binding redirect from the old .Net MsBuild to the new VS MsBuild failed.
				Log.Editor.WriteError("An error occurred while attempting to build a VS solution file: {0}{1}{1}This error usually means the binding redirect from the old .Net 4.0 MsBuild to the new Visual Studio MsBuild failed. To fix this, upgrade to any version of Visual Studio 2013.", 
					LogFormat.Exception(e),
					Environment.NewLine);
				return false;
			}
			catch (Exception e)
			{
				Log.Editor.WriteError("An error occurred while attempting to build a VS solution file: {0}", 
					LogFormat.Exception(e));
				return false;
			}
		}
		private static bool _BuildSolutionFile(string solutionFile, string configuration)
		{
			var buildProperties = new Dictionary<string,string>();
			buildProperties["Configuration"] = configuration;
			var buildRequest = new BuildRequestData(solutionFile, buildProperties, null, new string[] { "Build" }, null);
			var buildParameters = new BuildParameters();
			var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);
			return buildResult.OverallResult == BuildResultCode.Success;
		}
	}
}
