

Module Server
    Public SerialShouldBeConnected As Boolean = False
    Public SerialPort As Integer = 1
    Public SerialSpeed As Integer = 9600
    Public SerialDataBits As Integer = 8
    Public SerialStopBits As Integer = 1
    Public SerialMountPoint As String = ""
    Private SerialReceivedByteCount As Integer = 0

    Dim WithEvents COMPort As New System.IO.Ports.SerialPort



    Public Sub btnSerialConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If MainForm.btnSerialConnect.Text = "Connect" Then
            'Save settings
            If MainForm.boxSerialPort.SelectedItem = "No Serial Ports Found" Then
                MainForm.LogEvent("No serial port was selected. NTRIP Server cannot start.")
                Exit Sub
            Else 'Some serial port was selected
                SerialPort = CInt(Replace(MainForm.boxSerialPort.SelectedItem, "COM", ""))
                MainForm.SaveSetting("Serial Port Number", SerialPort)
            End If

            Select Case MainForm.boxSpeed.SelectedIndex
                Case 0
                    SerialSpeed = 2400
                Case 1
                    SerialSpeed = 4800
                Case 2
                    SerialSpeed = 9600
                Case 3
                    SerialSpeed = 14400
                Case 4
                    SerialSpeed = 19200
                Case 5
                    SerialSpeed = 38400
                Case 6
                    SerialSpeed = 57600
                Case 7
                    SerialSpeed = 115200
                Case 8
                    'custom speed selected. Don't change it
            End Select

            If MainForm.boxDataBits.SelectedIndex = 0 Then
                SerialDataBits = 7
            Else
                SerialDataBits = 8
            End If

            SerialMountPoint = MainForm.tbServerMountPoint.Text

            MainForm.SaveSetting("Serial Port Speed", SerialSpeed, "Serial Port Data Bits", SerialDataBits, "Serial Port Mount Point", SerialMountPoint)
            MainForm.LogEvent("Serial Port Settings Saved")


            If SerialMountPoint = "" Then
                MainForm.lblSerialStatus.Text = "No Mount Point Specified"
                Exit Sub
            End If

            OpenMySerialPort(True)
        Else
            CloseMySerialPort()
            MainForm.SaveSetting("Serial Should be Connected", "No")
        End If
    End Sub

    Public Sub OpenMySerialPort(ByVal UserClickedConnect As Boolean)
        If COMPort.IsOpen Then
            COMPort.RtsEnable = False
            COMPort.DtrEnable = False
            COMPort.Close()
            Application.DoEvents()
            System.Threading.Thread.Sleep(500)
        End If

        If SerialMountPoint = "" Then
            MainForm.lblSerialStatus.Text = "No Mount Point Specified"
            Exit Sub
        End If
        Dim FoundMP As Boolean = False
        For ctr As Integer = MainForm.dtmountpoints.Rows.Count - 1 To 0 Step -1
            If MainForm.dtmountpoints.Rows(ctr).Item("MountPoint") = SerialMountPoint Then
                FoundMP = True
            End If
        Next
        If Not FoundMP Then
            MainForm.lblSerialStatus.Text = "Mount Point not found"
            Exit Sub
        End If


        SerialReceivedByteCount = 0
        MainForm.lblSerialStatus.Text = "Connecting"

        COMPort.PortName = "COM" & SerialPort
        COMPort.BaudRate = SerialSpeed
        COMPort.DataBits = SerialDataBits
        'If SerialStopBits = 1 Then
        COMPort.StopBits = IO.Ports.StopBits.One
        'Else
        'COMPort.StopBits = IO.Ports.StopBits.None
        'End If
        COMPort.WriteTimeout = 2000
        COMPort.ReadTimeout = 2000

        AddHandler COMPort.DataReceived, AddressOf SerialInput

        Try
            COMPort.Open()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        If COMPort.IsOpen Then
            COMPort.RtsEnable = True
            COMPort.DtrEnable = True

            'Kick start the serial port so it starts reading data.
            COMPort.BreakState = True
            System.Threading.Thread.Sleep((11000 / COMPort.BaudRate) + 2) ' Min. 11 bit delay (startbit, 8 data bits, parity bit, stopbit)
            COMPort.BreakState = False


            If UserClickedConnect Then
                MainForm.SaveSetting("Serial Should be Connected", "Yes")
            End If

            'Change connect/disconnect button display status
            MainForm.lblSerialStatus.Text = "Connected to COM " & SerialPort & " at " & SerialSpeed & "bps"
            MainForm.btnSerialConnect.Text = "Disconnect"

            MainForm.tbServerMountPoint.Enabled = False
            MainForm.boxSerialPort.Enabled = False
            MainForm.boxSpeed.Enabled = False
            MainForm.boxDataBits.Enabled = False

        Else
            MainForm.lblSerialStatus.Text = "Unable to open serial port"
        End If
    End Sub
    Public Sub CloseMySerialPort()
        RemoveHandler COMPort.DataReceived, AddressOf SerialInput
        Application.DoEvents()
        Threading.Thread.Sleep(1000)

        If COMPort.IsOpen Then COMPort.Close()

        'Change connect/disconnect button display status
        MainForm.btnSerialConnect.Text = "Connect"
        MainForm.lblSerialStatus.Text = "Disconnected"

        MainForm.tbServerMountPoint.Enabled = True
        MainForm.boxSerialPort.Enabled = True
        MainForm.boxSpeed.Enabled = True
        MainForm.boxDataBits.Enabled = True
    End Sub

    Private Sub SerialInput(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs)
        Try
            Dim buffer(1023) As Byte
            Dim len As Integer = COMPort.Read(buffer, 0, buffer.Length)
            If len > 0 Then
                Dim bytes(len - 1) As Byte
                Array.Copy(buffer, bytes, len) 'Copy to another buffer
                SendSerialLineToUIThread(bytes)
            End If
        Catch ex As TimeoutException
        End Try
    End Sub
    Private Sub SendSerialLineToUIThread(ByVal MyBytes() As Byte)
        Try
            Dim uidel As New SendSerialLineToUIThreadDelegate(AddressOf CallBackSerialtoUIThread)
            Dim o(0) As Object
            o(0) = MyBytes
            MainForm.Invoke(uidel, o)
        Catch ex As Exception
        End Try
    End Sub
    Delegate Sub SendSerialLineToUIThreadDelegate(ByVal MyBytes() As Byte)
    Private Sub CallBackSerialtoUIThread(ByVal MyBytes() As Byte)
        SerialReceivedByteCount += MyBytes.Length
        MainForm.lblSerialStatus.Text = "Received " & SerialReceivedByteCount & " bytes."
        SendSerialBytesToCasterThread(SerialMountPoint, MyBytes)
    End Sub
    Private Sub SendSerialBytesToCasterThread(ByVal MountPoint As String, ByVal bytes() As Byte)
        Try
            Dim uidel As New SendSerialBytesToCasterThreadDelegate(AddressOf PropagateStreamData)
            Dim o(1) As Object
            o(0) = MountPoint
            o(1) = bytes
            MainForm.Invoke(uidel, o)
        Catch ex As Exception
        End Try
    End Sub
    Delegate Sub SendSerialBytesToCasterThreadDelegate(ByVal MountPoint As String, ByVal bytes() As Byte)

End Module
