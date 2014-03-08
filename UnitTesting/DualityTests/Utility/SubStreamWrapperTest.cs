using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class SubStreamWrapperTest
	{
		[Test] public void Read()
		{
			byte[] baseData = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();

			using (MemoryStream baseStream = new MemoryStream(baseData))
			using (Stream stream = baseStream.SubStream())
			{
				byte[] buffer = new byte[1024 * 4];
				int bytesRead = stream.Read(buffer, 0, buffer.Length);

				Assert.AreEqual(baseData.Length, bytesRead);
				Assert.IsTrue(baseData.SequenceEqual(buffer.Take(bytesRead)));
			}
		}
		[Test] public void Write()
		{
			byte[] baseData = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
			using (MemoryStream baseStream = new MemoryStream())
			{
				using (Stream stream = baseStream.SubStream())
				{
					stream.Write(baseData, 0, baseData.Length);
				}
				baseStream.Position = 0;

				byte[] buffer = new byte[1024 * 4];
				int bytesRead = baseStream.Read(buffer, 0, buffer.Length);

				Assert.AreEqual(baseData.Length, bytesRead);
				Assert.IsTrue(baseData.SequenceEqual(buffer.Take(baseData.Length)));
			}
		}
		[Test] public void SubSection()
		{
			byte[] baseData = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
			byte[] buffer = new byte[1024 * 4];
			int bytesRead;

			using (MemoryStream baseStream = new MemoryStream(baseData))
			{
				// Advance the base stream by one byte
				baseStream.ReadByte();
				Assert.AreEqual(1, baseStream.Position);

				// Create a SubStream beginning at byte 1 and spanning 4 bytes
				using (Stream stream = baseStream.SubStream(4))
				{
					Assert.AreEqual(0, stream.Position);
					Assert.AreEqual(4, stream.Length);

					// Attempt to read as much as possible. Check if it's the appropriate subsection.
					bytesRead = stream.Read(buffer, 0, buffer.Length);
					Assert.AreEqual(5, baseStream.Position);
					Assert.AreEqual(4, bytesRead);
					Assert.IsTrue(baseData.Skip(1).Take(4).SequenceEqual(buffer.Take(bytesRead)));
					
					// Rewind within the subsection
					stream.Position = 0;
					Assert.AreEqual(0, stream.Position);
					Assert.AreEqual(5, baseStream.Position);

					// Read after rewinding
					bytesRead = stream.Read(buffer, 0, buffer.Length);
					Assert.AreEqual(4, bytesRead);
					Assert.IsTrue(baseData.Skip(1).Take(4).SequenceEqual(buffer.Take(bytesRead)));
					Assert.AreEqual(4, stream.Position);
					Assert.AreEqual(5, baseStream.Position);

					// Attempting to exceed the subsections range should be unsuccessful.
					stream.SetLength(5);
					Assert.AreEqual(4, stream.Position);
					Assert.AreEqual(4, stream.Length);
					Assert.AreEqual(5, baseStream.Position);
					Assert.AreEqual(baseData.Length, baseStream.Length);

					// Shrinking the subsection is allowed
					stream.SetLength(2);
					Assert.AreEqual(2, stream.Position);
					Assert.AreEqual(2, stream.Length);
					Assert.AreEqual(5, baseStream.Position);
					Assert.AreEqual(baseData.Length, baseStream.Length);

					// Attempting to set invalid position values
					Assert.Throws<ArgumentOutOfRangeException>(() => stream.Position = -1);
					Assert.Throws<ArgumentOutOfRangeException>(() => stream.Position = 5);
					Assert.AreEqual(5, baseStream.Position);
				}
			}
		}
	}
}
