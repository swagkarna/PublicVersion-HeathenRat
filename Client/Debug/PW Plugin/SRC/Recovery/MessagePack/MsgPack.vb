Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO

Namespace Plugin.MessagePack
	' Token: 0x02000009 RID: 9
	Public Class MsgPack
		Implements IEnumerable

		' Token: 0x0600002E RID: 46 RVA: 0x00002A4D File Offset: 0x00000C4D
		Private Sub SetName(value As String)
			Me.name = value
			Me.lowerName = Me.name.ToLower()
		End Sub

		' Token: 0x0600002F RID: 47 RVA: 0x00002A68 File Offset: 0x00000C68
		Private Sub Clear()
			For i As Integer = 0 To Me.children.Count - 1
				Me.children(i).Clear()
			Next
			Me.children.Clear()
		End Sub

		' Token: 0x06000030 RID: 48 RVA: 0x00002AB0 File Offset: 0x00000CB0
		Private Function InnerAdd() As MsgPack
			Dim r As MsgPack = New MsgPack()
			r.parent = Me
			Me.children.Add(r)
			Return r
		End Function

		' Token: 0x06000031 RID: 49 RVA: 0x00002AE0 File Offset: 0x00000CE0
		Private Function IndexOf(name As String) As Integer
			Dim i As Integer = -1
			Dim r As Integer = -1
			Dim tmp As String = name.ToLower()
			For Each item As MsgPack In Me.children
				i += 1
				Dim flag As Boolean = tmp.Equals(item.lowerName)
				If flag Then
					r = i
					Exit For
				End If
			Next
			Return r
		End Function

		' Token: 0x06000032 RID: 50 RVA: 0x00002B60 File Offset: 0x00000D60
		Public Function FindObject(name As String) As MsgPack
			Dim i As Integer = Me.IndexOf(name)
			Dim flag As Boolean = i = -1
			Dim result As MsgPack
			If flag Then
				result = Nothing
			Else
				result = Me.children(i)
			End If
			Return result
		End Function

		' Token: 0x06000033 RID: 51 RVA: 0x00002B94 File Offset: 0x00000D94
		Private Function InnerAddMapChild() As MsgPack
			Dim flag As Boolean = Me.valueType <> MsgPackType.Map
			If flag Then
				Me.Clear()
				Me.valueType = MsgPackType.Map
			End If
			Return Me.InnerAdd()
		End Function

		' Token: 0x06000034 RID: 52 RVA: 0x00002BCC File Offset: 0x00000DCC
		Private Function InnerAddArrayChild() As MsgPack
			Dim flag As Boolean = Me.valueType <> MsgPackType.Array
			If flag Then
				Me.Clear()
				Me.valueType = MsgPackType.Array
			End If
			Return Me.InnerAdd()
		End Function

		' Token: 0x06000035 RID: 53 RVA: 0x00002C04 File Offset: 0x00000E04
		Public Function AddArrayChild() As MsgPack
			Return Me.InnerAddArrayChild()
		End Function

		' Token: 0x06000036 RID: 54 RVA: 0x00002C1C File Offset: 0x00000E1C
		Private Sub WriteMap(ms As Stream)
			Dim len As Integer = Me.children.Count
			Dim flag As Boolean = len <= 15
			If flag Then
				Dim b As Byte = 128 + CByte(len)
				ms.WriteByte(b)
			Else
				Dim flag2 As Boolean = len <= 65535
				If flag2 Then
					Dim b As Byte = 222
					ms.WriteByte(b)
					Dim lenBytes As Byte() = BytesTools.SwapBytes(BitConverter.GetBytes(CShort(len)))
					ms.Write(lenBytes, 0, lenBytes.Length)
				Else
					Dim b As Byte = 223
					ms.WriteByte(b)
					Dim lenBytes As Byte() = BytesTools.SwapBytes(BitConverter.GetBytes(len))
					ms.Write(lenBytes, 0, lenBytes.Length)
				End If
			End If
			For i As Integer = 0 To len - 1
				WriteTools.WriteString(ms, Me.children(i).name)
				Me.children(i).Encode2Stream(ms)
			Next
		End Sub

		' Token: 0x06000037 RID: 55 RVA: 0x00002D04 File Offset: 0x00000F04
		Private Sub WirteArray(ms As Stream)
			Dim len As Integer = Me.children.Count
			Dim flag As Boolean = len <= 15
			If flag Then
				Dim b As Byte = 144 + CByte(len)
				ms.WriteByte(b)
			Else
				Dim flag2 As Boolean = len <= 65535
				If flag2 Then
					Dim b As Byte = 220
					ms.WriteByte(b)
					Dim lenBytes As Byte() = BytesTools.SwapBytes(BitConverter.GetBytes(CShort(len)))
					ms.Write(lenBytes, 0, lenBytes.Length)
				Else
					Dim b As Byte = 221
					ms.WriteByte(b)
					Dim lenBytes As Byte() = BytesTools.SwapBytes(BitConverter.GetBytes(len))
					ms.Write(lenBytes, 0, lenBytes.Length)
				End If
			End If
			For i As Integer = 0 To len - 1
				Me.children(i).Encode2Stream(ms)
			Next
		End Sub

		' Token: 0x06000038 RID: 56 RVA: 0x00002DD0 File Offset: 0x00000FD0
		Public Sub SetAsInteger(value As Long)
			Me.innerValue = value
			Me.valueType = MsgPackType.[Integer]
		End Sub

		' Token: 0x06000039 RID: 57 RVA: 0x00002DE6 File Offset: 0x00000FE6
		Public Sub SetAsUInt64(value As ULong)
			Me.innerValue = value
			Me.valueType = MsgPackType.UInt64
		End Sub

		' Token: 0x0600003A RID: 58 RVA: 0x00002DFC File Offset: 0x00000FFC
		Public Function GetAsUInt64() As ULong
			Select Case Me.valueType
				Case MsgPackType.[String]
					Return ULong.Parse(Me.innerValue.ToString().Trim())
				Case MsgPackType.[Integer]
					Return Convert.ToUInt64(CLng(Me.innerValue))
				Case MsgPackType.UInt64
					Return CULng(Me.innerValue)
				Case MsgPackType.Float
					Return Convert.ToUInt64(CDbl(Me.innerValue))
				Case MsgPackType.[Single]
					Return Convert.ToUInt64(CSng(Me.innerValue))
				Case MsgPackType.DateTime
					Return Convert.ToUInt64(CType(Me.innerValue, DateTime))
			End Select
			Return 0UL
		End Function

		' Token: 0x0600003B RID: 59 RVA: 0x00002EB4 File Offset: 0x000010B4
		Public Function GetAsInteger() As Long
			Select Case Me.valueType
				Case MsgPackType.[String]
					Return Long.Parse(Me.innerValue.ToString().Trim())
				Case MsgPackType.[Integer]
					Return CLng(Me.innerValue)
				Case MsgPackType.UInt64
					Return Convert.ToInt64(CLng(Me.innerValue))
				Case MsgPackType.Float
					Return Convert.ToInt64(CDbl(Me.innerValue))
				Case MsgPackType.[Single]
					Return Convert.ToInt64(CSng(Me.innerValue))
				Case MsgPackType.DateTime
					Return Convert.ToInt64(CType(Me.innerValue, DateTime))
			End Select
			Return 0L
		End Function

		' Token: 0x0600003C RID: 60 RVA: 0x00002F6C File Offset: 0x0000116C
		Public Function GetAsFloat() As Double
			Select Case Me.valueType
				Case MsgPackType.[String]
					Return Double.Parse(CStr(Me.innerValue))
				Case MsgPackType.[Integer]
					Return Convert.ToDouble(CLng(Me.innerValue))
				Case MsgPackType.Float
					Return CDbl(Me.innerValue)
				Case MsgPackType.[Single]
					Return CDbl(CSng(Me.innerValue))
				Case MsgPackType.DateTime
					Return CDbl(Convert.ToInt64(CType(Me.innerValue, DateTime)))
			End Select
			Return 0.0
		End Function

		' Token: 0x0600003D RID: 61 RVA: 0x0000300D File Offset: 0x0000120D
		Public Sub SetAsBytes(value As Byte())
			Me.innerValue = value
			Me.valueType = MsgPackType.Binary
		End Sub

		' Token: 0x0600003E RID: 62 RVA: 0x00003020 File Offset: 0x00001220
		Public Function GetAsBytes() As Byte()
			Select Case Me.valueType
				Case MsgPackType.[String]
					Return BytesTools.GetUtf8Bytes(Me.innerValue.ToString())
				Case MsgPackType.[Integer]
					Return BitConverter.GetBytes(CLng(Me.innerValue))
				Case MsgPackType.Float
					Return BitConverter.GetBytes(CDbl(Me.innerValue))
				Case MsgPackType.[Single]
					Return BitConverter.GetBytes(CSng(Me.innerValue))
				Case MsgPackType.DateTime
					Dim dateval As Long = CType(Me.innerValue, DateTime).ToBinary()
					Return BitConverter.GetBytes(dateval)
				Case MsgPackType.Binary
					Return CType(Me.innerValue, Byte())
			End Select
			Return New Byte(-1) {}
		End Function

		' Token: 0x0600003F RID: 63 RVA: 0x000030E4 File Offset: 0x000012E4
		Public Sub Add(key As String, value As String)
			Dim tmp As MsgPack = Me.InnerAddArrayChild()
			tmp.name = key
			tmp.SetAsString(value)
		End Sub

		' Token: 0x06000040 RID: 64 RVA: 0x00003108 File Offset: 0x00001308
		Public Sub Add(key As String, value As Integer)
			Dim tmp As MsgPack = Me.InnerAddArrayChild()
			tmp.name = key
			tmp.SetAsInteger(CLng(value))
		End Sub

		' Token: 0x06000041 RID: 65 RVA: 0x00003130 File Offset: 0x00001330
		Public Function LoadFileAsBytes(fileName As String) As Boolean
			Dim flag As Boolean = File.Exists(fileName)
			Dim result As Boolean
			If flag Then
				Dim fs As FileStream = New FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)
				Dim value As Byte() = New Byte(fs.Length - 1) {}
				fs.Read(value, 0, CInt(fs.Length))
				fs.Close()
				fs.Dispose()
				Me.SetAsBytes(value)
				result = True
			Else
				result = False
			End If
			Return result
		End Function

		' Token: 0x06000042 RID: 66 RVA: 0x00003194 File Offset: 0x00001394
		Public Function SaveBytesToFile(fileName As String) As Boolean
			Dim flag As Boolean = Me.innerValue IsNot Nothing
			Dim result As Boolean
			If flag Then
				Dim fs As FileStream = New FileStream(fileName, FileMode.Append)
				fs.Write(CType(Me.innerValue, Byte()), 0, CType(Me.innerValue, Byte()).Length)
				fs.Close()
				fs.Dispose()
				result = True
			Else
				result = False
			End If
			Return result
		End Function

		' Token: 0x06000043 RID: 67 RVA: 0x000031F0 File Offset: 0x000013F0
		Public Function ForcePathObject(path As String) As MsgPack
			Dim tmpParent As MsgPack = Me
			Dim pathList As String() = path.Trim().Split(New Char() { "."c, "/"c, "\"c })
			Dim flag As Boolean = pathList.Length = 0
			Dim result As MsgPack
			If flag Then
				result = Nothing
			Else
				Dim flag2 As Boolean = pathList.Length > 1
				Dim tmp As String
				If flag2 Then
					For i As Integer = 0 To pathList.Length - 1 - 1
						tmp = pathList(i)
						Dim tmpObject As MsgPack = tmpParent.FindObject(tmp)
						Dim flag3 As Boolean = tmpObject Is Nothing
						If flag3 Then
							tmpParent = tmpParent.InnerAddMapChild()
							tmpParent.SetName(tmp)
						Else
							tmpParent = tmpObject
						End If
					Next
				End If
				tmp = pathList(pathList.Length - 1)
				Dim j As Integer = tmpParent.IndexOf(tmp)
				Dim flag4 As Boolean = j > -1
				If flag4 Then
					result = tmpParent.children(j)
				Else
					tmpParent = tmpParent.InnerAddMapChild()
					tmpParent.SetName(tmp)
					result = tmpParent
				End If
			End If
			Return result
		End Function

		' Token: 0x06000044 RID: 68 RVA: 0x000032D0 File Offset: 0x000014D0
		Public Sub SetAsNull()
			Me.Clear()
			Me.innerValue = Nothing
			Me.valueType = MsgPackType.Null
		End Sub

		' Token: 0x06000045 RID: 69 RVA: 0x000032E8 File Offset: 0x000014E8
		Public Sub SetAsString(value As String)
			Me.innerValue = value
			Me.valueType = MsgPackType.[String]
		End Sub

		' Token: 0x06000046 RID: 70 RVA: 0x000032FC File Offset: 0x000014FC
		Public Function GetAsString() As String
			Dim flag As Boolean = Me.innerValue Is Nothing
			Dim result As String
			If flag Then
				result = ""
			Else
				result = Me.innerValue.ToString()
			End If
			Return result
		End Function

		' Token: 0x06000047 RID: 71 RVA: 0x00003330 File Offset: 0x00001530
		Public Sub SetAsBoolean(bVal As Boolean)
			Me.valueType = MsgPackType.[Boolean]
			Me.innerValue = bVal
		End Sub

		' Token: 0x06000048 RID: 72 RVA: 0x00003346 File Offset: 0x00001546
		Public Sub SetAsSingle(fVal As Single)
			Me.valueType = MsgPackType.[Single]
			Me.innerValue = fVal
		End Sub

		' Token: 0x06000049 RID: 73 RVA: 0x0000335D File Offset: 0x0000155D
		Public Sub SetAsFloat(fVal As Double)
			Me.valueType = MsgPackType.Float
			Me.innerValue = fVal
		End Sub

		' Token: 0x0600004A RID: 74 RVA: 0x00003374 File Offset: 0x00001574
		Public Sub DecodeFromBytes(bytes As Byte())
			Using ms As MemoryStream = New MemoryStream()
				bytes = Zip.Decompress(bytes)
				ms.Write(bytes, 0, bytes.Length)
				ms.Position = 0L
				Me.DecodeFromStream(ms)
			End Using
		End Sub

		' Token: 0x0600004B RID: 75 RVA: 0x000033CC File Offset: 0x000015CC
		Public Sub DecodeFromFile(fileName As String)
			Dim fs As FileStream = New FileStream(fileName, FileMode.Open)
			Me.DecodeFromStream(fs)
			fs.Dispose()
		End Sub

		' Token: 0x0600004C RID: 76 RVA: 0x000033F4 File Offset: 0x000015F4
		Public Sub DecodeFromStream(ms As Stream)
			Dim lvByte As Byte = CByte(ms.ReadByte())
			Dim flag As Boolean = lvByte <= 127
			If flag Then
				Me.SetAsInteger(CLng(CULng(lvByte)))
			Else
				Dim flag2 As Boolean = lvByte >= 128 AndAlso lvByte <= 143
				If flag2 Then
					Me.Clear()
					Me.valueType = MsgPackType.Map
					Dim len As Integer = CInt((lvByte - 128))
					For i As Integer = 0 To len - 1
						Dim msgPack As MsgPack = Me.InnerAdd()
						msgPack.SetName(ReadTools.ReadString(ms))
						msgPack.DecodeFromStream(ms)
					Next
				Else
					Dim flag3 As Boolean = lvByte >= 144 AndAlso lvByte <= 159
					If flag3 Then
						Me.Clear()
						Me.valueType = MsgPackType.Array
						Dim len As Integer = CInt((lvByte - 144))
						For i As Integer = 0 To len - 1
							Dim msgPack As MsgPack = Me.InnerAdd()
							msgPack.DecodeFromStream(ms)
						Next
					Else
						Dim flag4 As Boolean = lvByte >= 160 AndAlso lvByte <= 191
						If flag4 Then
							Dim len As Integer = CInt((lvByte - 160))
							Me.SetAsString(ReadTools.ReadString(ms, len))
						Else
							Dim flag5 As Boolean = lvByte >= 224 AndAlso lvByte <= Byte.MaxValue
							If flag5 Then
								Me.SetAsInteger(CLng(CSByte(lvByte)))
							Else
								Dim flag6 As Boolean = lvByte = 192
								If flag6 Then
									Me.SetAsNull()
								Else
									Dim flag7 As Boolean = lvByte = 193
									If flag7 Then
										Throw New Exception("(never used) type $c1")
									End If
									Dim flag8 As Boolean = lvByte = 194
									If flag8 Then
										Me.SetAsBoolean(False)
									Else
										Dim flag9 As Boolean = lvByte = 195
										If flag9 Then
											Me.SetAsBoolean(True)
										Else
											Dim flag10 As Boolean = lvByte = 196
											If flag10 Then
												Dim len As Integer = ms.ReadByte()
												Dim rawByte As Byte() = New Byte(len - 1) {}
												ms.Read(rawByte, 0, len)
												Me.SetAsBytes(rawByte)
											Else
												Dim flag11 As Boolean = lvByte = 197
												If flag11 Then
													Dim rawByte As Byte() = New Byte(1) {}
													ms.Read(rawByte, 0, 2)
													rawByte = BytesTools.SwapBytes(rawByte)
													Dim len As Integer = CInt(BitConverter.ToUInt16(rawByte, 0))
													rawByte = New Byte(len - 1) {}
													ms.Read(rawByte, 0, len)
													Me.SetAsBytes(rawByte)
												Else
													Dim flag12 As Boolean = lvByte = 198
													If flag12 Then
														Dim rawByte As Byte() = New Byte(3) {}
														ms.Read(rawByte, 0, 4)
														rawByte = BytesTools.SwapBytes(rawByte)
														Dim len As Integer = BitConverter.ToInt32(rawByte, 0)
														rawByte = New Byte(len - 1) {}
														ms.Read(rawByte, 0, len)
														Me.SetAsBytes(rawByte)
													Else
														Dim flag13 As Boolean = lvByte = 199 OrElse lvByte = 200 OrElse lvByte = 201
														If flag13 Then
															Throw New Exception("(ext8,ext16,ex32) type $c7,$c8,$c9")
														End If
														Dim flag14 As Boolean = lvByte = 202
														If flag14 Then
															Dim rawByte As Byte() = New Byte(3) {}
															ms.Read(rawByte, 0, 4)
															rawByte = BytesTools.SwapBytes(rawByte)
															Me.SetAsSingle(BitConverter.ToSingle(rawByte, 0))
														Else
															Dim flag15 As Boolean = lvByte = 203
															If flag15 Then
																Dim rawByte As Byte() = New Byte(7) {}
																ms.Read(rawByte, 0, 8)
																rawByte = BytesTools.SwapBytes(rawByte)
																Me.SetAsFloat(BitConverter.ToDouble(rawByte, 0))
															Else
																Dim flag16 As Boolean = lvByte = 204
																If flag16 Then
																	lvByte = CByte(ms.ReadByte())
																	Me.SetAsInteger(CLng(CULng(lvByte)))
																Else
																	Dim flag17 As Boolean = lvByte = 205
																	If flag17 Then
																		Dim rawByte As Byte() = New Byte(1) {}
																		ms.Read(rawByte, 0, 2)
																		rawByte = BytesTools.SwapBytes(rawByte)
																		Me.SetAsInteger(CLng(CULng(BitConverter.ToUInt16(rawByte, 0))))
																	Else
																		Dim flag18 As Boolean = lvByte = 206
																		If flag18 Then
																			Dim rawByte As Byte() = New Byte(3) {}
																			ms.Read(rawByte, 0, 4)
																			rawByte = BytesTools.SwapBytes(rawByte)
																			Me.SetAsInteger(CLng(CULng(BitConverter.ToUInt32(rawByte, 0))))
																		Else
																			Dim flag19 As Boolean = lvByte = 207
																			If flag19 Then
																				Dim rawByte As Byte() = New Byte(7) {}
																				ms.Read(rawByte, 0, 8)
																				rawByte = BytesTools.SwapBytes(rawByte)
																				Me.SetAsUInt64(BitConverter.ToUInt64(rawByte, 0))
																			Else
																				Dim flag20 As Boolean = lvByte = 220
																				If flag20 Then
																					Dim rawByte As Byte() = New Byte(1) {}
																					ms.Read(rawByte, 0, 2)
																					rawByte = BytesTools.SwapBytes(rawByte)
																					Dim len As Integer = CInt(BitConverter.ToInt16(rawByte, 0))
																					Me.Clear()
																					Me.valueType = MsgPackType.Array
																					For i As Integer = 0 To len - 1
																						Dim msgPack As MsgPack = Me.InnerAdd()
																						msgPack.DecodeFromStream(ms)
																					Next
																				Else
																					Dim flag21 As Boolean = lvByte = 221
																					If flag21 Then
																						Dim rawByte As Byte() = New Byte(3) {}
																						ms.Read(rawByte, 0, 4)
																						rawByte = BytesTools.SwapBytes(rawByte)
																						Dim len As Integer = CInt(BitConverter.ToInt16(rawByte, 0))
																						Me.Clear()
																						Me.valueType = MsgPackType.Array
																						For i As Integer = 0 To len - 1
																							Dim msgPack As MsgPack = Me.InnerAdd()
																							msgPack.DecodeFromStream(ms)
																						Next
																					Else
																						Dim flag22 As Boolean = lvByte = 217
																						If flag22 Then
																							Me.SetAsString(ReadTools.ReadString(lvByte, ms))
																						Else
																							Dim flag23 As Boolean = lvByte = 222
																							If flag23 Then
																								Dim rawByte As Byte() = New Byte(1) {}
																								ms.Read(rawByte, 0, 2)
																								rawByte = BytesTools.SwapBytes(rawByte)
																								Dim len As Integer = CInt(BitConverter.ToInt16(rawByte, 0))
																								Me.Clear()
																								Me.valueType = MsgPackType.Map
																								For i As Integer = 0 To len - 1
																									Dim msgPack As MsgPack = Me.InnerAdd()
																									msgPack.SetName(ReadTools.ReadString(ms))
																									msgPack.DecodeFromStream(ms)
																								Next
																							Else
																								Dim flag24 As Boolean = lvByte = 222
																								If flag24 Then
																									Dim rawByte As Byte() = New Byte(1) {}
																									ms.Read(rawByte, 0, 2)
																									rawByte = BytesTools.SwapBytes(rawByte)
																									Dim len As Integer = CInt(BitConverter.ToInt16(rawByte, 0))
																									Me.Clear()
																									Me.valueType = MsgPackType.Map
																									For i As Integer = 0 To len - 1
																										Dim msgPack As MsgPack = Me.InnerAdd()
																										msgPack.SetName(ReadTools.ReadString(ms))
																										msgPack.DecodeFromStream(ms)
																									Next
																								Else
																									Dim flag25 As Boolean = lvByte = 223
																									If flag25 Then
																										Dim rawByte As Byte() = New Byte(3) {}
																										ms.Read(rawByte, 0, 4)
																										rawByte = BytesTools.SwapBytes(rawByte)
																										Dim len As Integer = BitConverter.ToInt32(rawByte, 0)
																										Me.Clear()
																										Me.valueType = MsgPackType.Map
																										For i As Integer = 0 To len - 1
																											Dim msgPack As MsgPack = Me.InnerAdd()
																											msgPack.SetName(ReadTools.ReadString(ms))
																											msgPack.DecodeFromStream(ms)
																										Next
																									Else
																										Dim flag26 As Boolean = lvByte = 218
																										If flag26 Then
																											Me.SetAsString(ReadTools.ReadString(lvByte, ms))
																										Else
																											Dim flag27 As Boolean = lvByte = 219
																											If flag27 Then
																												Me.SetAsString(ReadTools.ReadString(lvByte, ms))
																											Else
																												Dim flag28 As Boolean = lvByte = 208
																												If flag28 Then
																													Me.SetAsInteger(CLng(CSByte(ms.ReadByte())))
																												Else
																													Dim flag29 As Boolean = lvByte = 209
																													If flag29 Then
																														Dim rawByte As Byte() = New Byte(1) {}
																														ms.Read(rawByte, 0, 2)
																														rawByte = BytesTools.SwapBytes(rawByte)
																														Me.SetAsInteger(CLng(BitConverter.ToInt16(rawByte, 0)))
																													Else
																														Dim flag30 As Boolean = lvByte = 210
																														If flag30 Then
																															Dim rawByte As Byte() = New Byte(3) {}
																															ms.Read(rawByte, 0, 4)
																															rawByte = BytesTools.SwapBytes(rawByte)
																															Me.SetAsInteger(CLng(BitConverter.ToInt32(rawByte, 0)))
																														Else
																															Dim flag31 As Boolean = lvByte = 211
																															If flag31 Then
																																Dim rawByte As Byte() = New Byte(7) {}
																																ms.Read(rawByte, 0, 8)
																																rawByte = BytesTools.SwapBytes(rawByte)
																																Me.SetAsInteger(BitConverter.ToInt64(rawByte, 0))
																															End If
																														End If
																													End If
																												End If
																											End If
																										End If
																									End If
																								End If
																							End If
																						End If
																					End If
																				End If
																			End If
																		End If
																	End If
																End If
															End If
														End If
													End If
												End If
											End If
										End If
									End If
								End If
							End If
						End If
					End If
				End If
			End If
		End Sub

		' Token: 0x0600004D RID: 77 RVA: 0x00003B90 File Offset: 0x00001D90
		Public Function Encode2Bytes() As Byte()
			Dim result As Byte()
			Using ms As MemoryStream = New MemoryStream()
				Me.Encode2Stream(ms)
				Dim r As Byte() = New Byte(ms.Length - 1) {}
				ms.Position = 0L
				ms.Read(r, 0, CInt(ms.Length))
				result = Zip.Compress(r)
			End Using
			Return result
		End Function

		' Token: 0x0600004E RID: 78 RVA: 0x00003BF8 File Offset: 0x00001DF8
		Public Sub Encode2Stream(ms As Stream)
			Select Case Me.valueType
				Case MsgPackType.Unknown, MsgPackType.Null
					WriteTools.WriteNull(ms)
				Case MsgPackType.Map
					Me.WriteMap(ms)
				Case MsgPackType.Array
					Me.WirteArray(ms)
				Case MsgPackType.[String]
					WriteTools.WriteString(ms, CStr(Me.innerValue))
				Case MsgPackType.[Integer]
					WriteTools.WriteInteger(ms, CLng(Me.innerValue))
				Case MsgPackType.UInt64
					WriteTools.WriteUInt64(ms, CULng(Me.innerValue))
				Case MsgPackType.[Boolean]
					WriteTools.WriteBoolean(ms, CBool(Me.innerValue))
				Case MsgPackType.Float
					WriteTools.WriteFloat(ms, CDbl(Me.innerValue))
				Case MsgPackType.[Single]
					WriteTools.WriteFloat(ms, CDbl(CSng(Me.innerValue)))
				Case MsgPackType.DateTime
					WriteTools.WriteInteger(ms, Me.GetAsInteger())
				Case MsgPackType.Binary
					WriteTools.WriteBinary(ms, CType(Me.innerValue, Byte()))
				Case Else
					WriteTools.WriteNull(ms)
			End Select
		End Sub

		' Token: 0x1700000A RID: 10
		' (get) Token: 0x0600004F RID: 79 RVA: 0x00003D18 File Offset: 0x00001F18
		' (set) Token: 0x06000050 RID: 80 RVA: 0x00003D30 File Offset: 0x00001F30
		Public Property AsString As String
			Get
				Return Me.GetAsString()
			End Get
			Set(value As String)
				Me.SetAsString(value)
			End Set
		End Property

		' Token: 0x1700000B RID: 11
		' (get) Token: 0x06000051 RID: 81 RVA: 0x00003D3C File Offset: 0x00001F3C
		' (set) Token: 0x06000052 RID: 82 RVA: 0x00003D54 File Offset: 0x00001F54
		Public Property AsInteger As Long
			Get
				Return Me.GetAsInteger()
			End Get
			Set(value As Long)
				Me.SetAsInteger(value)
			End Set
		End Property

		' Token: 0x1700000C RID: 12
		' (get) Token: 0x06000053 RID: 83 RVA: 0x00003D60 File Offset: 0x00001F60
		' (set) Token: 0x06000054 RID: 84 RVA: 0x00003D78 File Offset: 0x00001F78
		Public Property AsFloat As Double
			Get
				Return Me.GetAsFloat()
			End Get
			Set(value As Double)
				Me.SetAsFloat(value)
			End Set
		End Property

		' Token: 0x1700000D RID: 13
		' (get) Token: 0x06000055 RID: 85 RVA: 0x00003D84 File Offset: 0x00001F84
		Public ReadOnly Property AsArray As MsgPackArray
			Get
				SyncLock Me
					Dim flag2 As Boolean = Me.refAsArray Is Nothing
					If flag2 Then
						Me.refAsArray = New MsgPackArray(Me, Me.children)
					End If
				End SyncLock
				Return Me.refAsArray
			End Get
		End Property

		' Token: 0x1700000E RID: 14
		' (get) Token: 0x06000056 RID: 86 RVA: 0x00003DEC File Offset: 0x00001FEC
		Public ReadOnly Property ValueType As MsgPackType
			Get
				Return Me.valueType
			End Get
		End Property

		' Token: 0x06000057 RID: 87 RVA: 0x00003E04 File Offset: 0x00002004
		Function GetEnumerator() As IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
			Return New MsgPackEnum(Me.children)
		End Function

		' Token: 0x04000012 RID: 18
		Private name As String

		' Token: 0x04000013 RID: 19
		Private lowerName As String

		' Token: 0x04000014 RID: 20
		Private innerValue As Object

		' Token: 0x04000015 RID: 21
		Private valueType As MsgPackType

		' Token: 0x04000016 RID: 22
		Private parent As MsgPack

		' Token: 0x04000017 RID: 23
		Private children As List(Of MsgPack) = New List(Of MsgPack)()

		' Token: 0x04000018 RID: 24
		Private refAsArray As MsgPackArray = Nothing
	End Class
End Namespace
