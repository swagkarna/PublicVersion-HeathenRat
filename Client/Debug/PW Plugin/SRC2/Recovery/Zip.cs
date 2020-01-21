using System;
using System.IO;
using System.IO.Compression;

namespace Plugin
{
	// Token: 0x02000005 RID: 5
	public static class Zip
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002600 File Offset: 0x00000800
		public static byte[] Decompress(byte[] input)
		{
			byte[] result2;
			using (MemoryStream source = new MemoryStream(input))
			{
				byte[] lengthBytes = new byte[4];
				source.Read(lengthBytes, 0, 4);
				int length = BitConverter.ToInt32(lengthBytes, 0);
				using (GZipStream decompressionStream = new GZipStream(source, CompressionMode.Decompress))
				{
					byte[] result = new byte[length];
					decompressionStream.Read(result, 0, length);
					result2 = result;
				}
			}
			return result2;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002688 File Offset: 0x00000888
		public static byte[] Compress(byte[] input)
		{
			byte[] result2;
			using (MemoryStream result = new MemoryStream())
			{
				byte[] lengthBytes = BitConverter.GetBytes(input.Length);
				result.Write(lengthBytes, 0, 4);
				using (GZipStream compressionStream = new GZipStream(result, CompressionMode.Compress))
				{
					compressionStream.Write(input, 0, input.Length);
					compressionStream.Flush();
				}
				result2 = result.ToArray();
			}
			return result2;
		}
	}
}
