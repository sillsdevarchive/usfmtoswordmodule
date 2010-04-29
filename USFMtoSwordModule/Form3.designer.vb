<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing AndAlso components IsNot Nothing Then
			components.Dispose()
		End If
		MyBase.Dispose(disposing)
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form3))
		Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
		Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
		Me.ListBox1 = New System.Windows.Forms.ListBox
		Me.Label1 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.ListBox2 = New System.Windows.Forms.ListBox
		Me.Label3 = New System.Windows.Forms.Label
		Me.ListBox3 = New System.Windows.Forms.ListBox
		Me.Button5 = New System.Windows.Forms.Button
		Me.Button8 = New System.Windows.Forms.Button
		Me.Button6 = New System.Windows.Forms.Button
		Me.Button7 = New System.Windows.Forms.Button
		Me.Button9 = New System.Windows.Forms.Button
		Me.Button10 = New System.Windows.Forms.Button
		Me.Button11 = New System.Windows.Forms.Button
		Me.Button12 = New System.Windows.Forms.Button
		Me.SaveFileDialog2 = New System.Windows.Forms.SaveFileDialog
		Me.OpenFileDialog2 = New System.Windows.Forms.OpenFileDialog
		Me.Label4 = New System.Windows.Forms.Label
		Me.Label5 = New System.Windows.Forms.Label
		Me.Label6 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		'
		'OpenFileDialog1
		'
		Me.OpenFileDialog1.DefaultExt = "txt"
		Me.OpenFileDialog1.Title = "Select file to open"
		'
		'SaveFileDialog1
		'
		Me.SaveFileDialog1.DefaultExt = "txt"
		'
		'ListBox1
		'
		Me.ListBox1.FormattingEnabled = True
		Me.ListBox1.Location = New System.Drawing.Point(4, 93)
		Me.ListBox1.Name = "ListBox1"
		Me.ListBox1.Size = New System.Drawing.Size(212, 108)
		Me.ListBox1.TabIndex = 5
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(2, 64)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(216, 13)
		Me.Label1.TabIndex = 6
		Me.Label1.Text = "Markers followed by whole line of characters"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(2, 237)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(179, 13)
		Me.Label2.TabIndex = 8
		Me.Label2.Text = "Markers with no characters following"
		'
		'ListBox2
		'
		Me.ListBox2.FormattingEnabled = True
		Me.ListBox2.Location = New System.Drawing.Point(4, 266)
		Me.ListBox2.Name = "ListBox2"
		Me.ListBox2.Size = New System.Drawing.Size(212, 108)
		Me.ListBox2.TabIndex = 7
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(2, 413)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(135, 13)
		Me.Label3.TabIndex = 10
		Me.Label3.Text = "Markers with end of marker"
		'
		'ListBox3
		'
		Me.ListBox3.FormattingEnabled = True
		Me.ListBox3.Location = New System.Drawing.Point(5, 442)
		Me.ListBox3.Name = "ListBox3"
		Me.ListBox3.Size = New System.Drawing.Size(212, 108)
		Me.ListBox3.TabIndex = 9
		'
		'Button5
		'
		Me.Button5.Enabled = False
		Me.Button5.Location = New System.Drawing.Point(162, 207)
		Me.Button5.Name = "Button5"
		Me.Button5.Size = New System.Drawing.Size(54, 20)
		Me.Button5.TabIndex = 12
		Me.Button5.Text = "Delete"
		Me.Button5.UseVisualStyleBackColor = True
		'
		'Button8
		'
		Me.Button8.Location = New System.Drawing.Point(97, 207)
		Me.Button8.Name = "Button8"
		Me.Button8.Size = New System.Drawing.Size(54, 20)
		Me.Button8.TabIndex = 15
		Me.Button8.Text = "Add..."
		Me.Button8.UseVisualStyleBackColor = True
		'
		'Button6
		'
		Me.Button6.Location = New System.Drawing.Point(97, 380)
		Me.Button6.Name = "Button6"
		Me.Button6.Size = New System.Drawing.Size(54, 20)
		Me.Button6.TabIndex = 17
		Me.Button6.Text = "Add..."
		Me.Button6.UseVisualStyleBackColor = True
		'
		'Button7
		'
		Me.Button7.Enabled = False
		Me.Button7.Location = New System.Drawing.Point(161, 380)
		Me.Button7.Name = "Button7"
		Me.Button7.Size = New System.Drawing.Size(54, 20)
		Me.Button7.TabIndex = 16
		Me.Button7.Text = "Delete"
		Me.Button7.UseVisualStyleBackColor = True
		'
		'Button9
		'
		Me.Button9.Location = New System.Drawing.Point(98, 556)
		Me.Button9.Name = "Button9"
		Me.Button9.Size = New System.Drawing.Size(54, 20)
		Me.Button9.TabIndex = 19
		Me.Button9.Text = "Add..."
		Me.Button9.UseVisualStyleBackColor = True
		'
		'Button10
		'
		Me.Button10.Enabled = False
		Me.Button10.Location = New System.Drawing.Point(162, 556)
		Me.Button10.Name = "Button10"
		Me.Button10.Size = New System.Drawing.Size(54, 20)
		Me.Button10.TabIndex = 18
		Me.Button10.Text = "Delete"
		Me.Button10.UseVisualStyleBackColor = True
		'
		'Button11
		'
		Me.Button11.Location = New System.Drawing.Point(5, 8)
		Me.Button11.Name = "Button11"
		Me.Button11.Size = New System.Drawing.Size(213, 20)
		Me.Button11.TabIndex = 21
		Me.Button11.Text = "Load set of markers..."
		Me.Button11.UseVisualStyleBackColor = True
		'
		'Button12
		'
		Me.Button12.Location = New System.Drawing.Point(7, 34)
		Me.Button12.Name = "Button12"
		Me.Button12.Size = New System.Drawing.Size(211, 20)
		Me.Button12.TabIndex = 22
		Me.Button12.Text = "Save set of markers..."
		Me.Button12.UseVisualStyleBackColor = True
		'
		'SaveFileDialog2
		'
		Me.SaveFileDialog2.Filter = "Marker files|*.mkr|All files|*.*"
		'
		'OpenFileDialog2
		'
		Me.OpenFileDialog2.FileName = "OpenFileDialog2"
		Me.OpenFileDialog2.Filter = "Marker files|*.mkr|All files|*.*"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(12, 77)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(38, 13)
		Me.Label4.TabIndex = 23
		Me.Label4.Text = "e.g. \s"
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(12, 250)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(58, 13)
		Me.Label5.TabIndex = 24
		Me.Label5.Text = "e.g. \p, \m"
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(12, 426)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(60, 13)
		Me.Label6.TabIndex = 25
		Me.Label6.Text = "e.g. \x, \fig"
		'
		'Form3
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(222, 585)
		Me.Controls.Add(Me.Label6)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Button12)
		Me.Controls.Add(Me.Button11)
		Me.Controls.Add(Me.Button9)
		Me.Controls.Add(Me.Button10)
		Me.Controls.Add(Me.Button6)
		Me.Controls.Add(Me.Button7)
		Me.Controls.Add(Me.Button8)
		Me.Controls.Add(Me.Button5)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.ListBox3)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.ListBox2)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.ListBox1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "Form3"
		Me.Text = "Ignore Markers"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
	Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
	Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents ListBox3 As System.Windows.Forms.ListBox
	Friend WithEvents Button5 As System.Windows.Forms.Button
	Friend WithEvents Button8 As System.Windows.Forms.Button
	Friend WithEvents Button6 As System.Windows.Forms.Button
	Friend WithEvents Button7 As System.Windows.Forms.Button
	Friend WithEvents Button9 As System.Windows.Forms.Button
	Friend WithEvents Button10 As System.Windows.Forms.Button
	Friend WithEvents Button11 As System.Windows.Forms.Button
	Friend WithEvents Button12 As System.Windows.Forms.Button
	Friend WithEvents SaveFileDialog2 As System.Windows.Forms.SaveFileDialog
	Friend WithEvents OpenFileDialog2 As System.Windows.Forms.OpenFileDialog
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents Label6 As System.Windows.Forms.Label

End Class
