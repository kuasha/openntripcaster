Imports System.Net.Sockets

Public Class ClientContext
    Private _socket As Socket
    Public FirstTime As Boolean = True
    Public ConnID As Integer = 0
    Public IncomingData As String = ""
    Public ConnLevel As Integer = 0
    Public MountPoint As String = ""
    Public RemoveAfterSend As Boolean = False

    Private _sendQueue As Queue
    Private _removeFlag As Boolean

    Public Sub New(ByVal Socket As Socket)
        _socket = Socket
        FirstTime = True
        Caster.ConnIDCount += 1
        ConnID = Caster.ConnIDCount
        IncomingData = ""
        ConnLevel = 0
        MountPoint = ""
        RemoveAfterSend = False
    End Sub
    Public ReadOnly Property Socket() As Socket
        Get
            Return _socket
        End Get
    End Property

    Public ReadOnly Property SendQueue() As Queue
        Get
            If _sendQueue Is Nothing Then
                _sendQueue = New Queue
            End If
            Return _sendQueue
        End Get
    End Property

    Public Property RemoveFlag() As Boolean
        Get
            Return _removeFlag
        End Get
        Set(ByVal Value As Boolean)
            _removeFlag = Value
        End Set
    End Property

    Public Function IsConnected() As Boolean
        Dim connected As Boolean
        Try
            connected = Not (Socket.Poll(1, SelectMode.SelectRead) And (Socket.Available = 0))
        Catch
        End Try
        Return connected
    End Function

    Public Sub SendNow(ByVal message As String)
        Try
            If Socket IsNot Nothing Then
                If IsConnected() Then
                    Dim bytes() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(message)
                    MsgBox(message)
                    Socket.Send(bytes, bytes.Length, SocketFlags.None)
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("handled error in ClientContext.SendNow::" & ex.Message)
        End Try
    End Sub
End Class
