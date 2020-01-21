Imports System
Imports System.IO

Namespace Plugin.MessagePack
	' Token: 0x0200000B RID: 11
	Friend Class ReadTools
		' Token: 0x06000059 RID: 89 RVA: 0x00003E3C File Offset: 0x0000203C
		Public Shared Function ReadString(ms As Stream, len As Integer) As String
			Dim rawBytes As Byte() = New Byte(len - 1) {}
			ms.Read(rawBytes, 0, len)
			Return BytesTools.GetString(rawBytes)
		End Function

		' Token: 0x0600005A RID: 90 RVA: 0x00003E68 File Offset: 0x00002068
		Public Shared Function ReadString(ms As Stream) As String
			Dim strFlag As Byte = CByte(ms.ReadByte())
			Return ReadTools.ReadString(strFlag, ms)
		End Function

		' Token: 0x0600005B RID: 91 RVA: 0x00003E8C File Offset: 0x0000208C
		Public Shared Function ReadString(strFlag As Byte, ms As Stream) As String
			Dim len As Integer = 0
			Dim flag As Boolean = strFlag >= 160 AndAlso strFlag <= 191
			Dim rawBytes As Byte()
			If flag Then
				len = CInt((strFlag - 160))
			Else
				Dim flag2 As Boolean = strFlag = 217
				If flag2 Then
					len = ms.ReadByte()
				Else
					Dim flag3 As Boolean = strFlag = 218
					If flag3 Then
						rawBytes = New Byte(1) {}
						ms.Read(rawBytes, 0, 2)
						rawBytes = BytesTools.SwapBytes(rawBytes)
						len = CInt(BitConverter.ToUInt16(rawBytes, 0))
					Else
						Dim flag4 As Boolean = strFlag = 219
						If flag4 Then
							rawBytes = New Byte(3) {}
							ms.Read(rawBytes, 0, 4)
							rawBytes = BytesTools.SwapBytes(rawBytes)
							len = BitConverter.ToInt32(rawBytes, 0)
						End If
					End If
				End If
			End If
			rawBytes = New Byte(len - 1) {}
			ms.Read(rawBytes, 0, len)
			Return BytesTools.GetString(rawBytes)
		End Function
	End Class
End Namespace
