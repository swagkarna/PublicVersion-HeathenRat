Imports System
Imports System.Text

Namespace Plugin.MessagePack
	' Token: 0x02000006 RID: 6
	Public Class BytesTools
		' Token: 0x06000018 RID: 24 RVA: 0x0000270C File Offset: 0x0000090C
		Public Shared Function GetUtf8Bytes(s As String) As Byte()
			Return BytesTools.utf8Encode.GetBytes(s)
		End Function

		' Token: 0x06000019 RID: 25 RVA: 0x0000272C File Offset: 0x0000092C
		Public Shared Function GetString(utf8Bytes As Byte()) As String
			Return BytesTools.utf8Encode.GetString(utf8Bytes)
		End Function

		' Token: 0x0600001A RID: 26 RVA: 0x0000274C File Offset: 0x0000094C
		Public Shared Function BytesAsString(bytes As Byte()) As String
			Dim sb As StringBuilder = New StringBuilder()
			For Each b As Byte In bytes
				sb.Append(String.Format("{0:D3} ", b))
			Next
			Return sb.ToString()
		End Function

		' Token: 0x0600001B RID: 27 RVA: 0x0000279C File Offset: 0x0000099C
		Public Shared Function BytesAsHexString(bytes As Byte()) As String
			Dim sb As StringBuilder = New StringBuilder()
			For Each b As Byte In bytes
				sb.Append(String.Format("{0:X2} ", b))
			Next
			Return sb.ToString()
		End Function

		' Token: 0x0600001C RID: 28 RVA: 0x000027EC File Offset: 0x000009EC
		Public Shared Function SwapBytes(v As Byte()) As Byte()
			Dim r As Byte() = New Byte(v.Length - 1) {}
			Dim i As Integer = v.Length - 1
			For j As Integer = 0 To r.Length - 1
				r(j) = v(i)
				i -= 1
			Next
			Return r
		End Function

		' Token: 0x0600001D RID: 29 RVA: 0x00002830 File Offset: 0x00000A30
		Public Shared Function SwapInt64(v As Long) As Byte()
			Return BytesTools.SwapBytes(BitConverter.GetBytes(v))
		End Function

		' Token: 0x0600001E RID: 30 RVA: 0x00002850 File Offset: 0x00000A50
		Public Shared Function SwapInt32(v As Integer) As Byte()
			Dim r As Byte() = New Byte() { 0, 0, 0, CByte(v) }
			r(2) = CByte((v >> 8))
			r(1) = CByte((v >> 16))
			r(0) = CByte((v >> 24))
			Return r
		End Function

		' Token: 0x0600001F RID: 31 RVA: 0x00002888 File Offset: 0x00000A88
		Public Shared Function SwapInt16(v As Short) As Byte()
			Dim r As Byte() = New Byte() { 0, CByte(v) }
			r(0) = CByte((v >> 8))
			Return r
		End Function

		' Token: 0x06000020 RID: 32 RVA: 0x000028B0 File Offset: 0x00000AB0
		Public Shared Function SwapDouble(v As Double) As Byte()
			Return BytesTools.SwapBytes(BitConverter.GetBytes(v))
		End Function

		' Token: 0x0400000D RID: 13
		Private Shared utf8Encode As UTF8Encoding = New UTF8Encoding()
	End Class
End Namespace
