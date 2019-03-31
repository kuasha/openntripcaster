
Public Class MainForm
    Dim settingsfile As String = Application.StartupPath & "\Settings.txt"

    Public dtconnections As New DataTable
    Public dtmountpoints As New DataTable
    Public dtusers As New DataTable

    Private TimerTickCount As Integer = 0
    Public WriteEventsToFile As Boolean = False

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Debug.WriteLine("started")
        tbServerInfo.Text += "Dieser NTRIP Caster muss per Netzwerkübertragung Daten von einem NTRIP Server erhalten. Diese FUnktion wird meist direkt von der RTK-Station direkt übernommen, muss aber explizit aktiviert werden." & _
             " Konfigurieren Sie ihren GNSS-RTK-Empfänger so, dass er sich mit diesem NTRIP-Caster verbindet und die Korrekturdaten sekündlich im CMR Mountpoint hochlädt."

        LogEvent("Anwendung gestartet: " & Now)

        dtconnections.Columns.Add("ID", GetType(Integer))
        dtconnections.PrimaryKey = New DataColumn() {dtconnections.Columns("ID")}
        dtconnections.Columns.Add("Startzeit")
        dtconnections.Columns.Add("MountPoint")
        dtconnections.Columns.Add("Username")
        dtconnections.Columns.Add("User Agent")
        dtconnections.Columns.Add("Address")
        dtconnections.Columns.Add("Bytes", GetType(Integer))
        dtconnections.DefaultView.Sort = "ID"

        dtmountpoints.Columns.Add("MountPoint")
        dtmountpoints.PrimaryKey = New DataColumn() {dtmountpoints.Columns("MountPoint")}
        dtmountpoints.Columns.Add("Password")
        dtmountpoints.Columns.Add("Format")
        dtmountpoints.Columns.Add("Carrier")
        dtmountpoints.Columns.Add("NavSys")
        dtmountpoints.Columns.Add("Lat")
        dtmountpoints.Columns.Add("Lon")
        dtmountpoints.DefaultView.Sort = "MountPoint"
        dgvMountPoints.DataSource = dtmountpoints

        dtusers.Columns.Add("Username")
        dtusers.PrimaryKey = New DataColumn() {dtusers.Columns("Username")}
        dtusers.Columns.Add("Password")
        dtusers.DefaultView.Sort = "Username"
        dgvUsers.DataSource = dtusers

        LoadMountPointList()
        LoadUserList()
        LoadSettingsFile()



        If WriteEventsToFile Then
            boxDoLogging.SelectedIndex = 1
        Else
            boxDoLogging.SelectedIndex = 0
        End If


        Caster.clients = ArrayList.Synchronized(Caster.clients) 'use a synchronized arraylist to store the clients
        Timer1.Start()
        StartServer()
    End Sub
    Private Sub MainForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        End
    End Sub
    Private Sub btnSerialConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSerialConnect.Click
        Server.btnSerialConnect_Click(sender, e)
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If TimerTickCount Mod 10 = 0 Then 'Do this once per second
            dgvConnections.DataSource = dtconnections
        End If
        'MsgBox("Tick")
        TimerTickCount += 1
        If TimerTickCount = 100 Then 'Do this once every 10 seconds
            TimerTickCount = 0

            'Find connections with no Username and an old Startzeit.
            For ctr As Integer = dtconnections.Rows.Count - 1 To 0 Step -1
                If dtconnections.Rows(ctr).Item("Username") = "" Then
                    If DateDiff(DateInterval.Second, dtconnections.Rows(ctr).Item("Startzeit"), Now) > 11 Then
                        Caster.SendStuffToSocketsThread(dtconnections.Rows(ctr).Item("ID"), "401 Unauthorized", True, 0)
                    End If
                End If
            Next
        End If
    End Sub


    Private Sub LoadSettingsFile()
        'Check to make sure directory exists, if not, throw a WTF message.
        If Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath) Then
            MsgBox("Error: The Application's folder doesn't exist. Settings file not loaded.")
            Exit Sub
        End If

        Dim datapathfile = Application.StartupPath & "\Settings.txt"

        If Not My.Computer.FileSystem.FileExists(datapathfile) Then
            'File doesn't exist. Create it.
            Dim fn As New IO.StreamWriter(IO.File.Open(datapathfile, IO.FileMode.Create))
            fn.WriteLine("#Settings Open NTRIP Caster") 'You need to use the format ""Key=Value"" for all settings. '#-Zeilen werden ignoriert.
            fn.WriteLine("")
            fn.Close()
        End If

        'Open and read file
        Dim SettingsArray(1, 0) As String
        Dim keyvalpair(1) As String
        Dim key As String
        Dim value As String
        Dim lCtr As Integer = 0

        Try
            Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(datapathfile)
            Dim linein

            While oRead.Peek <> -1
                linein = Trim(oRead.ReadLine)
                If Len(linein) < 3 Then
                    'Line is too short
                ElseIf Asc(linein) = 35 Then
                    'Line starts with a #
                ElseIf InStr(linein, "=") < 2 Then
                    'There is no equal sign in the string
                Else
                    keyvalpair = Split(linein, "=", 2)
                    key = Trim(keyvalpair(0))
                    value = Trim(keyvalpair(1))
                    If Len(key) > 0 And Len(value) > 0 Then
                        'Looks good, add it to the array
                        ReDim Preserve SettingsArray(1, lCtr)
                        SettingsArray(0, lCtr) = LCase(key)
                        SettingsArray(1, lCtr) = value
                        lCtr = lCtr + 1
                    End If
                End If
            End While
            oRead.Close()
        Catch ex As Exception
        End Try

        If lCtr > 0 Then
            For i = 0 To UBound(SettingsArray, 2)
                value = SettingsArray(1, i)
                Select Case SettingsArray(0, i)
                    Case "serial should be connected"
                        If LCase(value) = "yes" Then SerialShouldBeConnected = True
                    Case "serial port number"
                        If IsNumeric(value) Then
                            Dim portnumber As Integer = CInt(value)
                            If portnumber > 0 And portnumber < 1025 Then
                                SerialPort = portnumber
                            Else
                                LogEvent("Specified Serial Port Number isn't in the range of 1 to 1024.")
                            End If
                        Else
                            LogEvent("Specified Serial Port Number isn't numeric.")
                        End If
                    Case "serial port speed"
                        If IsNumeric(value) Then
                            Dim portspeed As Integer = CInt(value)
                            If portspeed > 2399 And portspeed < 115201 Then
                                SerialSpeed = portspeed
                            Else
                                LogEvent("Specified Serial Port Speed isn't in the range of 2400 to 115200.")
                            End If
                        Else
                            LogEvent("Specified Serial Port Speed isn't numeric.")
                        End If
                    Case "serial port data bits"
                        If value = "7" Then
                            SerialDataBits = 7
                        ElseIf value = "8" Then
                            SerialDataBits = 8
                        Else
                            LogEvent("Specified Serial Port Data bits should be 7 or 8.")
                        End If
                    Case "serial port stop bits"
                        If value = "0" Then
                            SerialStopBits = 0
                        ElseIf value = "1" Then
                            SerialStopBits = 1
                        Else
                            LogEvent("Specified Serial Port Stop bits should be 0 or 1.")
                        End If
                    Case "serial port mount point"
                        SerialMountPoint = value

                    Case "write events to file"
                        If LCase(value) = "yes" Then
                            WriteEventsToFile = True
                        End If

                    Case Else
                        'Key not found
                        'If Not SettingsArray(0, i) = "" Then
                        '    'This will be blank if no settings were loaded
                        'End If
                End Select
            Next
        End If



        tbServerMountPoint.Text = SerialMountPoint

        boxSerialPort.Items.Clear()
        Dim targetport As String = "COM" & SerialPort.ToString

        Dim portcount As Integer = 0
        Dim portindex As Integer = 0
        For Each portName As String In My.Computer.Ports.SerialPortNames
            boxSerialPort.Items.Add(portName)
            If portName = targetport Then
                portindex = portcount
            End If
            portcount += 1
        Next
        If portcount = 0 Then
            boxSerialPort.Items.Add("No Serial Ports Found")
        End If
        boxSerialPort.SelectedIndex = portindex

        If boxSpeed.Items.Count = 9 Then
            boxSpeed.Items.RemoveAt(8)
        End If

        Select Case SerialSpeed
            Case 2400
                boxSpeed.SelectedIndex = 0
            Case 4800
                boxSpeed.SelectedIndex = 1
            Case 9600
                boxSpeed.SelectedIndex = 2
            Case 14400
                boxSpeed.SelectedIndex = 3
            Case 19200
                boxSpeed.SelectedIndex = 4
            Case 38400
                boxSpeed.SelectedIndex = 5
            Case 57600
                boxSpeed.SelectedIndex = 6
            Case 115200
                boxSpeed.SelectedIndex = 7
            Case Else 'How did this happen
                If boxSpeed.Items.Count = 8 Then
                    boxSpeed.Items.Add(SerialSpeed.ToString)
                End If
                boxSpeed.SelectedIndex = 8
        End Select

        If SerialDataBits = 7 Then
            boxDataBits.SelectedIndex = 0
        Else
            boxDataBits.SelectedIndex = 1
        End If




        If WriteEventsToFile Then
            boxDoLogging.SelectedIndex = 1
        Else
            boxDoLogging.SelectedIndex = 0
        End If


        If SerialShouldBeConnected Then
            OpenMySerialPort(False)
        End If
    End Sub
    Public Sub SaveSetting(ByVal key1 As String, ByVal value1 As String, Optional ByVal key2 As String = "", Optional ByVal value2 As String = "", Optional ByVal key3 As String = "", Optional ByVal value3 As String = "")
        If Not My.Computer.FileSystem.FileExists(settingsfile) Then 'File doesn't exist. Create it.
            Dim fn As New IO.StreamWriter(IO.File.Open(settingsfile, IO.FileMode.Create))
            fn.WriteLine("# GPS Data Path Pointer Datei. You need to use the format ""Key=Value"" for all settings.")
            fn.WriteLine("# Any line that starts with a # symbol will be ignored.")
            fn.WriteLine("# The only setting in this file should be the Data Path Location.")
            fn.WriteLine("")
            fn.Close()
        End If


        Dim keyvalpair(1) As String
        Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(settingsfile)
        Dim linein As String
        Dim newfile As String = ""
        Dim foundkey1 As Boolean = False
        Dim foundkey2 As Boolean = False
        Dim foundkey3 As Boolean = False

        While oRead.Peek <> -1
            linein = Trim(oRead.ReadLine)
            If Len(linein) < 3 Then
                'Line is too short
                newfile += linein
            ElseIf Asc(linein) = 35 Then
                'Line starts with a #
                newfile += linein
            ElseIf InStr(linein, "=") < 2 Then
                'There is no equal sign in the string
                newfile += linein
            Else
                keyvalpair = Split(linein, "=", 2)
                If LCase(Trim(keyvalpair(0))) = LCase(key1) Then
                    'Found the right key, update it.
                    newfile += keyvalpair(0) & "=" & value1
                    foundkey1 = True
                ElseIf key2.Length > 0 And LCase(Trim(keyvalpair(0))) = LCase(key2) Then
                    newfile += keyvalpair(0) & "=" & value2
                    foundkey2 = True
                ElseIf key3.Length > 0 And LCase(Trim(keyvalpair(0))) = LCase(key3) Then
                    newfile += keyvalpair(0) & "=" & value3
                    foundkey3 = True
                Else
                    newfile += linein
                End If
            End If
            newfile += vbCrLf
        End While
        oRead.Close()

        If Not foundkey1 Then
            newfile += key1 & "=" & value1 & vbCrLf
        End If
        If key2.Length > 0 And Not foundkey2 Then
            newfile += key2 & "=" & value2 & vbCrLf
        End If
        If key3.Length > 0 And Not foundkey3 Then
            newfile += key3 & "=" & value3 & vbCrLf
        End If


        Try
            Dim sWriter As IO.StreamWriter = New IO.StreamWriter(settingsfile)
            sWriter.Write(newfile)
            sWriter.Flush()
            sWriter.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadMountPointList()
        Dim mountpointlistfile = Application.StartupPath & "\mountpoints.txt"
        dtmountpoints.Rows.Clear()
        If My.Computer.FileSystem.FileExists(mountpointlistfile) Then
            Try
                Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(mountpointlistfile)
                Dim linein

                While oRead.Peek <> -1
                    linein = Trim(oRead.ReadLine)
                    If Len(linein) < 3 Then 'Line is too short
                    ElseIf Asc(linein) = 35 Then 'Line starts with a #
                    ElseIf InStr(linein, ";") < 1 Then 'There is no semicolon sign in the string
                    Else
                        Dim objects() As String = Split(linein, ";")
                        If UBound(objects) >= 6 Then
                            dtmountpoints.Rows.Add(objects(0), objects(1), objects(2), objects(3), objects(4), objects(5), objects(6))
                        End If
                    End If
                End While
                oRead.Close()
            Catch ex As Exception
            End Try
        Else
            dtmountpoints.Rows.Add("ExampleStream", "1234", "RTCM", "2", "GPS", "41.0", "-91.0")
            SaveMountPointList()
        End If
    End Sub
    Private Sub SaveMountPointList()
        Dim mountpointlistfile = Application.StartupPath & "\mountpoints.txt"

        Dim fn As New IO.StreamWriter(IO.File.Open(mountpointlistfile, IO.FileMode.Create))
        fn.WriteLine("#MountPoint Name;MountPoint Password;Format;Carrier;Navigation System;Latitude;Longitude")
        For ctr As Integer = 0 To dtmountpoints.Rows.Count - 1
            fn.WriteLine(dtmountpoints.Rows(ctr).Item("MountPoint") & ";" & dtmountpoints.Rows(ctr).Item("Password") & ";" & dtmountpoints.Rows(ctr).Item("Format") & ";" & dtmountpoints.Rows(ctr).Item("Carrier") & ";" & dtmountpoints.Rows(ctr).Item("NavSys") & ";" & dtmountpoints.Rows(ctr).Item("Lat") & ";" & dtmountpoints.Rows(ctr).Item("Lon"))
        Next
        fn.Close()
    End Sub
    Private Sub btnReloadMountPoints_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReloadMountPoints.Click
        LoadMountPointList()
    End Sub
    Private Sub btnSaveMountPoints_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveMountPoints.Click
        SaveMountPointList()
    End Sub


    Private Sub LoadUserList()
        Dim userlistfile = Application.StartupPath & "\users.txt"
        dtusers.Rows.Clear()
        If My.Computer.FileSystem.FileExists(userlistfile) Then
            Try
                Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(userlistfile)
                Dim linein

                While oRead.Peek <> -1
                    linein = Trim(oRead.ReadLine)
                    If Len(linein) < 3 Then
                        'Line is too short
                    ElseIf Asc(linein) = 35 Then
                        'Line starts with a #
                    ElseIf InStr(linein, ";") < 1 Then
                        'There is no equal sign in the string
                    Else
                        Dim objects() As String = Split(linein, ";")
                        If UBound(objects) >= 1 Then
                            dtusers.Rows.Add(objects(0), objects(1))
                        End If
                    End If
                End While
                oRead.Close()
            Catch ex As Exception
            End Try
        Else
            dtusers.Rows.Add("ExampleUser", "1234")
            SaveUserList()
        End If
    End Sub
    Private Sub SaveUserList()
        Dim userlistfile = Application.StartupPath & "\users.txt"

        Dim fn As New IO.StreamWriter(IO.File.Open(userlistfile, IO.FileMode.Create))
        fn.WriteLine("#Username;Password")
        For ctr As Integer = 0 To dtusers.Rows.Count - 1
            fn.WriteLine(dtusers.Rows(ctr).Item("Username") & ";" & dtusers.Rows(ctr).Item("Password"))
        Next
        fn.Close()
    End Sub
    Private Sub btnReloadUsers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReloadUsers.Click
        LoadUserList()
    End Sub
    Private Sub btnSaveUsers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveUsers.Click
        SaveUserList()
    End Sub


    Public Sub LogEvent(ByVal Message As String)
        If rtbEvents.TextLength > 5000 Then
            Dim NewText As String = Mid(rtbEvents.Text, 1000) 'Drop first 1000 characters
            NewText = NewText.Remove(0, NewText.IndexOf(ChrW(10)) + 1) 'Drop up to the next new line
            rtbEvents.Text = NewText
        End If

        rtbEvents.AppendText(vbCrLf & TimeOfDay() & " - " & Message)
        rtbEvents.SelectionStart = rtbEvents.TextLength
        rtbEvents.ScrollToCaret()

        If WriteEventsToFile Then
            Dim logfolder As String = Application.StartupPath & "\Logs"

            If Not My.Computer.FileSystem.DirectoryExists(logfolder) Then
                Try
                    My.Computer.FileSystem.CreateDirectory(logfolder)
                Catch ex As Exception
                End Try
            End If

            Dim logfile As String = logfolder & "\" & Year(Now) & Format(Month(Now), "00") & Format(DatePart(DateInterval.Day, Now), "00") & ".txt"

            For i = 0 To 10
                Try
                    My.Computer.FileSystem.WriteAllText(logfile, Now() & " - " & Message & vbCrLf, True)
                    Exit For 'This worked, don't try it again
                    Threading.Thread.Sleep(20)
                    Application.DoEvents()
                Catch ex As Exception
                End Try
            Next
        End If
    End Sub
    Private Sub boxDoLogging_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles boxDoLogging.SelectionChangeCommitted
        If boxDoLogging.SelectedIndex = 0 Then
            If WriteEventsToFile Then
                SaveSetting("Write Events to File", "No")
                WriteEventsToFile = False
            End If
        Else
            If Not WriteEventsToFile Then
                SaveSetting("Write Events to File", "Yes")
                WriteEventsToFile = True
            End If
        End If
    End Sub


    Private Sub dgvConnections_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvConnections.CellContentClick
        Debug.WriteLine("clicked")
    End Sub
End Class
