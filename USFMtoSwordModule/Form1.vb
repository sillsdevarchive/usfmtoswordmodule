' *************************************************************************
' *************************************************************************
' **                                                                     **
' **                                                                     **
' **                         USFMtoSwordModule                           **
' **                                                                     **
' **                    Programmed by Ben Chenoweth                      **
' **                                                                     **
' **                                                                     **
' **  LICENCE INFORMATION:                                               **
' **                                                                     **
' **  Copyright (c) 2010 NEG Computer Services Department                **
' **                                                                     **
' **  Permission is hereby granted, free of charge, to any person        **
' **  obtaining a copy of this software and associated documentation     **
' **  files (the "Software"), to deal in the Software without            **
' **  restriction, including without limitation the rights to use,       **
' **  copy, modify, merge, publish, distribute, sublicense, and/or sell  **
' **  copies of the Software, and to permit persons to whom the          **
' **  Software is furnished to do so, subject to the following           **
' **  conditions:                                                        **
' **                                                                     **
' **  The above copyright notice and this permission notice shall be     **
' **  included in all copies or substantial portions of the Software.    **
' **                                                                     **
' **  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,    **
' **  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES    **
' **  OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND           **
' **  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT        **
' **  HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,       **
' **  WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING       **
' **  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR      **
' **  OTHER DEALINGS IN THE SOFTWARE.                                    **
' **                                                                     **
' **                                                                     **
' *************************************************************************
' *************************************************************************

' NOTE: Sword icon from http://www.iconarchive.com/show/legendora-icons-by-raindropmemory/sword-icon.html

Imports System.IO
Public Class Form1
	Dim workingdirectory, applicationdirectory, basedirectory As String
	Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
		If ComboBox2.SelectedIndex > 0 And ComboBox2.SelectedIndex < 6 Then
			TextBox5.Enabled = True
		Else
			TextBox5.Enabled = False
		End If
	End Sub

	Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
		OpenFileDialog1.Filter = "USFM Files (*.sfm)|*.sfm|Paratext Files (*.ptx)|*.ptx|All files (*.*)|*.*"
		OpenFileDialog1.FilterIndex = 1
		OpenFileDialog1.FileName = ""
		If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
			Dim file As String
			For Each file In OpenFileDialog1.FileNames
				ListBox1.Items.Add(file)
				Button3.Enabled = True
				workingdirectory = System.IO.Path.GetDirectoryName(file)
			Next
		End If
	End Sub

	Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
		ListBox1.Items.Clear()
		Button3.Enabled = False
	End Sub

	Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
		Dim temp As Integer
		Dim x As Integer
		Dim divider, perlbin As String
		Dim commandlineargs, sourcefilename, destinationfilename, encoding As String

		' Error checking
		' Check for Perl
		If System.IO.File.Exists(applicationdirectory & "\Perl\bin\perl.exe") Then
			perlbin = applicationdirectory & "\Perl\bin\perl.exe"
		ElseIf System.IO.File.Exists("C:\Perl\bin\perl.exe") Then
			perlbin = "C:\Perl\bin\perl.exe"
		Else
			temp = MsgBox("Problem: I can't find Perl.  Is Perl on the Path?", MsgBoxStyle.OkCancel, "USFMtoSwordModule")
			If temp = Windows.Forms.DialogResult.Yes Then
				perlbin = "perl.exe"
			Else
				temp = MsgBox("You must install Perl to run this program.", MsgBoxStyle.Critical, "USFMtoSwordModule")
				GoTo exitproc
			End If
		End If

		If ListBox1.Items.Count = 0 Then
			temp = MsgBox("Problem: You must add some USFM files.", MsgBoxStyle.Critical, "USFMtoSwordModule")
			GoTo exitproc
		End If
		If ComboBox1.SelectedIndex = -1 Then
			temp = MsgBox("Problem: You must select an encoding.", MsgBoxStyle.Critical, "USFMtoSwordModule")
			GoTo exitproc
		End If
		If TextBox1.Text = "" Then
			temp = MsgBox("Problem: You must provide an ID Code.", MsgBoxStyle.Critical, "USFMtoSwordModule")
			GoTo exitproc
		End If
		If TextBox2.Text = "" Then
			temp = MsgBox("Problem: You must provide a Description.", MsgBoxStyle.Critical, "USFMtoSwordModule")
			GoTo exitproc
		End If
		If TextBox6.Text = "" Then
			temp = MsgBox("Problem: You must provide a Version.", MsgBoxStyle.Critical, "USFMtoSwordModule")
			GoTo exitproc
		End If
		If TextBox3.Text = "" Then
			temp = MsgBox("Problem: You must provide a Language.", MsgBoxStyle.Critical, "USFMtoSwordModule")
			GoTo exitproc
		End If
		If ComboBox3.SelectedIndex = -1 Then
			temp = MsgBox("Problem: You must select a Versification.", MsgBoxStyle.Critical, "USFMtoSwordModule")
		End If
		If TextBox4.Text = "" Then
			temp = MsgBox("Problem: You must provide an About.", MsgBoxStyle.Critical, "USFMtoSwordModule")
			GoTo exitproc
		End If
		If ComboBox2.SelectedIndex = -1 Then
			temp = MsgBox("Problem: You must select a License.", MsgBoxStyle.Critical, "USFMtoSwordModule")
			GoTo exitproc
		End If
		If TextBox5.Text = "" And TextBox5.Enabled = True Then
			temp = MsgBox("Problem: You must provide a Copyright.", MsgBoxStyle.Critical, "USFMtoSwordModule")
			GoTo exitproc
		End If
		If CheckBox1.Checked = True Then
			If TextBox8.Text = "" Then
				temp = MsgBox("Problem: You must provide a Cipher Key if you want to encrypt your Sword module.", MsgBoxStyle.Critical, "USFMtoSwordModule")
				GoTo exitproc
			End If
			If Len(TextBox8.Text) <> 16 Then
				temp = MsgBox("Problem: The Cipher Key must be 16 characters in length.", MsgBoxStyle.Critical, "USFMtoSwordModule")
				GoTo exitproc
			End If
		End If

		' Get save location
		SaveFileDialog1.InitialDirectory = workingdirectory
		SaveFileDialog1.Filter = "Zip Files (*.zip)|*.zip|All files (*.*)|*.*"
		SaveFileDialog1.FilterIndex = 1
		SaveFileDialog1.FileName = LCase(TextBox1.Text)
		If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
			' Show Dialog1
			Dialog1.Button1.Enabled = False
			Dialog1.TextBox1.Text = ""
			divider = "================================================================"
			Dialog1.Show()

			' Create working folder
			ChDir(workingdirectory)
			If System.IO.Directory.Exists(workingdirectory & "\temp") Then
				System.IO.Directory.Delete(workingdirectory & "\temp", True)
			End If
			System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory & "\temp")
			basedirectory = System.IO.Directory.GetCurrentDirectory & "\temp"
			ChDir(basedirectory)

			' Create output folders
			Dim settingsdirectory, moduledirectory As String
			System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory & "\mods.d")
			settingsdirectory = System.IO.Directory.GetCurrentDirectory & "\mods.d"

			System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory & "\modules")
			ChDir(System.IO.Directory.GetCurrentDirectory & "\modules")
			System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory & "\texts")
			ChDir(System.IO.Directory.GetCurrentDirectory & "\texts")
			If CheckBox1.Checked = False Then
				System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory & "\rawtext")
				ChDir(System.IO.Directory.GetCurrentDirectory & "\rawtext")
			Else
				System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory & "\ztext")
				ChDir(System.IO.Directory.GetCurrentDirectory & "\ztext")
			End If
			System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory & "\" + LCase(TextBox1.Text))
			moduledirectory = System.IO.Directory.GetCurrentDirectory & "\" + LCase(TextBox1.Text)

			' Generate .conf file in mods.d folder
			Dialog1.Label1.Text = "Generating .conf file..."
			Dialog1.TextBox1.Text = divider & Chr(10) & divider & Chr(10) & "GENERATING .CONF FILE..."
			Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
			Dialog1.TextBox1.ScrollToCaret()
			Dialog1.TextBox1.Focus()
			My.Application.DoEvents()

			Dim writer As StreamWriter = _
				New StreamWriter(settingsdirectory & "\" & LCase(TextBox1.Text) & ".conf")
			writer.WriteLine("[" & UCase(TextBox1.Text) & "]")
			If CheckBox1.Checked = False Then
				writer.WriteLine("DataPath=./modules/texts/rawtext/" & LCase(TextBox1.Text) & "/")
				writer.WriteLine("ModDrv=RawText")
			Else
				writer.WriteLine("DataPath=./modules/texts/ztext/" & LCase(TextBox1.Text) & "/")
				writer.WriteLine("ModDrv=zText")
				writer.WriteLine("CompressType=ZIP")
				writer.WriteLine("CipherKey=")
			End If
			writer.WriteLine("SourceType=OSIS")
			writer.WriteLine("Encoding=UTF-8")
			writer.WriteLine("BlockType=BOOK")
			writer.WriteLine("GlobalOptionFilter=OSISStrongs")
			writer.WriteLine("GlobalOptionFilter=OSISMorph")
			writer.WriteLine("GlobalOptionFilter=OSISFootnotes")
			writer.WriteLine("GlobalOptionFilter=OSISHeadings")
			writer.WriteLine("GlobalOptionFilter=OSISRedLetterWords")
			writer.WriteLine("Version=" + TextBox6.Text)
			writer.WriteLine("Lang=" + TextBox3.Text)
			writer.WriteLine("Versification=" + ComboBox3.Text)
			If TextBox7.Text <> "" Then
				writer.WriteLine("Font=" + TextBox7.Text)
			End If
			writer.WriteLine("Description=" + TextBox2.Text)
			writer.WriteLine("About=" + TextBox4.Text)
			writer.WriteLine("DistributionLicense=" + ComboBox2.Text)
			If TextBox5.Enabled = True Then
				writer.WriteLine("Copyright=" + TextBox5.Text)
			End If
			writer.Close()
			Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & " DONE!" & Chr(10) & divider & Chr(10)
			Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
			Dialog1.TextBox1.ScrollToCaret()
			Dialog1.TextBox1.Focus()
			My.Application.DoEvents()

			' Count number of files to process and prepare progressbar
			Dim numberoffiles As Integer
			numberoffiles = ListBox1.Items.Count
			Dialog1.ProgressBar1.Maximum = numberoffiles * 2
			Dialog1.ProgressBar1.Value = 0

			' Call usfm2osis.pl script
			Dialog1.Label1.Text = "Converting from USFM to OSIS..."
			Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & divider & Chr(10) & "CONVERTING FROM USFM TO OSIS..." & Chr(10) & divider
			Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
			Dialog1.TextBox1.ScrollToCaret()
			Dialog1.TextBox1.Focus()
			My.Application.DoEvents()

			For x = 0 To numberoffiles - 1
				sourcefilename = ListBox1.Items(x)
				destinationfilename = Strings.Left(sourcefilename, Len(sourcefilename) - 3) & "xml"
				encoding = "utf8"
				If ComboBox1.SelectedIndex = 1 Then encoding = "cp1251"
				If ComboBox1.SelectedIndex = 2 Then encoding = "cp1252"

				commandlineargs = Chr(34) & applicationdirectory & "\usfm2osis.pl" & Chr(34) & " " & UCase(TextBox1.Text) & " -o " & Chr(34) & destinationfilename & Chr(34) & " -e " & encoding & " " & Chr(34) & sourcefilename & Chr(34)

				Dim psi As New _
				System.Diagnostics.ProcessStartInfo(perlbin)
				psi.RedirectStandardOutput = True
				psi.CreateNoWindow = True
				psi.UseShellExecute = False
				psi.Arguments = commandlineargs
				Dim listFiles As System.Diagnostics.Process
				listFiles = System.Diagnostics.Process.Start(psi)
				Dim myOutput As System.IO.StreamReader = listFiles.StandardOutput
				listFiles.WaitForExit(60000)
				If listFiles.HasExited Then
					Dim output As String = myOutput.ReadToEnd
					Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(10) & divider & Chr(10) & output
					Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
					Dialog1.TextBox1.ScrollToCaret()
					Dialog1.TextBox1.Focus()
				Else
					listFiles.Kill()
					Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(10) & divider & Chr(10) & "PROBLEM CONVERTING " & sourcefilename
					Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
					Dialog1.TextBox1.ScrollToCaret()
					Dialog1.TextBox1.Focus()
				End If

				Dialog1.ProgressBar1.Value = Dialog1.ProgressBar1.Value + 1
				Dialog1.ProgressBar1.Update()

				My.Application.DoEvents()

			Next x

			' Call osis2mod.exe
			If CheckBox1.Checked = False Then
				Dialog1.Label1.Text = "Building Sword module..."
				Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & divider & Chr(10) & divider & Chr(10) & "BUILDING SWORD MODULE..." & Chr(10) & divider
			Else
				Dialog1.Label1.Text = "Building Sword module with Cipher Key..."
				Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & divider & Chr(10) & divider & Chr(10) & "BUILDING SWORD MODULE WITH CIPHER KEY..." & Chr(10) & divider
			End If
			Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
			Dialog1.TextBox1.ScrollToCaret()
			Dialog1.TextBox1.Focus()
			My.Application.DoEvents()

			For x = 0 To numberoffiles - 1
				sourcefilename = ListBox1.Items(x)
				destinationfilename = Strings.Left(sourcefilename, Len(sourcefilename) - 3) & "xml"
				If Not System.IO.File.Exists(destinationfilename) Then
					Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(10) & divider & Chr(10) & "ERROR! The file " & destinationfilename & " was not found." & Chr(10) & "This indicates that the perl script failed to convert the usfm file " & sourcefilename & " into osis format."
					Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
					Dialog1.TextBox1.ScrollToCaret()
					Dialog1.TextBox1.Focus()
					My.Application.DoEvents()
					GoTo trynextfile
				End If
				Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(10) & divider & Chr(10) & "Adding " & destinationfilename
				Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
				Dialog1.TextBox1.ScrollToCaret()
				Dialog1.TextBox1.Focus()
				My.Application.DoEvents()
				If x = 0 Then
					If CheckBox1.Checked = False Then
						commandlineargs = Chr(34) & moduledirectory & Chr(34) & " " & Chr(34) & destinationfilename & Chr(34) & " -v " & ComboBox3.Text
					Else
						commandlineargs = Chr(34) & moduledirectory & Chr(34) & " " & Chr(34) & destinationfilename & Chr(34) & " -v " & ComboBox3.Text & " -z -c " & TextBox8.Text
					End If
				Else
					If CheckBox1.Checked = False Then
						commandlineargs = Chr(34) & moduledirectory & Chr(34) & " " & Chr(34) & destinationfilename & Chr(34) & " -a -v " & ComboBox3.Text
					Else
						commandlineargs = Chr(34) & moduledirectory & Chr(34) & " " & Chr(34) & destinationfilename & Chr(34) & " -a -v " & ComboBox3.Text & " -z -c " & TextBox8.Text
					End If
				End If

				Dim psi As New _
				System.Diagnostics.ProcessStartInfo(Chr(34) & applicationdirectory & "\osis2mod.exe" & Chr(34))
				psi.RedirectStandardOutput = True
				psi.CreateNoWindow = True
				psi.UseShellExecute = False
				psi.Arguments = commandlineargs
				Dim listFiles As System.Diagnostics.Process
				listFiles = System.Diagnostics.Process.Start(psi)
				Dim myOutput As System.IO.StreamReader = listFiles.StandardOutput
				listFiles.WaitForExit(60000)
				If listFiles.HasExited Then
					Dim output As String = myOutput.ReadToEnd
					Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(13) & Chr(10) & output
					Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
					Dialog1.TextBox1.ScrollToCaret()
					Dialog1.TextBox1.Focus()
				Else
					listFiles.Kill()
					Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(10) & "PROBLEM ADDING " & destinationfilename
					Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
					Dialog1.TextBox1.ScrollToCaret()
					Dialog1.TextBox1.Focus()
				End If
trynextfile:
				Dialog1.ProgressBar1.Value = Dialog1.ProgressBar1.Value + 1
				Dialog1.ProgressBar1.Update()

				My.Application.DoEvents()

			Next x

			' Zip output folders
			Dialog1.Label1.Text = "Creating Zip file..."
			Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & divider & Chr(10) & divider & Chr(10) & "CREATING ZIP FILE..."
			Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
			Dialog1.TextBox1.ScrollToCaret()
			Dialog1.TextBox1.Focus()
			My.Application.DoEvents()

			Dim file_name As String = SaveFileDialog1.FileName
			If System.IO.File.Exists(file_name) Then
				System.IO.File.Delete(file_name)
			End If

			Using cf As New CompressedFolder(file_name)
				cf.CompressFile(basedirectory & "\mods.d")
				cf.CompressFile(basedirectory & "\modules")
			End Using
			Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & " DONE!" & Chr(10) & divider & Chr(10)
			Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
			Dialog1.TextBox1.ScrollToCaret()
			Dialog1.TextBox1.Focus()
			My.Application.DoEvents()

			' Output success message
			Dialog1.Button1.Enabled = True
			Button7.Enabled = True
			Dialog1.Label1.Text = "Sword module created."
			Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(10) & divider & Chr(10) & "SWORD MODULE CREATED."
			Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(13) & Chr(10) & "Please import module into Sword-compatible program to check the output." & Chr(10) & divider & Chr(10) & divider
			Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
			Dialog1.TextBox1.ScrollToCaret()
			Dialog1.TextBox1.Focus()
			My.Application.DoEvents()
		End If
exitproc:
	End Sub

	Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
		TextBox1.Text = ""
		TextBox2.Text = ""
		TextBox3.Text = ""
		TextBox4.Text = ""
		TextBox5.Text = ""
		TextBox6.Text = ""
		TextBox7.Text = ""
		TextBox8.Text = ""
		CheckBox1.Checked = False
		ComboBox1.SelectedIndex = -1
		ComboBox2.SelectedIndex = -1
		ComboBox3.SelectedIndex = -1
	End Sub

	Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
		SaveFileDialog2.InitialDirectory = workingdirectory
		SaveFileDialog2.Filter = "Settings Files (*.ini)|*.ini|All files (*.*)|*.*"
		SaveFileDialog2.FilterIndex = 1
		SaveFileDialog2.FileName = TextBox1.Text
		If SaveFileDialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
			Dim strFilepath As String
			' create a file test.ini in the bin folder in your application path
			strFilepath = SaveFileDialog2.FileName
			WriteIni(strFilepath, "Settings", "Encoding", ComboBox1.Text)
			WriteIni(strFilepath, "Settings", "IDCode", TextBox1.Text)
			WriteIni(strFilepath, "Settings", "Description", TextBox2.Text)
			WriteIni(strFilepath, "Settings", "Version", TextBox6.Text)
			WriteIni(strFilepath, "Settings", "Language", TextBox3.Text)
			WriteIni(strFilepath, "Settings", "Versification", ComboBox3.Text)
			WriteIni(strFilepath, "Settings", "Font", TextBox7.Text)
			WriteIni(strFilepath, "Settings", "About", TextBox4.Text)
			WriteIni(strFilepath, "Settings", "License", ComboBox2.Text)
			WriteIni(strFilepath, "Settings", "Copyright", TextBox5.Text)
			WriteIni(strFilepath, "Settings", "Encrypt", TextBox8.Text)
		End If
	End Sub

	Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
		OpenFileDialog2.InitialDirectory = workingdirectory
		OpenFileDialog2.Filter = "Settings Files (*.ini)|*.ini|All files (*.*)|*.*"
		OpenFileDialog2.FilterIndex = 1
		OpenFileDialog2.FileName = ""
		If OpenFileDialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
			Dim strFilepath As String
			' create a file test.ini in the bin folder in your application path
			strFilepath = OpenFileDialog2.FileName
			ComboBox1.Text = ReadIni(strFilepath, "Settings", "Encoding", "utf-8 (Unicode)")
			TextBox1.Text = ReadIni(strFilepath, "Settings", "IDCode", "")
			TextBox2.Text = ReadIni(strFilepath, "Settings", "Description", "")
			TextBox6.Text = ReadIni(strFilepath, "Settings", "Version", "")
			TextBox3.Text = ReadIni(strFilepath, "Settings", "Language", "")
			ComboBox3.Text = ReadIni(strFilepath, "Settings", "Versification", "")
			TextBox7.Text = ReadIni(strFilepath, "Settings", "Font", "")
			TextBox4.Text = ReadIni(strFilepath, "Settings", "About", "")
			ComboBox2.Text = ReadIni(strFilepath, "Settings", "License", "")
			TextBox5.Text = ReadIni(strFilepath, "Settings", "Copyright", "")
			TextBox8.Text = ReadIni(strFilepath, "Settings", "Encrypt", "")
			If TextBox8.Text <> "" Then
				CheckBox1.Checked = True
			End If
		End If
	End Sub

	Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		applicationdirectory = CurDir()
	End Sub

	Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
		Dim numberoffiles, x As Integer
		Dim sourcefilename, destinationfilename As String

		' Deleting working folder and xml files

		numberoffiles = ListBox1.Items.Count
		For x = 0 To numberoffiles - 1
			sourcefilename = ListBox1.Items(x)
			destinationfilename = Strings.Left(sourcefilename, Len(sourcefilename) - 3) & "xml"
			System.IO.File.Delete(destinationfilename)
		Next x

		My.Application.DoEvents()
		System.IO.Directory.Delete(basedirectory, True)
		Button7.Enabled = False
	End Sub

	Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
		End
	End Sub

	Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
		Dim temp
		temp = FontDialog1.ShowDialog()
		If temp = Windows.Forms.DialogResult.OK Then
			TextBox7.Text = FontDialog1.Font.Name
		End If
	End Sub

	Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
		If CheckBox1.Checked = True Then
			TextBox8.Enabled = True
		Else
			TextBox8.Enabled = False
		End If
	End Sub
End Class
