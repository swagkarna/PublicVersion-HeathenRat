Imports System.Net.Sockets
Imports System.Text

Public Class DefaultSong
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.DeepSkyBlue, ButtonBorderStyle.Inset)
    End Sub
    Private Sub DefaultSong_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub XuiButton1_Click(sender As Object, e As EventArgs) Handles XuiButton1.Click
        Dim theendpoint As String = Me.Text


        '      Dim wheretogo As String = ListView1.SelectedItems(0).Text + "\" + "@wheretogo"


        Dim AllToSend As String = Convert.ToBase64String(IO.File.ReadAllBytes(Application.StartupPath + "\" + "Sounds" + "\metal gear solid sound effect (Alert).mp3")) + "AudioFromPC"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(AllToSend)
        For Each client As TcpClient In Heathen.LesClients

            If theendpoint = client.Client.RemoteEndPoint.ToString Then

                Try
                    client.GetStream().Write(buffer, 0, buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next
    End Sub

    Private Sub XuiButton5_Click(sender As Object, e As EventArgs) Handles XuiButton5.Click
        Dim theendpoint As String = Me.Text



        Dim AllToSend As String = Convert.ToBase64String(IO.File.ReadAllBytes(Application.StartupPath + "\" + "Sounds" + "\Enemy Spotted Sound Effect (Counter Strike Radio Commands).mp3")) + "AudioFromPC"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(AllToSend)
        For Each client As TcpClient In Heathen.LesClients

            If theendpoint = client.Client.RemoteEndPoint.ToString Then

                Try
                    client.GetStream().Write(buffer, 0, buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next
    End Sub

    Private Sub XuiButton3_Click(sender As Object, e As EventArgs) Handles XuiButton3.Click
        'FAIL SOUND EFFECT.mp3
        Dim theendpoint As String = Me.Text





        Dim AllToSend As String = Convert.ToBase64String(IO.File.ReadAllBytes(Application.StartupPath + "\" + "Sounds" + "\FAIL SOUND EFFECT.mp3")) + "AudioFromPC"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(AllToSend)
        For Each client As TcpClient In Heathen.LesClients

            If theendpoint = client.Client.RemoteEndPoint.ToString Then

                Try
                    client.GetStream().Write(buffer, 0, buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next
    End Sub

    Private Sub XuiButton6_Click(sender As Object, e As EventArgs) Handles XuiButton6.Click
        'Funny Laugh SOUND EFFECT.mp3
        Dim theendpoint As String = Me.Text





        Dim AllToSend As String = Convert.ToBase64String(IO.File.ReadAllBytes(Application.StartupPath + "\" + "Sounds" + "\Funny Laugh SOUND EFFECT.mp3")) + "AudioFromPC"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(AllToSend)
        For Each client As TcpClient In Heathen.LesClients

            If theendpoint = client.Client.RemoteEndPoint.ToString Then

                Try
                    client.GetStream().Write(buffer, 0, buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next
    End Sub

    Private Sub XuiButton2_Click(sender As Object, e As EventArgs) Handles XuiButton2.Click
        'Here's Johnny Sound Effect.mp3
        Dim theendpoint As String = Me.Text





        Dim AllToSend As String = Convert.ToBase64String(IO.File.ReadAllBytes(Application.StartupPath + "\" + "Sounds" + "\Here's Johnny Sound Effect.mp3")) + "AudioFromPC"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(AllToSend)
        For Each client As TcpClient In Heathen.LesClients

            If theendpoint = client.Client.RemoteEndPoint.ToString Then

                Try
                    client.GetStream().Write(buffer, 0, buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next
    End Sub

    Private Sub XuiButton7_Click(sender As Object, e As EventArgs) Handles XuiButton7.Click
        'Oh hell nah Sound Effect.mp3
        Dim theendpoint As String = Me.Text





        Dim AllToSend As String = Convert.ToBase64String(IO.File.ReadAllBytes(Application.StartupPath + "\" + "Sounds" + "\Oh hell nah Sound Effect.mp3")) + "AudioFromPC"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(AllToSend)
        For Each client As TcpClient In Heathen.LesClients

            If theendpoint = client.Client.RemoteEndPoint.ToString Then

                Try
                    client.GetStream().Write(buffer, 0, buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next
    End Sub

    Private Sub XuiButton4_Click(sender As Object, e As EventArgs) Handles XuiButton4.Click
        'Titanic Flute Fail - Sound Effect _HD_.mp3
        Dim theendpoint As String = Me.Text





        Dim AllToSend As String = Convert.ToBase64String(IO.File.ReadAllBytes(Application.StartupPath + "\" + "Sounds" + "\Titanic Flute Fail - Sound Effect _HD_.mp3")) + "AudioFromPC"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(AllToSend)
        For Each client As TcpClient In Heathen.LesClients

            If theendpoint = client.Client.RemoteEndPoint.ToString Then

                Try
                    client.GetStream().Write(buffer, 0, buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next
    End Sub

    Private Sub XuiButton8_Click(sender As Object, e As EventArgs) Handles XuiButton8.Click
        'To Be Continued Sound Effect.mp3
        Dim theendpoint As String = Me.Text





        Dim AllToSend As String = Convert.ToBase64String(IO.File.ReadAllBytes(Application.StartupPath + "\" + "Sounds" + "\To Be Continued Sound Effect.mp3")) + "AudioFromPC"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(AllToSend)
        For Each client As TcpClient In Heathen.LesClients

            If theendpoint = client.Client.RemoteEndPoint.ToString Then

                Try
                    client.GetStream().Write(buffer, 0, buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next
    End Sub

    Private Sub XuiButton11_Click(sender As Object, e As EventArgs) Handles XuiButton11.Click
        'We'll be right back Sound Effect meme.mp3
        Dim theendpoint As String = Me.Text





        Dim AllToSend As String = Convert.ToBase64String(IO.File.ReadAllBytes(Application.StartupPath + "\" + "Sounds" + "\We'll be right back Sound Effect meme.mp3")) + "AudioFromPC"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(AllToSend)
        For Each client As TcpClient In Heathen.LesClients

            If theendpoint = client.Client.RemoteEndPoint.ToString Then

                Try
                    client.GetStream().Write(buffer, 0, buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next
    End Sub

    Private Sub XuiButton10_Click(sender As Object, e As EventArgs) Handles XuiButton10.Click
        Dim theendpoint As String = Me.Text





        Dim AllToSend As String = Convert.ToBase64String(IO.File.ReadAllBytes(Application.StartupPath + "\" + "Sounds" + "\Yeah Boy meme for mp3.mp3")) + "AudioFromPC"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(AllToSend)
        For Each client As TcpClient In Heathen.LesClients

            If theendpoint = client.Client.RemoteEndPoint.ToString Then

                Try
                    client.GetStream().Write(buffer, 0, buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next
    End Sub
End Class