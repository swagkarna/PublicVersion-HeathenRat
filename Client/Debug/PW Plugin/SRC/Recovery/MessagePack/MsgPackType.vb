Imports System

Namespace Plugin.MessagePack
	' Token: 0x0200000A RID: 10
	Public Enum MsgPackType
		' Token: 0x0400001A RID: 26
		Unknown
		' Token: 0x0400001B RID: 27
		Null
		' Token: 0x0400001C RID: 28
		Map
		' Token: 0x0400001D RID: 29
		Array
		' Token: 0x0400001E RID: 30
		[String]
		' Token: 0x0400001F RID: 31
		[Integer]
		' Token: 0x04000020 RID: 32
		UInt64
		' Token: 0x04000021 RID: 33
		[Boolean]
		' Token: 0x04000022 RID: 34
		Float
		' Token: 0x04000023 RID: 35
		[Single]
		' Token: 0x04000024 RID: 36
		DateTime
		' Token: 0x04000025 RID: 37
		Binary
	End Enum
End Namespace
