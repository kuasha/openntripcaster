Imports System.Net.Sockets
Imports System.Threading
Imports System.Text

Module Caster
    Private listener As TcpListener
    Private listenerThread As Threading.Thread
    Dim ip As Net.IPAddress = Net.IPAddress.Any
    Dim port As Integer = 2101
    Dim socket As Socket
    Private stopFlag As Boolean 'Boolean used to indicate server stopping
    Private stopSyncObj As New Object 'Sync object used with StopFlag
    Public clients As New ArrayList 'List containing client contexts
    Public ConnIDCount As Integer = 0 'Incrementing count of unique connection IDs
    Public MyNetworkName As String = "OpenRTK"

    Public Sub StartServer() 'This gets called by a button on another form.
        If listener IsNot Nothing Then
            StopServer()
        End If

        Try
            listener = New TcpListener(ip, port)
        Catch ex As Exception
            Throw ex
        End Try

        stopFlag = False
        SendStuffToUIThread(0, 0, "0") 'Report no connections

        listenerThread = New Threading.Thread(AddressOf DoListen)
        listenerThread.Start()

        Dim processThread As New Thread(AddressOf ProcessQueues)
        processThread.Start()
    End Sub
    Public Sub StopServer() 'This gets called by a button on another form, and on form close.
        'To reuse a socket, call Disconnect(true) instead of Close(). Close will release all the socket resources.
        'It's recommended to call Shutdown() before Disconnect to allow all data to be sent and received. 
        stopFlag = True

        If listener IsNot Nothing Then
            Try
                listener.Stop() 'Causes DoListen() to abort
            Catch
            End Try
        End If

        Try
            If socket.Connected Then
                socket.Shutdown(SocketShutdown.Both)
                socket.Disconnect(True)
            End If

            If Not listenerThread Is Nothing Then
                listenerThread.Abort()
            End If
        Catch ex As Exception
        End Try

        SendStuffToUIThread(0, 0, "0") 'Report no connections
    End Sub
    Private Sub DoListen()
        Dim context As ClientContext
        Dim socket As Socket

        listener.Start() 'Start listening

        SendStuffToUIThread(-1, 0, "Caster is listening at " & listener.LocalEndpoint.ToString) 'Request Mountpoint

        Try 'The try block is merely to exit gracefully when you stop listening
            Do 'Loop to handle multiple connections
                socket = listener.AcceptSocket 'AcceptSocket blocks until a connection is established
                Debug.WriteLine("listener started")
                If Not socket Is Nothing Then 'can't hurt to double-check
                    Dim ConnectionCount As Int32 = -1
                    Try
                        If Not ConnectionCount < 1 Then
                            Debug.WriteLine((ConnectionCount.ToString()))
                        End If
                    Catch ex As Exception
                    End Try
                    SyncLock clients 'lock the list and add the ClientContext
                        context = New ClientContext(socket) 'create a new context
                        clients.Add(context) 'Add it to the list
                        ConnectionCount = clients.Count
                    End SyncLock
                    If ConnectionCount > -1 Then
                        SendStuffToUIThread(0, 0, clients.Count.ToString)
                    End If
                End If
            Loop
        Catch
        End Try
    End Sub
    Private Sub ProcessQueues()
        Dim buffer(1023) As Byte
        Dim len As Int32
        Do
            '-- Get a lock on the entire collection
            SyncLock clients
                If Not clients.Count < 1 Then Debug.WriteLine(clients.Count)
                For Each client As ClientContext In clients
                    If client.IsConnected = False Then 'Is the socket connected
                        client.RemoveFlag = True
                    Else
                        '-- Process outgoing messages.
                        If client.SendQueue.Count > 0 Then 'Is there an item to send?
                            SyncLock client.SendQueue.Peek 'Get a lock on the outgoing byte array
                                Dim bytes() As Byte = CType(client.SendQueue.Dequeue, Byte()) 'Retrieve the byte array
                                Try 'Try sending
                                    client.Socket.Send(bytes, bytes.Length, SocketFlags.None)
                                Catch ex As System.Net.Sockets.SocketException
                                    client.RemoveFlag = True 'The socket has disconnected. Mark it for death.
                                Catch ex As Exception
                                    Debug.WriteLine("0x1001: " & ex.Message & " | " & ex.InnerException.Message)
                                End Try
                            End SyncLock
                        Else
                            If client.RemoveAfterSend Then 'Send Queue is now empty, kill connection
                                client.RemoveFlag = True
                            End If
                        End If
                        SyncLock stopSyncObj 'Are we stopped?
                            If stopFlag = True Then
                                Exit Do
                            End If
                        End SyncLock

                        'Process incoming messages
                        If client.RemoveFlag = False Then
                            If client.Socket IsNot Nothing Then 'Do we have a socket?
                                If client.FirstTime Then
                                    SendStuffToUIThread(1, client.ConnID, client.Socket.RemoteEndPoint.ToString) 'Add ID to the users table on the UI thread
                                    client.FirstTime = False
                                End If

                                If client.Socket.Poll(10, SelectMode.SelectRead) = True Then 'Did we receive data?
                                    Try 'Try reading from the socket
                                        len = client.Socket.Receive(buffer, buffer.Length, SocketFlags.None) 'len returns the number of bytes received
                                        If len > 0 Then 'Data was received. 
                                            Dim bytes(len - 1) As Byte
                                            Array.Copy(buffer, bytes, len) 'Copy to another buffer

                                            If client.ConnLevel = 2 Then 'This is a server sending in data
                                                PropagateStreamData(client.MountPoint, bytes)
                                            ElseIf client.ConnLevel = 1 Then 'This is a client, and we don't care about the data.
                                            Else 'Not authenticated yet, process data
                                                'Decode data and append to incoming buffer
                                                client.IncomingData += Replace(Encoding.ASCII.GetString(bytes), Chr(10), "")
                                                If InStr(client.IncomingData, Chr(13)) Then 'Contains at least one carridge return
                                                    Dim lines() As String = Split(client.IncomingData, Chr(13))
                                                    For i = 0 To UBound(lines) - 1
                                                        ProcessMessages(client, Trim(lines(i)))
                                                    Next
                                                    client.IncomingData = lines(UBound(lines))
                                                Else 'Data doesn't contain any line breaks
                                                    If client.IncomingData.Length > 4000 Then
                                                        client.IncomingData = "" 'Clean out stale data
                                                    End If
                                                End If
                                            End If

                                        End If
                                    Catch ex As System.Net.Sockets.SocketException
                                        client.RemoveFlag = True 'The socket has disconnected. Mark it for death
                                    Catch ex As Exception
                                        Debug.WriteLine("0x1002: " & ex.Message & " | " & ex.InnerException.Message)
                                    End Try
                                End If
                            End If
                        End If

                        SyncLock stopSyncObj
                            If stopFlag = True Then 'Are we stopped?
                                Exit Do
                            End If
                        End SyncLock
                    End If

                    Thread.Sleep(10) 'To prevent CPU overload
                Next
            End SyncLock

            SyncLock clients 'Remove any dead sockets
                Dim removed As Boolean
                Dim curCount As Int32 = clients.Count
                Do
                    removed = False
                    For i As Int32 = 0 To clients.Count - 1
                        If CType(clients(i), ClientContext).RemoveFlag = True Then
                            SendStuffToUIThread(2, CType(clients(i), ClientContext).ConnID, Nothing) 'Remove ID from the connections table on the UI thread
                            CType(clients(i), ClientContext).Socket.Shutdown(SocketShutdown.Both)
                            CType(clients(i), ClientContext).Socket.Close()
                            clients.Remove(clients(i))
                            removed = True
                            Exit For
                        End If
                    Next
                Loop Until removed = False
                If clients.Count <> curCount Then 'Client count changed
                    SendStuffToUIThread(0, 0, clients.Count.ToString)
                End If
            End SyncLock

            SyncLock stopSyncObj
                If stopFlag = True Then 'Are we stopped?
                    Exit Do
                End If
            End SyncLock

            Thread.Sleep(10)
        Loop
    End Sub
    Sub PropagateStreamData(ByVal MountPoint As String, ByVal bytes() As Byte)
        SendStuffToUIThread(99, bytes.Length, MountPoint)
        For Each client As ClientContext In clients
            If client.MountPoint = MountPoint And client.ConnLevel = 1 Then
                client.SendQueue.Enqueue(bytes)
            End If
        Next
    End Sub


    Private Sub SendMessage(ByVal response As String, ByVal sender As ClientContext)
        sender.SendQueue.Enqueue(ASCIIEncoding.ASCII.GetBytes(response & vbCrLf))
    End Sub
    Private Sub ProcessMessages(ByVal sender As ClientContext, ByVal Message As String)
        If Message.StartsWith("GET ") Then
            Dim mp As String = Mid(Message, 5)
            mp = Mid(mp, 1, mp.IndexOf(" "))
            If mp.Substring(0, 1) = "/" Then mp = mp.Substring(1)
            SendStuffToUIThread(3, sender.ConnID, mp) 'Request Mountpoint
        ElseIf Message.StartsWith("User-Agent: NTRIP ") Then
            SendStuffToUIThread(4, sender.ConnID, Replace(Message, "User-Agent: NTRIP ", "")) 'Update User Agent
        ElseIf Message.StartsWith("Authorization: Basic ") Then
            SendStuffToUIThread(5, sender.ConnID, Replace(Message, "Authorization: Basic ", "")) 'Login


        ElseIf Message.StartsWith("SOURCE ") Then 'Request ability to send stream
            SendStuffToUIThread(6, sender.ConnID, Replace(Message, "SOURCE ", ""))
        ElseIf Message.StartsWith("Source-Agent: NTRIP ") Then
            SendStuffToUIThread(4, sender.ConnID, Replace(Message, "Source-Agent: NTRIP ", "")) 'Update User Agent
        Else
            'Some other message that we don't care about
        End If
    End Sub



    Public Sub SendStuffToSocketsThread(ByVal ConnID As Integer, ByVal Message As String, ByVal Disconnect As Boolean, ByVal NewStatus As Integer)
        Try
            Dim uidel As New SendStuffToSocketsThreadDelegate(AddressOf SendBackToClient)
            Dim o(3) As Object
            o(0) = ConnID
            o(1) = Message
            o(2) = Disconnect
            o(3) = NewStatus
            MainForm.Invoke(uidel, o)
        Catch ex As Exception
            Debug.WriteLine("0x1003: " & ex.Message & " | " & ex.InnerException.Message)
        End Try
    End Sub
    Delegate Sub SendStuffToSocketsThreadDelegate(ByVal ConnID As Integer, ByVal Message As String, ByVal Disconnect As Boolean, ByVal NewStatus As Integer)
    Public Sub SendBackToClient(ByVal ConnID As Integer, ByVal Message As String, ByVal Disconnect As Boolean, ByVal NewStatus As Integer)
        'SyncLock clients
        Dim tempmp As String = ""
        For Each client As ClientContext In clients
            If client.ConnID = ConnID Then
                If NewStatus = 3 Then 'Means the message is the mountpoint
                    client.MountPoint = Message
                Else
                    client.SendQueue.Enqueue(ASCIIEncoding.ASCII.GetBytes(Message & vbCrLf))
                    If Disconnect Then
                        client.RemoveAfterSend = True
                    End If
                    If NewStatus > 0 Then
                        client.ConnLevel = NewStatus
                        If NewStatus = 2 Then tempmp = client.MountPoint
                    End If
                End If
            End If
        Next
        If NewStatus = 2 And tempmp.Length > 0 Then 'this is a server sending data in, disconnect all other servers with same mountpoint
            For Each client As ClientContext In clients
                If Not client.ConnID = ConnID Then
                    If client.MountPoint = tempmp And client.ConnLevel = 2 Then
                        client.RemoveFlag = True
                    End If
                End If
            Next
        End If
        'End SyncLock
    End Sub



    'Message flow
    'When a connection is created, register the IP address and Connection ID
    'When a mountpoint is requested, add that to the list
    '   If an invalid mountpoint is requested, send back the source table and terminate connection
    'When a Username is sent, check if the Password is valid
    '   If good, add to list and tell queue thread that it is a client
    '   If bad, send message back to client saying it is bad.


    Private Sub SendStuffToUIThread(ByVal Action As Integer, ByVal ConnID As Integer, ByVal Message As String)
        Try
            Dim uidel As New SendStuffToUIThreadDelegate(AddressOf ProcessOnUIThread)
            Dim o(2) As Object
            o(0) = Action
            o(1) = ConnID
            o(2) = Message
            MainForm.Invoke(uidel, o)
        Catch ex As Exception
        End Try
    End Sub
    Delegate Sub SendStuffToUIThreadDelegate(ByVal Action As Integer, ByVal ConnID As Integer, ByVal Message As String)
    Public Sub ProcessOnUIThread(ByVal Action As Integer, ByVal ConnID As Integer, ByVal Message As String)
        Debug.WriteLine("0x9001: " & Action.ToString() & " | " & Message.ToString())
        Select Case Action
            Case -1 'On start up, list IP and port
                MainForm.LogEvent(Message)
            Case 0 'Connection count change
                MainForm.lblStatusBar.Text = "Connections: " & Message
            Case 1 'Register user
                MainForm.dtconnections.Rows.Add(ConnID, Now.ToString, "", "", "", Message, 0)
            Case 2 'UnRegister user
                For ctr As Integer = MainForm.dtconnections.Rows.Count - 1 To 0 Step -1
                    If MainForm.dtconnections.Rows(ctr).Item("ID") = ConnID Then
                        'If dtconnections.Rows(ctr).Item("Bytes") > 0 Then
                        MainForm.LogEvent("Connection ID " & ConnID & " - User " & MainForm.dtconnections.Rows(ctr).Item("Username") & " disconnected after moving " & MainForm.dtconnections.Rows(ctr).Item("Bytes") & " bytes.")
                        'End If
                        MainForm.dtconnections.Rows(ctr).Delete()
                    End If
                Next
            Case 3 'Request Mountpoint
                Dim FoundMP As Boolean = False
                For ctr As Integer = MainForm.dtmountpoints.Rows.Count - 1 To 0 Step -1
                    If MainForm.dtmountpoints.Rows(ctr).Item("MountPoint") = Message Then
                        FoundMP = True
                    End If
                Next

                If Not FoundMP Then
                    Message = "Sending SourceTable"
                End If

                For ctr As Integer = MainForm.dtconnections.Rows.Count - 1 To 0 Step -1
                    If MainForm.dtconnections.Rows(ctr).Item("ID") = ConnID Then
                        MainForm.dtconnections.Rows(ctr).Item("MountPoint") = Message
                    End If
                Next

                If FoundMP Then
                    SendStuffToSocketsThread(ConnID, Message, False, 3)
                Else
                    GenerateAndSendSourceTable(ConnID)
                End If

            Case 4 'Update User Agent
                For ctr As Integer = MainForm.dtconnections.Rows.Count - 1 To 0 Step -1
                    If MainForm.dtconnections.Rows(ctr).Item("ID") = ConnID Then
                        MainForm.dtconnections.Rows(ctr).Item("User Agent") = Message
                    End If
                Next

            Case 5 'Login
                For ctr As Integer = MainForm.dtconnections.Rows.Count - 1 To 0 Step -1
                    If MainForm.dtconnections.Rows(ctr).Item("ID") = ConnID Then
                        If MainForm.dtconnections.Rows(ctr).Item("MountPoint").ToString.Length > 0 Then 'A mountpoint was specified
                            DecryptLoginInfo(ConnID, Message)
                        End If
                    End If
                Next

            Case 6 'A server sending in the Password and mount point
                Dim FoundMP As Boolean = False
                Dim GoodPassword As Boolean = False

                Dim mountpoint As String = ""
                If Message.Contains(" ") Then
                    Dim firstspace As Integer = Message.IndexOf(" ")
                    If Message.Length > firstspace + 2 Then
                        Dim Password As String = Message.Substring(0, firstspace)
                        mountpoint = Message.Substring(firstspace + 2)
                        For ctr As Integer = MainForm.dtmountpoints.Rows.Count - 1 To 0 Step -1
                            If MainForm.dtmountpoints.Rows(ctr).Item("MountPoint") = mountpoint Then
                                FoundMP = True
                                If MainForm.dtmountpoints.Rows(ctr).Item("Password") = Password Then
                                    GoodPassword = True
                                End If
                            End If
                        Next
                    End If
                End If
                If Not FoundMP Then
                    mountpoint = "NonExistant"
                End If
                For ctr As Integer = MainForm.dtconnections.Rows.Count - 1 To 0 Step -1
                    If MainForm.dtconnections.Rows(ctr).Item("ID") = ConnID Then
                        MainForm.dtconnections.Rows(ctr).Item("MountPoint") = mountpoint
                    End If
                Next
                If FoundMP Then
                    If GoodPassword Then
                        For ctr As Integer = MainForm.dtconnections.Rows.Count - 1 To 0 Step -1
                            If MainForm.dtconnections.Rows(ctr).Item("ID") = ConnID Then
                                MainForm.dtconnections.Rows(ctr).Item("Startzeit") = Now.ToString
                                MainForm.dtconnections.Rows(ctr).Item("Username") = "--SERVER--"
                                MainForm.LogEvent("Connection ID " & ConnID & " - Server " & MainForm.dtconnections.Rows(ctr).Item("Address") & " is streaming to mountpoint " & mountpoint & ".")
                            End If
                        Next
                        SendStuffToSocketsThread(ConnID, mountpoint, False, 3)
                        SendStuffToSocketsThread(ConnID, "ICY 200 OK", False, 2)

                    Else
                        SendStuffToSocketsThread(ConnID, "ERROR - Bad Password", True, 0)
                    End If
                Else
                    SendStuffToSocketsThread(ConnID, "ERROR - Invalid MountPoint", True, 0)
                End If


            Case 99
                For ctr As Integer = MainForm.dtconnections.Rows.Count - 1 To 0 Step -1
                    If MainForm.dtconnections.Rows(ctr).Item("MountPoint") = Message And Not MainForm.dtconnections.Rows(ctr).Item("Username") = "" Then
                        MainForm.dtconnections.Rows(ctr).Item("Bytes") += ConnID
                    End If
                Next
        End Select
    End Sub

    Public Sub GenerateAndSendSourceTable(ByVal ConnID As Integer)
        Dim ST As String = ""
        For ctr As Integer = 0 To MainForm.dtmountpoints.Rows.Count - 1
            ST += "STR;" & MainForm.dtmountpoints.Rows(ctr).Item("MountPoint") & ";" & MainForm.dtmountpoints.Rows(ctr).Item("MountPoint") & ";" & MainForm.dtmountpoints.Rows(ctr).Item("Format") & ";;" & MainForm.dtmountpoints.Rows(ctr).Item("Carrier") & ";" & MainForm.dtmountpoints.Rows(ctr).Item("NavSys")
            ST += ";" & MyNetworkName & ";;" & MainForm.dtmountpoints.Rows(ctr).Item("Lat") & ";" & MainForm.dtmountpoints.Rows(ctr).Item("Lon") & ";0;0;Unknown;none;B;N;9600;" & vbCrLf
        Next

        Dim msg As String = "SOURCETABLE 200 OK" & vbCrLf
        msg += "Server: Open NTRIP Caster/" & My.Application.Info.Version.Major & "." & Format(My.Application.Info.Version.Minor, "00") & "." & Format(My.Application.Info.Version.Build, "00")
        If My.Application.Info.Version.Revision <> 0 Then msg += " (Rev " & My.Application.Info.Version.Revision & ")"
        msg += vbCrLf
        msg += "Content-Type: text/plain" & vbCrLf
        msg += "Content-Length: " & ST.Length & vbCrLf '& vbCrLf

        msg += ST '& vbCrLf

        msg += "ENDSOURCETABLE" & vbCrLf

        SendStuffToSocketsThread(ConnID, msg, True, 0)

        'Remove client from the list
        For ctr As Integer = MainForm.dtconnections.Rows.Count - 1 To 0 Step -1
            If MainForm.dtconnections.Rows(ctr).Item("ID") = ConnID Then
                If MainForm.dtconnections.Rows(ctr).Item("Bytes") > 0 Then
                    MainForm.LogEvent("Connection ID " & ConnID & " - User " & MainForm.dtconnections.Rows(ctr).Item("Username") & " disconnected after moving " & MainForm.dtconnections.Rows(ctr).Item("Bytes") & " bytes.")
                End If
                MainForm.dtconnections.Rows(ctr).Delete()
            End If
        Next
    End Sub
    Public Sub DecryptLoginInfo(ByVal ConnID As Integer, ByVal Message As String)
        Dim LoginFound As Boolean = False
        Dim Username As String = ""

        If Message.Length > 1 Then
            'Decrypt info
            Dim originalbytes() As Byte = Convert.FromBase64String(Message)
            Dim originaltext As String = System.Text.Encoding.ASCII.GetString(originalbytes)
            If InStr(originaltext, ":") Then
                'Check info
                Dim colinlocation As Integer = originaltext.IndexOf(":")
                If originaltext.Length >= colinlocation + 1 Then
                    Username = originaltext.Substring(0, colinlocation)
                    Dim Password As String = originaltext.Substring(colinlocation + 1)

                    For ctr As Integer = MainForm.dtusers.Rows.Count - 1 To 0 Step -1
                        If MainForm.dtusers.Rows(ctr).Item("Username") = Username And MainForm.dtusers.Rows(ctr).Item("Password") = Password Then
                            LoginFound = True
                        End If
                    Next

                    Dim stophere As Integer = 0
                End If
            End If
        End If

        If LoginFound Then
            For ctr As Integer = MainForm.dtconnections.Rows.Count - 1 To 0 Step -1
                If MainForm.dtconnections.Rows(ctr).Item("ID") = ConnID Then
                    'dtconnections.Rows(ctr).Item("Startzeit") = Now.ToString
                    MainForm.dtconnections.Rows(ctr).Item("Username") = Username
                End If
            Next
            SendStuffToSocketsThread(ConnID, "ICY 200 OK", False, 1)
            MainForm.LogEvent("Connection ID " & ConnID & " - User " & Username & " is connected.")

        Else 'Oops, send back a fail message
            SendStuffToSocketsThread(ConnID, "401 Unauthorized", True, 0)
        End If
    End Sub



    Public Function CalculateChecksum(ByVal sentence As String) As String
        ' Calculates the checksum for a sentence
        ' Loop through all chars to get a checksum
        Dim Character As Char
        Dim Checksum As Integer
        For Each Character In sentence
            Select Case Character
                Case "$"c
                    ' Ignore the dollar sign
                Case "*"c
                    ' Stop processing before the asterisk
                    Exit For
                Case Else
                    ' Is this the first value for the checksum?
                    If Checksum = 0 Then
                        ' Yes. Set the checksum to the value
                        Checksum = Convert.ToByte(Character)
                    Else
                        ' No. XOR the checksum with this character's value
                        Checksum = Checksum Xor Convert.ToByte(Character)
                    End If
            End Select
        Next
        ' Return the checksum formatted as a two-character hexadecimal
        Return Checksum.ToString("X2")
    End Function

End Module
