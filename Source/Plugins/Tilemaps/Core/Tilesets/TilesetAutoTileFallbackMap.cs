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


		private TileConnection[][] tileToFallbackList;
		private TileConnection[] tileToBasePermutation;
		private TileConnection[] basePermutation;

		public TilesetAutoTileFallbackMap()
		{
			this.GenerateData();
		}


		/// <summary>
		/// The reduced list of 47 tile connectivity permutations that are required to form 
		/// a full AutoTile.
		/// </summary>
		public IReadOnlyList<TileConnection> BaseConnectivityTiles
		{
			get { return this.basePermutation; }
		}


		/// <summary>
		/// Returns a connectivity bitmask that contains all relevant bits for the specified
		/// subtile quadrant of an AutoTile.
		/// </summary>
		/// <param name="quadrant"></param>
		/// <returns></returns>
		public TileConnection GetSubTileMask(TileQuadrant quadrant)
		{
			switch (quadrant)
			{
				case TileQuadrant.TopLeft:     return TileConnection.Top | TileConnection.TopLeft | TileConnection.Left;
				case TileQuadrant.TopRight:    return TileConnection.Top | TileConnection.TopRight | TileConnection.Right;
				case TileQuadrant.BottomRight: return TileConnection.Bottom | TileConnection.BottomRight | TileConnection.Right;
				case TileQuadrant.BottomLeft:  return TileConnection.Bottom | TileConnection.BottomLeft | TileConnection.Left;
				default:                       return TileConnection.None;
			}
		}
		/// <summary>
		/// Given the specified connectivity bitmask, this method returns the functionally
		/// equivalent base connectivity with all the DontCare bits for this case removed.
		/// 
		/// Not all bits in a connectivity mask make a difference in all cases, and instead
		/// of the theoretical 256 tiles per AutoTile, we can get down to 47 tiles if we ignore
		/// all the scenarios where individual connectivity bits don't matter. 
		/// 
		/// See this image for an illustration of this:
		/// https://cloud.githubusercontent.com/assets/14859411/11279962/ccc1ac2e-8ef3-11e5-8e99-861b0d7a1c9a.png
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		public TileConnection GetBaseConnectivity(TileConnection connection)
		{
			return this.tileToBasePermutation[(int)connection];
		}
		/// <summary>
		/// Given the specified connectivity bitmask, this method returns a list of other
		/// connectivities that can be used to find a suitable fallback tile, if no tile
		/// with the specified connectivity is available.
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		public IReadOnlyList<TileConnection> GetFallback(TileConnection connection)
		{
			return this.tileToFallbackList[(int)connection] ?? NoFallbacks;
		}

		private void GenerateData()
		{
			// By default, every tile is considered a base permutation itself
			this.tileToBasePermutation = new TileConnection[StateCount];
			for (int i = 0; i < this.tileToBasePermutation.Length; i++)
			{
				this.tileToBasePermutation[i] = (TileConnection)i;
			}

			// Apply DontCare permutations, reducing the overall number of required connectivity states to 47.
			// See here: https://cloud.githubusercontent.com/assets/14859411/11279962/ccc1ac2e-8ef3-11e5-8e99-861b0d7a1c9a.png
			MapAllPermutations(this.tileToBasePermutation, Left | Right | BottomLeft | Bottom | BottomRight, TopLeft, TopRight);
			MapAllPermutations(this.tileToBasePermutation, TopLeft | Top | Left | BottomLeft | Bottom,       TopRight, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, Left | BottomLeft | Bottom,                       TopLeft, TopRight, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, TopLeft | Top | TopRight | Left | Right,          BottomLeft, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, Left | Right,                                     TopLeft, TopRight, BottomLeft, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, TopLeft | Top | Left,                             TopRight, BottomLeft, BottomRight);

			MapAllPermutations(this.tileToBasePermutation, Left,                                             TopLeft, TopRight, BottomLeft, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, Top | TopRight | Right | Bottom | BottomRight,    TopLeft, BottomLeft);
			MapAllPermutations(this.tileToBasePermutation, Right | Bottom | BottomRight,                     TopLeft, TopRight, BottomLeft);
			MapAllPermutations(this.tileToBasePermutation, Top | Bottom,                                     TopLeft, TopRight, BottomLeft, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, Bottom,                                           TopLeft, TopRight, BottomLeft, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, Top | TopRight | Right,                           TopLeft, BottomLeft, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, Right,                                            TopLeft, TopRight, BottomLeft, BottomRight);

			MapAllPermutations(this.tileToBasePermutation, Top,                                              TopLeft, TopRight, BottomLeft, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, None,                                             TopLeft, TopRight, BottomLeft, BottomRight);

			MapAllPermutations(this.tileToBasePermutation, Left | Right | BottomLeft | Bottom,               TopLeft, TopRight);
			MapAllPermutations(this.tileToBasePermutation, Left | Right | Bottom | BottomRight,              TopLeft, TopRight);
			MapAllPermutations(this.tileToBasePermutation, Left | Right | Bottom,                            TopLeft, TopRight);
			MapAllPermutations(this.tileToBasePermutation, Top | Left | BottomLeft | Bottom,                 TopRight, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, TopLeft | Top | Left | Bottom,                    TopRight, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, Top | Left | Bottom,                              TopRight, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, Left | Bottom,                                    TopLeft, TopRight, BottomRight);

			MapAllPermutations(this.tileToBasePermutation, Top | TopRight | Left | Right,                    BottomLeft, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, TopLeft | Top | Left | Right,                     BottomLeft, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, Top | Left | Right,                               BottomLeft, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, Top | Left,                                       TopRight, BottomLeft, BottomRight);
			MapAllPermutations(this.tileToBasePermutation, Top | Right | Bottom | BottomRight,               TopLeft, BottomLeft);
			MapAllPermutations(this.tileToBasePermutation, Top | TopRight | Right | Bottom,                  TopLeft, BottomLeft);
			MapAllPermutations(this.tileToBasePermutation, Top | Right | Bottom,                             TopLeft, BottomLeft);

			MapAllPermutations(this.tileToBasePermutation, Right | Bottom,                                   TopLeft, TopRight, BottomLeft);
			MapAllPermutations(this.tileToBasePermutation, Top | Right,                                      TopLeft, BottomLeft, BottomRight);

			// Initialize the fallback map with falling back to each tiles base permutation, 
			// and a sensible default otherwise
			List<TileConnection> basePermutationList = new List<TileConnection>();
			TileConnection[] directFallbacks = new TileConnection[StateCount];
			for (int i = 0; i < directFallbacks.Length; i++)
			{
				TileConnection connectivity = (TileConnection)i;
				TileConnection basePermutation = this.tileToBasePermutation[i];

				// If there is a base permutation to this connectivity, fall back to this one first
				if (connectivity != basePermutation)
				{
					directFallbacks[i] = basePermutation;
				}
				// Otherwise, fall back to the AutoTile's base tile, which is fully connected
				else
				{
					directFallbacks[i] = TileConnection.All;
					basePermutationList.Add(connectivity);
				}
			}
			this.basePermutation = basePermutationList.ToArray();

			// Define actual fallbacks in case a certain connectivity state is unavailable.
			// Unlike the DontCare states above, these fallbacks are not functionally equivalent,
			// so they will all look different degrees of wrong - but they'll work in some cases,
			// and it's better than not having a fallback at all.
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
			this.tileToFallbackList = CreateTransitiveMap(directFallbacks);
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
