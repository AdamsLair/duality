﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	/// <summary>
	/// Provides extension methods for enumerations.
	/// </summary>
	public static class ExtMethodsIEnumerable
	{
		/// <summary>
		/// Enumerates the <see cref="Duality.GameObject">GameObjects</see> children.
		/// </summary>
		/// <param name="objEnum"></param>
		public static IEnumerable<GameObject> Children(this IEnumerable<GameObject> objEnum)
		{
			List<GameObject> result = new List<GameObject>();
			foreach (GameObject obj in objEnum)
			{
				result.AddRange(obj.Children);
			}
			return result;
		}
		/// <summary>
		/// Enumerates the <see cref="Duality.GameObject">GameObjects</see> children, grandchildren, etc.
		/// </summary>
		/// <param name="objEnum"></param>
		public static IEnumerable<GameObject> ChildrenDeep(this IEnumerable<GameObject> objEnum)
		{
			List<GameObject> result = new List<GameObject>();
			foreach (GameObject obj in objEnum)
			{
				obj.GetChildrenDeep(result);
			}
			return result;
		}
		/// <summary>
		/// Enumerates all <see cref="Duality.GameObject">GameObjects</see> that match the specified name.
		/// </summary>
		/// <param name="objEnum"></param>
		/// <param name="name"></param>
		public static IEnumerable<GameObject> ByName(this IEnumerable<GameObject> objEnum, string name)
		{
			if (name.IndexOf('/') != -1)
			{
				string[] names = name.Split('/');
				IEnumerable<GameObject> cur = objEnum.ByName(names[0]);
				for (int i = 1; i < names.Length; i++)
				{
					if (cur == null) return null;
					cur = cur.Children().ByName(names[i]);
				}
				return cur;
			}
			else
				return objEnum.Where(o => o.Name == name);
		}
		/// <summary>
		/// Returns the first <see cref="Duality.GameObject"/> that matches the specified name.
		/// </summary>
		/// <param name="objEnum"></param>
		/// <param name="name"></param>
		public static GameObject FirstByName(this IEnumerable<GameObject> objEnum, string name)
		{
			if (name.IndexOf('/') != -1)
			{
				string[] names = name.Split('/');
				GameObject cur = objEnum.FirstByName(names[0]);
				for (int i = 1; i < names.Length; i++)
				{
					if (cur == null) return null;
					cur = cur.Children.FirstByName(names[i]);
				}
				return cur;
			}
			else
				return objEnum.FirstOrDefault(o => o.Name == name);
		}

		/// <summary>
		/// Enumerates all <see cref="Duality.GameObject">GameObjects</see> <see cref="Component">Components</see> of the specified type.
		/// </summary>
		public static IEnumerable<T> GetComponents<T>(this IEnumerable<GameObject> objEnum) where T : class
		{
			return objEnum.SelectMany(o => o.GetComponents<T>());
		}
		/// <summary>
		/// Enumerates all <see cref="Duality.GameObject">GameObjects</see> childrens <see cref="Component">Components</see> of the specified type.
		/// </summary>
		public static IEnumerable<T> GetComponentsInChildren<T>(this IEnumerable<GameObject> objEnum) where T : class
		{
			return objEnum.SelectMany(o => o.GetComponentsInChildren<T>());
		}
		/// <summary>
		/// Enumerates all <see cref="Duality.GameObject">GameObjects</see> (and their childrens) <see cref="Component">Components</see> of the specified type.
		/// </summary>
		public static IEnumerable<T> GetComponentsDeep<T>(this IEnumerable<GameObject> objEnum) where T : class
		{
			return objEnum.SelectMany(o => o.GetComponentsDeep<T>());
		}

		/// <summary>
		/// Enumerates all <see cref="Duality.GameObject">GameObjects</see> <see cref="Duality.Components.Transform"/> Components.
		/// </summary>
		public static IEnumerable<Components.Transform> Transform(this IEnumerable<GameObject> objEnum)
		{
			return objEnum.Select(o => o.Transform).NotNull();
		}

		/// <summary>
		/// Enumerates all <see cref="Component">Components</see> parent <see cref="Duality.GameObject">GameObjects</see>.
		/// </summary>
		public static IEnumerable<GameObject> GameObject(this IEnumerable<Component> compEnum)
		{
			return compEnum.Select(c => c.GameObj).NotNull();
		}

		/// <summary>
		/// Converts an enumeration of Resources to an enumeration of content references to it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="res"></param>
		public static IEnumerable<ContentRef<T>> Ref<T>(this IEnumerable<T> res) where T : Resource
		{
			return res.Select(r => new ContentRef<T>(r));
		}
		/// <summary>
		/// Converts an enumeration of content references to an enumeration of Resources.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="res"></param>
		public static IEnumerable<T> Res<T>(this IEnumerable<ContentRef<T>> res) where T : Resource
		{
			return res.Select(r => r.Res);
		}
		/// <summary>
		/// Converts an enumeration of content references to an enumeration of Resources.
		/// </summary>
		/// <param name="res"></param>
		public static IEnumerable<Resource> Res(this IEnumerable<IContentRef> res)
		{
			return res.Select(r => r.Res);
		}

		/// <summary>
		/// Creates a separated list of the string versions of a set of objects.
		/// </summary>
		/// <typeparam name="T">The type of the incoming objects.</typeparam>
		/// <param name="collection">A set of objects.</param>
		/// <param name="separator">The string to use as separator between two string values.</param>
		public static string ToString<T>(this IEnumerable<T> collection, string separator)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var item in collection)
			{
				sb.Append(item != null ? item.ToString() : "null");
				sb.Append(separator);
			}
			return sb.ToString(0, Math.Max(0, sb.Length - separator.Length));  // Remove at the end is faster
		}
		/// <summary>
		/// Creates a separated list of the string versions of a set of objects.
		/// </summary>
		/// <typeparam name="T">The type of the incoming objects.</typeparam>
		/// <param name="collection">A set of objects.</param>
		/// <param name="toString">A function that transforms objects to strings.</param>
		/// <param name="separator">The string to use as separator between two string values.</param>
		public static string ToString<T>(this IEnumerable<T> collection, Func<T, string> toString, string separator)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var item in collection)
			{
				sb.Append(toString(item));
				sb.Append(separator);
			}
			return sb.ToString(0, Math.Max(0, sb.Length - separator.Length));  // Remove at the end is faster
		}

		/// <summary>
		/// Enumerates objects that aren't null.
		/// </summary>
		/// <typeparam name="T">The type of the incoming objects.</typeparam>
		/// <param name="collection">A set of objects.</param>
		public static IEnumerable<T> NotNull<T>(this IEnumerable<T> collection) where T : class
		{
			return collection.Where(i => i != null);
		}
		/// <summary>
		/// Enumerates a all objects within a specific index range.
		/// </summary>
		/// <typeparam name="T">The type of the incoming objects.</typeparam>
		/// <param name="collection">A set of objects.</param>
		/// <param name="startIndex">Index of the first object to be enumerated.</param>
		/// <param name="length">Number of objects to be enumerated.</param>
		public static IEnumerable<T> Range<T>(this IEnumerable<T> collection, int startIndex, int length)
		{
			return collection.Skip(startIndex).Take(length);
		}

		/// <summary>
		/// Returns whether two sets of objects equal each other. This is the case if both sets contain an equal number of elements
		/// and for each element in one set, there is a matching element in the other.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <param name="comparer"></param>
		public static bool SetEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer)
		{
			if (object.ReferenceEquals(first, second)) return true;
			if (object.ReferenceEquals(first, null) || object.ReferenceEquals(second, null)) return false;

			int count = first.Count();
			if (count != second.Count()) return false;
			
			if (count < 25)
			{
				return first.All(o => second.Contains(o, comparer));
			}
			else
			{
				var cnt = new Dictionary<T,int>(comparer);
				foreach (T s in first)
				{
					if (cnt.ContainsKey(s))
						cnt[s]++;
					else
						cnt.Add(s, 1);
				}
				foreach (T s in second)
				{
					if (cnt.ContainsKey(s))
						cnt[s]--;
					else
						return false;
				}
				return cnt.Values.All(c => c == 0);
			}
		}
		/// <summary>
		/// Returns whether two sets of objects equal each other. This is the case if both sets contain an equal number of elements
		/// and for each element in one set, there is a matching element in the other.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		public static bool SetEqual<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			return SetEqual<T>(first, second, EqualityComparer<T>.Default);
		}
	}
}
