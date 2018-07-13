using System.Collections.Generic;

namespace Duality
{
	internal static class CoroutineManager
	{
		private static List<Coroutine> _coroutines = new List<Coroutine>();
		private static List<Coroutine> _trashcan = new List<Coroutine>();
		private static HashSet<string> _signals = new HashSet<string>();

		internal static void Register(Coroutine coroutine)
		{
			_coroutines.Add(coroutine);
		}

		internal static void Update()
		{
			foreach(Coroutine c in _coroutines)
			{
				if (c.Current is StopAction)
					_trashcan.Add(c);

				else if (c.Current == null || c.Current.IsComplete)
				{
					if (!c.Enumerator.MoveNext())
						_trashcan.Add(c);
				}
			}

			foreach (Coroutine c in _trashcan)
			{
				_coroutines.Remove(c);
				c.Dispose();
			}

			_trashcan.Clear();
		}

		internal static bool IsSet(string signal)
		{
			return _signals.Contains(signal);
		}

		internal static bool ConsumeSignal(string signal)
		{
			return _signals.Remove(signal);
		}

		internal static bool EmitSignal(string signal)
		{
			return _signals.Add(signal);
		}
	}
}
