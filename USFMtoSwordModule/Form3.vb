Public Class Form3

	Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
		If ListBox1.SelectedIndex >= 0 Then Button5.Enabled = True
	End Sub

	Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
		ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
		ListBox1.Refresh()
		Button5.Enabled = False
	End Sub

	Private Sub ListBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox2.SelectedIndexChanged
		If ListBox2.SelectedIndex >= 0 Then Button7.Enabled = True
	End Sub

	Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
		ListBox2.Items.RemoveAt(ListBox2.SelectedIndex)
		ListBox2.Refresh()
		Button7.Enabled = False
	End Sub

	Private Sub ListBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox3.SelectedIndexChanged
		If ListBox3.SelectedIndex >= 0 Then Button10.Enabled = True
	End Sub

	Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
		ListBox3.Items.RemoveAt(ListBox3.SelectedIndex)
		ListBox3.Refresh()
		Button10.Enabled = False
	End Sub

	Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
		Dim newmarker As String
redoentry:
		newmarker = InputDialog.InputBox("Please type the marker to be deleted:", "StripMarkers", "\")
		If Microsoft.VisualBasic.Left(newmarker, 1) <> "\" Then
			MessageBox.Show("You need the '\' character.  Please re-enter the marker.", "StripMarkers", MessageBoxButtons.OK)
			GoTo redoentry
		End If
		If newmarker = "" Then
			Exit Sub
		End If
		If newmarker = "\" Then
			Exit Sub
		End If
		ListBox1.Items.Add(newmarker)
		ListBox1.Refresh()
	End Sub

	Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
		Dim newmarker As String
redoentry:
		newmarker = InputDialog.InputBox("Please type the marker to be deleted:", "StripMarkers", "\")
		If Microsoft.VisualBasic.Left(newmarker, 1) <> "\" Then
			MessageBox.Show("You need the '\' character.  Please re-enter the marker.", "StripMarkers", MessageBoxButtons.OK)
			GoTo redoentry
		End If
		If newmarker = "" Then
			Exit Sub
		End If
		If newmarker = "\" Then
			Exit Sub
		End If
		ListBox2.Items.Add(newmarker)
		ListBox2.Refresh()
	End Sub

	Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
		Dim newmarker As String
redoentry:
		newmarker = InputDialog.InputBox("Please type the marker to be deleted:", "StripMarkers", "\")
		If Microsoft.VisualBasic.Left(newmarker, 1) <> "\" Then
			MessageBox.Show("You need the '\' character.  Please re-enter the marker.", "StripMarkers", MessageBoxButtons.OK)
			GoTo redoentry
		End If
		If newmarker = "" Then
			Exit Sub
		End If
		If newmarker = "\" Then
			Exit Sub
		End If
		ListBox3.Items.Add(newmarker)
		ListBox3.Refresh()
	End Sub

	Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
		Dim xloop As Integer
		If SaveFileDialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
			FileOpen(1, SaveFileDialog2.FileName, OpenMode.Output)
			For xloop = 1 To ListBox1.Items.Count
				Write(1, ListBox1.Items(xloop - 1))
			Next
			Write(1, "***SEP***")
			For xloop = 1 To ListBox2.Items.Count
				Write(1, ListBox2.Items(xloop - 1))
			Next
			Write(1, "***SEP***")
			For xloop = 1 To ListBox3.Items.Count
				Write(1, ListBox3.Items(xloop - 1))
			Next
			FileClose(1)
		End If
	End Sub

	Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
		Dim nextentry As String
		nextentry = ""
		If OpenFileDialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
			ListBox1.Items.Clear()
			ListBox2.Items.Clear()
			ListBox3.Items.Clear()
			FileOpen(1, OpenFileDialog2.FileName, OpenMode.Input)
			While Not EOF(1)
				Input(1, nextentry)
				While nextentry <> "***SEP***"
					ListBox1.Items.Add(nextentry)
					Input(1, nextentry)
				End While
				Input(1, nextentry)
				While nextentry <> "***SEP***"
					ListBox2.Items.Add(nextentry)
					Input(1, nextentry)
				End While
				Input(1, nextentry)
				While Not EOF(1)
					ListBox3.Items.Add(nextentry)
					Input(1, nextentry)
				End While
				ListBox3.Items.Add(nextentry)
			End While
			FileClose(1)
		End If
	End Sub

	Private Sub Form3_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		e.Cancel = True
		Me.Hide()
	End Sub
End Class
