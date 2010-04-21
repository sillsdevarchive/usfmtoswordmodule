Module Module1
	' From http://rprateek.blogspot.com/2008/09/how-to-read-or-write-ini-file-from.html

	Private Declare Unicode Function WritePrivateProfileString Lib "kernel32" _
	Alias "WritePrivateProfileStringW" (ByVal lpApplicationName As String, _
	ByVal lpKeyName As String, ByVal lpString As String, _
	ByVal lpFileName As String) As Int32

	Private Declare Unicode Function GetPrivateProfileString Lib "kernel32" _
	Alias "GetPrivateProfileStringW" (ByVal lpApplicationName As String, _
	ByVal lpKeyName As String, ByVal lpDefault As String, _
	ByVal lpReturnedString As String, ByVal nSize As Int32, _
	ByVal lpFileName As String) As Int32


	Public Sub WriteIni(ByVal iniFileName As String, ByVal Section As String, ByVal ParamName As String, ByVal ParamVal As String)
		Dim Result As Integer
		Result = WritePrivateProfileString(Section, ParamName, ParamVal, iniFileName)
	End Sub

	Public Function ReadIni(ByVal IniFileName As String, ByVal Section As String, ByVal ParamName As String, ByVal ParamDefault As String) As String
		Dim ParamVal As String
		Dim LenParamVal As Long
		ParamVal = Space$(1024)
		LenParamVal = GetPrivateProfileString(Section, ParamName, ParamDefault, ParamVal, Len(ParamVal), IniFileName)
		ReadIni = Left$(ParamVal, LenParamVal)
	End Function
End Module
