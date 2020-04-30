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
using System.Collections.Generic;
using System.Diagnostics;
using FarseerPhysics.Common;
using Duality;

namespace FarseerPhysics.Collision
{
	/// <summary>
	/// A node in the dynamic tree. The client does not interact with this directly.
	/// </summary>
	internal struct DynamicTreeNode<T>
	{
		/// <summary>
		/// This is the fattened AABB.
		/// </summary>
		internal AABB AABB;

		internal int Child1;
		internal int Child2;

		internal int LeafCount;
		internal int ParentOrNext;
		internal T UserData;

		internal bool IsLeaf()
		{
			return this.Child1 == DynamicTree<T>.NullNode;
		}
	}

	/// <summary>
	/// A dynamic tree arranges data in a binary tree to accelerate
	/// queries such as volume queries and ray casts. Leafs are proxies
	/// with an AABB. In the tree we expand the proxy AABB by Settings.b2_fatAABBFactor
	/// so that the proxy AABB is bigger than the client object. This allows the client
	/// object to move by small amounts without triggering a tree update.
	///
	/// Nodes are pooled and relocatable, so we use node indices rather than pointers.
	/// </summary>
	public class DynamicTree<T>
	{
		internal const int NullNode = -1;
		private static Stack<int> _stack = new Stack<int>(256);
		private int _freeList;
		private int _insertionCount;
		private int _nodeCapacity;
		private int _nodeCount;
		private DynamicTreeNode<T>[] _nodes;

		/// <summary>
		/// This is used incrementally traverse the tree for re-balancing.
		/// </summary>
		private int _path;

		private int _root;

		/// <summary>
		/// Constructing the tree initializes the node pool.
		/// </summary>
		public DynamicTree()
		{
			this._root = NullNode;

			this._nodeCapacity = 16;
			this._nodes = new DynamicTreeNode<T>[this._nodeCapacity];

			// Build a linked list for the free list.
			for (int i = 0; i < this._nodeCapacity - 1; ++i)
			{
				this._nodes[i].ParentOrNext = i + 1;
			}
			this._nodes[this._nodeCapacity - 1].ParentOrNext = NullNode;
		}

		/// <summary>
		/// Create a proxy in the tree as a leaf node. We return the index
		/// of the node instead of a pointer so that we can grow
		/// the node pool.        
		/// /// </summary>
		/// <param name="aabb">The aabb.</param>
		/// <param name="userData">The user data.</param>
		/// <returns>Index of the created proxy</returns>
		public int AddProxy(ref AABB aabb, T userData)
		{
			int proxyId = AllocateNode();

			// Fatten the aabb.
			Vector2 r = new Vector2(Settings.AABBExtension, Settings.AABBExtension);
			this._nodes[proxyId].AABB.LowerBound = aabb.LowerBound - r;
			this._nodes[proxyId].AABB.UpperBound = aabb.UpperBound + r;
			this._nodes[proxyId].UserData = userData;
			this._nodes[proxyId].LeafCount = 1;

			InsertLeaf(proxyId);

			return proxyId;
		}

		/// <summary>
		/// Destroy a proxy. This asserts if the id is invalid.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		public void RemoveProxy(int proxyId)
		{
			Debug.Assert(0 <= proxyId && proxyId < this._nodeCapacity);
			Debug.Assert(this._nodes[proxyId].IsLeaf());

			RemoveLeaf(proxyId);
			FreeNode(proxyId);
		}

		/// <summary>
		/// Move a proxy with a swepted AABB. If the proxy has moved outside of its fattened AABB,
		/// then the proxy is removed from the tree and re-inserted. Otherwise
		/// the function returns immediately.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		/// <param name="aabb">The aabb.</param>
		/// <param name="displacement">The displacement.</param>
		/// <returns>true if the proxy was re-inserted.</returns>
		public bool MoveProxy(int proxyId, ref AABB aabb, Vector2 displacement)
		{
			Debug.Assert(0 <= proxyId && proxyId < this._nodeCapacity);

			Debug.Assert(this._nodes[proxyId].IsLeaf());

			if (this._nodes[proxyId].AABB.Contains(ref aabb))
			{
				return false;
			}

			RemoveLeaf(proxyId);

			// Extend AABB.
			AABB b = aabb;
			Vector2 r = new Vector2(Settings.AABBExtension, Settings.AABBExtension);
			b.LowerBound = b.LowerBound - r;
			b.UpperBound = b.UpperBound + r;

			// Predict AABB displacement.
			Vector2 d = Settings.AABBMultiplier * displacement;

			if (d.X < 0.0f)
			{
				b.LowerBound.X += d.X;
			}
			else
			{
				b.UpperBound.X += d.X;
			}

			if (d.Y < 0.0f)
			{
				b.LowerBound.Y += d.Y;
			}
			else
			{
				b.UpperBound.Y += d.Y;
			}

			this._nodes[proxyId].AABB = b;

			InsertLeaf(proxyId);
			return true;
		}

		/// <summary>
		/// Perform some iterations to re-balance the tree.
		/// </summary>
		/// <param name="iterations">The iterations.</param>
		public void Rebalance(int iterations)
		{
			if (this._root == NullNode)
			{
				return;
			}

			// Rebalance the tree by removing and re-inserting leaves.
			for (int i = 0; i < iterations; ++i)
			{
				int node = this._root;

				int bit = 0;
				while (this._nodes[node].IsLeaf() == false)
				{
					// Child selector based on a bit in the path
					int selector = (this._path >> bit) & 1;

					// Select the child nod
					node = (selector == 0) ? this._nodes[node].Child1 : this._nodes[node].Child2;

					// Keep bit between 0 and 31 because _path has 32 bits
					// bit = (bit + 1) % 31
					bit = (bit + 1) & 0x1F;
				}
				++this._path;

				RemoveLeaf(node);
				InsertLeaf(node);
			}
		}

		/// <summary>
		/// Get proxy user data.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		/// <returns>the proxy user data or 0 if the id is invalid.</returns>
		public T GetUserData(int proxyId)
		{
			Debug.Assert(0 <= proxyId && proxyId < this._nodeCapacity);
			return this._nodes[proxyId].UserData;
		}

		/// <summary>
		/// Get the fat AABB for a proxy.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		/// <param name="fatAABB">The fat AABB.</param>
		public void GetFatAABB(int proxyId, out AABB fatAABB)
		{
			Debug.Assert(0 <= proxyId && proxyId < this._nodeCapacity);
			fatAABB = this._nodes[proxyId].AABB;
		}

		/// <summary>
		/// Compute the height of the binary tree in O(N) time. Should not be
		/// called often.
		/// </summary>
		public int ComputeHeight()
		{
			return ComputeHeight(this._root);
		}

		/// <summary>
		/// Query an AABB for overlapping proxies. The callback class
		/// is called for each proxy that overlaps the supplied AABB.
		/// </summary>
		/// <param name="callback">The callback.</param>
		/// <param name="aabb">The aabb.</param>
		public void Query(Func<int, bool> callback, ref AABB aabb)
		{
			_stack.Clear();
			_stack.Push(this._root);

			while (_stack.Count > 0)
			{
				int nodeId = _stack.Pop();
				if (nodeId == NullNode)
				{
					continue;
				}

				DynamicTreeNode<T> node = this._nodes[nodeId];

				if (AABB.TestOverlap(ref node.AABB, ref aabb))
				{
					if (node.IsLeaf())
					{
						bool proceed = callback(nodeId);
						if (proceed == false)
						{
							return;
						}
					}
					else
					{
						_stack.Push(node.Child1);
						_stack.Push(node.Child2);
					}
				}
			}
		}

		/// <summary>
		/// Ray-cast against the proxies in the tree. This relies on the callback
		/// to perform a exact ray-cast in the case were the proxy contains a Shape.
		/// The callback also performs the any collision filtering. This has performance
		/// roughly equal to k * log(n), where k is the number of collisions and n is the
		/// number of proxies in the tree.
		/// </summary>
		/// <param name="callback">A callback class that is called for each proxy that is hit by the ray.</param>
		/// <param name="input">The ray-cast input data. The ray extends from p1 to p1 + maxFraction * (p2 - p1).</param>
		public void RayCast(Func<RayCastInput, int, float> callback, ref RayCastInput input)
		{
			Vector2 p1 = input.Point1;
			Vector2 p2 = input.Point2;
			Vector2 r = p2 - p1;
			Debug.Assert(r.LengthSquared > 0.0f);
			r.Normalize();

			// v is perpendicular to the segment.
			Vector2 absV = MathUtils.Abs(new Vector2(-r.Y, r.X));

			// Separating axis for segment (Gino, p80).
			// |dot(v, p1 - c)| > dot(|v|, h)

			float maxFraction = input.MaxFraction;

			// Build a bounding box for the segment.
			AABB segmentAABB = new AABB();
			{
				Vector2 t = p1 + maxFraction * (p2 - p1);
				Vector2.Min(ref p1, ref t, out segmentAABB.LowerBound);
				Vector2.Max(ref p1, ref t, out segmentAABB.UpperBound);
			}

			_stack.Clear();
			_stack.Push(this._root);

			while (_stack.Count > 0)
			{
				int nodeId = _stack.Pop();
				if (nodeId == NullNode)
				{
					continue;
				}

				DynamicTreeNode<T> node = this._nodes[nodeId];

				if (AABB.TestOverlap(ref node.AABB, ref segmentAABB) == false)
				{
					continue;
				}

				// Separating axis for segment (Gino, p80).
				// |dot(v, p1 - c)| > dot(|v|, h)
				Vector2 c = node.AABB.Center;
				Vector2 h = node.AABB.Extents;
				float separation = Math.Abs(Vector2.Dot(new Vector2(-r.Y, r.X), p1 - c)) - Vector2.Dot(absV, h);
				if (separation > 0.0f)
				{
					continue;
				}

				if (node.IsLeaf())
				{
					RayCastInput subInput;
					subInput.Point1 = input.Point1;
					subInput.Point2 = input.Point2;
					subInput.MaxFraction = maxFraction;

					float value = callback(subInput, nodeId);

					if (value == 0.0f)
					{
						// the client has terminated the raycast.
						return;
					}

					if (value > 0.0f)
					{
						// Update segment bounding box.
						maxFraction = value;
						Vector2 t = p1 + maxFraction * (p2 - p1);
						segmentAABB.LowerBound = Vector2.Min(p1, t);
						segmentAABB.UpperBound = Vector2.Max(p1, t);
					}
				}
				else
				{
					_stack.Push(node.Child1);
					_stack.Push(node.Child2);
				}
			}
		}

		private int CountLeaves(int nodeId)
		{
			if (nodeId == NullNode)
			{
				return 0;
			}

			Debug.Assert(0 <= nodeId && nodeId < this._nodeCapacity);
			DynamicTreeNode<T> node = this._nodes[nodeId];

			if (node.IsLeaf())
			{
				Debug.Assert(node.LeafCount == 1);
				return 1;
			}

			int count1 = CountLeaves(node.Child1);
			int count2 = CountLeaves(node.Child2);
			int count = count1 + count2;
			Debug.Assert(count == node.LeafCount);
			return count;
		}

		private void Validate()
		{
			CountLeaves(this._root);
		}

		private int AllocateNode()
		{
			// Expand the node pool as needed.
			if (this._freeList == NullNode)
			{
				Debug.Assert(this._nodeCount == this._nodeCapacity);

				// The free list is empty. Rebuild a bigger pool.
				DynamicTreeNode<T>[] oldNodes = this._nodes;
				this._nodeCapacity *= 2;
				this._nodes = new DynamicTreeNode<T>[this._nodeCapacity];
				Array.Copy(oldNodes, this._nodes, this._nodeCount);

				// Build a linked list for the free list. The parent
				// pointer becomes the "next" pointer.
				for (int i = this._nodeCount; i < this._nodeCapacity - 1; ++i)
				{
					this._nodes[i].ParentOrNext = i + 1;
				}
				this._nodes[this._nodeCapacity - 1].ParentOrNext = NullNode;
				this._freeList = this._nodeCount;
			}

			// Peel a node off the free list.
			int nodeId = this._freeList;
			this._freeList = this._nodes[nodeId].ParentOrNext;
			this._nodes[nodeId].ParentOrNext = NullNode;
			this._nodes[nodeId].Child1 = NullNode;
			this._nodes[nodeId].Child2 = NullNode;
			this._nodes[nodeId].LeafCount = 0;
			++this._nodeCount;
			return nodeId;
		}

		private void FreeNode(int nodeId)
		{
			Debug.Assert(0 <= nodeId && nodeId < this._nodeCapacity);
			Debug.Assert(0 < this._nodeCount);
			this._nodes[nodeId].ParentOrNext = this._freeList;
			this._freeList = nodeId;
			--this._nodeCount;
		}

		private void InsertLeaf(int leaf)
		{
			++this._insertionCount;

			if (this._root == NullNode)
			{
				this._root = leaf;
				this._nodes[this._root].ParentOrNext = NullNode;
				return;
			}

			// Find the best sibling for this node
			AABB leafAABB = this._nodes[leaf].AABB;
			int sibling = this._root;
			while (this._nodes[sibling].IsLeaf() == false)
			{
				int child1 = this._nodes[sibling].Child1;
				int child2 = this._nodes[sibling].Child2;

				// Expand the node's AABB.
				this._nodes[sibling].AABB.Combine(ref leafAABB);
				this._nodes[sibling].LeafCount += 1;

				float siblingArea = this._nodes[sibling].AABB.Perimeter;
				AABB parentAABB = new AABB();
				parentAABB.Combine(ref this._nodes[sibling].AABB, ref leafAABB);
				float parentArea = parentAABB.Perimeter;
				float cost1 = 2.0f * parentArea;

				float inheritanceCost = 2.0f * (parentArea - siblingArea);

				float cost2;
				if (this._nodes[child1].IsLeaf())
				{
					AABB aabb = new AABB();
					aabb.Combine(ref leafAABB, ref this._nodes[child1].AABB);
					cost2 = aabb.Perimeter + inheritanceCost;
				}
				else
				{
					AABB aabb = new AABB();
					aabb.Combine(ref leafAABB, ref this._nodes[child1].AABB);
					float oldArea = this._nodes[child1].AABB.Perimeter;
					float newArea = aabb.Perimeter;
					cost2 = (newArea - oldArea) + inheritanceCost;
				}

				float cost3;
				if (this._nodes[child2].IsLeaf())
				{
					AABB aabb = new AABB();
					aabb.Combine(ref leafAABB, ref this._nodes[child2].AABB);
					cost3 = aabb.Perimeter + inheritanceCost;
				}
				else
				{
					AABB aabb = new AABB();
					aabb.Combine(ref leafAABB, ref this._nodes[child2].AABB);
					float oldArea = this._nodes[child2].AABB.Perimeter;
					float newArea = aabb.Perimeter;
					cost3 = newArea - oldArea + inheritanceCost;
				}

				// Descend according to the minimum cost.
				if (cost1 < cost2 && cost1 < cost3)
				{
					break;
				}

				// Expand the node's AABB to account for the new leaf.
				this._nodes[sibling].AABB.Combine(ref leafAABB);

				// Descend
				if (cost2 < cost3)
				{
					sibling = child1;
				}
				else
				{
					sibling = child2;
				}
			}

			// Create a new parent for the siblings.
			int oldParent = this._nodes[sibling].ParentOrNext;
			int newParent = AllocateNode();
			this._nodes[newParent].ParentOrNext = oldParent;
			this._nodes[newParent].UserData = default(T);
			this._nodes[newParent].AABB.Combine(ref leafAABB, ref this._nodes[sibling].AABB);
			this._nodes[newParent].LeafCount = this._nodes[sibling].LeafCount + 1;

			if (oldParent != NullNode)
			{
				// The sibling was not the root.
				if (this._nodes[oldParent].Child1 == sibling)
				{
					this._nodes[oldParent].Child1 = newParent;
				}
				else
				{
					this._nodes[oldParent].Child2 = newParent;
				}

				this._nodes[newParent].Child1 = sibling;
				this._nodes[newParent].Child2 = leaf;
				this._nodes[sibling].ParentOrNext = newParent;
				this._nodes[leaf].ParentOrNext = newParent;
			}
			else
			{
				// The sibling was the root.
				this._nodes[newParent].Child1 = sibling;
				this._nodes[newParent].Child2 = leaf;
				this._nodes[sibling].ParentOrNext = newParent;
				this._nodes[leaf].ParentOrNext = newParent;
				this._root = newParent;
			}
		}

		private void RemoveLeaf(int leaf)
		{
			if (leaf == this._root)
			{
				this._root = NullNode;
				return;
			}

			int parent = this._nodes[leaf].ParentOrNext;
			int grandParent = this._nodes[parent].ParentOrNext;
			int sibling;
			if (this._nodes[parent].Child1 == leaf)
			{
				sibling = this._nodes[parent].Child2;
			}
			else
			{
				sibling = this._nodes[parent].Child1;
			}

			if (grandParent != NullNode)
			{
				// Destroy parent and connect sibling to grandParent.
				if (this._nodes[grandParent].Child1 == parent)
				{
					this._nodes[grandParent].Child1 = sibling;
				}
				else
				{
					this._nodes[grandParent].Child2 = sibling;
				}
				this._nodes[sibling].ParentOrNext = grandParent;
				FreeNode(parent);

				// Adjust ancestor bounds.
				parent = grandParent;
				while (parent != NullNode)
				{
					this._nodes[parent].AABB.Combine(ref this._nodes[this._nodes[parent].Child1].AABB,
												ref this._nodes[this._nodes[parent].Child2].AABB);

					Debug.Assert(this._nodes[parent].LeafCount > 0);
					this._nodes[parent].LeafCount -= 1;

					parent = this._nodes[parent].ParentOrNext;
				}
			}
			else
			{
				this._root = sibling;
				this._nodes[sibling].ParentOrNext = NullNode;
				FreeNode(parent);
			}
		}

		private int ComputeHeight(int nodeId)
		{
			if (nodeId == NullNode)
			{
				return 0;
			}

			Debug.Assert(0 <= nodeId && nodeId < this._nodeCapacity);
			DynamicTreeNode<T> node = this._nodes[nodeId];
			int height1 = ComputeHeight(node.Child1);
			int height2 = ComputeHeight(node.Child2);
			return 1 + Math.Max(height1, height2);
		}
	}
}