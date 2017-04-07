﻿Imports System.IO

Public Class Editor
    Dim FileContent
    Dim DialogBoxResult
    Dim CurrentLine = 1
    Dim SelectionIndex = 0
    Dim CurrentCol = 0

    Private Sub ContextMenuLabel_Click(sender As Object, e As EventArgs) Handles ContextMenuLabel.Click
        ContextMenu1.Show()
    End Sub

    Private Sub OpenFileLabel_Click(sender As Object, e As EventArgs) Handles OpenFileLabel.Click
        If Not ((OpenFileDialog.FileName = "") And (TextArea.Text = "")) Then
            DialogBoxResult = MsgBox("Save changes to current file?", MsgBoxStyle.YesNoCancel)
            ' Yes = 6
            ' No = 7
            ' Cancel = 2
        Else
            DialogBoxResult = 7
        End If
        If Not (DialogBoxResult = 2) Then
            If DialogBoxResult = 6 Then
                SaveFileDialog.ShowDialog()
            End If
            OpenFileDialog.InitialDirectory = Environment.GetEnvironmentVariable("USERPROFILE")
            OpenFileDialog.ShowDialog()
        End If
    End Sub

    Private Sub OpenFileDialog_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog.FileOk
        Me.Cursor = Cursors.WaitCursor
        TextArea.ReadOnly = True
        Try
            FileContent = File.ReadAllText(OpenFileDialog.FileName)
            TextArea.Text = FileContent
            Me.Text = OpenFileDialog.FileName & " - Nano IDE"
        Catch ex As Exception
            MsgBox("Couldn't open " & OpenFileDialog.FileName & vbCrLf & vbCrLf & ex.ToString, vbCritical)
        End Try
        TextArea.ReadOnly = False
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub SaveFileDialog_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog.FileOk
        Me.Cursor = Cursors.WaitCursor
        TextArea.ReadOnly = True
        Try
            File.WriteAllText(SaveFileDialog.FileName, TextArea.Text)
            MsgBox("Successfully saved file!", MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox("Couldn't save file!" & vbCrLf & vbCrLf & ex.ToString, vbCritical)
        End Try
        Me.Cursor = Cursors.Default
        TextArea.ReadOnly = False
    End Sub

    Private Sub SaveFileLabel_Click(sender As Object, e As EventArgs) Handles SaveFileLabel.Click
        SaveFileDialog.InitialDirectory = OpenFileDialog.FileName
        SaveFileDialog.ShowDialog()
    End Sub

    Private Sub NewFileLabel_Click(sender As Object, e As EventArgs) Handles NewFileLabel.Click
        Me.Cursor = Cursors.WaitCursor
        If Not ((OpenFileDialog.FileName = "") And (TextArea.Text = "")) Then
            DialogBoxResult = MsgBox("Save changes to current file?", MsgBoxStyle.YesNoCancel)
            ' Yes = 6
            ' No = 7
            ' Cancel = 2
        Else
            DialogBoxResult = 7
        End If
        If Not (DialogBoxResult = 2) Then
            If DialogBoxResult = 6 Then
                SaveFileDialog.ShowDialog()
            End If
            TextArea.Text = ""
        End If
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub Editor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not ((OpenFileDialog.FileName = "") And (TextArea.Text = "")) Then
            DialogBoxResult = MsgBox("Save changes to current file?", MsgBoxStyle.YesNoCancel)
            ' Yes = 6
            ' No = 7
            ' Cancel = 2
        Else
            DialogBoxResult = 7
        End If
        If DialogBoxResult = 2 Then
            e.Cancel = True
        Else
            If DialogBoxResult = 6 Then
                SaveFileDialog.ShowDialog()
            End If
            e.Cancel = False
        End If
    End Sub

    Private Sub Editor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextArea.Font = My.Settings.Font
        TopMenu.BackColor = My.Settings.BackColor
        TopMenu.ForeColor = My.Settings.ForeColor
        BottomMenuText.Text = "Line 1 / Char 1"
    End Sub

    Private Sub TextArea_KeyDown(sender As Object, e As KeyEventArgs) Handles TextArea.KeyDown
        If e.KeyCode = Keys.A And My.Computer.Keyboard.CtrlKeyDown Then
            TextArea.SelectAll()
            e.Handled = True
        End If
    End Sub

    Private Sub TextArea_TextChanged(sender As Object, e As EventArgs) Handles TextArea.TextChanged
        SelectionIndex = TextArea.SelectionStart
        CurrentLine = TextArea.GetLineFromCharIndex(SelectionIndex) + 1
        CurrentCol = SelectionIndex - TextArea.GetFirstCharIndexFromLine(CurrentLine)
        BottomMenuText.Text = "Line " & CurrentLine.ToString & vbTab & " / Char " & CurrentCol.ToString
    End Sub
End Class