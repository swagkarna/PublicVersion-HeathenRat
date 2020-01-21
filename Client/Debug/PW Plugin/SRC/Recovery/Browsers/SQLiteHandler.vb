Imports System
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices

Namespace Plugin.Browsers
	' Token: 0x0200000F RID: 15
	Public Class SQLiteHandler
		' Token: 0x0600006F RID: 111 RVA: 0x000043A4 File Offset: 0x000025A4
		Public Sub New(baseName As String)
			Dim flag As Boolean = File.Exists(baseName)
			If flag Then
				FileSystem.FileOpen(1, baseName, OpenMode.Binary, OpenAccess.Read, OpenShare.[Shared], -1)
				Dim s As String = Strings.Space(CInt(FileSystem.LOF(1)))
				FileSystem.FileGet(1, s, -1L, False)
				FileSystem.FileClose(New Integer() { 1 })
				Me.db_bytes = Encoding.[Default].GetBytes(s)
				Dim flag2 As Boolean = Encoding.[Default].GetString(Me.db_bytes, 0, 15).CompareTo("SQLite format 3") <> 0
				If flag2 Then
					Throw New Exception("Not a valid SQLite 3 Database File")
				End If
				Dim flag3 As Boolean = Me.db_bytes(52) > 0
				If flag3 Then
					Throw New Exception("Auto-vacuum capable database is not supported")
				End If
				Me.page_size = CUShort(Me.ConvertToInteger(16, 2))
				Me.encoding = Me.ConvertToInteger(56, 4)
				Dim flag4 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 0D) = 0
				If flag4 Then
					Me.encoding = 1UL
				End If
				Me.ReadMasterTable(100UL)
			End If
		End Sub

		' Token: 0x06000070 RID: 112 RVA: 0x000044C4 File Offset: 0x000026C4
		Private Function ConvertToInteger(startIndex As Integer, Size As Integer) As ULong
			Dim flag As Boolean = Size > 8 Or Size = 0
			Dim result As ULong
			If flag Then
				result = 0UL
			Else
				Dim num As ULong = 0UL
				Dim num2 As Integer = Size - 1
				For i As Integer = 0 To num2
					num = (num << 8 Or CULng(Me.db_bytes(startIndex + i)))
				Next
				result = num
			End If
			Return result
		End Function

		' Token: 0x06000071 RID: 113 RVA: 0x00004520 File Offset: 0x00002720
		Private Function CVL(startIndex As Integer, endIndex As Integer) As Long
			endIndex += 1
			Dim array As Byte() = New Byte(7) {}
			Dim num As Integer = endIndex - startIndex
			Dim flag As Boolean = False
			Dim flag2 As Boolean = num = 0 Or num > 9
			Dim result As Long
			If flag2 Then
				result = 0L
			Else
				Dim flag3 As Boolean = num = 1
				If flag3 Then
					array(0) = (Me.db_bytes(startIndex) And 127)
					result = BitConverter.ToInt64(array, 0)
				Else
					Dim flag4 As Boolean = num = 9
					If flag4 Then
						flag = True
					End If
					Dim num2 As Integer = 1
					Dim num3 As Integer = 7
					Dim num4 As Integer = 0
					Dim flag5 As Boolean = flag
					If flag5 Then
						array(0) = Me.db_bytes(endIndex - 1)
						endIndex -= 1
						num4 = 1
					End If
					For i As Integer = endIndex - 1 To startIndex Step -1
						Dim flag6 As Boolean = i - 1 >= startIndex
						If flag6 Then
							array(num4) = CByte(((CInt(CByte((Me.db_bytes(i) >> (num2 - 1 And 7)))) And 255 >> num2) Or CInt(CByte((Me.db_bytes(i - 1) << (num3 And 7))))))
							num2 += 1
							num4 += 1
							num3 -= 1
						Else
							Dim flag7 As Boolean = Not flag
							If flag7 Then
								array(num4) = CByte((CInt(CByte((Me.db_bytes(i) >> (num2 - 1 And 7)))) And 255 >> num2))
							End If
						End If
					Next
					result = BitConverter.ToInt64(array, 0)
				End If
			End If
			Return result
		End Function

		' Token: 0x06000072 RID: 114 RVA: 0x00004674 File Offset: 0x00002874
		Public Function GetRowCount() As Integer
			Return Me.table_entries.Length
		End Function

		' Token: 0x06000073 RID: 115 RVA: 0x00004690 File Offset: 0x00002890
		Public Function GetTableNames() As String()
			Dim array As String() = Nothing
			Dim num As Integer = 0
			Dim num2 As Integer = Me.master_table_entries.Length - 1
			For i As Integer = 0 To num2
				Dim flag As Boolean = Me.master_table_entries(i).item_type = "table"
				If flag Then
					array = CType(Utils.CopyArray(array, New String(num + 1 - 1) {}), String())
					array(num) = Me.master_table_entries(i).item_name
					num += 1
				End If
			Next
			Return array
		End Function

		' Token: 0x06000074 RID: 116 RVA: 0x0000471C File Offset: 0x0000291C
		Public Function GetValue(row_num As Integer, field As Integer) As String
			Dim flag As Boolean = row_num >= Me.table_entries.Length
			Dim result As String
			If flag Then
				result = Nothing
			Else
				Dim flag2 As Boolean = field >= Me.table_entries(row_num).content.Length
				If flag2 Then
					result = Nothing
				Else
					result = Me.table_entries(row_num).content(field)
				End If
			End If
			Return result
		End Function

		' Token: 0x06000075 RID: 117 RVA: 0x0000477C File Offset: 0x0000297C
		Public Function GetValue(row_num As Integer, field As String) As String
			Dim num As Integer = -1
			Dim num2 As Integer = Me.field_names.Length - 1
			For i As Integer = 0 To num2
				Dim flag As Boolean = Me.field_names(i).ToLower().CompareTo(field.ToLower()) = 0
				If flag Then
					num = i
					Exit For
				End If
			Next
			Dim flag2 As Boolean = num = -1
			Dim result As String
			If flag2 Then
				result = Nothing
			Else
				result = Me.GetValue(row_num, num)
			End If
			Return result
		End Function

		' Token: 0x06000076 RID: 118 RVA: 0x000047F0 File Offset: 0x000029F0
		Private Function GVL(startIndex As Integer) As Integer
			Dim flag As Boolean = startIndex > Me.db_bytes.Length
			Dim result As Integer
			If flag Then
				result = 0
			Else
				Dim num As Integer = startIndex + 8
				For i As Integer = startIndex To num
					Dim flag2 As Boolean = i > Me.db_bytes.Length - 1
					If flag2 Then
						Return 0
					End If
					Dim flag3 As Boolean = (Me.db_bytes(i) And 128) <> 128
					If flag3 Then
						Return i
					End If
				Next
				result = startIndex + 8
			End If
			Return result
		End Function

		' Token: 0x06000077 RID: 119 RVA: 0x00004870 File Offset: 0x00002A70
		Private Function IsOdd(value As Long) As Boolean
			Return(value And 1L) = 1L
		End Function

		' Token: 0x06000078 RID: 120 RVA: 0x0000488C File Offset: 0x00002A8C
		Private Sub ReadMasterTable(Offset As ULong)
			Dim flag As Boolean = Me.db_bytes(CInt(Offset)) = 13
			If flag Then
				Dim num As UShort = Convert.ToUInt16(Decimal.Subtract(New Decimal(Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(New Decimal(Offset), 3D)), 2)), 1D))
				Dim num2 As Integer = 0
				Dim flag2 As Boolean = Me.master_table_entries IsNot Nothing
				If flag2 Then
					num2 = Me.master_table_entries.Length
					Me.master_table_entries = CType(Utils.CopyArray(Me.master_table_entries, New SQLiteHandler.sqlite_master_entry(Me.master_table_entries.Length + CInt(num) + 1 - 1) {}), SQLiteHandler.sqlite_master_entry())
				Else
					Me.master_table_entries = New SQLiteHandler.sqlite_master_entry(CInt((num + 1US)) - 1) {}
				End If
				Dim num3 As Integer = CInt(num)
				For i As Integer = 0 To num3
					Dim num4 As ULong = Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(Offset), 8D), New Decimal(i * 2))), 2)
					Dim flag3 As Boolean = Decimal.Compare(New Decimal(Offset), 100D) <> 0
					If flag3 Then
						num4 += Offset
					End If
					Dim num5 As Integer = Me.GVL(CInt(num4))
					Me.CVL(CInt(num4), num5)
					Dim num6 As Integer = Me.GVL(Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), Decimal.Subtract(New Decimal(num5), New Decimal(num4))), 1D)))
					Me.master_table_entries(num2 + i).row_id = Me.CVL(Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), Decimal.Subtract(New Decimal(num5), New Decimal(num4))), 1D)), num6)
					num4 = Convert.ToUInt64(Decimal.Add(Decimal.Add(New Decimal(num4), Decimal.Subtract(New Decimal(num6), New Decimal(num4))), 1D))
					num5 = Me.GVL(CInt(num4))
					num6 = num5
					Dim value As Long = Me.CVL(CInt(num4), num5)
					Dim array As Long() = New Long(4) {}
					Dim num7 As Integer = 0
					Do
						num5 = num6 + 1
						num6 = Me.GVL(num5)
						array(num7) = Me.CVL(num5, num6)
						Dim flag4 As Boolean = array(num7) > 9L
						If flag4 Then
							Dim flag5 As Boolean = Me.IsOdd(array(num7))
							If flag5 Then
								array(num7) = CLng(Math.Round(CDbl((array(num7) - 13L)) / 2.0))
							Else
								array(num7) = CLng(Math.Round(CDbl((array(num7) - 12L)) / 2.0))
							End If
						Else
							array(num7) = CLng(CULng(Me.SQLDataTypeSize(CInt(array(num7)))))
						End If
						num7 += 1
					Loop While num7 <= 4
					Dim flag6 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 1D) = 0
					If flag6 Then
						Me.master_table_entries(num2 + i).item_type = Encoding.[Default].GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(New Decimal(num4), New Decimal(value))), CInt(array(0)))
					Else
						Dim flag7 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 2D) = 0
						If flag7 Then
							Me.master_table_entries(num2 + i).item_type = Encoding.Unicode.GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(New Decimal(num4), New Decimal(value))), CInt(array(0)))
						Else
							Dim flag8 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 3D) = 0
							If flag8 Then
								Me.master_table_entries(num2 + i).item_type = Encoding.BigEndianUnicode.GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(New Decimal(num4), New Decimal(value))), CInt(array(0)))
							End If
						End If
					End If
					Dim flag9 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 1D) = 0
					If flag9 Then
						Me.master_table_entries(num2 + i).item_name = Encoding.[Default].GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(value)), New Decimal(array(0)))), CInt(array(1)))
					Else
						Dim flag10 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 2D) = 0
						If flag10 Then
							Me.master_table_entries(num2 + i).item_name = Encoding.Unicode.GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(value)), New Decimal(array(0)))), CInt(array(1)))
						Else
							Dim flag11 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 3D) = 0
							If flag11 Then
								Me.master_table_entries(num2 + i).item_name = Encoding.BigEndianUnicode.GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(value)), New Decimal(array(0)))), CInt(array(1)))
							End If
						End If
					End If
					Me.master_table_entries(num2 + i).root_num = CLng(Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(Decimal.Add(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(value)), New Decimal(array(0))), New Decimal(array(1))), New Decimal(array(2)))), CInt(array(3))))
					Dim flag12 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 1D) = 0
					If flag12 Then
						Me.master_table_entries(num2 + i).sql_statement = Encoding.[Default].GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(Decimal.Add(Decimal.Add(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(value)), New Decimal(array(0))), New Decimal(array(1))), New Decimal(array(2))), New Decimal(array(3)))), CInt(array(4)))
					Else
						Dim flag13 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 2D) = 0
						If flag13 Then
							Me.master_table_entries(num2 + i).sql_statement = Encoding.Unicode.GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(Decimal.Add(Decimal.Add(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(value)), New Decimal(array(0))), New Decimal(array(1))), New Decimal(array(2))), New Decimal(array(3)))), CInt(array(4)))
						Else
							Dim flag14 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 3D) = 0
							If flag14 Then
								Me.master_table_entries(num2 + i).sql_statement = Encoding.BigEndianUnicode.GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(Decimal.Add(Decimal.Add(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(value)), New Decimal(array(0))), New Decimal(array(1))), New Decimal(array(2))), New Decimal(array(3)))), CInt(array(4)))
							End If
						End If
					End If
				Next
			Else
				Dim flag15 As Boolean = Me.db_bytes(CInt(Offset)) = 5
				If flag15 Then
					Dim num8 As Integer = CInt(Convert.ToUInt16(Decimal.Subtract(New Decimal(Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(New Decimal(Offset), 3D)), 2)), 1D)))
					For j As Integer = 0 To num8
						Dim num9 As UShort = CUShort(Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(Offset), 12D), New Decimal(j * 2))), 2))
						Dim flag16 As Boolean = Decimal.Compare(New Decimal(Offset), 100D) = 0
						If flag16 Then
							Me.ReadMasterTable(Convert.ToUInt64(Decimal.Multiply(Decimal.Subtract(New Decimal(Me.ConvertToInteger(CInt(num9), 4)), 1D), New Decimal(CInt(Me.page_size)))))
						Else
							Me.ReadMasterTable(Convert.ToUInt64(Decimal.Multiply(Decimal.Subtract(New Decimal(Me.ConvertToInteger(CInt((Offset + CULng(num9))), 4)), 1D), New Decimal(CInt(Me.page_size)))))
						End If
					Next
					Me.ReadMasterTable(Convert.ToUInt64(Decimal.Multiply(Decimal.Subtract(New Decimal(Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(New Decimal(Offset), 8D)), 4)), 1D), New Decimal(CInt(Me.page_size)))))
				End If
			End If
		End Sub

		' Token: 0x06000079 RID: 121 RVA: 0x00005184 File Offset: 0x00003384
		Public Function ReadTable(TableName As String) As Boolean
			Dim num As Integer = -1
			Dim num2 As Integer = Me.master_table_entries.Length - 1
			For i As Integer = 0 To num2
				Dim flag As Boolean = Me.master_table_entries(i).item_name.ToLower().CompareTo(TableName.ToLower()) = 0
				If flag Then
					num = i
					Exit For
				End If
			Next
			Dim flag2 As Boolean = num = -1
			Dim result As Boolean
			If flag2 Then
				result = False
			Else
				Dim array As String() = Me.master_table_entries(num).sql_statement.Substring(Me.master_table_entries(num).sql_statement.IndexOf("(") + 1).Split(New Char() { ","c })
				Dim num3 As Integer = array.Length - 1
				For j As Integer = 0 To num3
					array(j) = array(j).TrimStart(New Char(-1) {})
					Dim num4 As Integer = array(j).IndexOf(" ")
					Dim flag3 As Boolean = num4 > 0
					If flag3 Then
						array(j) = array(j).Substring(0, num4)
					End If
					Dim flag4 As Boolean = array(j).IndexOf("UNIQUE") = 0
					If flag4 Then
						Exit For
					End If
					Me.field_names = CType(Utils.CopyArray(Me.field_names, New String(j + 1 - 1) {}), String())
					Me.field_names(j) = array(j)
				Next
				result = Me.ReadTableFromOffset(CULng(((Me.master_table_entries(num).root_num - 1L) * CLng(CULng(Me.page_size)))))
			End If
			Return result
		End Function

		' Token: 0x0600007A RID: 122 RVA: 0x00005318 File Offset: 0x00003518
		Private Function ReadTableFromOffset(Offset As ULong) As Boolean
			Dim flag As Boolean = Me.db_bytes(CInt(Offset)) = 13
			If flag Then
				Dim num As Integer = Convert.ToInt32(Decimal.Subtract(New Decimal(Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(New Decimal(Offset), 3D)), 2)), 1D))
				Dim num2 As Integer = 0
				Dim flag2 As Boolean = Me.table_entries IsNot Nothing
				If flag2 Then
					num2 = Me.table_entries.Length
					Me.table_entries = CType(Utils.CopyArray(Me.table_entries, New SQLiteHandler.table_entry(Me.table_entries.Length + num + 1 - 1) {}), SQLiteHandler.table_entry())
				Else
					Me.table_entries = New SQLiteHandler.table_entry(num + 1 - 1) {}
				End If
				Dim num3 As Integer = num
				For i As Integer = 0 To num3
					Dim array As SQLiteHandler.record_header_field() = Nothing
					Dim num4 As ULong = Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(Offset), 8D), New Decimal(i * 2))), 2)
					Dim flag3 As Boolean = Decimal.Compare(New Decimal(Offset), 100D) <> 0
					If flag3 Then
						num4 += Offset
					End If
					Dim num5 As Integer = Me.GVL(CInt(num4))
					Me.CVL(CInt(num4), num5)
					Dim num6 As Integer = Me.GVL(Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), Decimal.Subtract(New Decimal(num5), New Decimal(num4))), 1D)))
					Me.table_entries(num2 + i).row_id = Me.CVL(Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), Decimal.Subtract(New Decimal(num5), New Decimal(num4))), 1D)), num6)
					num4 = Convert.ToUInt64(Decimal.Add(Decimal.Add(New Decimal(num4), Decimal.Subtract(New Decimal(num6), New Decimal(num4))), 1D))
					num5 = Me.GVL(CInt(num4))
					num6 = num5
					Dim num7 As Long = Me.CVL(CInt(num4), num5)
					Dim num8 As Long = Convert.ToInt64(Decimal.Add(Decimal.Subtract(New Decimal(num4), New Decimal(num5)), 1D))
					Dim num9 As Integer = 0
					While num8 < num7
						array = CType(Utils.CopyArray(array, New SQLiteHandler.record_header_field(num9 + 1 - 1) {}), SQLiteHandler.record_header_field())
						num5 = num6 + 1
						num6 = Me.GVL(num5)
						array(num9).type = Me.CVL(num5, num6)
						Dim flag4 As Boolean = array(num9).type > 9L
						If flag4 Then
							Dim flag5 As Boolean = Me.IsOdd(array(num9).type)
							If flag5 Then
								array(num9).size = CLng(Math.Round(CDbl((array(num9).type - 13L)) / 2.0))
							Else
								array(num9).size = CLng(Math.Round(CDbl((array(num9).type - 12L)) / 2.0))
							End If
						Else
							array(num9).size = CLng(CULng(Me.SQLDataTypeSize(CInt(array(num9).type))))
						End If
						num8 = num8 + CLng((num6 - num5)) + 1L
						num9 += 1
					End While
					Me.table_entries(num2 + i).content = New String(array.Length - 1 + 1 - 1) {}
					Dim num10 As Integer = 0
					Dim num11 As Integer = array.Length - 1
					For j As Integer = 0 To num11
						Dim flag6 As Boolean = array(j).type > 9L
						If flag6 Then
							Dim flag7 As Boolean = Not Me.IsOdd(array(j).type)
							If flag7 Then
								Dim flag8 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 1D) = 0
								If flag8 Then
									Me.table_entries(num2 + i).content(j) = Encoding.[Default].GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(num7)), New Decimal(num10))), CInt(array(j).size))
								Else
									Dim flag9 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 2D) = 0
									If flag9 Then
										Me.table_entries(num2 + i).content(j) = Encoding.Unicode.GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(num7)), New Decimal(num10))), CInt(array(j).size))
									Else
										Dim flag10 As Boolean = Decimal.Compare(New Decimal(Me.encoding), 3D) = 0
										If flag10 Then
											Me.table_entries(num2 + i).content(j) = Encoding.BigEndianUnicode.GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(num7)), New Decimal(num10))), CInt(array(j).size))
										End If
									End If
								End If
							Else
								Me.table_entries(num2 + i).content(j) = Encoding.[Default].GetString(Me.db_bytes, Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(num7)), New Decimal(num10))), CInt(array(j).size))
							End If
						Else
							Me.table_entries(num2 + i).content(j) = Conversions.ToString(Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(num4), New Decimal(num7)), New Decimal(num10))), CInt(array(j).size)))
						End If
						num10 += CInt(array(j).size)
					Next
				Next
			Else
				Dim flag11 As Boolean = Me.db_bytes(CInt(Offset)) = 5
				If flag11 Then
					Dim num12 As Integer = CInt(Convert.ToUInt16(Decimal.Subtract(New Decimal(Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(New Decimal(Offset), 3D)), 2)), 1D)))
					For k As Integer = 0 To num12
						Dim num13 As UShort = CUShort(Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(Decimal.Add(New Decimal(Offset), 12D), New Decimal(k * 2))), 2))
						Me.ReadTableFromOffset(Convert.ToUInt64(Decimal.Multiply(Decimal.Subtract(New Decimal(Me.ConvertToInteger(CInt((Offset + CULng(num13))), 4)), 1D), New Decimal(CInt(Me.page_size)))))
					Next
					Me.ReadTableFromOffset(Convert.ToUInt64(Decimal.Multiply(Decimal.Subtract(New Decimal(Me.ConvertToInteger(Convert.ToInt32(Decimal.Add(New Decimal(Offset), 8D)), 4)), 1D), New Decimal(CInt(Me.page_size)))))
				End If
			End If
			Return True
		End Function

		' Token: 0x04000029 RID: 41
		Private db_bytes As Byte()

		' Token: 0x0400002A RID: 42
		Private encoding As ULong

		' Token: 0x0400002B RID: 43
		Private field_names As String()

		' Token: 0x0400002C RID: 44
		Private master_table_entries As SQLiteHandler.sqlite_master_entry()

		' Token: 0x0400002D RID: 45
		Private page_size As UShort

		' Token: 0x0400002E RID: 46
		Private SQLDataTypeSize As Byte() = New Byte() { 0, 1, 2, 3, 4, 6, 8, 8, 0, 0 }

		' Token: 0x0400002F RID: 47
		Private table_entries As SQLiteHandler.table_entry()

		' Token: 0x02000019 RID: 25
		Private Structure record_header_field
			' Token: 0x04000039 RID: 57
			Public size As Long

			' Token: 0x0400003A RID: 58
			Public type As Long
		End Structure

		' Token: 0x0200001A RID: 26
		Private Structure sqlite_master_entry
			' Token: 0x0400003B RID: 59
			Public row_id As Long

			' Token: 0x0400003C RID: 60
			Public item_type As String

			' Token: 0x0400003D RID: 61
			Public item_name As String

			' Token: 0x0400003E RID: 62
			Public root_num As Long

			' Token: 0x0400003F RID: 63
			Public sql_statement As String
		End Structure

		' Token: 0x0200001B RID: 27
		Private Structure table_entry
			' Token: 0x04000040 RID: 64
			Public row_id As Long

			' Token: 0x04000041 RID: 65
			Public content As String()
		End Structure
	End Class
End Namespace
