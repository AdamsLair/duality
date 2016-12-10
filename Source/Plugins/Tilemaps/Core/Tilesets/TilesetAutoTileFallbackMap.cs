using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Cloning;
using Duality.Resources;
using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	public class TilesetAutoTileFallbackMap
	{
		private static readonly TileConnection[] NoFallbacks = new TileConnection[0];

		private static readonly int            StateCount  = (int)TileConnection.All + 1;
		private static readonly TileConnection None        = TileConnection.None;
		private static readonly TileConnection All         = TileConnection.All;
		private static readonly TileConnection TopLeft     = TileConnection.TopLeft;
		private static readonly TileConnection Top         = TileConnection.Top;
		private static readonly TileConnection TopRight    = TileConnection.TopRight;
		private static readonly TileConnection Left        = TileConnection.Left;
		private static readonly TileConnection Right       = TileConnection.Right;
		private static readonly TileConnection BottomLeft  = TileConnection.BottomLeft;
		private static readonly TileConnection Bottom      = TileConnection.Bottom;
		private static readonly TileConnection BottomRight = TileConnection.BottomRight;


		private TileConnection[][] data;

		public TilesetAutoTileFallbackMap()
		{
			this.GenerateData();
		}


		public IReadOnlyList<TileConnection> GetFallback(TileConnection connection)
		{
			return this.data[(int)connection] ?? NoFallbacks;
		}

		private void GenerateData()
		{
			TileConnection[] directFallbacks = new TileConnection[StateCount];
			for (int i = 0; i < directFallbacks.Length; i++)
			{
				// Since our default is the AutoTile's base tile, which is fully connected,
				// use the fully connected state as a default.
				directFallbacks[i] = TileConnection.All;
			}

			//
			// DontCare permutations, reducing the overall number of required connectivity states to 47.
			// See here: https://cloud.githubusercontent.com/assets/14859411/11279962/ccc1ac2e-8ef3-11e5-8e99-861b0d7a1c9a.png
			//
			MapAllPermutations(directFallbacks, Left | Right | BottomLeft | Bottom | BottomRight, TopLeft, TopRight);
			MapAllPermutations(directFallbacks, TopLeft | Top | Left | BottomLeft | Bottom,       TopRight, BottomRight);
			MapAllPermutations(directFallbacks, Left | BottomLeft | Bottom,                       TopLeft, TopRight, BottomRight);
			MapAllPermutations(directFallbacks, TopLeft | Top | TopRight | Left | Right,          BottomLeft, BottomRight);
			MapAllPermutations(directFallbacks, Left | Right,                                     TopLeft, TopRight, BottomLeft, BottomRight);
			MapAllPermutations(directFallbacks, TopLeft | Top | Left,                             TopRight, BottomLeft, BottomRight);

			MapAllPermutations(directFallbacks, Left,                                             TopLeft, TopRight, BottomLeft, BottomRight);
			MapAllPermutations(directFallbacks, Top | TopRight | Right | Bottom | BottomRight,    TopLeft, BottomLeft);
			MapAllPermutations(directFallbacks, Right | Bottom | BottomRight,                     TopLeft, TopRight, BottomLeft);
			MapAllPermutations(directFallbacks, Top | Bottom,                                     TopLeft, TopRight, BottomLeft, BottomRight);
			MapAllPermutations(directFallbacks, Bottom,                                           TopLeft, TopRight, BottomLeft, BottomRight);
			MapAllPermutations(directFallbacks, Top | TopRight | Right,                           TopLeft, BottomLeft, BottomRight);
			MapAllPermutations(directFallbacks, Right,                                            TopLeft, TopRight, BottomLeft, BottomRight);

			MapAllPermutations(directFallbacks, Top,                                              TopLeft, TopRight, BottomLeft, BottomRight);
			MapAllPermutations(directFallbacks, None,                                             TopLeft, TopRight, BottomLeft, BottomRight);

			MapAllPermutations(directFallbacks, Left | Right | BottomLeft | Bottom,               TopLeft, TopRight);
			MapAllPermutations(directFallbacks, Left | Right | Bottom | BottomRight,              TopLeft, TopRight);
			MapAllPermutations(directFallbacks, Left | Right | Bottom,                            TopLeft, TopRight);
			MapAllPermutations(directFallbacks, Top | Left | BottomLeft | Bottom,                 TopRight, BottomRight);
			MapAllPermutations(directFallbacks, TopLeft | Top | Left | Bottom,                    TopRight, BottomRight);
			MapAllPermutations(directFallbacks, Top | Left | Bottom,                              TopRight, BottomRight);
			MapAllPermutations(directFallbacks, Left | Bottom,                                    TopLeft, TopRight, BottomRight);

			MapAllPermutations(directFallbacks, Top | TopRight | Left | Right,                    BottomLeft, BottomRight);
			MapAllPermutations(directFallbacks, TopLeft | Top | Left | Right,                     BottomLeft, BottomRight);
			MapAllPermutations(directFallbacks, Top | Left | Right,                               BottomLeft, BottomRight);
			MapAllPermutations(directFallbacks, Top | Left,                                       TopRight, BottomLeft, BottomRight);
			MapAllPermutations(directFallbacks, Top | Right | Bottom | BottomRight,               TopLeft, BottomLeft);
			MapAllPermutations(directFallbacks, Top | TopRight | Right | Bottom,                  TopLeft, BottomLeft);
			MapAllPermutations(directFallbacks, Top | Right | Bottom,                             TopLeft, BottomLeft);

			MapAllPermutations(directFallbacks, Right | Bottom,                                   TopLeft, TopRight, BottomLeft);
			MapAllPermutations(directFallbacks, Top | Right,                                      TopLeft, BottomLeft, BottomRight);
			
			//
			// Actual fallbacks in case a certain connectivity state is unavailable.
			//
			directFallbacks[(int)(Top | Left | Right | BottomLeft | Bottom | BottomRight)] = All;
			directFallbacks[(int)(TopLeft | Top | Left | Right | BottomLeft | Bottom)]     = All;
			directFallbacks[(int)(Top | TopRight | Left | Right | Bottom | BottomRight)]   = All;
			directFallbacks[(int)(TopLeft | Top | TopRight | Left | Right | Bottom)]       = All;

			directFallbacks[(int)(Top | TopRight | Left | Right | BottomLeft | Bottom | BottomRight)] = All;
			directFallbacks[(int)(TopLeft | Top | Left | Right | BottomLeft | Bottom | BottomRight)]  = All;
			directFallbacks[(int)(TopLeft | Top | TopRight | Left | Right | Bottom | BottomRight)]    = All;
			directFallbacks[(int)(TopLeft | Top | TopRight | Left | Right | BottomLeft | Bottom)]     = All;

			directFallbacks[(int)(Top | TopRight | Left | Right | BottomLeft | Bottom)] = All;
			directFallbacks[(int)(TopLeft | Top | Left | Right | Bottom | BottomRight)] = All;

			directFallbacks[(int)(TopLeft | Top | Left | Right | Bottom)]     = All;
			directFallbacks[(int)(Top | TopRight | Left | Right | Bottom)]    = All;
			directFallbacks[(int)(Top | Left | Right | BottomLeft | Bottom)]  = All;
			directFallbacks[(int)(Top | Left | Right | Bottom | BottomRight)] = All;

			directFallbacks[(int)(Left | Right | Top | Bottom)] = All;

			directFallbacks[(int)(Left | Right | BottomLeft | Bottom)]  = Left | Right | BottomLeft | Bottom | BottomRight;
			directFallbacks[(int)(Left | Right | BottomRight | Bottom)] = Left | Right | BottomLeft | Bottom | BottomRight;
			directFallbacks[(int)(Left | Right | Bottom)]               = Left | Right | BottomLeft | Bottom | BottomRight;

			directFallbacks[(int)(Top | Left | BottomLeft | Bottom)] = TopLeft | Top | Left | BottomLeft | Bottom;
			directFallbacks[(int)(TopLeft | Top | Left | Bottom)]    = TopLeft | Top | Left | BottomLeft | Bottom;
			directFallbacks[(int)(Top | Left | Bottom)]              = TopLeft | Top | Left | BottomLeft | Bottom;

			directFallbacks[(int)(Top | TopRight | Left | Right)] = TopLeft | Top | TopRight | Left | Right;
			directFallbacks[(int)(TopLeft | Top | Left | Right)]  = TopLeft | Top | TopRight | Left | Right;
			directFallbacks[(int)(Top | Left | Right)]            = TopLeft | Top | TopRight | Left | Right;

			directFallbacks[(int)(Top | Right | Bottom | BottomRight)] = Top | TopRight | Right | Bottom | BottomRight;
			directFallbacks[(int)(Top | TopRight | Right | Bottom)]    = Top | TopRight | Right | Bottom | BottomRight;
			directFallbacks[(int)(Top | Right | Bottom)]               = Top | TopRight | Right | Bottom | BottomRight;

			directFallbacks[(int)(Top | Left)]     = TopLeft | Top | Left;
			directFallbacks[(int)(Top | Right)]    = Top | TopRight | Right;
			directFallbacks[(int)(Left | Bottom)]  = Left | BottomLeft | Bottom;
			directFallbacks[(int)(Right | Bottom)] = Right | Bottom | BottomRight;

			// Create a transitive fallback chain for each connectivity state
			this.data = CreateTransitiveMap(directFallbacks);
		}

		/// <summary>
		/// Modifies the specified fallback map so that all permutations of the specified 
		/// base fallback and state flags map back to the base fallback.
		/// </summary>
		/// <param name="mapToFallback"></param>
		/// <param name="fallback"></param>
		/// <param name="stateFlags"></param>
		private static void MapAllPermutations(TileConnection[] mapToFallback, TileConnection fallback, params TileConnection[] stateFlags)
		{
			int permutationCount = 1 << stateFlags.Length;
			for (int permutationIndex = 1; permutationIndex < permutationCount; permutationIndex++)
			{
				// Generate the state bitmask for this permutation of state flags
				TileConnection state = fallback;
				for (int stateIndex = 0; stateIndex < stateFlags.Length; stateIndex++)
				{
					int permutationFlag = (1 << stateIndex);
					if ((permutationIndex & permutationFlag) == permutationFlag)
					{
						state |= stateFlags[stateIndex];
					}
				}

				// Set all permutations to map to this fallback
				mapToFallback[(int)state] = fallback;
			}
		}
		/// <summary>
		/// Generates a transitive fallback map where each item is a sorted list of all items in the items fallback chain.
		/// </summary>
		/// <param name="directMap"></param>
		/// <returns></returns>
		private static TileConnection[][] CreateTransitiveMap(TileConnection[] directMap)
		{
			TileConnection[][] transitiveMap = new TileConnection[StateCount][];

			List<TileConnection> currentChain = new List<TileConnection>();
			for (int stateIndex = 0; stateIndex < StateCount; stateIndex++)
			{
				// Build the transitive chain of this state
				int current = stateIndex;
				while (directMap[current] != TileConnection.All && (int)directMap[current] != current)
				{
					currentChain.Add(directMap[current]);
					current = (int)directMap[current];
				}

				// Map it and start over
				if (currentChain.Count > 0)
				{
					transitiveMap[stateIndex] = currentChain.ToArray();
					currentChain.Clear();
				}
			}

			return transitiveMap;
		}
	}
}
