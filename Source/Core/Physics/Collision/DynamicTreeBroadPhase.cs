/*
* Farseer Physics Engine based on Box2D.XNA port:
* Copyright (c) 2010 Ian Qvist
* 
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using System;
using FarseerPhysics.Dynamics;
using Duality;

namespace FarseerPhysics.Collision
{
	internal struct Pair : IComparable<Pair>
	{
		public int ProxyIdA;
		public int ProxyIdB;

		#region IComparable<Pair> Members

		public int CompareTo(Pair other)
		{
			if (this.ProxyIdA < other.ProxyIdA)
			{
				return -1;
			}
			if (this.ProxyIdA == other.ProxyIdA)
			{
				if (this.ProxyIdB < other.ProxyIdB)
				{
					return -1;
				}
				if (this.ProxyIdB == other.ProxyIdB)
				{
					return 0;
				}
			}

			return 1;
		}

		#endregion
	}

	/// <summary>
	/// The broad-phase is used for computing pairs and performing volume queries and ray casts.
	/// This broad-phase does not persist pairs. Instead, this reports potentially new pairs.
	/// It is up to the client to consume the new pairs and to track subsequent overlap.
	/// </summary>
	public class DynamicTreeBroadPhase : IBroadPhase
	{
		private int[] _moveBuffer;
		private int _moveCapacity;
		private int _moveCount;

		private Pair[] _pairBuffer;
		private int _pairCapacity;
		private int _pairCount;
		private int _proxyCount;
		private Func<int, bool> _queryCallback;
		private int _queryProxyId;
		private DynamicTree<FixtureProxy> _tree = new DynamicTree<FixtureProxy>();

		public DynamicTreeBroadPhase()
		{
			this._queryCallback = new Func<int, bool>(this.QueryCallback);

			this._pairCapacity = 16;
			this._pairBuffer = new Pair[this._pairCapacity];

			this._moveCapacity = 16;
			this._moveBuffer = new int[this._moveCapacity];
		}

		#region IBroadPhase Members

		/// <summary>
		/// Get the number of proxies.
		/// </summary>
		/// <value>The proxy count.</value>
		public int ProxyCount
		{
			get { return this._proxyCount; }
		}

		/// <summary>
		/// Create a proxy with an initial AABB. Pairs are not reported until
		/// UpdatePairs is called.
		/// </summary>
		/// <param name="proxy">The user data.</param>
		/// <returns></returns>
		public int AddProxy(ref FixtureProxy proxy)
		{
			int proxyId = this._tree.AddProxy(ref proxy.AABB, proxy);
			++this._proxyCount;
			BufferMove(proxyId);
			return proxyId;
		}

		/// <summary>
		/// Destroy a proxy. It is up to the client to remove any pairs.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		public void RemoveProxy(int proxyId)
		{
			UnBufferMove(proxyId);
			--this._proxyCount;
			this._tree.RemoveProxy(proxyId);
		}

		public void MoveProxy(int proxyId, ref AABB aabb, Vector2 displacement)
		{
			bool buffer = this._tree.MoveProxy(proxyId, ref aabb, displacement);
			if (buffer)
			{
				BufferMove(proxyId);
			}
		}

		/// <summary>
		/// Get the AABB for a proxy.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		/// <param name="aabb">The aabb.</param>
		public void GetFatAABB(int proxyId, out AABB aabb)
		{
			this._tree.GetFatAABB(proxyId, out aabb);
		}

		/// <summary>
		/// Get user data from a proxy. Returns null if the id is invalid.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		/// <returns></returns>
		public FixtureProxy GetProxy(int proxyId)
		{
			return this._tree.GetUserData(proxyId);
		}

		/// <summary>
		/// Test overlap of fat AABBs.
		/// </summary>
		/// <param name="proxyIdA">The proxy id A.</param>
		/// <param name="proxyIdB">The proxy id B.</param>
		/// <returns></returns>
		public bool TestOverlap(int proxyIdA, int proxyIdB)
		{
			AABB aabbA, aabbB;
			this._tree.GetFatAABB(proxyIdA, out aabbA);
			this._tree.GetFatAABB(proxyIdB, out aabbB);
			return AABB.TestOverlap(ref aabbA, ref aabbB);
		}

		/// <summary>
		/// Update the pairs. This results in pair callbacks. This can only add pairs.
		/// </summary>
		/// <param name="callback">The callback.</param>
		public void UpdatePairs(BroadphaseDelegate callback)
		{
			// Reset pair buffer
			this._pairCount = 0;

			// Perform tree queries for all moving proxies.
			for (int j = 0; j < this._moveCount; ++j)
			{
				this._queryProxyId = this._moveBuffer[j];
				if (this._queryProxyId == -1)
				{
					continue;
				}

				// We have to query the tree with the fat AABB so that
				// we don't fail to create a pair that may touch later.
				AABB fatAABB;
				this._tree.GetFatAABB(this._queryProxyId, out fatAABB);

				// Query tree, create pairs and add them pair buffer.
				this._tree.Query(this._queryCallback, ref fatAABB);
			}

			// Reset move buffer
			this._moveCount = 0;

			// Sort the pair buffer to expose duplicates.
			Array.Sort(this._pairBuffer, 0, this._pairCount);

			// Send the pairs back to the client.
			int i = 0;
			while (i < this._pairCount)
			{
				Pair primaryPair = this._pairBuffer[i];
				FixtureProxy userDataA = this._tree.GetUserData(primaryPair.ProxyIdA);
				FixtureProxy userDataB = this._tree.GetUserData(primaryPair.ProxyIdB);

				callback(ref userDataA, ref userDataB);
				++i;

				// Skip any duplicate pairs.
				while (i < this._pairCount)
				{
					Pair pair = this._pairBuffer[i];
					if (pair.ProxyIdA != primaryPair.ProxyIdA || pair.ProxyIdB != primaryPair.ProxyIdB)
					{
						break;
					}
					++i;
				}
			}

			// Try to keep the tree balanced.
			this._tree.Rebalance(4);
		}

		/// <summary>
		/// Query an AABB for overlapping proxies. The callback class
		/// is called for each proxy that overlaps the supplied AABB.
		/// </summary>
		/// <param name="callback">The callback.</param>
		/// <param name="aabb">The aabb.</param>
		public void Query(Func<int, bool> callback, ref AABB aabb)
		{
			this._tree.Query(callback, ref aabb);
		}

		/// <summary>
		/// Ray-cast against the proxies in the tree. This relies on the callback
		/// to perform a exact ray-cast in the case were the proxy contains a shape.
		/// The callback also performs the any collision filtering. This has performance
		/// roughly equal to k * log(n), where k is the number of collisions and n is the
		/// number of proxies in the tree.
		/// </summary>
		/// <param name="callback">A callback class that is called for each proxy that is hit by the ray.</param>
		/// <param name="input">The ray-cast input data. The ray extends from p1 to p1 + maxFraction * (p2 - p1).</param>
		public void RayCast(Func<RayCastInput, int, float> callback, ref RayCastInput input)
		{
			this._tree.RayCast(callback, ref input);
		}

		public void TouchProxy(int proxyId)
		{
			BufferMove(proxyId);
		}

		#endregion

		/// <summary>
		/// Compute the height of the embedded tree.
		/// </summary>
		/// <returns></returns>
		public int ComputeHeight()
		{
			return this._tree.ComputeHeight();
		}

		private void BufferMove(int proxyId)
		{
			if (this._moveCount == this._moveCapacity)
			{
				int[] oldBuffer = this._moveBuffer;
				this._moveCapacity *= 2;
				this._moveBuffer = new int[this._moveCapacity];
				Array.Copy(oldBuffer, this._moveBuffer, this._moveCount);
			}

			this._moveBuffer[this._moveCount] = proxyId;
			++this._moveCount;
		}

		private void UnBufferMove(int proxyId)
		{
			for (int i = 0; i < this._moveCount; ++i)
			{
				if (this._moveBuffer[i] == proxyId)
				{
					this._moveBuffer[i] = -1;
					return;
				}
			}
		}

		private bool QueryCallback(int proxyId)
		{
			// A proxy cannot form a pair with itself.
			if (proxyId == this._queryProxyId)
			{
				return true;
			}

			// Grow the pair buffer as needed.
			if (this._pairCount == this._pairCapacity)
			{
				Pair[] oldBuffer = this._pairBuffer;
				this._pairCapacity *= 2;
				this._pairBuffer = new Pair[this._pairCapacity];
				Array.Copy(oldBuffer, this._pairBuffer, this._pairCount);
			}

			this._pairBuffer[this._pairCount].ProxyIdA = Math.Min(proxyId, this._queryProxyId);
			this._pairBuffer[this._pairCount].ProxyIdB = Math.Max(proxyId, this._queryProxyId);
			++this._pairCount;

			return true;
		}
	}
}