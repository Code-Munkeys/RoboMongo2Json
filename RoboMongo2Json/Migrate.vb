'Developed by Franz Ayestaran on 12/2/15.
'Copyright (c) 2014 Zonk Technology. All rights reserved.

'You may use this code in your own projects and upon doing so, you the programmer are solely
'responsible for determining it's worthiness for any given application or task. Here clearly
'states that the code is for learning purposes only and is not guaranteed to conform to any
'programming style, standard, or be an adequate answer for any given problem.

Imports System
Imports System.IO

Public Class Migrate

    Public Function GetFileContents(ByVal FullPath As String, _
       Optional ByRef ErrInfo As String = "") As String

        Dim strContents As String
        Dim objReader As StreamReader
        Try

            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
            Return strContents
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
    End Function

    Public Function SaveTextToFile(ByVal strData As String, _
     ByVal FullPath As String, _
       Optional ByVal ErrInfo As String = "") As Boolean

        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        Try
            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message

        End Try
        Return bAns
    End Function

    Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Double
        Dim cnt As Double = 0
        For Each c As Char In value
            If c = ch Then cnt += 1
        Next
        Return cnt
    End Function

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.InitialDirectory = "c:"
        openFileDialog1.Filter = "txt files (*.txt)|*.txt"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            txtFilename.Text = openFileDialog1.FileName
            txtFilename.Text = openFileDialog1.FileName.Replace(".txt", "")
        End If
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        txtSource.Text = GetFileContents(txtFilename.Text + ".txt")
        txtDestination.Text = "{" + Chr(34) + "records" + Chr(34) + ":[" + txtSource.Text

        Dim TotalRecords As Double = CountCharacter(txtSource.Text, "{")

        lblTotalRecords.Text = TotalRecords.ToString()

        prgCompleted.Maximum = TotalRecords

        For index As Double = 0 To TotalRecords
            txtDestination.Text = txtDestination.Text.Replace("/* " + Convert.ToString(index) + " */", "")
            prgCompleted.Value = index
        Next

        txtDestination.Text = txtDestination.Text.Replace("}", "},")
        txtDestination.Text = txtDestination.Text.Replace("LUUID(", "")

        txtDestination.Text = txtDestination.Text.Replace(")", "")

        txtDestination.Text = txtDestination.Text + "]}"

        txtDestination.Text = txtDestination.Text.Replace("},]}", "}]}")

        SaveTextToFile(txtDestination.Text, txtFilename.Text + ".json")
    End Sub

End Class
