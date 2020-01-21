Imports System
Imports System.IO

Namespace Plugin.MessagePack
	' Token: 0x0200000C RID: 12
	Friend Class WriteTools
		' Token: 0x0600005D RID: 93 RVA: 0x00003F63 File Offset: 0x00002163
		Public Shared Sub WriteNull(ms As Stream)
			ms.WriteByte(192)
		End Sub

		' Token: 0x0600005E RID: 94 RVA: 0x00003F74 File Offset: 0x00002174
		Public Shared Sub WriteString(ms As Stream, strVal As String)
			Dim rawBytes As Byte() = BytesTools.GetUtf8Bytes(strVal)
			Dim len As Integer = rawBytes.Length
			Dim flag As Boolean = len <= 31
			If flag Then
				Dim b As Byte = 160 + CByte(len)
				ms.WriteByte(b)
			Else
				Dim flag2 As Boolean = len <= 255
				If flag2 Then
					Dim b As Byte = 217
					ms.WriteByte(b)
					b = CByte(len)
					ms.WriteByte(b)
				Else
					Dim flag3 As Boolean = len <= 65535
					If flag3 Then
						Dim b As Byte = 218
						ms.WriteByte(b)
						Dim lenBytes As Byte() = BytesTools.SwapBytes(BitConverter.GetBytes(CShort(len)))
						ms.Write(lenBytes, 0, lenBytes.Length)
					Else
						Dim b As Byte = 219
						ms.WriteByte(b)
						Dim lenBytes As Byte() = BytesTools.SwapBytes(BitConverter.GetBytes(len))
						ms.Write(lenBytes, 0, lenBytes.Length)
					End If
				End If
			End If
			ms.Write(rawBytes, 0, rawBytes.Length)
		End Sub

		' Token: 0x0600005F RID: 95 RVA: 0x00004058 File Offset: 0x00002258
		Public Shared Sub WriteBinary(ms As Stream, rawBytes As Byte())
			Dim len As Integer = rawBytes.Length
			Dim flag As Boolean = len <= 255
			If flag Then
				Dim b As Byte = 196
				ms.WriteByte(b)
				b = CByte(len)
				ms.WriteByte(b)
			Else
				Dim flag2 As Boolean = len <= 65535
				If flag2 Then
					Dim b As Byte = 197
					ms.WriteByte(b)
					Dim lenBytes As Byte() = BytesTools.SwapBytes(BitConverter.GetBytes(CShort(len)))
					ms.Write(lenBytes, 0, lenBytes.Length)
				Else
					Dim b As Byte = 198
					ms.WriteByte(b)
					Dim lenBytes As Byte() = BytesTools.SwapBytes(BitConverter.GetBytes(len))
					ms.Write(lenBytes, 0, lenBytes.Length)
				End If
			End If
			ms.Write(rawBytes, 0, rawBytes.Length)
		End Sub

		' Token: 0x06000060 RID: 96 RVA: 0x0000410A File Offset: 0x0000230A
		Public Shared Sub WriteFloat(ms As Stream, fVal As Double)
			ms.WriteByte(203)
			ms.Write(BytesTools.SwapDouble(fVal), 0, 8)
		End Sub

		' Token: 0x06000061 RID: 97 RVA: 0x00004128 File Offset: 0x00002328
		Public Shared Sub WriteSingle(ms As Stream, fVal As Single)
			ms.WriteByte(202)
			ms.Write(BytesTools.SwapBytes(BitConverter.GetBytes(fVal)), 0, 4)
		End Sub

		' Token: 0x06000062 RID: 98 RVA: 0x0000414C File Offset: 0x0000234C
		Public Shared Sub WriteBoolean(ms As Stream, bVal As Boolean)
			If bVal Then
				ms.WriteByte(195)
			Else
				ms.WriteByte(194)
			End If
		End Sub

		' Token: 0x06000063 RID: 99 RVA: 0x00004180 File Offset: 0x00002380
		Public Shared Sub WriteUInt64(ms As Stream, iVal As ULong)
			ms.WriteByte(207)
			Dim dataBytes As Byte() = BitConverter.GetBytes(iVal)
			ms.Write(BytesTools.SwapBytes(dataBytes), 0, 8)
		End Sub

		' Token: 0x06000064 RID: 100 RVA: 0x000041B0 File Offset: 0x000023B0
		Public Shared Sub WriteInteger(ms As Stream, iVal As Long)
			Dim flag As Boolean = iVal >= 0L
			If flag Then
				Dim flag2 As Boolean = iVal <= 127L
				If flag2 Then
					ms.WriteByte(CByte(iVal))
				Else
					Dim flag3 As Boolean = iVal <= 255L
					If flag3 Then
						ms.WriteByte(204)
						ms.WriteByte(CByte(iVal))
					Else
						Dim flag4 As Boolean = iVal <= 65535L
						If flag4 Then
							ms.WriteByte(205)
							ms.Write(BytesTools.SwapInt16(CShort(iVal)), 0, 2)
						Else
							Dim flag5 As Boolean = iVal <= CLng(CULng(-1))
							If flag5 Then
								ms.WriteByte(206)
								ms.Write(BytesTools.SwapInt32(CInt(iVal)), 0, 4)
							Else
								ms.WriteByte(211)
								ms.Write(BytesTools.SwapInt64(iVal), 0, 8)
							End If
						End If
					End If
				End If
			Else
				Dim flag6 As Boolean = iVal <= -2147483648L
				If flag6 Then
					ms.WriteByte(211)
					ms.Write(BytesTools.SwapInt64(iVal), 0, 8)
				Else
					Dim flag7 As Boolean = iVal <= -32768L
					If flag7 Then
						ms.WriteByte(210)
						ms.Write(BytesTools.SwapInt32(CInt(iVal)), 0, 4)
					Else
						Dim flag8 As Boolean = iVal <= -128L
						If flag8 Then
							ms.WriteByte(209)
							ms.Write(BytesTools.SwapInt16(CShort(iVal)), 0, 2)
						Else
							Dim flag9 As Boolean = iVal <= -32L
							If flag9 Then
								ms.WriteByte(208)
								ms.WriteByte(CByte(iVal))
							Else
								ms.WriteByte(CByte(iVal))
							End If
						End If
					End If
				End If
			End If
		End Sub
	End Class
End Namespace
