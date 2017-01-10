using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;

using NUnit.Framework;


namespace Duality.Tests.Resources
{
	[TestFixture]
	public class FontKerningLookupTest
	{
		[Test] public void EmptyLookup()
		{
			// Initializing with both an empty array and null should be fine
			Assert.DoesNotThrow(() => new FontKerningLookup(new FontKerningPair[0]));
			Assert.DoesNotThrow(() => new FontKerningLookup(null));

			// Check if the lookup works and expectedly returns zero values
			FontKerningLookup lookup = new FontKerningLookup(null);
			Assert.AreEqual(0.0f, lookup.GetAdvanceOffset('a', 'b'));
			Assert.AreEqual(0.0f, lookup.GetAdvanceOffset('b', 'a'));
		}
		[Test] public void SingleItemLookup()
		{
			// A lookup where each pair is completely unique - the easiest case
			FontKerningLookup lookup = new FontKerningLookup(new FontKerningPair[]
			{
				new FontKerningPair('a', 'b', 1.0f),
				new FontKerningPair('c', 'd', 2.0f),
				new FontKerningPair('e', 'f', 3.0f),
			});

			Assert.AreEqual(0.0f, lookup.GetAdvanceOffset('x', 'y'));

			Assert.AreEqual(1.0f, lookup.GetAdvanceOffset('a', 'b'));
			Assert.AreEqual(2.0f, lookup.GetAdvanceOffset('c', 'd'));
			Assert.AreEqual(3.0f, lookup.GetAdvanceOffset('e', 'f'));
		}
		[Test] public void MultiItemLookup()
		{
			// A lookup where there are multiple pairs sharing the same items
			FontKerningPair[] pairs = new FontKerningPair[]
			{
				new FontKerningPair('a', 'a', 1.0f),
				new FontKerningPair('a', 'b', 2.0f),
				new FontKerningPair('a', 'c', 3.0f),

				new FontKerningPair('b', 'a', 4.0f),
				new FontKerningPair('b', 'b', 5.0f),
				new FontKerningPair('b', 'c', 6.0f),

				new FontKerningPair('c', 'a', 7.0f),
				new FontKerningPair('c', 'b', 8.0f),
				new FontKerningPair('c', 'c', 9.0f),
			};

			// Shuffle the pairs array, as they are no expected to be in order
			Random rnd = new Random(1);
			rnd.Shuffle(pairs);

			FontKerningLookup lookup = new FontKerningLookup(pairs);

			Assert.AreEqual(0.0f, lookup.GetAdvanceOffset('x', 'y'));

			Assert.AreEqual(1.0f, lookup.GetAdvanceOffset('a', 'a'));
			Assert.AreEqual(2.0f, lookup.GetAdvanceOffset('a', 'b'));
			Assert.AreEqual(3.0f, lookup.GetAdvanceOffset('a', 'c'));

			Assert.AreEqual(4.0f, lookup.GetAdvanceOffset('b', 'a'));
			Assert.AreEqual(5.0f, lookup.GetAdvanceOffset('b', 'b'));
			Assert.AreEqual(6.0f, lookup.GetAdvanceOffset('b', 'c'));

			Assert.AreEqual(7.0f, lookup.GetAdvanceOffset('c', 'a'));
			Assert.AreEqual(8.0f, lookup.GetAdvanceOffset('c', 'b'));
			Assert.AreEqual(9.0f, lookup.GetAdvanceOffset('c', 'c'));
		}
	}
}
