Imports System
Imports System.Collections.Generic

Namespace Plugin.MessagePack
	' Token: 0x02000008 RID: 8
	Public Class MsgPackArray
		' Token: 0x06000027 RID: 39 RVA: 0x0000295D File Offset: 0x00000B5D
		Public Sub New(msgpackObj As MsgPack, listObj As List(Of MsgPack))
			Me.owner = msgpackObj
			Me.children = listObj
		End Sub

		' Token: 0x06000028 RID: 40 RVA: 0x00002978 File Offset: 0x00000B78
		Public Function Add() As MsgPack
			Return Me.owner.AddArrayChild()
		End Function

		' Token: 0x06000029 RID: 41 RVA: 0x00002998 File Offset: 0x00000B98
		Public Function Add(value As String) As MsgPack
			Dim obj As MsgPack = Me.owner.AddArrayChild()
			obj.AsString = value
			Return obj
		End Function

		' Token: 0x0600002A RID: 42 RVA: 0x000029C0 File Offset: 0x00000BC0
		Public Function Add(value As Long) As MsgPack
			Dim obj As MsgPack = Me.owner.AddArrayChild()
			obj.SetAsInteger(value)
			Return obj
		End Function

		' Token: 0x0600002B RID: 43 RVA: 0x000029E8 File Offset: 0x00000BE8
		Public Function Add(value As Double) As MsgPack
			Dim obj As MsgPack = Me.owner.AddArrayChild()
			obj.SetAsFloat(value)
			Return obj
		End Function

		' Token: 0x17000008 RID: 8
		Public ReadOnly Default Property Item(index As Integer) As MsgPack
			Get
				Return Me.children(index)
			End Get
		End Property

		' Token: 0x17000009 RID: 9
		' (get) Token: 0x0600002D RID: 45 RVA: 0x00002A30 File Offset: 0x00000C30
		Public ReadOnly Property Length As Integer
			Get
				Return Me.children.Count
			End Get
		End Property

		' Token: 0x04000010 RID: 16
		Private children As List(Of MsgPack)

		' Token: 0x04000011 RID: 17
		Private owner As MsgPack
	End Class
End Namespace
