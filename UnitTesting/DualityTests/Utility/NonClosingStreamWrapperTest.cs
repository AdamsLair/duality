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
	public class NonClosingStreamWrapperTest
	{
		[Test] public void Close()
		{
			using (MemoryStream baseStream = new MemoryStream())
			{
				Stream stream = baseStream.NonClosing();
				stream.Close();
				Assert.IsFalse(stream.CanRead);
				Assert.IsTrue(baseStream.CanRead);
				Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
			}
		}
		[Test] public void Read()
		{
			byte[] baseData = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();

			using (MemoryStream baseStream = new MemoryStream(baseData))
			using (Stream stream = baseStream.NonClosing())
			{
				byte[] buffer = new byte[1024 * 4];
				int bytesRead = stream.Read(buffer, 0, buffer.Length);

				Assert.AreEqual(baseData.Length, bytesRead);
				Assert.IsTrue(baseData.SequenceEqual(buffer.Take(baseData.Length)));
			}
		}
		[Test] public void Write()
		{
			byte[] baseData = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
			using (MemoryStream baseStream = new MemoryStream())
			{
				using (Stream stream = baseStream.NonClosing())
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
	}
}
