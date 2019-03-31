<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.lblStatusBar = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabConnections = New System.Windows.Forms.TabPage()
        Me.dgvConnections = New System.Windows.Forms.DataGridView()
        Me.TabMountPoints = New System.Windows.Forms.TabPage()
        Me.btnSaveMountPoints = New System.Windows.Forms.Button()
        Me.btnReloadMountPoints = New System.Windows.Forms.Button()
        Me.dgvMountPoints = New System.Windows.Forms.DataGridView()
        Me.tabUsers = New System.Windows.Forms.TabPage()
        Me.btnSaveUsers = New System.Windows.Forms.Button()
        Me.btnReloadUsers = New System.Windows.Forms.Button()
        Me.dgvUsers = New System.Windows.Forms.DataGridView()
        Me.TabServer = New System.Windows.Forms.TabPage()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblSerialStatus = New System.Windows.Forms.Label()
        Me.tbServerMountPoint = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnSerialConnect = New System.Windows.Forms.Button()
        Me.lblServerSP = New System.Windows.Forms.Label()
        Me.lblServer1 = New System.Windows.Forms.Label()
        Me.lblServerBR = New System.Windows.Forms.Label()
        Me.lblServerNone = New System.Windows.Forms.Label()
        Me.lblServerDB = New System.Windows.Forms.Label()
        Me.boxDataBits = New System.Windows.Forms.ComboBox()
        Me.lblServerP = New System.Windows.Forms.Label()
        Me.boxSpeed = New System.Windows.Forms.ComboBox()
        Me.lblServerSB = New System.Windows.Forms.Label()
        Me.boxSerialPort = New System.Windows.Forms.ComboBox()
        Me.tbServerInfo = New System.Windows.Forms.TextBox()
        Me.TabLogs = New System.Windows.Forms.TabPage()
        Me.rtbEvents = New System.Windows.Forms.RichTextBox()
        Me.boxDoLogging = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.StatusStrip.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabConnections.SuspendLayout()
        CType(Me.dgvConnections, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabMountPoints.SuspendLayout()
        CType(Me.dgvMountPoints, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabUsers.SuspendLayout()
        CType(Me.dgvUsers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabServer.SuspendLayout()
        Me.TabLogs.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer1
        '
        '
        'StatusStrip
        '
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatusBar})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 377)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(630, 22)
        Me.StatusStrip.TabIndex = 5
        Me.StatusStrip.Text = "StatusStrip"
        '
        'lblStatusBar
        '
        Me.lblStatusBar.Name = "lblStatusBar"
        Me.lblStatusBar.Size = New System.Drawing.Size(88, 17)
        Me.lblStatusBar.Text = "Not Connected"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabConnections)
        Me.TabControl1.Controls.Add(Me.TabMountPoints)
        Me.TabControl1.Controls.Add(Me.tabUsers)
        Me.TabControl1.Controls.Add(Me.TabServer)
        Me.TabControl1.Controls.Add(Me.TabLogs)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(606, 364)
        Me.TabControl1.TabIndex = 6
        '
        'TabConnections
        '
        Me.TabConnections.BackColor = System.Drawing.SystemColors.Window
        Me.TabConnections.Controls.Add(Me.dgvConnections)
        Me.TabConnections.Location = New System.Drawing.Point(4, 22)
        Me.TabConnections.Name = "TabConnections"
        Me.TabConnections.Padding = New System.Windows.Forms.Padding(3)
        Me.TabConnections.Size = New System.Drawing.Size(598, 338)
        Me.TabConnections.TabIndex = 1
        Me.TabConnections.Text = "Connections"
        '
        'dgvConnections
        '
        Me.dgvConnections.AllowUserToAddRows = False
        Me.dgvConnections.AllowUserToDeleteRows = False
        Me.dgvConnections.AllowUserToResizeRows = False
        Me.dgvConnections.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvConnections.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvConnections.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvConnections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvConnections.Location = New System.Drawing.Point(0, 0)
        Me.dgvConnections.MultiSelect = False
        Me.dgvConnections.Name = "dgvConnections"
        Me.dgvConnections.ReadOnly = True
        Me.dgvConnections.RowHeadersVisible = False
        Me.dgvConnections.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvConnections.ShowEditingIcon = False
        Me.dgvConnections.Size = New System.Drawing.Size(598, 338)
        Me.dgvConnections.TabIndex = 2
        '
        'TabMountPoints
        '
        Me.TabMountPoints.BackColor = System.Drawing.SystemColors.Window
        Me.TabMountPoints.Controls.Add(Me.btnSaveMountPoints)
        Me.TabMountPoints.Controls.Add(Me.btnReloadMountPoints)
        Me.TabMountPoints.Controls.Add(Me.dgvMountPoints)
        Me.TabMountPoints.Location = New System.Drawing.Point(4, 22)
        Me.TabMountPoints.Name = "TabMountPoints"
        Me.TabMountPoints.Padding = New System.Windows.Forms.Padding(3)
        Me.TabMountPoints.Size = New System.Drawing.Size(598, 338)
        Me.TabMountPoints.TabIndex = 2
        Me.TabMountPoints.Text = "Mount Points"
        '
        'btnSaveMountPoints
        '
        Me.btnSaveMountPoints.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveMountPoints.Location = New System.Drawing.Point(520, 312)
        Me.btnSaveMountPoints.Name = "btnSaveMountPoints"
        Me.btnSaveMountPoints.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveMountPoints.TabIndex = 5
        Me.btnSaveMountPoints.Text = "Save"
        Me.btnSaveMountPoints.UseVisualStyleBackColor = True
        '
        'btnReloadMountPoints
        '
        Me.btnReloadMountPoints.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReloadMountPoints.Location = New System.Drawing.Point(3, 312)
        Me.btnReloadMountPoints.Name = "btnReloadMountPoints"
        Me.btnReloadMountPoints.Size = New System.Drawing.Size(110, 23)
        Me.btnReloadMountPoints.TabIndex = 4
        Me.btnReloadMountPoints.Text = "Reload from File"
        Me.btnReloadMountPoints.UseVisualStyleBackColor = True
        '
        'dgvMountPoints
        '
        Me.dgvMountPoints.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvMountPoints.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvMountPoints.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvMountPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMountPoints.Location = New System.Drawing.Point(0, 0)
        Me.dgvMountPoints.MultiSelect = False
        Me.dgvMountPoints.Name = "dgvMountPoints"
        Me.dgvMountPoints.RowHeadersWidth = 20
        Me.dgvMountPoints.Size = New System.Drawing.Size(598, 306)
        Me.dgvMountPoints.TabIndex = 3
        '
        'tabUsers
        '
        Me.tabUsers.BackColor = System.Drawing.SystemColors.Window
        Me.tabUsers.Controls.Add(Me.btnSaveUsers)
        Me.tabUsers.Controls.Add(Me.btnReloadUsers)
        Me.tabUsers.Controls.Add(Me.dgvUsers)
        Me.tabUsers.Location = New System.Drawing.Point(4, 22)
        Me.tabUsers.Name = "tabUsers"
        Me.tabUsers.Padding = New System.Windows.Forms.Padding(3)
        Me.tabUsers.Size = New System.Drawing.Size(598, 338)
        Me.tabUsers.TabIndex = 3
        Me.tabUsers.Text = "Users"
        '
        'btnSaveUsers
        '
        Me.btnSaveUsers.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveUsers.Location = New System.Drawing.Point(520, 312)
        Me.btnSaveUsers.Name = "btnSaveUsers"
        Me.btnSaveUsers.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveUsers.TabIndex = 6
        Me.btnSaveUsers.Text = "Save"
        Me.btnSaveUsers.UseVisualStyleBackColor = True
        '
        'btnReloadUsers
        '
        Me.btnReloadUsers.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReloadUsers.Location = New System.Drawing.Point(3, 312)
        Me.btnReloadUsers.Name = "btnReloadUsers"
        Me.btnReloadUsers.Size = New System.Drawing.Size(110, 23)
        Me.btnReloadUsers.TabIndex = 5
        Me.btnReloadUsers.Text = "Reload from File"
        Me.btnReloadUsers.UseVisualStyleBackColor = True
        '
        'dgvUsers
        '
        Me.dgvUsers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvUsers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUsers.Location = New System.Drawing.Point(0, 0)
        Me.dgvUsers.MultiSelect = False
        Me.dgvUsers.Name = "dgvUsers"
        Me.dgvUsers.RowHeadersWidth = 20
        Me.dgvUsers.Size = New System.Drawing.Size(598, 306)
        Me.dgvUsers.TabIndex = 4
        '
        'TabServer
        '
        Me.TabServer.BackColor = System.Drawing.SystemColors.Window
        Me.TabServer.Controls.Add(Me.Label3)
        Me.TabServer.Controls.Add(Me.lblSerialStatus)
        Me.TabServer.Controls.Add(Me.tbServerMountPoint)
        Me.TabServer.Controls.Add(Me.Label11)
        Me.TabServer.Controls.Add(Me.btnSerialConnect)
        Me.TabServer.Controls.Add(Me.lblServerSP)
        Me.TabServer.Controls.Add(Me.lblServer1)
        Me.TabServer.Controls.Add(Me.lblServerBR)
        Me.TabServer.Controls.Add(Me.lblServerNone)
        Me.TabServer.Controls.Add(Me.lblServerDB)
        Me.TabServer.Controls.Add(Me.boxDataBits)
        Me.TabServer.Controls.Add(Me.lblServerP)
        Me.TabServer.Controls.Add(Me.boxSpeed)
        Me.TabServer.Controls.Add(Me.lblServerSB)
        Me.TabServer.Controls.Add(Me.boxSerialPort)
        Me.TabServer.Controls.Add(Me.tbServerInfo)
        Me.TabServer.Location = New System.Drawing.Point(4, 22)
        Me.TabServer.Name = "TabServer"
        Me.TabServer.Padding = New System.Windows.Forms.Padding(3)
        Me.TabServer.Size = New System.Drawing.Size(598, 338)
        Me.TabServer.TabIndex = 5
        Me.TabServer.Text = "Server"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(172, 307)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 20)
        Me.Label3.TabIndex = 65
        Me.Label3.Text = "Status:"
        '
        'lblSerialStatus
        '
        Me.lblSerialStatus.AutoSize = True
        Me.lblSerialStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSerialStatus.Location = New System.Drawing.Point(228, 307)
        Me.lblSerialStatus.Name = "lblSerialStatus"
        Me.lblSerialStatus.Size = New System.Drawing.Size(107, 20)
        Me.lblSerialStatus.TabIndex = 64
        Me.lblSerialStatus.Text = "Disconnected"
        '
        'tbServerMountPoint
        '
        Me.tbServerMountPoint.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbServerMountPoint.Location = New System.Drawing.Point(110, 97)
        Me.tbServerMountPoint.Name = "tbServerMountPoint"
        Me.tbServerMountPoint.Size = New System.Drawing.Size(303, 26)
        Me.tbServerMountPoint.TabIndex = 63
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(6, 100)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(98, 20)
        Me.Label11.TabIndex = 61
        Me.Label11.Text = "Mount Point:"
        '
        'btnSerialConnect
        '
        Me.btnSerialConnect.BackColor = System.Drawing.Color.LightGray
        Me.btnSerialConnect.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSerialConnect.Location = New System.Drawing.Point(30, 299)
        Me.btnSerialConnect.Name = "btnSerialConnect"
        Me.btnSerialConnect.Size = New System.Drawing.Size(136, 33)
        Me.btnSerialConnect.TabIndex = 60
        Me.btnSerialConnect.Text = "Connect"
        Me.btnSerialConnect.UseVisualStyleBackColor = False
        '
        'lblServerSP
        '
        Me.lblServerSP.AutoSize = True
        Me.lblServerSP.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerSP.Location = New System.Drawing.Point(18, 134)
        Me.lblServerSP.Name = "lblServerSP"
        Me.lblServerSP.Size = New System.Drawing.Size(86, 20)
        Me.lblServerSP.TabIndex = 15
        Me.lblServerSP.Text = "Serial Port:"
        '
        'lblServer1
        '
        Me.lblServer1.AutoSize = True
        Me.lblServer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServer1.Location = New System.Drawing.Point(110, 270)
        Me.lblServer1.Name = "lblServer1"
        Me.lblServer1.Size = New System.Drawing.Size(18, 20)
        Me.lblServer1.TabIndex = 24
        Me.lblServer1.Text = "1"
        '
        'lblServerBR
        '
        Me.lblServerBR.AutoSize = True
        Me.lblServerBR.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerBR.Location = New System.Drawing.Point(14, 168)
        Me.lblServerBR.Name = "lblServerBR"
        Me.lblServerBR.Size = New System.Drawing.Size(90, 20)
        Me.lblServerBR.TabIndex = 16
        Me.lblServerBR.Text = "Baud Rate:"
        '
        'lblServerNone
        '
        Me.lblServerNone.AutoSize = True
        Me.lblServerNone.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerNone.Location = New System.Drawing.Point(110, 236)
        Me.lblServerNone.Name = "lblServerNone"
        Me.lblServerNone.Size = New System.Drawing.Size(47, 20)
        Me.lblServerNone.TabIndex = 23
        Me.lblServerNone.Text = "None"
        '
        'lblServerDB
        '
        Me.lblServerDB.AutoSize = True
        Me.lblServerDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerDB.Location = New System.Drawing.Point(25, 202)
        Me.lblServerDB.Name = "lblServerDB"
        Me.lblServerDB.Size = New System.Drawing.Size(79, 20)
        Me.lblServerDB.TabIndex = 17
        Me.lblServerDB.Text = "Data Bits:"
        '
        'boxDataBits
        '
        Me.boxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxDataBits.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxDataBits.FormattingEnabled = True
        Me.boxDataBits.Items.AddRange(New Object() {"7", "8"})
        Me.boxDataBits.Location = New System.Drawing.Point(110, 199)
        Me.boxDataBits.Name = "boxDataBits"
        Me.boxDataBits.Size = New System.Drawing.Size(77, 28)
        Me.boxDataBits.TabIndex = 22
        '
        'lblServerP
        '
        Me.lblServerP.AutoSize = True
        Me.lblServerP.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerP.Location = New System.Drawing.Point(52, 236)
        Me.lblServerP.Name = "lblServerP"
        Me.lblServerP.Size = New System.Drawing.Size(52, 20)
        Me.lblServerP.TabIndex = 18
        Me.lblServerP.Text = "Parity:"
        '
        'boxSpeed
        '
        Me.boxSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxSpeed.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxSpeed.FormattingEnabled = True
        Me.boxSpeed.Items.AddRange(New Object() {"2400", "4800", "9600", "14400", "19200", "38400", "57600", "115200"})
        Me.boxSpeed.Location = New System.Drawing.Point(110, 165)
        Me.boxSpeed.Name = "boxSpeed"
        Me.boxSpeed.Size = New System.Drawing.Size(150, 28)
        Me.boxSpeed.TabIndex = 21
        '
        'lblServerSB
        '
        Me.lblServerSB.AutoSize = True
        Me.lblServerSB.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerSB.Location = New System.Drawing.Point(26, 270)
        Me.lblServerSB.Name = "lblServerSB"
        Me.lblServerSB.Size = New System.Drawing.Size(78, 20)
        Me.lblServerSB.TabIndex = 19
        Me.lblServerSB.Text = "Stop Bits:"
        '
        'boxSerialPort
        '
        Me.boxSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxSerialPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxSerialPort.FormattingEnabled = True
        Me.boxSerialPort.Location = New System.Drawing.Point(110, 131)
        Me.boxSerialPort.Name = "boxSerialPort"
        Me.boxSerialPort.Size = New System.Drawing.Size(303, 28)
        Me.boxSerialPort.TabIndex = 20
        '
        'tbServerInfo
        '
        Me.tbServerInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbServerInfo.Location = New System.Drawing.Point(6, 6)
        Me.tbServerInfo.Multiline = True
        Me.tbServerInfo.Name = "tbServerInfo"
        Me.tbServerInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbServerInfo.Size = New System.Drawing.Size(586, 71)
        Me.tbServerInfo.TabIndex = 0
        '
        'TabLogs
        '
        Me.TabLogs.BackColor = System.Drawing.SystemColors.Window
        Me.TabLogs.Controls.Add(Me.rtbEvents)
        Me.TabLogs.Controls.Add(Me.boxDoLogging)
        Me.TabLogs.Controls.Add(Me.Label9)
        Me.TabLogs.Location = New System.Drawing.Point(4, 22)
        Me.TabLogs.Name = "TabLogs"
        Me.TabLogs.Padding = New System.Windows.Forms.Padding(3)
        Me.TabLogs.Size = New System.Drawing.Size(598, 338)
        Me.TabLogs.TabIndex = 6
        Me.TabLogs.Text = "Event Log"
        '
        'rtbEvents
        '
        Me.rtbEvents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbEvents.BackColor = System.Drawing.Color.White
        Me.rtbEvents.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbEvents.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbEvents.ForeColor = System.Drawing.Color.Black
        Me.rtbEvents.Location = New System.Drawing.Point(6, 40)
        Me.rtbEvents.Name = "rtbEvents"
        Me.rtbEvents.ReadOnly = True
        Me.rtbEvents.Size = New System.Drawing.Size(592, 298)
        Me.rtbEvents.TabIndex = 19
        Me.rtbEvents.Text = "Events will show up here."
        '
        'boxDoLogging
        '
        Me.boxDoLogging.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxDoLogging.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxDoLogging.FormattingEnabled = True
        Me.boxDoLogging.Items.AddRange(New Object() {"No", "Yes"})
        Me.boxDoLogging.Location = New System.Drawing.Point(313, 3)
        Me.boxDoLogging.Name = "boxDoLogging"
        Me.boxDoLogging.Size = New System.Drawing.Size(77, 28)
        Me.boxDoLogging.TabIndex = 17
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(6, 6)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(301, 20)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Record Events in /Logs/YYYYMMDD.txt?"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 399)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.StatusStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "Open GNSS-Correction-Caster"
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabConnections.ResumeLayout(False)
        CType(Me.dgvConnections, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabMountPoints.ResumeLayout(False)
        CType(Me.dgvMountPoints, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabUsers.ResumeLayout(False)
        CType(Me.dgvUsers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabServer.ResumeLayout(False)
        Me.TabServer.PerformLayout()
        Me.TabLogs.ResumeLayout(False)
        Me.TabLogs.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatusBar As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabConnections As System.Windows.Forms.TabPage
    Friend WithEvents dgvConnections As System.Windows.Forms.DataGridView
    Friend WithEvents TabMountPoints As System.Windows.Forms.TabPage
    Friend WithEvents tabUsers As System.Windows.Forms.TabPage
    Friend WithEvents dgvMountPoints As System.Windows.Forms.DataGridView
    Friend WithEvents dgvUsers As System.Windows.Forms.DataGridView
    Friend WithEvents TabServer As System.Windows.Forms.TabPage
    Friend WithEvents tbServerInfo As System.Windows.Forms.TextBox
    Friend WithEvents TabLogs As System.Windows.Forms.TabPage
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnSerialConnect As System.Windows.Forms.Button
    Friend WithEvents lblServerSP As System.Windows.Forms.Label
    Friend WithEvents lblServer1 As System.Windows.Forms.Label
    Friend WithEvents lblServerBR As System.Windows.Forms.Label
    Friend WithEvents lblServerNone As System.Windows.Forms.Label
    Friend WithEvents lblServerDB As System.Windows.Forms.Label
    Friend WithEvents boxDataBits As System.Windows.Forms.ComboBox
    Friend WithEvents lblServerP As System.Windows.Forms.Label
    Friend WithEvents boxSpeed As System.Windows.Forms.ComboBox
    Friend WithEvents lblServerSB As System.Windows.Forms.Label
    Friend WithEvents boxSerialPort As System.Windows.Forms.ComboBox
    Friend WithEvents boxDoLogging As System.Windows.Forms.ComboBox
    Friend WithEvents tbServerMountPoint As System.Windows.Forms.TextBox
    Friend WithEvents lblSerialStatus As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSaveMountPoints As System.Windows.Forms.Button
    Friend WithEvents btnReloadMountPoints As System.Windows.Forms.Button
    Friend WithEvents btnReloadUsers As System.Windows.Forms.Button
    Friend WithEvents btnSaveUsers As System.Windows.Forms.Button
    Friend WithEvents rtbEvents As System.Windows.Forms.RichTextBox

End Class
