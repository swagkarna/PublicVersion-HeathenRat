Imports System
Imports System.Collections.Generic

Namespace Plugin.Browsers
	' Token: 0x0200000E RID: 14
	Friend Interface IPassReader
		' Token: 0x0600006D RID: 109
		Function ReadPasswords() As IEnumerable(Of CredentialModel)

		' Token: 0x17000012 RID: 18
		' (get) Token: 0x0600006E RID: 110
		ReadOnly Property BrowserName As String
	End Interface
End Namespace
