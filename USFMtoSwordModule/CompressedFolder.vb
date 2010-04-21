'---------------------------------------------------------------------------------------
'
' CompressedFolder (CompressedFolder.cls)
'
'---------------------------------------------------------------------------------------
'
' Author: Eduardo A. Morcillo
' E-Mail: emorcillo@mvps.org
' Web Page: http://www.mvps.org/emorcillo
'
' Distribution: You can freely use this code in your own applications but you
'               can't publish this code in a web site, online service, or any
'               other media, without my express permission.
'
' Usage: at your own risk.
'
' Tested with:
'               * Windows XP Pro SP1a
'               * Visual Basic.NET 2003 v7.1.3088
'               * .NET Framework v1.1.4322
'
' History:
'           10/14/2003 - This code was released
'
' WARNING! Only the used members of the COM interfaces are declared correctly. Do not call
'          the members that are commented as "Not used".
'
' (Some changes by Rod.)
' (This class is at: http://www.mvps.org/emorcillo/en/code/shell/xpzip.shtml)
'
'---------------------------------------------------------------------------------------
Option Explicit On

Imports System
Imports System.Collections.Specialized
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.UnmanagedType
Imports System.Runtime.InteropServices.VarEnum
Imports System.Runtime.InteropServices.Marshal
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class CompressedFolder
	Implements IDisposable

#Region " Private Fields "

	Private Shared IID_IDataObject As New System.Guid("0000010e-0000-0000-C000-000000000046")
	Private Shared IID_IShellFolder2 As New System.Guid("93F2F68C-1D1B-11d3-A30E-00C04F79ABD1")
	Private Shared IID_IDropTarget As New System.Guid("00000122-0000-0000-C000-000000000046")
	Private Shared IID_IContextMenu As New System.Guid("000214E4-0000-0000-C000-000000000046")

	Private ZipFile As IStorage

#End Region

#Region " Public Enums "

	Public Enum FileInfo
		Filename
		FileType
		PackedSize
		HasAPassword
		Method
		Size
		Ratio
		[Date]
		CRC32
	End Enum

#End Region

#Region " Constructors "

	Public Sub New(ByVal Filename As String)

		If IO.File.Exists(Filename) Then
			openZip(Filename)
		Else
			createZip(Filename)
			openZip(Filename)
		End If

	End Sub

	Private Sub New(ByVal Storage As IStorage)
		ZipFile = Storage
	End Sub

#End Region

#Region " Public Methods "

	Public Sub CompressFile(ByVal Filename As String)
		Dim DropTarget As IDropTarget = Nothing
		Dim DataObject As IDataObject
		Dim ShellFolder As IShellFolder2

		' Get the folder IDropTarget interface
		ShellFolder = ZipFile
		ShellFolder.CreateViewObject(IntPtr.Zero, IID_IDropTarget, DropTarget)

		' Get the file IDataObject interface
		DataObject = getFileDataObject(Filename)

		' Simulate a drag-drop operation
		DropTarget.DragEnter(DataObject, _
				 KeyStates.LeftButton, _
				 0, 0, DragDropEffects.Copy)
		DropTarget.Drop(DataObject, _
				 KeyStates.LeftButton, _
				 0, 0, DragDropEffects.Copy)

	End Sub

	Public Sub DeleteFile(ByVal Name As String)

		ZipFile.DestroyElement(Name)

	End Sub

	Public Sub ExtractFile(ByVal Name As String, ByVal DestFolder As String)
		Dim Stream As IStream
		Dim Data() As Byte
		Dim File As IO.BinaryWriter

		' Open the stream
		Stream = ZipFile.OpenStream(Name)

		' Read the stream data
		Data = readStream(Stream)

		' Close the stream
		Stream = Nothing

		' Save the data to a file
		File = New IO.BinaryWriter(IO.File.Create(DestFolder & "\" & Name))
		File.Write(Data)
		File.Close()

	End Sub

	Public Function GetFileInfo( _
	   ByVal Filename As String, _
	   ByVal Index As FileInfo) As String
		Dim ShellFolder As IShellFolder2
		Dim SD As SHELLDETAILS
		Dim StrPtr As IntPtr
		Dim Pidl As IntPtr

		If Index < 0 Or Index > FileInfo.CRC32 Then Throw New ArgumentException("Invalid Index")

		' Get the IShellFolder2 interface
		ShellFolder = ZipFile

		' Get the file PIDL
		ShellFolder.ParseDisplayName(IntPtr.Zero, 0, Filename, 0, Pidl, 0)

		' Get the column info
		ReDim SD.str(263)
		ShellFolder.GetDetailsOf(Pidl, Index, SD)

		' Convert the info to string
		StrRetToStrW(SD.str, Pidl, StrPtr)
		GetFileInfo = PtrToStringUni(StrPtr)

		' Release the pointers
		FreeCoTaskMem(StrPtr)
		FreeCoTaskMem(Pidl)

	End Function

	Public Function GetFiles() As StringCollection
		Dim oEnum As IEnumSTATSTG
		Dim Stat As StatStg = Nothing
		Dim List As New StringCollection

		oEnum = ZipFile.EnumElements

		Do While oEnum.Next(1, Stat) = 0

			' Files are stored as streams
			If Stat.type = ElementType.Stream Then

				' Get the file name
				List.Add(Stat.name)

			End If

		Loop

		' Return the array
		GetFiles = List

	End Function

	Public Function GetFolders() As StringCollection
		Dim oEnum As IEnumSTATSTG
		Dim Stat As StatStg = Nothing
		Dim List As New StringCollection

		oEnum = ZipFile.EnumElements

		Do While oEnum.Next(1, Stat) = 0

			' Folders are stored as storages
			If Stat.type = ElementType.Storage Then

				' Get the file name
				List.Add(Stat.name)

			End If

		Loop

		' Return the array
		GetFolders = List

	End Function

	Public Function OpenSubFolder(ByVal Name As String) As CompressedFolder
		Dim oFolder As IStorage

		' Open the storage
		oFolder = ZipFile.OpenStorage(Name)

		' Create a new CompressedFolder object
		OpenSubFolder = New CompressedFolder(oFolder)

	End Function

	Public Sub ShowAddPassword()
		Dim oCtxMenu As IContextMenu
		Dim tICI As CMINVOKECOMMANDINFO

		oCtxMenu = getContextMenu()

		tICI.cbSize = Len(tICI)
		tICI.lpVerb = 1
		oCtxMenu.InvokeCommand(tICI)

	End Sub

	Public Sub ShowExtractAll()
		Dim CtxMenu As IContextMenu
		Dim tICI As CMINVOKECOMMANDINFO

		CtxMenu = getContextMenu()

		tICI.cbSize = Len(tICI)
		tICI.lpVerb = 0

		CtxMenu.InvokeCommand(tICI)

	End Sub

	Public Sub Dispose() Implements System.IDisposable.Dispose

		If Not ZipFile Is Nothing Then
			ZipFile.Commit()
			ZipFile = Nothing
		End If

		GC.SuppressFinalize(Me)

	End Sub

#End Region

#Region " Private Methods "

	Private Sub createZip(ByVal Filename As String)
		Dim File As IO.BinaryWriter
		Dim Data(22) As Byte

		' Write an empty .zip file
		File = New IO.BinaryWriter(IO.File.Create(Filename))
		Data(0) = &H50
		Data(1) = &H4B
		Data(2) = &H5
		Data(3) = &H6
		File.Write(Data)
		File.Close()

	End Sub

	Private Sub openZip(ByVal Filename As String)
		Dim PersistFile As ComTypes.IPersistFile ' UCOMIPersistFile

		ZipFile = Nothing

		' Create the CompressedFolder object
		ZipFile = CreateObject("CompressedFolder")

		' Get the IPersistFile interface
		' and load the zip file
		PersistFile = ZipFile
		PersistFile.Load(Filename, Modes.AccessReadWrite Or Modes.ShareExclusive)

	End Sub

	Private Function getContextMenu() As IContextMenu
		Dim ShellFolder As IShellFolder2

		' Get the folder object
		ShellFolder = ZipFile

		' Get the context menu
		Dim result As IContextMenu = Nothing
		ShellFolder.CreateViewObject(IntPtr.Zero, IID_IContextMenu, result)
		Return result
	End Function

	Private Function getFileDataObject( _
	   ByVal Filename As String) As IDataObject
		Dim oDesktop As IShellFolder2 = Nothing
		Dim oParent As IShellFolder2 = Nothing
		Dim sFolder As String
		Dim Pidl As IntPtr

		sFolder = Left$(Filename, InStrRev(Filename, "\") - 1)
		Filename = Mid$(Filename, Len(sFolder) + 2)
		If Right(sFolder, 1) = ":" Then sFolder = sFolder & "\"

		' Get the parent folder object
		SHGetDesktopFolder(oDesktop)

		' Get the parent folder IDL
		oDesktop.ParseDisplayName(IntPtr.Zero, 0, sFolder, 0, Pidl, 0)

		' Get the parent folder object
		oDesktop.BindToObject(Pidl, 0, IID_IShellFolder2, oParent)

		' Release the PIDL
		FreeCoTaskMem(Pidl)

		' Get the file PIDL
		oParent.ParseDisplayName(IntPtr.Zero, 0, Filename, 0, Pidl, 0)

		' Get the file IDataObject
		Dim result As Object = Nothing
		oParent.GetUIObjectOf(IntPtr.Zero, 1, Pidl, IID_IDataObject, 0, result)

		' Release the file PIDL
		FreeCoTaskMem(Pidl)

		Return result
	End Function

	Private Function readStream(ByVal Stream As IStream) As Byte()
		Dim DataPtr As IntPtr
		Dim Data() As Byte
		Dim Stat As StatStg = Nothing

		' Get the stream size
		Stream.Stat(Stat, StatFlags.NoName)

		' Initialize the buffer
		DataPtr = AllocCoTaskMem(Stat.size)

		' Read the data from the stream
		Stream.Read(DataPtr, Stat.size)

		' Copy from the pointer to a byte array
		ReDim Data(Stat.size)
		Copy(DataPtr, Data, 0, Stat.size)

		' Release the pointer
		FreeCoTaskMem(DataPtr)

		Return Data
	End Function

#End Region

#Region " Protected Methods "

	Protected Overrides Sub Finalize()
		MyBase.Finalize()
		Dispose()
	End Sub

#End Region

#Region " COM Interfaces "

	Private Declare Function SHGetDesktopFolder Lib "shell32" ( _
	   ByRef ppshf As IShellFolder2) As Int32

	Private Declare Unicode Function StrRetToStrW Lib "shlwapi" ( _
	   <MarshalAs(LPArray)> ByVal pstr() As Byte, _
	   ByVal pidl As IntPtr, _
	   ByRef ppsz As IntPtr) As Int32

	<Flags()> _
	Private Enum Modes As Integer
		AccessRead = &H0
		AccessWrite = &H1
		AccessReadWrite = &H2
		ShareExclusive = &H10
	End Enum

	< _
	 ComImport(), _
	 Guid("0000000c-0000-0000-C000-000000000046"), _
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown) _
	> _
	Private Interface IStream

		Function Read( _
			<Out()> ByVal pv As IntPtr, _
			<[In]()> ByVal cb As Int32) As Int32

		Sub Write() ' Not used

		Sub Seek() ' Not used

		Sub SetSize() ' Not used

		Sub CopyTo() ' Not used

		Sub Commit() ' Not used

		Sub Revert() ' Not used

		Sub LockRegion() ' Not used

		Sub UnlockRegion() ' Not used

		Sub Stat( _
		  <Out()> ByRef pstatstg As StatStg, _
		  <[In]()> ByVal grfStatFlag As StatFlags)

		Sub Clone() ' Not used

	End Interface

	< _
	 ComImport(), _
	 Guid("0000000b-0000-0000-C000-000000000046"), _
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown) _
	> _
	Private Interface IStorage

		Function CreateStream() ' Not used

		Function OpenStream( _
			<[In](), MarshalAs(LPWStr)> ByVal Name As String, _
			<[In]()> Optional ByVal reserved1 As Int32 = 0I, _
			<[In]()> Optional ByVal grfMode As Int32 = Modes.AccessRead Or Modes.ShareExclusive, _
			<[In]()> Optional ByVal reserved2 As Int32 = 0I) As <MarshalAs([Interface])> IStream

		Function CreateStorage() ' Not used

		Function OpenStorage( _
			<[In](), MarshalAs(LPWStr)> ByVal Name As String, _
			<[In]()> Optional ByVal pstgPriority As Int32 = 0I, _
			<[In]()> Optional ByVal grfMode As Int32 = Modes.AccessReadWrite Or Modes.ShareExclusive, _
			<[In]()> Optional ByVal snbExclude As Int32 = 0I, _
			<[In]()> Optional ByVal reserved As Int32 = 0I) As <MarshalAs([Interface])> IStorage

		Sub CopyTo() ' Not used

		Sub MoveElementTo() ' Not used

		Sub Commit( _
			<[In]()> Optional ByVal grfCommitFlags As Int32 = 0I)

		Sub Revert() ' Not used

		Function EnumElements( _
			<[In]()> Optional ByVal reserved1 As Int32 = 0, _
			<[In]()> Optional ByVal reserved2 As Int32 = 0, _
			<[In]()> Optional ByVal reserved3 As Int32 = 0) As <MarshalAs([Interface])> IEnumSTATSTG

		Sub DestroyElement( _
			<[In](), MarshalAs(LPWStr)> ByVal Name As String)

		Sub RenameElement() ' Not used

		Sub SetElementTimes() ' Not used

		Sub SetClass() ' Not used

		Sub SeStateBits() ' Not used

		Sub Stat() ' Not used

	End Interface

	<Flags()> _
	Private Enum StatFlags As Integer
		[Default] = 0
		NoName = 1
		NoOpen = 2
	End Enum

	Private Enum ElementType As Integer
		Storage = 1
		Stream = 2
		LockBytes = 3
		[Property] = 4
	End Enum

	<ComVisible(False)> _
	Private Structure StatStg

		<FieldOffset(0), MarshalAs(LPWStr)> Public name As String
		<FieldOffset(4)> Public type As ElementType
		<FieldOffset(8)> Public size As Int64
		<FieldOffset(16)> Public mtime As Int64
		<FieldOffset(24)> Public ctime As Int64
		<FieldOffset(32)> Public atime As Int64
		<FieldOffset(40)> Public mode As Int32
		<FieldOffset(44)> Public locksSupported As Int32
		<FieldOffset(48)> Public clsid As System.Guid
		<FieldOffset(52)> Public stateBits As Int32
		<FieldOffset(56)> Public reserved As Int32

	End Structure

	<Flags()> _
	Private Enum KeyStates
		LeftButton = &H1
		RightButton = &H2
		Shift = &H4
		Control = &H8
		MedButton = &H10
		Alt = &H20
	End Enum

	< _
	   ComImport(), _
	   InterfaceType(ComInterfaceType.InterfaceIsIUnknown), _
	   Guid("00000122-0000-0000-C000-000000000046") _
	> _
	Private Interface IDropTarget

		Sub DragEnter( _
		   ByVal pDataObj As IDataObject, _
		   ByVal grfKeyState As KeyStates, _
		   ByVal ptX As Int32, _
		   ByVal ptY As Int32, _
		   ByRef pdwEffect As DragDropEffects)

		Sub DragOver() ' Not used

		Sub DragLeave() ' Not used

		Sub Drop( _
		   ByVal pDataObj As IDataObject, _
		   ByVal grfKeyState As KeyStates, _
		   ByVal ptX As Int32, _
		   ByVal ptY As Int32, _
		   ByRef pdwEffect As DragDropEffects)

	End Interface

	< _
	   ComImport(), _
	   Guid("0000010e-0000-0000-C000-000000000046"), _
	   InterfaceType(ComInterfaceType.InterfaceIsIUnknown) _
	> _
	Private Interface IDataObject

	End Interface

	<StructLayout(LayoutKind.Sequential, Size:=272)> _
	Private Structure SHELLDETAILS
		Dim fmt As Int32
		Dim cxChar As Int32
		<MarshalAs(ByValArray, SizeConst:=264), VBFixedArray(263)> Dim str() As Byte
	End Structure

	< _
	   ComImport(), _
	   InterfaceType(ComInterfaceType.InterfaceIsIUnknown), _
	   Guid("93F2F68C-1D1B-11d3-A30E-00C04F79ABD1") _
	> _
	Private Interface IShellFolder2

		Sub ParseDisplayName( _
			<[In]()> ByVal hwndOwner As IntPtr, _
			<[In]()> ByVal pbcReserved As Int32, _
			<[In](), MarshalAs(LPWStr)> ByVal lpszDisplayName As String, _
			<[In](), Out()> ByRef pchEaten As Int32, _
			<[In](), Out()> ByRef ppidl As IntPtr, _
			<[In](), Out()> ByRef pdwAttributes As Int32)

		Sub EnumObjects() ' Not used

		Sub BindToObject( _
		   <[In]()> ByVal pidl As IntPtr, _
		   <[In]()> ByVal pbcReserved As Int32, _
		   <[In](), Out()> ByRef riid As Guid, _
		   <[In](), Out(), MarshalAs([Interface])> ByRef ppvOut As Object)

		Sub BindToStorage() ' Not used

		Sub CompareIDs() ' Not used

		Sub CreateViewObject( _
		   <[In]()> ByVal hwndOwner As IntPtr, _
		   <[In](), Out()> ByRef riid As Guid, _
		   <[In](), Out(), MarshalAs([Interface])> ByRef ppvOut As Object)

		Sub GetAttributesOf() ' Not used

		Sub GetUIObjectOf( _
		   <[In]()> ByVal hwndOwner As IntPtr, _
		   <[In]()> ByVal cidl As Int32, _
		   <[In]()> ByRef apidl As IntPtr, _
		   <[In](), Out()> ByRef riid As Guid, _
		   <[In](), Out()> ByRef prgfInOut As Int32, _
		   <[In](), Out(), MarshalAs([Interface])> ByRef ppvOut As Object)

		Sub GetDisplayNameOf() ' Not used

		Sub SetNameOf() ' Not used

		Sub GetDefaultSearchGUID() ' Not used

		Sub EnumSearches() ' Not used

		Sub GetDefaultColumn() ' Not used

		Sub GetDefaultColumnState() ' Not used

		Sub GetDetailsEx() ' Not used

		Sub GetDetailsOf( _
		   <[In]()> ByVal pidl As IntPtr, _
		   <[In]()> ByVal iColumn As Int32, _
		   <Out()> ByRef psd As SHELLDETAILS)

		Sub MapColumnToSCID() ' Not used

	End Interface

	< _
	   ComImport(), _
	   Guid("0000000d-0000-0000-C000-000000000046"), _
	   InterfaceType(ComInterfaceType.InterfaceIsIUnknown) _
	> _
	Private Interface IEnumSTATSTG

		<PreserveSig()> _
		Function [Next]( _
		 <[In]()> ByVal celt As Integer, _
		 <Out()> ByRef rgelt As StatStg, _
		 <Out()> Optional ByRef pceltFetched As Integer = 0) As Integer

		Sub Skip( _
		 <[In]()> ByVal celt As Integer)

		Sub Reset()

		Function Clone() As <MarshalAs([Interface])> IEnumSTATSTG

	End Interface

	Private Structure CMINVOKECOMMANDINFO
		Dim cbSize As Int32
		Dim fMask As Int32
		Dim hwnd As IntPtr
		Dim lpVerb As Int32
		Dim lpParameters As IntPtr
		Dim lpDirectory As IntPtr
		Dim nShow As Int32
		Dim dwHotKey As Int32
		Dim hIcon As IntPtr
	End Structure

	< _
	   ComImport(), _
	   Guid("000214E4-0000-0000-C000-000000000046"), _
	   InterfaceType(ComInterfaceType.InterfaceIsIUnknown) _
	> _
	Private Interface IContextMenu

		Sub QueryContextMenu() ' Not Used

		Sub InvokeCommand( _
		   ByRef lpici As CMINVOKECOMMANDINFO)

		Sub GetCommandString() ' Not used

	End Interface

#End Region

End Class
