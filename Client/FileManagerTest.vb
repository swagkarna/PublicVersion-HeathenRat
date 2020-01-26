Imports System.IO

Public Class FileManagerTest
    Private Sub FileManagerTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Dim Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Label3.Text = Path
        Dim imageList = New ImageList()
        ListView1.SmallImageList = imageList
        For Each yu In Directory.GetFiles(Path, "*.*", SearchOption.TopDirectoryOnly)
            ' Dim lvi As New ListViewItem(yu) 'first column

            Dim kleff = IO.Path.GetFileName(yu)



            imageList.ImageSize = New Size(20, 20)

            imageList.Images.Add("file", My.Resources.icons8_codeazdazd_file)



            Dim listViewItem = ListView1.Items.Add(kleff)
            listViewItem.ImageKey = "file"


        Next
        For Each yu In Directory.GetDirectories(Path, "*.*", SearchOption.TopDirectoryOnly)
            ' Dim lvi As New ListViewItem(yu) 'first column
            Dim kleff = IO.Path.GetFileName(yu)




            imageList.ImageSize = New Size(20, 20)

            imageList.Images.Add("folder", My.Resources.icons8_File_Eqsdqsdxplorer)

            ' ListView1.SmallImageList = imageList
            Dim listViewItem = ListView1.Items.Add(kleff)
            listViewItem.ImageKey = "folder"

        Next


    End Sub
    Public Sub GetDirGo(ByVal NextDir As String)
        ListView1.Items.Clear()

        Dim NewPath As String = Label3.Text + NextDir + "\"

        Label3.Text = NewPath
        Dim imageList = New ImageList()
        ListView1.SmallImageList = imageList
        For Each yu In Directory.GetFiles(NewPath, "*.*", SearchOption.TopDirectoryOnly)
            ' Dim lvi As New ListViewItem(yu) 'first column

            Dim kleff = IO.Path.GetFileName(yu)



            imageList.ImageSize = New Size(20, 20)

            imageList.Images.Add("file", My.Resources.icons8_codeazdazd_file)



            Dim listViewItem = ListView1.Items.Add(kleff)
            listViewItem.ImageKey = "file"


        Next

        For Each yu In Directory.GetDirectories(NewPath, "*.*", SearchOption.TopDirectoryOnly)
            ' Dim lvi As New ListViewItem(yu) 'first column
            Dim kleff = IO.Path.GetFileName(yu)




            imageList.ImageSize = New Size(20, 20)

            imageList.Images.Add("folder", My.Resources.icons8_File_Eqsdqsdxplorer)

            ' ListView1.SmallImageList = imageList
            Dim listViewItem = ListView1.Items.Add(kleff)
            listViewItem.ImageKey = "folder"

        Next
    End Sub
    Public Sub GetDirBack(ByVal PreviousDir As String)
        ListView1.Items.Clear()


        PreviousDir = Label3.Text.Split("/").Last()

        Dim NewPath As String = IO.Directory.GetParent(PreviousDir).FullName

        Label3.Text = NewPath


        Dim imageList = New ImageList()
        ListView1.SmallImageList = imageList
        For Each yu In Directory.GetFiles(NewPath, "*.*", SearchOption.TopDirectoryOnly)
            ' Dim lvi As New ListViewItem(yu) 'first column

            Dim kleff = IO.Path.GetFileName(yu)



            imageList.ImageSize = New Size(20, 20)

            imageList.Images.Add("file", My.Resources.icons8_codeazdazd_file)



            Dim listViewItem = ListView1.Items.Add(kleff)
            listViewItem.ImageKey = "file"


        Next

        For Each yu In Directory.GetDirectories(NewPath, "*.*", SearchOption.TopDirectoryOnly)
            ' Dim lvi As New ListViewItem(yu) 'first column
            Dim kleff = IO.Path.GetFileName(yu)




            imageList.ImageSize = New Size(20, 20)

            imageList.Images.Add("folder", My.Resources.icons8_File_Eqsdqsdxplorer)

            ' ListView1.SmallImageList = imageList
            Dim listViewItem = ListView1.Items.Add(kleff)
            listViewItem.ImageKey = "folder"

        Next
    End Sub
    Private Sub GoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GoToolStripMenuItem.Click
        Dim lk As String = ""
        If ListView1.SelectedItems.Count > 0 Then
            For Each h In ListView1.SelectedItems

                '     MessageBox.Show(h.ToString)

            Next
            For Each h In ListView1.SelectedItems

                Dim o As String = h.ToString.Replace("ListViewItem: ", "")
                Dim odd As String = o.Replace("{", "")
                Dim odd2 As String = odd.Replace("}", "")
                Dim odd3() As String = Split(odd2, ":")

                lk = odd2


                GetDirGo(lk)


            Next

        End If
    End Sub

    Private Sub BackToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackToolStripMenuItem.Click
        Dim lk As String = ""
        If ListView1.SelectedItems.Count > 0 Then
            For Each h In ListView1.SelectedItems

                '     MessageBox.Show(h.ToString)

            Next
            For Each h In ListView1.SelectedItems

                Dim o As String = h.ToString.Replace("ListViewItem: ", "")
                Dim odd As String = o.Replace("{", "")
                Dim odd2 As String = odd.Replace("}", "")
                Dim odd3() As String = Split(odd2, ":")

                lk = odd2


                GetDirBack(lk)


            Next

        End If
    End Sub
End Class