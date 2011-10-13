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
				If Not (ListBox1.Items.Contains(file)) Then
					ListBox1.Items.Add(file)
					Button3.Enabled = True
					workingdirectory = System.IO.Path.GetDirectoryName(file)
					ToolStripMenuItem1.Enabled = True
				End If
			Next
			Label1.Text = "Files:" + Str(ListBox1.Items.Count) + "                   Encoding:"
		End If
	End Sub

	Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
		ListBox1.Items.Clear()
		Button3.Enabled = False
		ToolStripMenuItem1.Enabled = False
		Label1.Text = "Encoding:"
	End Sub

	Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
		Dim temp As Integer
		Dim x, numberoffiles As Integer
		Dim divider, perlbin As String
		Dim commandlineargs, sourcefilename, destinationfilename, encoding, controlfile, logfile As String
		Dim ignoremarkers As Boolean
		Dim ListItems() As String
		Dim bitremoved As String

		ListItems = Nothing

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

			' Count number of files to process and prepare progressbar
			numberoffiles = ListBox1.Items.Count
			If CheckBox2.Checked = True Then
				' User requested that cross references be added
				Dialog1.ProgressBar1.Maximum = numberoffiles * 3
			Else
				Dialog1.ProgressBar1.Maximum = numberoffiles * 2
			End If
			Dialog1.ProgressBar1.Value = 0

			' Check for markers to ignore
			If ((Form3.ListBox1.Items.Count > 0) Or (Form3.ListBox2.Items.Count > 0) Or (Form3.ListBox3.Items.Count > 0)) Then
				ignoremarkers = True

				If CheckBox2.Checked = True Then
					' User requested that cross references be added
					Dialog1.ProgressBar1.Maximum = numberoffiles * 4
				Else
					Dialog1.ProgressBar1.Maximum = numberoffiles * 3
				End If

				Dialog1.TextBox1.Text = Dialog1.TextBox1.Text + divider & Chr(10) & divider & Chr(10) & "IGNORING MARKERS..."
				Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
				Dialog1.TextBox1.ScrollToCaret()
				Dialog1.TextBox1.Focus()
				My.Application.DoEvents()

				' Save all ListBox1 entries to temporary array
				ListItems = New String(numberoffiles) {}
				ListBox1.Items.CopyTo(ListItems, 0)

				' Process files
				For x = 0 To numberoffiles - 1
					' Open file
					sourcefilename = ListBox1.Items(x)
					Dim objReader As StreamReader
					Dim tempstring As String

					objReader = New StreamReader(sourcefilename)
					tempstring = objReader.ReadToEnd()
					objReader.Close()

					' Strip markers
					Dim prevpos, startpos, textpos, textlen, xloop As Double
					Dim marker, newstring As String

					' This is where the markers are stripped
					prevpos = 1
					startpos = 1
					textpos = 1
					textlen = Len(tempstring)
					newstring = ""

					While (textpos < textlen)
						' Search for next marker
						While (Mid(tempstring, textpos, 1) <> "\")
							textpos = textpos + 1
							If textpos > textlen Then GoTo finished
						End While

						' Get what marker has been found
						startpos = textpos
						While ((Mid(tempstring, textpos, 1) <> " ") And (Mid(tempstring, textpos, 1) <> Chr(10)))
							textpos = textpos + 1
							If (Mid(tempstring, textpos, 1) = "*") Then
								' End of marker found (but it might not be followed by space)
								textpos = textpos + 1
								GoTo foundmarker
							End If
							If textpos > textlen Then GoTo finished
						End While
foundmarker:
						marker = Mid(tempstring, startpos, textpos - startpos)

						' Dealing with markers with a line of characters following
						For xloop = 1 To Form3.ListBox1.Items.Count
							If (marker = Form3.ListBox1.Items(xloop - 1)) Then
								' Need to scan to end of line by scanning for next marker
								textpos = textpos + 1
								While (Mid(tempstring, textpos, 1) <> Chr(10))
									textpos = textpos + 1
								End While
								textpos = textpos + 1
								GoTo stripmarker
							End If
						Next

						' Dealing with markers that have no characters following
						For xloop = 1 To Form3.ListBox2.Items.Count
							If (marker = Form3.ListBox2.Items(xloop - 1)) Then
								textpos = textpos + 1
								GoTo stripmarker
							End If
						Next

						' Dealing with markers that have an end of marker
						For xloop = 1 To Form3.ListBox3.Items.Count
							If (marker = Form3.ListBox3.Items(xloop - 1)) Then
								' Need to scan to find \[MKR]*
								textpos = textpos + 1
								While (Mid(tempstring, textpos, Len(marker) + 1) <> marker + "*")
									textpos = textpos + 1
								End While
								textpos = textpos + Len(marker) + 1
								GoTo stripmarker
							End If
						Next

						GoTo nextmarker

						' Copy the text from the previous position to the start of the unwanted marker, then continue scanning after the end of the marker's content
stripmarker:
						newstring = newstring + Mid(tempstring, prevpos, startpos - prevpos)
						bitremoved = Mid(tempstring, startpos, textpos - startpos)
						prevpos = textpos
						If marker = "\f" Then
							newstring = newstring + " "
						End If

nextmarker:
					End While

finished:
					newstring = newstring + Mid(tempstring, prevpos, textlen - prevpos)

reallyfinished:
					' Save using new filename
					destinationfilename = Strings.Left(sourcefilename, Len(sourcefilename) - 3) & "tmp"

					Dim objWriter As StreamWriter

					objWriter = New StreamWriter(destinationfilename)
					objWriter.Write(newstring)
					objWriter.Close()

					' Replace entry in ListBox1
					ListBox1.Items.Remove(sourcefilename)
					ListBox1.Items.Add(destinationfilename)

					Dialog1.ProgressBar1.Value = Dialog1.ProgressBar1.Value + 1
					Dialog1.ProgressBar1.Update()

					My.Application.DoEvents()
				Next x

				Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & " DONE!" & Chr(10)
				Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
				Dialog1.TextBox1.ScrollToCaret()
				Dialog1.TextBox1.Focus()
				My.Application.DoEvents()
			End If

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
			Dialog1.TextBox1.Text = Dialog1.TextBox1.Text + divider & Chr(10) & divider & Chr(10) & "GENERATING .CONF FILE..."
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

			' Add crossreferences if requested
			If CheckBox2.Checked = True Then
				Dialog1.Label1.Text = "Adding Cross References to OSIS files..."
				Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & divider & Chr(10) & "ADDING CROSS REFERENCES TO OSIS FILES..." & Chr(10) & divider
				Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
				Dialog1.TextBox1.ScrollToCaret()
				Dialog1.TextBox1.Focus()
				My.Application.DoEvents()
				ChDir(applicationdirectory) ' Need to do this to get access to CrossReferences files

				For x = 0 To numberoffiles - 1
					'perl.exe addCrossRefs.pl CF/CF_addCrossRefs_NLT96_GEN.txt NLT96/genxml/01GENNLT96.xml NLT96/genxml/01GENNLT96_CR.xml NLT96_addCrossRefs.log
					sourcefilename = Strings.Left(ListBox1.Items(x), Len(ListBox1.Items(x)) - 3) & "xml"
					destinationfilename = Strings.Left(sourcefilename, Len(sourcefilename) - 4) & "_CR.xml"
					logfile = workingdirectory + "\logfile_CR.txt"
					If System.IO.File.Exists(logfile) Then
						System.IO.File.Delete(logfile)
					End If
					If ComboBox2.SelectedIndex = 1 Then
						controlfile = applicationdirectory & "\CF_West\"
					Else
						controlfile = applicationdirectory & "\CF_East\"
					End If

					If sourcefilename.Contains("COL") Then
						' This is necessary to avoid conflict with 2CO
						controlfile = controlfile + "CF_addCrossRefs_COL.txt"
						GoTo continueprocessing
					End If
					If sourcefilename.Contains("1CH") Then controlfile = controlfile + "CF_addCrossRefs_1CH.txt"
					If sourcefilename.Contains("1CO") Then controlfile = controlfile + "CF_addCrossRefs_1CO.txt"
					If sourcefilename.Contains("1JN") Then controlfile = controlfile + "CF_addCrossRefs_1JN.txt"
					If sourcefilename.Contains("1KI") Then controlfile = controlfile + "CF_addCrossRefs_1KI.txt"
					If sourcefilename.Contains("1PE") Then controlfile = controlfile + "CF_addCrossRefs_1PE.txt"
					If sourcefilename.Contains("1SA") Then controlfile = controlfile + "CF_addCrossRefs_1SA.txt"
					If sourcefilename.Contains("1TH") Then controlfile = controlfile + "CF_addCrossRefs_1TH.txt"
					If sourcefilename.Contains("1TI") Then controlfile = controlfile + "CF_addCrossRefs_1TI.txt"
					If sourcefilename.Contains("2CH") Then controlfile = controlfile + "CF_addCrossRefs_2CH.txt"
					If sourcefilename.Contains("2CO") Then controlfile = controlfile + "CF_addCrossRefs_2CO.txt"
					If sourcefilename.Contains("2JN") Then controlfile = controlfile + "CF_addCrossRefs_2JN.txt"
					If sourcefilename.Contains("2KI") Then controlfile = controlfile + "CF_addCrossRefs_2KI.txt"
					If sourcefilename.Contains("2PE") Then controlfile = controlfile + "CF_addCrossRefs_2PE.txt"
					If sourcefilename.Contains("2SA") Then controlfile = controlfile + "CF_addCrossRefs_2SA.txt"
					If sourcefilename.Contains("2TH") Then controlfile = controlfile + "CF_addCrossRefs_2TH.txt"
					If sourcefilename.Contains("2TI") Then controlfile = controlfile + "CF_addCrossRefs_2TI.txt"
					If sourcefilename.Contains("3JN") Then controlfile = controlfile + "CF_addCrossRefs_3JN.txt"
					If sourcefilename.Contains("ACT") Then controlfile = controlfile + "CF_addCrossRefs_ACT.txt"
					If sourcefilename.Contains("AMO") Then controlfile = controlfile + "CF_addCrossRefs_AMO.txt"
					If sourcefilename.Contains("DAN") Then controlfile = controlfile + "CF_addCrossRefs_DAN.txt"
					If sourcefilename.Contains("DEU") Then controlfile = controlfile + "CF_addCrossRefs_DEU.txt"
					If sourcefilename.Contains("ECC") Then controlfile = controlfile + "CF_addCrossRefs_ECC.txt"
					If sourcefilename.Contains("EPH") Then controlfile = controlfile + "CF_addCrossRefs_EPH.txt"
					If sourcefilename.Contains("EST") Then controlfile = controlfile + "CF_addCrossRefs_EST.txt"
					If sourcefilename.Contains("EXO") Then controlfile = controlfile + "CF_addCrossRefs_EXO.txt"
					If sourcefilename.Contains("EZK") Then controlfile = controlfile + "CF_addCrossRefs_EZK.txt"
					If sourcefilename.Contains("EZR") Then controlfile = controlfile + "CF_addCrossRefs_EZR.txt"
					If sourcefilename.Contains("GAL") Then controlfile = controlfile + "CF_addCrossRefs_GAL.txt"
					If sourcefilename.Contains("GEN") Then controlfile = controlfile + "CF_addCrossRefs_GEN.txt"
					If sourcefilename.Contains("HAB") Then controlfile = controlfile + "CF_addCrossRefs_HAB.txt"
					If sourcefilename.Contains("HAG") Then controlfile = controlfile + "CF_addCrossRefs_HAG.txt"
					If sourcefilename.Contains("HEB") Then controlfile = controlfile + "CF_addCrossRefs_HEB.txt"
					If sourcefilename.Contains("HOS") Then controlfile = controlfile + "CF_addCrossRefs_HOS.txt"
					If sourcefilename.Contains("ISA") Then controlfile = controlfile + "CF_addCrossRefs_ISA.txt"
					If sourcefilename.Contains("JAS") Then controlfile = controlfile + "CF_addCrossRefs_JAS.txt"
					If sourcefilename.Contains("JDG") Then controlfile = controlfile + "CF_addCrossRefs_JDG.txt"
					If sourcefilename.Contains("JER") Then controlfile = controlfile + "CF_addCrossRefs_JER.txt"
					If sourcefilename.Contains("JHN") Then controlfile = controlfile + "CF_addCrossRefs_JHN.txt"
					If sourcefilename.Contains("JOB") Then controlfile = controlfile + "CF_addCrossRefs_JOB.txt"
					If sourcefilename.Contains("JOL") Then controlfile = controlfile + "CF_addCrossRefs_JOL.txt"
					If sourcefilename.Contains("JON") Then controlfile = controlfile + "CF_addCrossRefs_JON.txt"
					If sourcefilename.Contains("JOS") Then controlfile = controlfile + "CF_addCrossRefs_JOS.txt"
					If sourcefilename.Contains("JUD") Then controlfile = controlfile + "CF_addCrossRefs_JUD.txt"
					If sourcefilename.Contains("LAM") Then controlfile = controlfile + "CF_addCrossRefs_LAM.txt"
					If sourcefilename.Contains("LEV") Then controlfile = controlfile + "CF_addCrossRefs_LEV.txt"
					If sourcefilename.Contains("LUK") Then controlfile = controlfile + "CF_addCrossRefs_LUK.txt"
					If sourcefilename.Contains("MAL") Then controlfile = controlfile + "CF_addCrossRefs_MAL.txt"
					If sourcefilename.Contains("MAT") Then controlfile = controlfile + "CF_addCrossRefs_MAT.txt"
					If sourcefilename.Contains("MIC") Then controlfile = controlfile + "CF_addCrossRefs_MIC.txt"
					If sourcefilename.Contains("MRK") Then controlfile = controlfile + "CF_addCrossRefs_MRK.txt"
					If sourcefilename.Contains("NAM") Then controlfile = controlfile + "CF_addCrossRefs_NAM.txt"
					If sourcefilename.Contains("NEH") Then controlfile = controlfile + "CF_addCrossRefs_NEH.txt"
					If sourcefilename.Contains("NUM") Then controlfile = controlfile + "CF_addCrossRefs_NUM.txt"
					If sourcefilename.Contains("OBA") Then controlfile = controlfile + "CF_addCrossRefs_OBA.txt"
					If sourcefilename.Contains("PHM") Then controlfile = controlfile + "CF_addCrossRefs_PHM.txt"
					If sourcefilename.Contains("PHP") Then controlfile = controlfile + "CF_addCrossRefs_PHP.txt"
					If sourcefilename.Contains("PRO") Then controlfile = controlfile + "CF_addCrossRefs_PRO.txt"
					If sourcefilename.Contains("PSA") Then controlfile = controlfile + "CF_addCrossRefs_PSA.txt"
					If sourcefilename.Contains("REV") Then controlfile = controlfile + "CF_addCrossRefs_REV.txt"
					If sourcefilename.Contains("ROM") Then controlfile = controlfile + "CF_addCrossRefs_ROM.txt"
					If sourcefilename.Contains("RUT") Then controlfile = controlfile + "CF_addCrossRefs_RUT.txt"
					If sourcefilename.Contains("SNG") Then controlfile = controlfile + "CF_addCrossRefs_SNG.txt"
					If sourcefilename.Contains("TIT") Then controlfile = controlfile + "CF_addCrossRefs_TIT.txt"
					If sourcefilename.Contains("ZEC") Then controlfile = controlfile + "CF_addCrossRefs_ZEC.txt"
					If sourcefilename.Contains("ZEP") Then controlfile = controlfile + "CF_addCrossRefs_ZEP.txt"
continueprocessing:
					commandlineargs = Chr(34) & applicationdirectory & "\addCrossRefs.pl" & Chr(34) & " " & Chr(34) & controlfile & Chr(34) & " " & Chr(34) & sourcefilename & Chr(34) & " " & Chr(34) & destinationfilename & Chr(34) & " " & Chr(34) & logfile & Chr(34)

					Dim psi As New _
					System.Diagnostics.ProcessStartInfo(perlbin)
					psi.RedirectStandardOutput = True
					psi.CreateNoWindow = True
					psi.UseShellExecute = False
					psi.Arguments = commandlineargs
					Dim listFiles As System.Diagnostics.Process
					listFiles = System.Diagnostics.Process.Start(psi)
					listFiles.WaitForExit(60000)
					If listFiles.HasExited Then
						Dim oRead As System.IO.StreamReader
						oRead = File.OpenText(logfile)
						Dim output As String = oRead.ReadToEnd()
						oRead.Close()
						oRead.Dispose()
						Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(10) & divider & Chr(10) & output
						Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
						Dialog1.TextBox1.ScrollToCaret()
						Dialog1.TextBox1.Focus()
					Else
						listFiles.Kill()
						Dim oRead As System.IO.StreamReader
						oRead = File.OpenText(logfile)
						Dim output As String = oRead.ReadToEnd()
						oRead.Close()
						oRead.Dispose()
						Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(10) & divider & Chr(10) & "PROBLEM ADDING CROSSREFERENCES " & sourcefilename
						Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(10) & "OUTPUT LOGFILE:"
						Dialog1.TextBox1.Text = Dialog1.TextBox1.Text & Chr(10) & output
						Dialog1.TextBox1.SelectionStart = Dialog1.TextBox1.TextLength
						Dialog1.TextBox1.ScrollToCaret()
						Dialog1.TextBox1.Focus()
					End If

					' Copy temp file to actual file and delete temp file
					System.IO.File.Copy(destinationfilename, sourcefilename, True)
					System.IO.File.Delete(destinationfilename)
					If System.IO.File.Exists(logfile) Then
						System.IO.File.Delete(logfile)
					End If
					Dialog1.ProgressBar1.Value = Dialog1.ProgressBar1.Value + 1
					Dialog1.ProgressBar1.Update()

					My.Application.DoEvents()

				Next x
			End If

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
				Dim timecount
				timecount = 0
				Do Until listFiles.HasExited
					Application.DoEvents()
					If timecount > 60000 * 1 Then Exit Do
					System.Threading.Thread.Sleep(250)
					timecount = timecount + 250
				Loop
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
		' If markers were ignored, restore ListBox1 items from temporary array
		If ignoremarkers Then
			ListBox1.Items.Clear()
			For x = 0 To numberoffiles - 1
				ListBox1.Items.Add(ListItems(x))
			Next x
		End If
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
		CheckBox2.Checked = False
		ComboBox1.SelectedIndex = -1
		ComboBox2.SelectedIndex = -1
		ComboBox3.SelectedIndex = -1
		ComboBox4.SelectedIndex = -1
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
			WriteIni(strFilepath, "Settings", "CrossReferences", ComboBox4.Text)
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
			ComboBox4.Text = ReadIni(strFilepath, "Settings", "CrossReferences", "")
			If TextBox8.Text <> "" Then
				CheckBox1.Checked = True
			End If
			If ComboBox4.Text <> "" Then
				CheckBox2.Checked = True
			End If
		End If
	End Sub

	Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		applicationdirectory = CurDir()
		Form3.Show()
		Form3.Hide()
		Me.Text = Me.Text + " " + Mid(Application.ProductVersion, 1, Len(Application.ProductVersion) - 4)
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

	Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
		Form3.Show()
	End Sub

	Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
		ListBox1.Items.Remove(ListBox1.SelectedItem)
		If ListBox1.Items.Count = 0 Then
			ToolStripMenuItem1.Enabled = False
		End If
	End Sub

	Private Sub ListBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListBox1.MouseDown
		If e.Button = MouseButtons.Right Then
			Dim pt As Point
			pt.X = e.X
			pt.Y = e.Y
			ListBox1.SelectedIndex = ListBox1.IndexFromPoint(pt)
		End If
	End Sub

	Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
		If CheckBox2.Checked = True Then
			ComboBox4.Enabled = True
		Else
			ComboBox4.Enabled = False
		End If
	End Sub
End Class
