using System;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Plugin.Browsers
{
	// Token: 0x0200000F RID: 15
	public class SQLiteHandler
	{
		// Token: 0x0600006F RID: 111 RVA: 0x000043A4 File Offset: 0x000025A4
		public SQLiteHandler(string baseName)
		{
			bool flag = File.Exists(baseName);
			if (flag)
			{
				FileSystem.FileOpen(1, baseName, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared, -1);
				string s = Strings.Space((int)FileSystem.LOF(1));
				FileSystem.FileGet(1, ref s, -1L, false);
				FileSystem.FileClose(new int[]
				{
					1
				});
				this.db_bytes = Encoding.Default.GetBytes(s);
				bool flag2 = Encoding.Default.GetString(this.db_bytes, 0, 15).CompareTo("SQLite format 3") != 0;
				if (flag2)
				{
					throw new Exception("Not a valid SQLite 3 Database File");
				}
				bool flag3 = this.db_bytes[52] > 0;
				if (flag3)
				{
					throw new Exception("Auto-vacuum capable database is not supported");
				}
				this.page_size = (ushort)this.ConvertToInteger(16, 2);
				this.encoding = this.ConvertToInteger(56, 4);
				bool flag4 = decimal.Compare(new decimal(this.encoding), 0m) == 0;
				if (flag4)
				{
					this.encoding = 1UL;
				}
				this.ReadMasterTable(100UL);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000044C4 File Offset: 0x000026C4
		private ulong ConvertToInteger(int startIndex, int Size)
		{
			bool flag = Size > 8 | Size == 0;
			ulong result;
			if (flag)
			{
				result = 0UL;
			}
			else
			{
				ulong num = 0UL;
				int num2 = Size - 1;
				for (int i = 0; i <= num2; i++)
				{
					num = (num << 8 | (ulong)this.db_bytes[startIndex + i]);
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004520 File Offset: 0x00002720
		private long CVL(int startIndex, int endIndex)
		{
			endIndex++;
			byte[] array = new byte[8];
			int num = endIndex - startIndex;
			bool flag = false;
			bool flag2 = num == 0 | num > 9;
			long result;
			if (flag2)
			{
				result = 0L;
			}
			else
			{
				bool flag3 = num == 1;
				if (flag3)
				{
					array[0] = Convert.ToByte( (db_bytes[startIndex] & 127));
					result = BitConverter.ToInt64(array, 0);
				}
				else
				{
					bool flag4 = num == 9;
					if (flag4)
					{
						flag = true;
					}
					int num2 = 1;
					int num3 = 7;
					int num4 = 0;
					bool flag5 = flag;
					if (flag5)
					{
						array[0] = this.db_bytes[endIndex - 1];
						endIndex--;
						num4 = 1;
					}
					for (int i = endIndex - 1; i >= startIndex; i += -1)
					{
						bool flag6 = i - 1 >= startIndex;
						if (flag6)
						{
							array[num4] = (byte)(((int)((byte)(this.db_bytes[i] >> (num2 - 1 & 7))) & 255 >> num2) | (int)((byte)(this.db_bytes[i - 1] << (num3 & 7))));
							num2++;
							num4++;
							num3--;
						}
						else
						{
							bool flag7 = !flag;
							if (flag7)
							{
								array[num4] = (byte)((int)((byte)(this.db_bytes[i] >> (num2 - 1 & 7))) & 255 >> num2);
							}
						}
					}
					result = BitConverter.ToInt64(array, 0);
				}
			}
			return result;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004674 File Offset: 0x00002874
		public int GetRowCount()
		{
			return this.table_entries.Length;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004690 File Offset: 0x00002890
		public string[] GetTableNames()
		{
			string[] array = null;
			int num = 0;
			int num2 = this.master_table_entries.Length - 1;
			for (int i = 0; i <= num2; i++)
			{
				bool flag = this.master_table_entries[i].item_type == "table";
				if (flag)
				{
					array = (string[])Utils.CopyArray(array, new string[num + 1]);
					array[num] = this.master_table_entries[i].item_name;
					num++;
				}
			}
			return array;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000471C File Offset: 0x0000291C
		public string GetValue(int row_num, int field)
		{
			bool flag = row_num >= this.table_entries.Length;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = field >= this.table_entries[row_num].content.Length;
				if (flag2)
				{
					result = null;
				}
				else
				{
					result = this.table_entries[row_num].content[field];
				}
			}
			return result;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000477C File Offset: 0x0000297C
		public string GetValue(int row_num, string field)
		{
			int num = -1;
			int num2 = this.field_names.Length - 1;
			for (int i = 0; i <= num2; i++)
			{
				bool flag = this.field_names[i].ToLower().CompareTo(field.ToLower()) == 0;
				if (flag)
				{
					num = i;
					break;
				}
			}
			bool flag2 = num == -1;
			string result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				result = this.GetValue(row_num, num);
			}
			return result;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000047F0 File Offset: 0x000029F0
		private int GVL(int startIndex)
		{
			bool flag = startIndex > this.db_bytes.Length;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int num = startIndex + 8;
				for (int i = startIndex; i <= num; i++)
				{
					bool flag2 = i > this.db_bytes.Length - 1;
					if (flag2)
					{
						return 0;
					}
					bool flag3 = (this.db_bytes[i] & 128) != 128;
					if (flag3)
					{
						return i;
					}
				}
				result = startIndex + 8;
			}
			return result;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004870 File Offset: 0x00002A70
		private bool IsOdd(long value)
		{
			return (value & 1L) == 1L;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000488C File Offset: 0x00002A8C
		private void ReadMasterTable(ulong Offset)
		{
			bool flag = this.db_bytes[(int)Offset] == 13;
			if (flag)
			{
				ushort num = Convert.ToUInt16(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				int num2 = 0;
				bool flag2 = this.master_table_entries != null;
				if (flag2)
				{
					num2 = this.master_table_entries.Length;
					this.master_table_entries = (SQLiteHandler.sqlite_master_entry[])Utils.CopyArray(this.master_table_entries, new SQLiteHandler.sqlite_master_entry[this.master_table_entries.Length + (int)num + 1]);
				}
				else
				{
					this.master_table_entries = new SQLiteHandler.sqlite_master_entry[(int)(num + 1)];
				}
				int num3 = (int)num;
				for (int i = 0; i <= num3; i++)
				{
					ulong num4 = this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 8m), new decimal(i * 2))), 2);
					bool flag3 = decimal.Compare(new decimal(Offset), 100m) != 0;
					if (flag3)
					{
						num4 += Offset;
					}
					int num5 = this.GVL((int)num4);
					this.CVL((int)num4, num5);
					int num6 = this.GVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)));
					this.master_table_entries[num2 + i].row_id = this.CVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)), num6);
					num4 = Convert.ToUInt64(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num6), new decimal(num4))), 1m));
					num5 = this.GVL((int)num4);
					num6 = num5;
					long value = this.CVL((int)num4, num5);
					long[] array = new long[5];
					int num7 = 0;
					do
					{
						num5 = num6 + 1;
						num6 = this.GVL(num5);
						array[num7] = this.CVL(num5, num6);
						bool flag4 = array[num7] > 9L;
						if (flag4)
						{
							bool flag5 = this.IsOdd(array[num7]);
							if (flag5)
							{
								array[num7] = (long)Math.Round((double)(array[num7] - 13L) / 2.0);
							}
							else
							{
								array[num7] = (long)Math.Round((double)(array[num7] - 12L) / 2.0);
							}
						}
						else
						{
							array[num7] = (long)((ulong)this.SQLDataTypeSize[(int)array[num7]]);
						}
						num7++;
					}
					while (num7 <= 4);
					bool flag6 = decimal.Compare(new decimal(this.encoding), 1m) == 0;
					if (flag6)
					{
						this.master_table_entries[num2 + i].item_type = Encoding.Default.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(new decimal(num4), new decimal(value))), (int)array[0]);
					}
					else
					{
						bool flag7 = decimal.Compare(new decimal(this.encoding), 2m) == 0;
						if (flag7)
						{
							this.master_table_entries[num2 + i].item_type = Encoding.Unicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(new decimal(num4), new decimal(value))), (int)array[0]);
						}
						else
						{
							bool flag8 = decimal.Compare(new decimal(this.encoding), 3m) == 0;
							if (flag8)
							{
								this.master_table_entries[num2 + i].item_type = Encoding.BigEndianUnicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(new decimal(num4), new decimal(value))), (int)array[0]);
							}
						}
					}
					bool flag9 = decimal.Compare(new decimal(this.encoding), 1m) == 0;
					if (flag9)
					{
						this.master_table_entries[num2 + i].item_name = Encoding.Default.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0]))), (int)array[1]);
					}
					else
					{
						bool flag10 = decimal.Compare(new decimal(this.encoding), 2m) == 0;
						if (flag10)
						{
							this.master_table_entries[num2 + i].item_name = Encoding.Unicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0]))), (int)array[1]);
						}
						else
						{
							bool flag11 = decimal.Compare(new decimal(this.encoding), 3m) == 0;
							if (flag11)
							{
								this.master_table_entries[num2 + i].item_name = Encoding.BigEndianUnicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0]))), (int)array[1]);
							}
						}
					}
					this.master_table_entries[num2 + i].root_num = (long)this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2]))), (int)array[3]);
					bool flag12 = decimal.Compare(new decimal(this.encoding), 1m) == 0;
					if (flag12)
					{
						this.master_table_entries[num2 + i].sql_statement = Encoding.Default.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2])), new decimal(array[3]))), (int)array[4]);
					}
					else
					{
						bool flag13 = decimal.Compare(new decimal(this.encoding), 2m) == 0;
						if (flag13)
						{
							this.master_table_entries[num2 + i].sql_statement = Encoding.Unicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2])), new decimal(array[3]))), (int)array[4]);
						}
						else
						{
							bool flag14 = decimal.Compare(new decimal(this.encoding), 3m) == 0;
							if (flag14)
							{
								this.master_table_entries[num2 + i].sql_statement = Encoding.BigEndianUnicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2])), new decimal(array[3]))), (int)array[4]);
							}
						}
					}
				}
			}
			else
			{
				bool flag15 = this.db_bytes[(int)Offset] == 5;
				if (flag15)
				{
					int num8 = (int)Convert.ToUInt16(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
					for (int j = 0; j <= num8; j++)
					{
						ushort num9 = (ushort)this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 12m), new decimal(j * 2))), 2);
						bool flag16 = decimal.Compare(new decimal(Offset), 100m) == 0;
						if (flag16)
						{
							this.ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger((int)num9, 4)), 1m), new decimal((int)this.page_size))));
						}
						else
						{
							this.ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger((int)(Offset + (ulong)num9), 4)), 1m), new decimal((int)this.page_size))));
						}
					}
					this.ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 8m)), 4)), 1m), new decimal((int)this.page_size))));
				}
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005184 File Offset: 0x00003384
		public bool ReadTable(string TableName)
		{
			int num = -1;
			int num2 = this.master_table_entries.Length - 1;
			for (int i = 0; i <= num2; i++)
			{
				bool flag = this.master_table_entries[i].item_name.ToLower().CompareTo(TableName.ToLower()) == 0;
				if (flag)
				{
					num = i;
					break;
				}
			}
			bool flag2 = num == -1;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				string[] array = this.master_table_entries[num].sql_statement.Substring(this.master_table_entries[num].sql_statement.IndexOf("(") + 1).Split(new char[]
				{
					','
				});
				int num3 = array.Length - 1;
				for (int j = 0; j <= num3; j++)
				{
					array[j] = array[j].TrimStart(new char[0]);
					int num4 = array[j].IndexOf(" ");
					bool flag3 = num4 > 0;
					if (flag3)
					{
						array[j] = array[j].Substring(0, num4);
					}
					bool flag4 = array[j].IndexOf("UNIQUE") == 0;
					if (flag4)
					{
						break;
					}
					this.field_names = (string[])Utils.CopyArray(this.field_names, new string[j + 1]);
					this.field_names[j] = array[j];
				}
				result = this.ReadTableFromOffset((ulong)((this.master_table_entries[num].root_num - 1L) * (long)((ulong)this.page_size)));
			}
			return result;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005318 File Offset: 0x00003518
		private bool ReadTableFromOffset(ulong Offset)
		{
			bool flag = this.db_bytes[(int)Offset] == 13;
			if (flag)
			{
				int num = Convert.ToInt32(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				int num2 = 0;
				bool flag2 = this.table_entries != null;
				if (flag2)
				{
					num2 = this.table_entries.Length;
					this.table_entries = (SQLiteHandler.table_entry[])Utils.CopyArray(this.table_entries, new SQLiteHandler.table_entry[this.table_entries.Length + num + 1]);
				}
				else
				{
					this.table_entries = new SQLiteHandler.table_entry[num + 1];
				}
				int num3 = num;
				for (int i = 0; i <= num3; i++)
				{
					SQLiteHandler.record_header_field[] array = null;
					ulong num4 = this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 8m), new decimal(i * 2))), 2);
					bool flag3 = decimal.Compare(new decimal(Offset), 100m) != 0;
					if (flag3)
					{
						num4 += Offset;
					}
					int num5 = this.GVL((int)num4);
					this.CVL((int)num4, num5);
					int num6 = this.GVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)));
					this.table_entries[num2 + i].row_id = this.CVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)), num6);
					num4 = Convert.ToUInt64(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num6), new decimal(num4))), 1m));
					num5 = this.GVL((int)num4);
					num6 = num5;
					long num7 = this.CVL((int)num4, num5);
					long num8 = Convert.ToInt64(decimal.Add(decimal.Subtract(new decimal(num4), new decimal(num5)), 1m));
					int num9 = 0;
					while (num8 < num7)
					{
						array = (SQLiteHandler.record_header_field[])Utils.CopyArray(array, new SQLiteHandler.record_header_field[num9 + 1]);
						num5 = num6 + 1;
						num6 = this.GVL(num5);
						array[num9].type = this.CVL(num5, num6);
						bool flag4 = array[num9].type > 9L;
						if (flag4)
						{
							bool flag5 = this.IsOdd(array[num9].type);
							if (flag5)
							{
								array[num9].size = (long)Math.Round((double)(array[num9].type - 13L) / 2.0);
							}
							else
							{
								array[num9].size = (long)Math.Round((double)(array[num9].type - 12L) / 2.0);
							}
						}
						else
						{
							array[num9].size = (long)((ulong)this.SQLDataTypeSize[(int)array[num9].type]);
						}
						num8 = num8 + (long)(num6 - num5) + 1L;
						num9++;
					}
					this.table_entries[num2 + i].content = new string[array.Length - 1 + 1];
					int num10 = 0;
					int num11 = array.Length - 1;
					for (int j = 0; j <= num11; j++)
					{
						bool flag6 = array[j].type > 9L;
						if (flag6)
						{
							bool flag7 = !this.IsOdd(array[j].type);
							if (flag7)
							{
								bool flag8 = decimal.Compare(new decimal(this.encoding), 1m) == 0;
								if (flag8)
								{
									this.table_entries[num2 + i].content[j] = Encoding.Default.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].size);
								}
								else
								{
									bool flag9 = decimal.Compare(new decimal(this.encoding), 2m) == 0;
									if (flag9)
									{
										this.table_entries[num2 + i].content[j] = Encoding.Unicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].size);
									}
									else
									{
										bool flag10 = decimal.Compare(new decimal(this.encoding), 3m) == 0;
										if (flag10)
										{
											this.table_entries[num2 + i].content[j] = Encoding.BigEndianUnicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].size);
										}
									}
								}
							}
							else
							{
								this.table_entries[num2 + i].content[j] = Encoding.Default.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].size);
							}
						}
						else
						{
							this.table_entries[num2 + i].content[j] = Conversions.ToString(this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].size));
						}
						num10 += (int)array[j].size;
					}
				}
			}
			else
			{
				bool flag11 = this.db_bytes[(int)Offset] == 5;
				if (flag11)
				{
					int num12 = (int)Convert.ToUInt16(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
					for (int k = 0; k <= num12; k++)
					{
						ushort num13 = (ushort)this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 12m), new decimal(k * 2))), 2);
						this.ReadTableFromOffset(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger((int)(Offset + (ulong)num13), 4)), 1m), new decimal((int)this.page_size))));
					}
					this.ReadTableFromOffset(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 8m)), 4)), 1m), new decimal((int)this.page_size))));
				}
			}
			return true;
		}

		// Token: 0x04000029 RID: 41
		private byte[] db_bytes;

		// Token: 0x0400002A RID: 42
		private ulong encoding;

		// Token: 0x0400002B RID: 43
		private string[] field_names;

		// Token: 0x0400002C RID: 44
		private SQLiteHandler.sqlite_master_entry[] master_table_entries;

		// Token: 0x0400002D RID: 45
		private ushort page_size;

		// Token: 0x0400002E RID: 46
		private byte[] SQLDataTypeSize = new byte[]
		{
			0,
			1,
			2,
			3,
			4,
			6,
			8,
			8,
			0,
			0
		};

		// Token: 0x0400002F RID: 47
		private SQLiteHandler.table_entry[] table_entries;

		// Token: 0x02000019 RID: 25
		private struct record_header_field
		{
			// Token: 0x04000039 RID: 57
			public long size;

			// Token: 0x0400003A RID: 58
			public long type;
		}

		// Token: 0x0200001A RID: 26
		private struct sqlite_master_entry
		{
			// Token: 0x0400003B RID: 59
			public long row_id;

			// Token: 0x0400003C RID: 60
			public string item_type;

			// Token: 0x0400003D RID: 61
			public string item_name;

			// Token: 0x0400003E RID: 62
			public long root_num;

			// Token: 0x0400003F RID: 63
			public string sql_statement;
		}

		// Token: 0x0200001B RID: 27
		private struct table_entry
		{
			// Token: 0x04000040 RID: 64
			public long row_id;

			// Token: 0x04000041 RID: 65
			public string[] content;
		}
	}
}
