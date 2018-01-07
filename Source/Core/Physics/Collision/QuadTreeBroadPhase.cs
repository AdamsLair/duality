using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using Duality;

public class QuadTreeBroadPhase : IBroadPhase
{
	private const int TreeUpdateThresh = 10000;
	private int _currID;
	private Dictionary<int, Element<FixtureProxy>> _idRegister;
	private List<Element<FixtureProxy>> _moveBuffer;
	private List<Pair> _pairBuffer;
	private QuadTree<FixtureProxy> _quadTree;
	private int _treeMoveNum;

	/// <summary>
	/// Creates a new quad tree broadphase with the specified span.
	/// </summary>
	/// <param name="span">the maximum span of the tree (world size)</param>
	public QuadTreeBroadPhase(AABB span)
	{
		this._quadTree = new QuadTree<FixtureProxy>(span, 5, 10);
		this._idRegister = new Dictionary<int, Element<FixtureProxy>>();
		this._moveBuffer = new List<Element<FixtureProxy>>();
		this._pairBuffer = new List<Pair>();
	}

	#region IBroadPhase Members

	///<summary>
	/// The number of proxies
	///</summary>
	public int ProxyCount
	{
		get { return this._idRegister.Count; }
	}

	public void GetFatAABB(int proxyID, out AABB aabb)
	{
		if (this._idRegister.ContainsKey(proxyID))
			aabb = this._idRegister[proxyID].Span;
		else
			throw new KeyNotFoundException("proxyID not found in register");
	}

	public void UpdatePairs(BroadphaseDelegate callback)
	{
		this._pairBuffer.Clear();
		foreach (Element<FixtureProxy> qtnode in this._moveBuffer)
		{
			// Query tree, create pairs and add them pair buffer.
			Query(proxyID => PairBufferQueryCallback(proxyID, qtnode.Value.ProxyId), ref qtnode.Span);
		}
		this._moveBuffer.Clear();

		// Sort the pair buffer to expose duplicates.
		this._pairBuffer.Sort();

		// Send the pairs back to the client.
		int i = 0;
		while (i < this._pairBuffer.Count)
		{
			Pair primaryPair = this._pairBuffer[i];
			FixtureProxy userDataA = GetProxy(primaryPair.ProxyIdA);
			FixtureProxy userDataB = GetProxy(primaryPair.ProxyIdB);

			callback(ref userDataA, ref userDataB);
			++i;

			// Skip any duplicate pairs.
			while (i < this._pairBuffer.Count && this._pairBuffer[i].ProxyIdA == primaryPair.ProxyIdA &&
				   this._pairBuffer[i].ProxyIdB == primaryPair.ProxyIdB)
				++i;
		}
	}

	/// <summary>
	/// Test overlap of fat AABBs.
	/// </summary>
	/// <param name="proxyIdA">The proxy id A.</param>
	/// <param name="proxyIdB">The proxy id B.</param>
	/// <returns></returns>
	public bool TestOverlap(int proxyIdA, int proxyIdB)
	{
		AABB aabb1;
		AABB aabb2;
		GetFatAABB(proxyIdA, out aabb1);
		GetFatAABB(proxyIdB, out aabb2);
		return AABB.TestOverlap(ref aabb1, ref aabb2);
	}

	public int AddProxy(ref FixtureProxy proxy)
	{
		int proxyID = this._currID++;
		proxy.ProxyId = proxyID;
		AABB aabb = Fatten(ref proxy.AABB);
		Element<FixtureProxy> qtnode = new Element<FixtureProxy>(proxy, aabb);

		this._idRegister.Add(proxyID, qtnode);
		this._quadTree.AddNode(qtnode);

		return proxyID;
	}

	public void RemoveProxy(int proxyId)
	{
		if (this._idRegister.ContainsKey(proxyId))
		{
			Element<FixtureProxy> qtnode = this._idRegister[proxyId];
			UnbufferMove(qtnode);
			this._idRegister.Remove(proxyId);
			this._quadTree.RemoveNode(qtnode);
		}
		else
			throw new KeyNotFoundException("proxyID not found in register");
	}

	public void MoveProxy(int proxyId, ref AABB aabb, Vector2 displacement)
	{
		AABB fatAABB;
		GetFatAABB(proxyId, out fatAABB);

		//exit if movement is within fat aabb
		if (fatAABB.Contains(ref aabb))
			return;

		// Extend AABB.
		AABB b = aabb;
		Vector2 r = new Vector2(Settings.AABBExtension, Settings.AABBExtension);
		b.LowerBound = b.LowerBound - r;
		b.UpperBound = b.UpperBound + r;

		// Predict AABB displacement.
		Vector2 d = Settings.AABBMultiplier * displacement;

		if (d.X < 0.0f)
			b.LowerBound.X += d.X;
		else
			b.UpperBound.X += d.X;

		if (d.Y < 0.0f)
			b.LowerBound.Y += d.Y;
		else
			b.UpperBound.Y += d.Y;


		Element<FixtureProxy> qtnode = this._idRegister[proxyId];
		qtnode.Value.AABB = b; //not neccesary for QTree, but might be accessed externally
		qtnode.Span = b;

		ReinsertNode(qtnode);

		BufferMove(qtnode);
	}

	public FixtureProxy GetProxy(int proxyId)
	{
		if (this._idRegister.ContainsKey(proxyId))
			return this._idRegister[proxyId].Value;
		else
			throw new KeyNotFoundException("proxyID not found in register");
	}

	public void TouchProxy(int proxyId)
	{
		if (this._idRegister.ContainsKey(proxyId))
			BufferMove(this._idRegister[proxyId]);
		else
			throw new KeyNotFoundException("proxyID not found in register");
	}

	public void Query(Func<int, bool> callback, ref AABB query)
	{
		this._quadTree.QueryAABB(TransformPredicate(callback), ref query);
	}

	public void RayCast(Func<RayCastInput, int, float> callback, ref RayCastInput input)
	{
		this._quadTree.RayCast(TransformRayCallback(callback), ref input);
	}

	#endregion

	private AABB Fatten(ref AABB aabb)
	{
		Vector2 r = new Vector2(Settings.AABBExtension, Settings.AABBExtension);
		return new AABB(aabb.LowerBound - r, aabb.UpperBound + r);
	}

	private Func<Element<FixtureProxy>, bool> TransformPredicate(Func<int, bool> idPredicate)
	{
		Func<Element<FixtureProxy>, bool> qtPred = qtnode => idPredicate(qtnode.Value.ProxyId);
		return qtPred;
	}

	private Func<RayCastInput, Element<FixtureProxy>, float> TransformRayCallback(
		Func<RayCastInput, int, float> callback)
	{
		Func<RayCastInput, Element<FixtureProxy>, float> newCallback =
			(input, qtnode) => callback(input, qtnode.Value.ProxyId);
		return newCallback;
	}

	private bool PairBufferQueryCallback(int proxyID, int baseID)
	{
		// A proxy cannot form a pair with itself.
		if (proxyID == baseID)
			return true;

		Pair p = new Pair();
		p.ProxyIdA = Math.Min(proxyID, baseID);
		p.ProxyIdB = Math.Max(proxyID, baseID);
		this._pairBuffer.Add(p);

		return true;
	}

	private void ReconstructTree()
	{
		//this is faster than _quadTree.Reconstruct(), since the quadtree method runs a recusive query to find all nodes.
		this._quadTree.Clear();
		foreach (Element<FixtureProxy> elem in this._idRegister.Values)
			this._quadTree.AddNode(elem);
	}

	private void ReinsertNode(Element<FixtureProxy> qtnode)
	{
		this._quadTree.RemoveNode(qtnode);
		this._quadTree.AddNode(qtnode);

		if (++this._treeMoveNum > TreeUpdateThresh)
		{
			ReconstructTree();
			this._treeMoveNum = 0;
		}
	}

	private void BufferMove(Element<FixtureProxy> proxy)
	{
		this._moveBuffer.Add(proxy);
	}

	private void UnbufferMove(Element<FixtureProxy> proxy)
	{
		this._moveBuffer.Remove(proxy);
	}
}