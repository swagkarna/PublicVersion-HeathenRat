Imports System
Imports System.IO
Imports System.IO.Compression

Namespace Plugin
	' Token: 0x02000005 RID: 5
	Public Module Zip
		' Token: 0x06000016 RID: 22 RVA: 0x00002600 File Offset: 0x00000800
		Public Function Decompress(input As Byte()) As Byte()
			Dim result2 As Byte()
			Using source As MemoryStream = New MemoryStream(input)
				Dim lengthBytes As Byte() = New Byte(3) {}
				source.Read(lengthBytes, 0, 4)
				Dim length As Integer = BitConverter.ToInt32(lengthBytes, 0)
				Using decompressionStream As GZipStream = New GZipStream(source, CompressionMode.Decompress)
					Dim result As Byte() = New Byte(length - 1) {}
					decompressionStream.Read(result, 0, length)
					result2 = result
				End Using
			End Using
			Return result2
		End Function

		' Token: 0x06000017 RID: 23 RVA: 0x00002688 File Offset: 0x00000888
		Public Function Compress(input As Byte()) As Byte()
			Dim result2 As Byte()
			Using result As MemoryStream = New MemoryStream()
				Dim lengthBytes As Byte() = BitConverter.GetBytes(input.Length)
				result.Write(lengthBytes, 0, 4)
				Using compressionStream As GZipStream = New GZipStream(result, CompressionMode.Compress)
					compressionStream.Write(input, 0, input.Length)
					compressionStream.Flush()
				End Using
				result2 = result.ToArray()
			End Using
			Return result2
		End Function
	End Module
End Namespace
