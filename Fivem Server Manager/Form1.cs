using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Net;


namespace Fivem_Server_Manager
{
    public partial class Form1 : Form
    {
        string appversion = "1.3";

        string FileConfigServer = "server.cfg";
        string FileConfigApp = "Config.cfg";
        string LocationFivemServer = "";
        string ServerName = "";
        string License = "";
        string Tcp = "";
        string Udp = "";
        String MaxClient = "";
        string IconFile = "";
        String DatabaseServer = "";
        string DatabaseName = "";
        string DatabaseUser = "";
        string DatabasePass = "";

        public string Id = "";
        public string SteamId = "";
        public string NamePlayer = "";
        public string Ip = "";
        public string PingPlayer = "";
        int playerDetected = 0;
        int intervalCheckPlayer = 5;
        

        string Hour;
        string Minute;
        string Second;
        bool Schedule1 = false;
        string SchHour1;
        string SchMinute1;
        bool Schedule2 = false;
        string SchHour2;
        string SchMinute2;
        bool Schedule3 = false;
        string SchHour3;
        string SchMinute3;

        int Change_ApplyMode = 0;
        int Start_StopMode = 0;
        bool ServerStarted = false;
        // bool ServerStoped = false;
        bool isrunning = false;
        int selecteditem = -1;


        public Form1()
        {
            InitializeComponent();
            //MessageBox.Show("This Program Created By DroidNetPC \r\n"
            //    + "Github: Oky12 \r\n", "Credit", MessageBoxButtons.OK, MessageBoxIcon.Information

            //    );

            ReadConfig();
            if (IsProcessOpen("FXServer"))
            {
                StopProc("FXServer");
            }
            //CheckProcess();

            Thread T = new Thread(Time);
            T.Start();

            Thread SC = new Thread(Schedule);
            SC.Start();
           
            using (WebClient client = new WebClient())
            {                              
                string s = client.DownloadString("https://raw.githubusercontent.com/Oky12/FivemServerManager/master/Version");
                Console.WriteLine(s);
                
                int a = s.IndexOf(appversion);
                if (a == -1)
                {
                    DialogResult result = MessageBox.Show("New Version is Available. Do you want to Download now?", "Check for Update",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        Process.Start("https://github.com/Oky12/FivemServerManager/releases");

                        StopProc("Fivem Server Manager");
                    }           
                }
                
            }            
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (isrunning)
            {
                MessageBox.Show("Please Stop The Server to Close this software", "Warning",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            else
            {
                DialogResult result = MessageBox.Show("Do you want to Close?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {

                    if (IsProcessOpen("FXServer"))
                    {
                        StopProc("FXServer");
                    }
                    StopProc("Fivem Server Manager");

                }
                else if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

        }

        void CheckProcess()
        {
            //Console.WriteLine(ServerStarted);
            isrunning = IsProcessOpen("FXServer");
            if (isrunning && ServerStarted)
            {
                StatusText("STARTED");
                //button1.Enabled = false;
                BrowserFolder.Enabled = false;
                contextMenuStrip1.Items[0].Enabled = false;
                contextMenuStrip1.Items[1].Enabled = true;
                contextMenuStrip1.Items[2].Enabled = true;
                contextMenuStrip1.Items[3].Enabled = true;
                contextMenuStrip2.Items[0].Enabled = true;
            }
            if (isrunning && !ServerStarted)
            {
                StatusText("STARTING");
            }
            if (!isrunning)
            {
                StatusText("STOP");
                //button1.Enabled = true;
                BrowserFolder.Enabled = true;
                contextMenuStrip1.Items[0].Enabled = true;
                contextMenuStrip1.Items[1].Enabled = false;
                contextMenuStrip1.Items[2].Enabled = false;
                contextMenuStrip1.Items[3].Enabled = false;
                contextMenuStrip2.Items[0].Enabled = false;
            }

            if (!isrunning && ServerStarted)
            {
                MessageBox.Show("Server Not responding or Crash. Restarting....", "Warning",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                StopProc("FXServer");
                ServerStarted = false;
                //MessageBox.Show("Restarting");
                //CheckProcess();
                //Directory.Delete("cache", true);
                Thread.Sleep(1000);
                Thread StartServer = new Thread(start);
                StartServer.Start();

                //CheckProcess();
            }
        }

        public bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }
            }

            return false;
        }

        string GetData(string file, string text, int StartIndex, bool DelLastChar)
        {
            int indexLine = GetIndex(file, text);
            String SubData = "(Empty)";
            if (indexLine != 0)
            {
                String lineData = GetLine(file, indexLine);
                int LenghtData = lineData.Length;
                int SubLenghtData;
                if (DelLastChar)
                {
                    SubLenghtData = LenghtData - (StartIndex + 1);
                }
                else
                {
                    SubLenghtData = LenghtData - StartIndex;
                }
                SubData = lineData.Substring(StartIndex, SubLenghtData);

            }
            return SubData;
        }

        public void StopProc(string proc)
        {
            foreach (var process in Process.GetProcessesByName(proc))
            {
                process.Kill();
            }
        }

        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }

        string GetLine(string fileName, int line)
        {
            using (var sr = new StreamReader(fileName))
            {
                for (int i = 1; i < line; i++)
                    sr.ReadLine();
                return sr.ReadLine();
            }
        }

        int GetIndex(string file, string text)
        {
            string[] c = File.ReadAllLines(file);
            var index = Array.FindIndex(c, row => row.Contains(text)) + 1;
            return index;

        }

        void ScanResource(string file, string startTag, string endTag)
        {

            int StartLineResource = GetIndex(file, startTag);
            int EndLineResource = GetIndex(file, endTag);
            if (StartLineResource == 0 || EndLineResource == 0)
            {
                MessageBox.Show(startTag + " or " + endTag + " Tag not found inside " + FileConfigServer, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int slot = 0;
                int endslot = (EndLineResource - StartLineResource) - 1;
                while (slot < endslot)
                {
                    slot++;
                    string StartLineData = GetLine(file, StartLineResource + slot);
                    int L = StartLineData.Length;
                    if (L != 0)
                    {
                        string StatusResource = StartLineData.Substring(0, 6);

                        if (StatusResource == "start ")
                        {
                            int ResourcesName = L - 6;
                            string RName = StartLineData.Substring(6, ResourcesName);
                            checkedListBox1.Items.Add(RName, true);


                        }
                        else
                        {
                            int ResourcesName = L - 7;
                            string RName = StartLineData.Substring(7, ResourcesName);
                            checkedListBox1.Items.Add(RName, false);
                        }

                    }
                    
                }
            }
        }


        void ReadConfig()
        {

            if (LocationFivemServer == "")
            {
                LocationFivemServer = GetData(FileConfigApp, "PathFivemServer", 18, false);
                BoxPathServer.Text = LocationFivemServer;
            }

            if (!File.Exists(LocationFivemServer + "\\run.cmd"))
            {
                MessageBox.Show("Fivem Server Not Found. make sure you have set the folder fivem server location", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectTab(tabPage5);
            }
            else
            {
                Console.WriteLine(LocationFivemServer + "\\server-data\\" + FileConfigServer);
                if (File.Exists(LocationFivemServer + "\\server-data\\" + FileConfigServer) && File.Exists(FileConfigApp))
                {
                    int indexDB = GetIndex(FileConfigServer, "set mysql_connection_string");
                    if (indexDB != 0)
                    {
                        String DataDB = GetLine(FileConfigServer, indexDB);
                        int lenghDataDB = DataDB.Length;
                        int indexServer = DataDB.IndexOf("server");
                        int indexDatabase = DataDB.IndexOf("database");
                        int indexUsername = DataDB.IndexOf("userid");
                        int indexPassword = DataDB.IndexOf("password");
                        int SubLenghtDBS = (indexDatabase - indexServer) - 8;
                        int SubLenghtDBN = (indexUsername - indexDatabase) - 10;
                        int SubLenghtDBU = (indexPassword - indexUsername) - 8;
                        int SubLenghtDBP = (lenghDataDB - indexPassword) - 11;
                        if (SubLenghtDBS > 1)
                        {
                            DatabaseServer = DataDB.Substring(indexServer + 7, SubLenghtDBS);
                        }
                        else
                        {
                            DatabaseServer = "(Empty)";
                        }
                        if (SubLenghtDBN > 1)
                        {
                            DatabaseName = DataDB.Substring(indexDatabase + 9, SubLenghtDBN);
                        }
                        else
                        {
                            DatabaseName = "(Empty)";
                        }
                        if (SubLenghtDBU > 1)
                        {
                            DatabaseUser = DataDB.Substring(indexUsername + 7, SubLenghtDBU);
                        }
                        else
                        {
                            DatabaseUser = "(Empty)";
                        }
                        if (SubLenghtDBP > 1)
                        {
                            DatabasePass = DataDB.Substring(indexPassword + 9, SubLenghtDBP);
                        }
                        else
                        {
                            DatabasePass = "(Empty)";
                        }

                        DbPass.Text = DatabasePass;
                        DbUser.Text = DatabaseUser;
                        DbName.Text = DatabaseName;
                        DbServer.Text = DatabaseServer;
                    }
                    ServerName = GetData(FileConfigServer, "sv_hostname", 13, true);
                    MaxClient = GetData(FileConfigServer, "sv_maxclients", 14, false);
                    License = GetData(FileConfigServer, "sv_licenseKey", 14, false);
                    Udp = GetData(FileConfigServer, "endpoint_add_udp", 18, true);
                    Tcp = GetData(FileConfigServer, "endpoint_add_tcp", 18, true);
                    IconFile = GetData(FileConfigServer, "load_server_icon", 17, false);
                    SchHour1 = GetData(FileConfigApp, "Schedule1_H", 14, false);
                    SchMinute1 = GetData(FileConfigApp, "Schedule1_M", 14, false);
                    SchHour2 = GetData(FileConfigApp, "Schedule2_H", 14, false);
                    SchMinute2 = GetData(FileConfigApp, "Schedule2_M", 14, false);
                    SchHour3 = GetData(FileConfigApp, "Schedule3_H", 14, false);
                    SchMinute3 = GetData(FileConfigApp, "Schedule3_M", 14, false);

                    checkedListBox1.Items.Clear();
                    ScanResource(FileConfigServer, "#System", "#endSystem");
                    ScanResource(FileConfigServer, "#InESXFolder", "#endInESXFolder");
                    ScanResource(FileConfigServer, "#OutESXFolder", "#endOutESXFolder");
                    ScanResource(FileConfigServer, "#Addons", "#endAddons");

                    if (IconFile == null || IconFile == "")
                    {
                        ICON.Text = "(Empty)";
                    }
                    else
                    {
                        ICON.Text = IconFile;
                    }

                    if (License == null || License == "")
                    {
                        Key.Text = "(Empty)";
                    }
                    else
                    {
                        Key.Text = License;
                    }

                    if (Tcp == null || Tcp == "")
                    {
                        TCP.Text = "(Empty)";
                    }
                    else
                    {
                        TCP.Text = Tcp;
                    }

                    if (Udp == null || Udp == "")
                    {
                        UDP.Text = "(Empty)";
                    }
                    else
                    {
                        UDP.Text = Udp;
                    }

                    if (MaxClient == null || MaxClient == "")
                    {
                        MAXC.Text = "(Empty)";
                    }
                    else
                    {
                        MAXC.Text = MaxClient;
                    }

                    if (ServerName == null || ServerName == "")
                    {
                        Server_Name.Text = "(Empty)";
                    }
                    else
                    {
                        Server_Name.Text = ServerName;
                    }

                    Hour1.Text = SchHour1;
                    Minute1.Text = SchMinute1;
                    Hour2.Text = SchHour2;
                    Minute2.Text = SchMinute2;
                    Hour3.Text = SchHour3;
                    Minute3.Text = SchMinute3;
                }
                else
                {
                    MessageBox.Show("File " + FileConfigApp + " or " + FileConfigServer + " is Missing, Make sure file in same directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        Process proc = new Process();

        void start()
        {

            File.Delete("LOG.txt");
            
        proc.StartInfo.FileName = LocationFivemServer + "\\run.cmd";
            proc.StartInfo.Arguments = " +exec " + FileConfigServer;
            proc.StartInfo.UseShellExecute = false;
            // set up output redirection           
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.EnableRaisingEvents = true;
            proc.StartInfo.CreateNoWindow = true;
            // see below for output handler

            //proc.ErrorDataReceived += proc_DataReceived;
            //proc.OutputDataReceived += proc_DataReceived;

            proc.Start();

            StreamReader read = proc.StandardOutput;
            StreamWriter write = proc.StandardInput;
            
            String line = read.ReadLine();
            while (line != null)
            {

                int PL = line.IndexOf(" steam:");
                if (PL != -1)
                {
                    PrintPlayerList(line);
                    Thread.Sleep(10);
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[playerDetected].Cells[0].Value = Id;
                    dataGridView1.Rows[playerDetected].Cells[1].Value = NamePlayer;
                    dataGridView1.Rows[playerDetected].Cells[3].Value = Ip;
                    dataGridView1.Rows[playerDetected].Cells[2].Value = PingPlayer;
                    playerDetected++;

                }
                else
                {
                    playerDetected = 0;
                    WriteToFileThreadSafe("[" + Hour + ":" + Minute + ":" + Second + "]: " + line, "LOG.txt");
                    SetText(line);
                    int a = line.IndexOf("Welcome!");
                    //Console.WriteLine(a);
                    if (a != -1 && isrunning)
                    {

                        ServerStarted = true;
                        CheckProcess();
                    }
                }

                line = read.ReadLine();

            }

            //proc.BeginErrorReadLine();

            //proc.BeginOutputReadLine();
            
            proc.WaitForExit();
        }

        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public void WriteToFileThreadSafe(string text, string path)
        {
            // Set Status to Locked
            _readWriteLock.EnterWriteLock();
            try
            {
                // Append text to the file
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }

        //void proc_DataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    // output will be in string e.Data
        //    SetText(e.Data);
        //    WriteToFileThreadSafe("[" + Hour + ":" + Minute + ":" + Second + "]: " + e.Data, "LOG.txt");
        //    //using (StreamWriter sw = File.AppendText())
        //    //{
        //    //    //Console.WriteLine("Writing LOG");
        //    //    sw.WriteLine();

        //    //}

        //    if (e.Data != null)
        //    {
        //        int a = e.Data.IndexOf("Welcome!");
        //        //Console.WriteLine(a);
        //        if (a != -1 && isrunning)
        //        {

        //            ServerStarted = true;
        //            CheckProcess();
        //        }
        //        //else
        //        //{
        //        //    ServerStarted = false;
        //        //}
        //    }
        //}

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.AppendText(text + "\r\n");
            }
        }

        void printTime(string text)
        {
            if (this.LTIME.InvokeRequired)
            {
                SetTextCallback a = new SetTextCallback(printTime);
                this.Invoke(a, new object[] { text });
            }
            else
            {
                this.LTIME.Text = text;

            }
        }

        void StatusText(string text)
        {
            if (this.Status_Server.InvokeRequired)
            {
                SetTextCallback f = new SetTextCallback(StatusText);
                this.Invoke(f, new object[] { text });
            }
            else
            {
                this.Status_Server.Text = text;
                if (text == "STARTED")
                {
                    Status_Server.ForeColor = Color.Green;
                }
                if (text == "STARTING")
                {
                    Status_Server.ForeColor = Color.Orange;
                }
                if (text == "STOP")
                {
                    Status_Server.ForeColor = Color.Red;
                }

            }
        }

        void OverWrite(string file, string FindText, string TextForEdit)
        {
            int indexLine = GetIndex(file, FindText);
            lineChanger(TextForEdit, file, indexLine);

        }

        void MoveValueLTT()
        {
            BoxServerName.Text = ServerName;
            BoxTcp.Text = Tcp;
            BoxUdp.Text = Udp;
            BoxLc.Text = License;
            BoxClient.Text = MaxClient;
            BoxIcon.Text = IconFile;
            BoxDbServer.Text = DatabaseServer;
            BoxDbName.Text = DatabaseName;
            BoxDbUser.Text = DatabaseUser;
            BoxDbPass.Text = DatabasePass;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Change_ApplyMode++;
            if (Change_ApplyMode == 2)
            {
                Change_ApplyMode = 0;
            }
            if (Change_ApplyMode == 1)
            {
                change.Text = "Apply";
               // BrowserFolder.Enabled = true;

                if (LocationFivemServer != "")
                {
                    BoxClient.Visible = true;
                    BoxServerName.Visible = true;
                    BoxTcp.Visible = true;
                    BoxUdp.Visible = true;
                    BoxLc.Visible = true;
                    BoxIcon.Visible = true;
                    BoxDbName.Visible = true;
                    BoxDbServer.Visible = true;
                    BoxDbUser.Visible = true;
                    BoxDbPass.Visible = true;

                    MoveValueLTT();
                }
            }
            if (Change_ApplyMode == 0)
            {
                change.Text = "Change";
               // BrowserFolder.Enabled = false;

                BoxClient.Visible = false;
                BoxServerName.Visible = false;
                BoxTcp.Visible = false;
                BoxUdp.Visible = false;
                BoxLc.Visible = false;
                BoxIcon.Visible = false;
                BoxDbName.Visible = false;
                BoxDbServer.Visible = false;
                BoxDbUser.Visible = false;
                BoxDbPass.Visible = false;


                string NewPathFolderFivemServer = LocationFivemServer;
                string RawDataPathFolderFivemServer = "PathFivemServer = " + NewPathFolderFivemServer;
                lineChanger(RawDataPathFolderFivemServer, FileConfigApp, 2);

                if (LocationFivemServer == "")
                {

                    ReadConfig();
                    Console.WriteLine("Read Config");
                    MoveValueLTT();
                }
                else
                {
                    Console.WriteLine("saving");

                    string NewServerName = BoxServerName.Text;
                    string RawDataServerName = "sv_hostname " + "\"" + NewServerName + "\"";
                    OverWrite(FileConfigServer, "sv_hostname", RawDataServerName);

                    string NewTcp = BoxTcp.Text;
                    string RawDataTcp = "endpoint_add_tcp " + "\"" + NewTcp + "\"";
                    OverWrite(FileConfigServer, "endpoint_add_tcp", RawDataTcp);

                    string NewUdp = BoxUdp.Text;
                    string RawDataUdp = "endpoint_add_udp " + "\"" + NewUdp + "\"";
                    OverWrite(FileConfigServer, "endpoint_add_udp", RawDataUdp);

                    string NewLc = BoxLc.Text;
                    string RawDataLc = "sv_licenseKey " + NewLc;
                    OverWrite(FileConfigServer, "sv_licenseKey", RawDataLc);

                    string NewMcl = BoxClient.Text;
                    string RawDataMcl = "sv_maxclients " + NewMcl;
                    OverWrite(FileConfigServer, "sv_maxclients", RawDataMcl);
                    Console.WriteLine(RawDataMcl);

                    string NewIcon = BoxIcon.Text;
                    string RawDataIcon = "load_server_icon " + NewIcon;
                    OverWrite(FileConfigServer, "load_server_icon", RawDataIcon);

                    string NewDbServer = BoxDbServer.Text;
                    string NewDbName = BoxDbName.Text;
                    string NewDbUser = BoxDbUser.Text;
                    string NewDbPass = BoxDbPass.Text;
                    string RawDataDb = "set mysql_connection_string " + "\"server=" + NewDbServer + ";database=" + NewDbName + ";userid=" + NewDbUser + ";password=" + NewDbPass + "\"";
                    OverWrite(FileConfigServer, "set mysql_connection_string", RawDataDb);

                    ReadConfig();
                    MessageBox.Show("Configuration Saved", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }



        void AutoRestart()
        {
            textBox1.Clear();
            StopProc("FXServer");
            //MessageBox.Show("Restarting");
            //CheckProcess();
            ServerStarted = false;
            Directory.Delete("cache", true);
            Thread.Sleep(1000);
            Thread StartServer = new Thread(start);
            StartServer.Start();
            MessageBox.Show("Restart Success");
            //CheckProcess();
        }

        void Schedule()
        {
            bool Schedule1RUN = true;
            bool Schedule2RUN = true;
            bool Schedule3RUN = true;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                if (Hour == Hour1.Text && Minute == Minute1.Text && Schedule1 == true && Schedule1RUN == true)
                {
                    AutoRestart();
                    MessageBox.Show("Server Restart On Schedule 1", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Schedule1RUN = false;
                }
                if (Hour != Hour1.Text || Minute != Minute1.Text)
                {
                    Schedule1RUN = true;
                }

                if (Hour == Hour2.Text && Minute == Minute2.Text && Schedule2 == true && Schedule2RUN == true)
                {
                    AutoRestart();
                    MessageBox.Show("Server Restart On Schedule 2", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Schedule2RUN = false;
                }
                if (Hour != Hour2.Text || Minute != Minute2.Text)
                {
                    Schedule2RUN = true;
                }

                if (Hour == Hour3.Text && Minute == Minute3.Text && Schedule3 == true && Schedule3RUN == true)
                {
                    AutoRestart();
                    MessageBox.Show("Server Restart On Schedule 3", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Schedule3RUN = false;
                }
                if (Hour != Hour3.Text || Minute != Minute3.Text)
                {
                    Schedule3RUN = true;
                }
                if (i == 9) { i = 0; }
            }
        }
        void Time()
        {
            int CheckStatusTime = 0;
            for (int i = 0; i < 10; i++)
            {
                DateTime LocalTime = DateTime.Now;
                Hour = LocalTime.Hour.ToString();
                Minute = LocalTime.Minute.ToString();
                Second = LocalTime.Second.ToString();
                printTime(Hour + ":" + Minute + ":" + Second);
                Thread.Sleep(1000);
                CheckProcess();
                
                CheckStatusTime++;
                if (CheckStatusTime >= intervalCheckPlayer && isrunning && ServerStarted)
                {
                   int R = dataGridView1.Rows.Count;
                    if(R != 0)
                    {                       
                        dataGridView1.Rows.Clear();
                    }
                    
                    StreamWriter Send = proc.StandardInput;
                    Send.WriteLine("status");
                    CheckStatusTime = 0;
                }
                else
                {
                    playerDetected = 0;
                }
                if (i == 9) { i = 0; }
            }

        }



        private void Label4_Click(object sender, EventArgs e)
        {

        }

        Thread StartServer;
        private void Button1_Click_1(object sender, EventArgs e)

        {
            Start_StopMode++;

            if (Start_StopMode == 2)
            {
                Start_StopMode = 0;
            }

            if (Start_StopMode == 1)
            {

                Start.Text = "STOP";
                StartServer = new Thread(start);
                tabControl1.SelectTab(tabPage2);

                StartServer.Start();
                textBox1.Text = "";

                Thread.Sleep(2500);
                //CheckProcess();
                RSTART.Enabled = true;
                if (!isrunning)
                {
                    MessageBox.Show("Server Failed to Start", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AutoRestart();
                }
            }

            if (Start_StopMode == 0)
            {
                //proc.CancelOutputRead();
                Start.Text = "START";
                StopProc("FXServer");
                //proc.Kill();
                //StartServer.Abort();
                ServerStarted = false;
                //ServerStoped = true;
                Thread.Sleep(500);
                //CheckProcess();
                RSTART.Enabled = false;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button4_Click(object sender, EventArgs e)
        {


        }

        private void TabPage1_Click(object sender, EventArgs e)
        {

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Schedule1 = true;
            }
            else { Schedule1 = false; }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Schedule2 = true;
            }
            else { Schedule2 = false; }
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                Schedule3 = true;
            }
            else { Schedule3 = false; }
        }

        private void EditSch1_Click_1(object sender, EventArgs e)
        {

            Hour1.Enabled = true;
            Minute1.Enabled = true;
            ApplySch1.Enabled = true;
            EditSch1.Enabled = false;

        }

        private void EditSch2_Click_1(object sender, EventArgs e)
        {
            Hour2.Enabled = true;
            Minute2.Enabled = true;
            ApplySch2.Enabled = true;
            EditSch2.Enabled = false;

        }

        private void EditSch3_Click_1(object sender, EventArgs e)
        {
            Hour3.Enabled = true;
            Minute3.Enabled = true;
            ApplySch3.Enabled = true;
            EditSch3.Enabled = false;

        }

        private void ApplySch1_Click(object sender, EventArgs e)
        {
            Hour1.Enabled = false;
            Minute1.Enabled = false;
            ApplySch1.Enabled = false;
            EditSch1.Enabled = true;
            lineChanger("Schedule1_H = " + Hour1.Text, "Config.cfg", 3);
            lineChanger("Schedule1_M = " + Minute1.Text, "Config.cfg", 4);
            SchHour1 = Hour1.Text;
            SchMinute1 = Minute1.Text;
            MessageBox.Show("Schedule 1 Saved", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ApplySch2_Click(object sender, EventArgs e)
        {
            Hour2.Enabled = false;
            Minute2.Enabled = false;
            ApplySch2.Enabled = false;
            EditSch2.Enabled = true;
            lineChanger("Schedule2_H = " + Hour2.Text, "Config.cfg", 5);
            lineChanger("Schedule2_M = " + Minute2.Text, "Config.cfg", 6);
            SchHour2 = Hour2.Text;
            SchMinute2 = Minute2.Text;
            MessageBox.Show("Schedule 2 Saved", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ApplySch3_Click(object sender, EventArgs e)
        {
            Hour3.Enabled = false;
            Minute3.Enabled = false;
            ApplySch3.Enabled = false;
            EditSch3.Enabled = true;
            lineChanger("Schedule3_H = " + Hour3.Text, "Config.cfg", 7);
            lineChanger("Schedule3_M = " + Minute3.Text, "Config.cfg", 8);
            SchHour3 = Hour3.Text;
            SchMinute3 = Minute3.Text;
            MessageBox.Show("Schedule 3 Saved", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal static void insertLineToSimpleFile(string fileName, string lineToSearch, string lineToAdd, bool aboveBelow = false)
        {
            var txtLines = File.ReadAllLines(fileName).ToList();
            int index = aboveBelow ? txtLines.IndexOf(lineToSearch) + 1 : txtLines.IndexOf(lineToSearch);
            if (index > 0)
            {
                txtLines.Insert(index, lineToAdd);
                File.WriteAllLines(fileName, txtLines);
            }
        }

        public void CreateEntry(string file, string Tag, string textBeforeTag) //npcName = "item1"
        {
            var fileName = file;
            var endTag = String.Format("[/{0}]", Tag);
            var lineToAdd = textBeforeTag;

            var txtLines = File.ReadAllLines(fileName).ToList();   //Fill a list with the lines from the txt file.
            txtLines.Insert(txtLines.IndexOf(endTag), lineToAdd);  //Insert the line you want to add last under the tag 'item1'.
            File.WriteAllLines(fileName, txtLines);                    //Add the lines including the new one.
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string Tag = "";
            if (comboBox1.Text != "Categories")
            {
                if (comboBox1.Text == "ESX")
                {
                    Tag = "#endInESXFolder";
                }
                if (comboBox1.Text == "NON-ESX")
                {
                    Tag = "#endOutESXFolder";
                }
                if (comboBox1.Text == "ADDONS")
                {
                    Tag = "#endAddons";
                }
            }
            else
            {
                MessageBox.Show("please select a category", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            string newResource = "start " + NewResource.Text;
            insertLineToSimpleFile(FileConfigServer, Tag, newResource, false);
            ReadConfig();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (selecteditem != -1)
            {
                var selectitem = checkedListBox1.SelectedItem;
                string ResourceSelect = selectitem.ToString();
                checkedListBox1.Items.Remove(ResourceSelect);
                string NewResourceStatus = "";
                int LineResouce = GetIndex(FileConfigServer, ResourceSelect);
                lineChanger(NewResourceStatus, FileConfigServer, LineResouce);
            }

        }



        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            checkedListBox1.ClearSelected();
            
            
        }


        private void CheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selecteditem = checkedListBox1.SelectedIndex;
            if (selecteditem != -1)
            {

                if (checkedListBox1.GetItemChecked(selecteditem))
                {
                    var NameListResource = checkedListBox1.SelectedItem;
                    string NameResource = NameListResource.ToString();
                    string NewResourceStatus = "start " + NameResource;
                    int LineResouce = GetIndex(FileConfigServer, NameResource);
                    lineChanger(NewResourceStatus, FileConfigServer, LineResouce);

                    Console.WriteLine("enable");
                }
                else
                {
                    var NameListResource = checkedListBox1.SelectedItem;
                    string NameResource = NameListResource.ToString();
                    string NewResourceStatus = "#start " + NameResource;
                    int LineResouce = GetIndex(FileConfigServer, NameResource);
                    lineChanger(NewResourceStatus, FileConfigServer, LineResouce);
                    Console.WriteLine("disable");
                }
            }
        }

        private void RSTART_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            StopProc("FXServer");
            ServerStarted = false;
            //MessageBox.Show("Restarting");
            //CheckProcess();
            //Directory.Delete("cache", true);
            Thread.Sleep(1000);
            Thread StartServer = new Thread(start);
            StartServer.Start();
            MessageBox.Show("Restart Success");
            //CheckProcess();
        }

        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                checkedListBox1.ClearSelected();
                string s = SearchText.Text;
                int h = checkedListBox1.FindString(s);
                if (h == -1)
                {
                    checkedListBox1.ClearSelected();
                    MessageBox.Show("Resources Not Found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    SearchText.Text = "";
                    checkedListBox1.SetSelected(h, true);
                }
            }
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
           
            
            MessageBox.Show("Version: " + appversion + "\r\n"
               + "This Program Created By DroidNetPC \r\n"
               + "Github: Oky12 \r\n", "Credit", MessageBoxButtons.OK, MessageBoxIcon.Information

               );
        }

        private void SearchText_Leave(object sender, EventArgs e)
        {
            if (SearchText.Text == "")
            {
                SearchText.Text = "Press Enter to Search";
            }
            SearchText.ForeColor = Color.Gray;
        }

        private void SearchText_Enter(object sender, EventArgs e)
        {
            if (SearchText.Text == "Press Enter to Search")
            {
                SearchText.Text = "";
            }
            SearchText.ForeColor = Color.Black;
        }

        private void SearchText_TextChanged(object sender, EventArgs e)
        {

        }

        private void NewResource_Leave(object sender, EventArgs e)
        {
            if (NewResource.Text == "")
            {
                NewResource.Text = "Name for New Resources";
            }
            NewResource.ForeColor = Color.Gray;
        }

        private void NewResource_Enter(object sender, EventArgs e)
        {
            if (NewResource.Text == "Name for New Resources")
            {
                NewResource.Text = "";
            }
            NewResource.ForeColor = Color.Black;
        }

        private void TextBox2_Enter(object sender, EventArgs e)
        {

        }

        
       
              
        private void Button2_Click_1(object sender, EventArgs e)
        {
            //Thread ts = new Thread(TestCMDIO);
            //ts.Start();
            StreamWriter Send = proc.StandardInput;
            Send.WriteLine(textBox3.Text);
        }

        void PrintPlayerList(string text)
        {
            int space1 = text.IndexOf(" ", 0);
            int space2 = text.IndexOf(" ", space1 + 2);
            int space3 = text.IndexOf(" ", space2 + 2);
            int space4 = text.IndexOf(" ", space3 + 2);

            int LD2 = (space2 - space1) - 1;
            int LD3 = (space3 - space2) - 1;
            int LD4 = (space4 - space3) - 1;
            int LD5 = (text.Length - space4) - 1;

            string ID = text.Substring(0, space1);
            Console.WriteLine(ID);
            Id = ID;
            string Steam = text.Substring(space1 + 1, LD2);
            Console.WriteLine(Steam);
            SteamId = Steam;
            string Name = text.Substring(space2 + 1, LD3);
            Console.WriteLine(Name);
            NamePlayer = Name;
            string IP = text.Substring(space3 + 1, LD4);
            Console.WriteLine(IP);
            Ip = IP;
            string Ping = text.Substring(space4 + 1, LD5);
            Console.WriteLine(Ping);
            PingPlayer = Ping;
            
        }

       

        private void BrowserFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Find and Select Fivem Server Folder";
            folderBrowserDialog1.ShowDialog();
            string FivemFolder = folderBrowserDialog1.SelectedPath;

            if (File.Exists(FivemFolder + "\\run.cmd"))
            {
                BoxPathServer.Text = FivemFolder;
                LocationFivemServer = FivemFolder;
                Thread.Sleep(100);
                ReadConfig();
                MoveValueLTT();
                MessageBox.Show("Fivem Server Detected", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Fivem Server Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Console.WriteLine(checkedListBox1.SelectedItem);
            var item = checkedListBox1.SelectedItem;
            string Rsource = item.ToString();
            if (isrunning && selecteditem != -1)
            {
                StreamWriter Send = proc.StandardInput;
                Send.WriteLine("start " + Rsource);
            }
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = checkedListBox1.SelectedItem;
            string Rsource = item.ToString();
            if (isrunning && selecteditem != -1)
            {
                StreamWriter Send = proc.StandardInput;
                Send.WriteLine("stop " + Rsource);
            }
        }

        private void RestartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = checkedListBox1.SelectedItem;
            string Rsource = item.ToString();
            if (isrunning && selecteditem != -1)
            {
                StreamWriter Send = proc.StandardInput;
                Send.WriteLine("restart " + Rsource);
            }
        }

        

        private void KickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("what is the reason you kicked him from the server?", "Reason Kick", "", 0, 0);
            int  indexRow = dataGridView1.CurrentCell.RowIndex;
            string DataCell = dataGridView1.Rows[indexRow].Cells[0].FormattedValue.ToString();
            //Console.WriteLine(DataCell);
            //Console.WriteLine(input);
            if (isrunning && input != "")
            {
                Console.WriteLine("Send kick");
                StreamWriter Send = proc.StandardInput;
                Send.WriteLine("clientkick " + DataCell + " " + input);
                Send.WriteLine("status");
            }
            
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (isrunning && ServerStarted)
            {
                
                using (WebClient client = new WebClient())
                {

                    string DataNewVer = client.DownloadString("https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/");
                    int b = DataNewVer.IndexOf("</tr><tr>");
                    b = b + 22;
                    string NewV = DataNewVer.Substring(b, 4);

                    int Lip = Tcp.Length;
                    int Iip = Tcp.IndexOf(":");
                    string ip = Tcp.Substring(0, Iip);
                    string port = Tcp.Substring(Iip + 1, Lip - (Iip + 1));
                    
                    string DataLocalVer = "";
                    if (ip == "0.0.0.0")
                    {
                        DataLocalVer = client.DownloadString("http://127.0.0.1:" + port + "/info.json");
                    }
                    else
                    {
                        DataLocalVer = client.DownloadString("http://" + ip + ":" + port + "/info.json");
                    }
                    int c = DataLocalVer.IndexOf("SERVER v1");
                    c = c + 14;
                    string LocalVer = DataLocalVer.Substring(c, 4);

                    SetText("new version" + NewV);
                    SetText("Installed version" + LocalVer);
                    if (NewV != LocalVer)
                    {
                        DialogResult result = MessageBox.Show("New Version of Fivem Server Windows is Available. Do you want to download now?", "New Version",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (result == DialogResult.Yes)
                        {
                            Process.Start("https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Your Fivem Server is the latest version", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Start Server to check Server Version", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
    }
}
