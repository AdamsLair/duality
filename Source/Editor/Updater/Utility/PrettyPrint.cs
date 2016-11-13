using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Duality.Updater
{
	public static class PrettyPrint
	{
		public enum ElementType
		{
			Command,
			Argument,
			FilePathArgument
		}
		public struct Element
		{
			public string Text;
			public ElementType Type;

			public Element(string text, ElementType type)
			{
				this.Text = text;
				this.Type = type;
			}
		}

		public static void PrintCommand(params Element[] elements)
		{
			bool first = true;
			foreach (var element in elements)
			{
				if (!first) Console.Write(' ');
				switch (element.Type)
				{
					default:
					case ElementType.Command:
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write(element.Text);
						Console.ResetColor();
						break;
					case ElementType.Argument:
						Console.Write("'");
						Console.Write(element.Text);
						Console.Write("'");
						break;
					case ElementType.FilePathArgument:
						Console.Write("'");
						PrintPath(element.Text);
						Console.Write("'");
						break;
				}
				first = false;
			}
			Console.Write("... ");
		}
		public static void PrintPath(string path)
		{
			string fileName = Path.GetFileName(path);

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(path.Substring(0, path.Length - fileName.Length));
			Console.ResetColor();
			Console.Write(fileName);
		}
	}
}
