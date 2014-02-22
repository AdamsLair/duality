using System;
using Duality;
using Duality.Components;

namespace Duality.Editor
{
	public static class ExtMethodsCamera
	{
		public static void AddEditorRendererFilter(this Camera c, Predicate<ICmpRenderer> predicate)
		{
			c.AddEditorRendererFilter(predicate);
		}
		public static void RemoveEditorRendererFilter(this Camera c, Predicate<ICmpRenderer> predicate)
		{
			c.RemoveEditorRendererFilter(predicate);
		}
	}
}
