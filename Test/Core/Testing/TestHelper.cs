using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Text;

namespace Duality.Tests
{
	public static class TestHelper
	{
		private static TestMemory localTestMemory = null;

		public static string EmbeddedResourcesDir
		{
			get { return Path.Combine("..", "..", "Test", "Core", "EmbeddedResources"); }
		}
		public static string LocalTestMemoryFilePath
		{
			get
			{
				string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				string testingDir = Path.Combine(appDataDir, "Duality", "UnitTesting");
				string testingFile = "DualityTests";
#if DEBUG
				testingFile += "Debug";
#else
				testingFile += "Release";
#endif
				testingFile += ".dat";
				return Path.Combine(testingDir, testingFile);
			}
		}
		public static TestMemory LocalTestMemory
		{
			get { return localTestMemory; }
			internal set { localTestMemory = value ?? new TestMemory(); }
		}

		public static string GetEmbeddedResourcePath(string resName, string resEnding)
		{
			return Path.Combine(TestHelper.EmbeddedResourcesDir, resName + resEnding);
		}
		public static void LogNumericTestResult(object testFixture, string testName, long resultValue, string unit)
		{
			if (!string.IsNullOrEmpty(unit)) unit = " " + unit;
			
			List<long> lastValueList;
			if (!TestHelper.LocalTestMemory.GetValue(testFixture, testName, out lastValueList))
			{
				lastValueList = new List<long>();
			}
			lastValueList.Add(resultValue);
			if (lastValueList.Count > 10) lastValueList.RemoveAt(0);
			TestHelper.LocalTestMemory.SetValue(testFixture, testName, lastValueList);

			long localAverage = (long)lastValueList.Average();

			string nameStr = (testFixture.GetType().Name + "." + testName);
			string newValueStr = string.Format("{0}{1}", resultValue, unit);
			string lastValueStr = string.Format("{0}{1}", localAverage, unit);

			double relativeChange = ((double)resultValue - (double)localAverage) / (double)localAverage;
			LogNumericTestResult(nameStr, newValueStr, lastValueStr, relativeChange);
		}
		public static void LogNumericTestResult(object testFixture, string testName, double resultValue, string unit)
		{
			if (!string.IsNullOrEmpty(unit)) unit = " " + unit;

			List<double> lastValueList;
			if (!TestHelper.LocalTestMemory.GetValue(testFixture, testName, out lastValueList))
			{
				lastValueList = new List<double>();
			}
			lastValueList.Add(resultValue);
			if (lastValueList.Count > 10) lastValueList.RemoveAt(0);
			TestHelper.LocalTestMemory.SetValue(testFixture, testName, lastValueList);

			double localAverage = lastValueList.Average();

			string nameStr = (testFixture.GetType().Name + "." + testName);
			string newValueStr = string.Format("{0:F}{1}", resultValue, unit);
			string lastValueStr = string.Format("{0:F}{1}", localAverage, unit);

			double relativeChange = ((double)resultValue - (double)localAverage) / (double)localAverage;
			LogNumericTestResult(nameStr, newValueStr, lastValueStr, relativeChange);
		}
		private static void LogNumericTestResult(string nameStr, string newValueStr, string lastValueStr, double relativeChange)
		{
			if (Math.Abs(relativeChange) > 0.03)
			{
				Console.WriteLine(string.Format("{0}: {2} --> {1} Changed by {3}%", 
					nameStr.PadRight(50),
					newValueStr.PadRight(12), 
					lastValueStr.PadRight(12),
					(int)Math.Round(100.0d * relativeChange)));
			}
			else
			{
				Console.WriteLine(string.Format("{0}: {1}", 
					nameStr.PadRight(50),
					newValueStr));
			}
		}
	}

	public class TestMemory
	{
		private Dictionary<string,object> data = new Dictionary<string,object>();

		public bool SwitchValue<T>(object testFixture, string key, out T oldValue, T newValue)
		{
			bool result = this.GetValue(testFixture, key, out oldValue);
			this.SetValue(testFixture, key, newValue);
			return result;
		}
		public void SetValue<T>(object testFixture, string key, T value)
		{
			if (testFixture != null)
			{
				key = testFixture.GetType().Name + "_" + key;
			}
			this.data[key] = value;
		}
		public bool GetValue<T>(object testFixture, string key, out T value)
		{
			if (testFixture != null)
			{
				key = testFixture.GetType().Name + "_" + key;
			}
			object valueObj;
			if (this.data.TryGetValue(key, out valueObj) && valueObj is T)
			{
				value = (T)valueObj;
				return true;
			}
			else
			{
				value = default(T);
				return false;
			}
		}
	}
}
