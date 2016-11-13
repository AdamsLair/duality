using System.Text;

namespace Duality
{
	/// <summary>
	/// Provides extension methods for strings.
	/// </summary>
	public static class ExtMethodsString
	{
		/// <summary>
		/// Returns a string containing n times the source string.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="times"></param>
		/// <returns></returns>
		public static string Multiply(this string source, int times)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < times; i++)
			{
				builder.Append(source);
			}
			return builder.ToString();
		}
	}
}
