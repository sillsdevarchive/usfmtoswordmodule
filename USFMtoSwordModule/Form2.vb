' Create an InputBox like the one of VB6. You can define the dialog's title,
'  question and default value.
' Example: MessageBox.Show(InputDialog.InputBox("Type your name:", "Test",
'  "Marco"))

Public Class InputDialog
	Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		'Add any initialization after the InitializeComponent() call
	End Sub

	'Form overrides dispose to clean up the component list.
	Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	Friend WithEvents btnOK As System.Windows.Forms.Button
	Friend WithEvents btnCancel As System.Windows.Forms.Button
	Friend WithEvents txtValue As System.Windows.Forms.TextBox
	Friend WithEvents lblPrompt As System.Windows.Forms.Label
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.btnOK = New System.Windows.Forms.Button
		Me.btnCancel = New System.Windows.Forms.Button
		Me.txtValue = New System.Windows.Forms.TextBox
		Me.lblPrompt = New System.Windows.Forms.Label
		Me.SuspendLayout()
		'
		'btnOK
		'
		Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or _
			System.Windows.Forms.AnchorStyles.Right), _
			System.Windows.Forms.AnchorStyles)
		Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOK.Location = New System.Drawing.Point(232, 84)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.Size = New System.Drawing.Size(72, 24)
		Me.btnOK.TabIndex = 6
		Me.btnOK.Text = "&OK"
		'
		'btnCancel
		'
		Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom _
			Or System.Windows.Forms.AnchorStyles.Right), _
			System.Windows.Forms.AnchorStyles)
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Location = New System.Drawing.Point(312, 84)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.Size = New System.Drawing.Size(72, 24)
		Me.btnCancel.TabIndex = 5
		Me.btnCancel.Text = "&Cancel"
		'
		'txtValue
		'
		Me.txtValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or _
			System.Windows.Forms.AnchorStyles.Left) Or _
			System.Windows.Forms.AnchorStyles.Right), _
			System.Windows.Forms.AnchorStyles)
		Me.txtValue.Location = New System.Drawing.Point(8, 48)
		Me.txtValue.Name = "txtValue"
		Me.txtValue.Size = New System.Drawing.Size(376, 20)
		Me.txtValue.TabIndex = 3
		Me.txtValue.Text = ""
		'
		'lblPrompt
		'
		Me.lblPrompt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or _
			System.Windows.Forms.AnchorStyles.Left) Or _
			System.Windows.Forms.AnchorStyles.Right), _
			System.Windows.Forms.AnchorStyles)
		Me.lblPrompt.Location = New System.Drawing.Point(8, 8)
		Me.lblPrompt.Name = "lblPrompt"
		Me.lblPrompt.Size = New System.Drawing.Size(376, 32)
		Me.lblPrompt.TabIndex = 4
		'
		'InputDialog
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(392, 113)
		Me.Controls.Add(Me.btnOK)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.txtValue)
		Me.Controls.Add(Me.lblPrompt)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "InputDialog"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.ResumeLayout(False)

	End Sub

#End Region

	Public Property Prompt() As String
		Get
			Return lblPrompt.Text
		End Get
		Set(ByVal Value As String)
			lblPrompt.Text = Value
		End Set
	End Property

	Public Property Value() As String
		Get
			Return txtValue.Text.Trim()
		End Get
		Set(ByVal Value As String)
			txtValue.Text = Value.Trim()
			' preselect the text, and give the focus to this control
			txtValue.SelectAll()
			txtValue.Focus()
		End Set
	End Property

	' create an InputDialog window, and return the typed text
	Public Shared Function InputBox(ByVal prompt As String, _
		ByVal title As String, ByVal defaultVal As String) As String
		Dim dlg As New InputDialog
		dlg.Text = title
		dlg.Prompt = prompt
		dlg.Value = defaultVal

		If dlg.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
			Return dlg.Value
		Else
			Return defaultVal
		End If
	End Function
End Class
