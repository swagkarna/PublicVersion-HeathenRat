Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace Plugin.MessagePack
	' Token: 0x02000007 RID: 7
	Public Class MsgPackEnum
		Implements IEnumerator

		' Token: 0x06000023 RID: 35 RVA: 0x000028E2 File Offset: 0x00000AE2
		Public Sub New(obj As List(Of MsgPack))
			Me.children = obj
		End Sub

		' Token: 0x17000007 RID: 7
		' (get) Token: 0x06000024 RID: 36 RVA: 0x000028FC File Offset: 0x00000AFC
		ReadOnly Property Current As Object Implements System.Collections.IEnumerator.Current
			Get
				Return Me.children(Me.position)
			End Get
		End Property

		' Token: 0x06000025 RID: 37 RVA: 0x00002920 File Offset: 0x00000B20
		Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
			Me.position += 1
			Return Me.position < Me.children.Count
		End Function

		' Token: 0x06000026 RID: 38 RVA: 0x00002953 File Offset: 0x00000B53
		Sub Reset() Implements System.Collections.IEnumerator.Reset
			Me.position = -1
		End Sub

		' Token: 0x0400000E RID: 14
		Private children As List(Of MsgPack)

		' Token: 0x0400000F RID: 15
		Private position As Integer = -1
	End Class
End Namespace
