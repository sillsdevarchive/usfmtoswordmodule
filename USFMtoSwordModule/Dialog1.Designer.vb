<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Dialog1
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
		Me.Label1 = New System.Windows.Forms.Label
		Me.TextBox1 = New System.Windows.Forms.TextBox
		Me.Button1 = New System.Windows.Forms.Button
		Me.SuspendLayout()
		'
		'ProgressBar1
		'
		Me.ProgressBar1.Location = New System.Drawing.Point(13, 30)
		Me.ProgressBar1.Name = "ProgressBar1"
		Me.ProgressBar1.Size = New System.Drawing.Size(410, 23)
		Me.ProgressBar1.TabIndex = 2
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(9, 9)
		Me.Label1.MinimumSize = New System.Drawing.Size(410, 0)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(410, 13)
		Me.Label1.TabIndex = 3
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
		'
		'TextBox1
		'
		Me.TextBox1.Location = New System.Drawing.Point(13, 75)
		Me.TextBox1.Multiline = True
		Me.TextBox1.Name = "TextBox1"
		Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.TextBox1.Size = New System.Drawing.Size(410, 290)
		Me.TextBox1.TabIndex = 4
		'
		'Button1
		'
		Me.Button1.Enabled = False
		Me.Button1.Location = New System.Drawing.Point(159, 379)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(110, 28)
		Me.Button1.TabIndex = 5
		Me.Button1.Text = "OK"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'Dialog1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(435, 418)
		Me.Controls.Add(Me.Button1)
		Me.Controls.Add(Me.TextBox1)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.ProgressBar1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "Dialog1"
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "USFMtoSwordModule"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
	Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
