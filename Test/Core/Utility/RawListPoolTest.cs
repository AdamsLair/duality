using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class RawListPoolTest
	{
		[Test] public void RentReturn()
		{
			RawListPool<int> intPool = new RawListPool<int>();

			// Rent a few lists with varying capacities, and do so a few times
			for (int n = 0; n < 5; n++)
			{
				int[] capacities = new int[] { 0, 7, 3, 10, 5, 19 };
				for (int i = 0; i < capacities.Length; i++)
				{
					RawList<int> list = intPool.Rent(capacities[i]);

					// Asset that they're empty, but match the required min capacity
					Assert.AreEqual(0, list.Count);
					Assert.GreaterOrEqual(list.Capacity, capacities[i]);

					// Add a few elements and return the list
					for (int k = 0; k < capacities[i]; k++)
					{
						list.Add(k + 1);
					}

					intPool.Return(list);

					// Assert that the list is again empty after return, but not shrinked in capacity
					Assert.AreEqual(0, list.Count);
					Assert.GreaterOrEqual(list.Capacity, capacities[i]);
				}
			}
		}
		[Test] public void RentReset()
		{
			RawListPool<int> intPool = new RawListPool<int>();
			List<RawList<int>> previouslyRented = new List<RawList<int>>();
			
			// Rent a few lists with varying capacities, and do so a few times
			for (int n = 0; n < 5; n++)
			{
				int[] capacities = new int[] { 0, 7, 3, 10, 5, 19 };
				for (int i = 0; i < capacities.Length; i++)
				{
					RawList<int> list = intPool.Rent(capacities[i]);
					previouslyRented.Add(list);

					// Asset that they're empty, but match the required min capacity
					Assert.AreEqual(0, list.Count);
					Assert.GreaterOrEqual(list.Capacity, capacities[i]);

					// Add a few elements
					for (int k = 0; k < capacities[i]; k++)
					{
						list.Add(k + 1);
					}
				}

				// Reset the pool. We haven't returned any lists so far.
				intPool.Reset();

				// Assert that, after the reset, all previously rented lists
				// have been cleared, but still have their required min capacity.
				for (int i = 0; i < capacities.Length; i++)
				{
					RawList<int> list = previouslyRented[i];
					Assert.AreEqual(0, list.Count);
					Assert.GreaterOrEqual(list.Capacity, capacities[i]);
				}
			}
		}
	}
}
