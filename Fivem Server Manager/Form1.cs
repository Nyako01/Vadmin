using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;



namespace Fivem_Server_Manager
{
    public partial class Form1 : Form
    {
        string FileConfigServer = "server.cfg";
        string FileConfigApp = "Config.cfg";
        string ServerName;
        string License;       
        string Tcp;
        string Udp;
        String MaxClient;
        string IconFile;
        String DatabaseServer;
        string DatabaseName;
        string DatabaseUser;
        string DatabasePass;

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

        bool isrunning = false;
        int selecteditem = -1;
       
        public Form1()
        {
            InitializeComponent();
            ReadConfig();
            CheckProcess();

            Thread T = new Thread(Time);
            T.Start();

            Thread SC = new Thread(Schedule);
            SC.Start();
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
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

        void CheckProcess()
        {
            isrunning = IsProcessOpen("FXServer");
            if (isrunning)
            {
                StatusText("START");
                
            }
            else {
                StatusText("STOP");
                
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
                MessageBox.Show(startTag + " or " + endTag + " Tag not found inside " + FileConfigServer);
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
            int indexDB = GetIndex(FileConfigServer, "set mysql_connection_string");
            String DataDB = GetLine(FileConfigServer, indexDB);
            int lenghDataDB = DataDB.Length;
            int indexServer = DataDB.IndexOf("server");
            int indexDatabase = DataDB.IndexOf("database");
            int indexUsername = DataDB.IndexOf("userid");
            int indexPassword = DataDB.IndexOf("password");
            int SubLenghtDBS = (indexDatabase - indexServer) - 8;
            int SubLenghtDBN = (indexUsername - indexDatabase) - 10 ;
            int SubLenghtDBU = (indexPassword - indexUsername) - 8;
            int SubLenghtDBP = (lenghDataDB - indexPassword) - 11;

            DatabaseServer = DataDB.Substring(indexServer + 7, SubLenghtDBS);
            DatabaseName = DataDB.Substring(indexDatabase + 9, SubLenghtDBN);
            DatabaseUser = DataDB.Substring(indexUsername + 7, SubLenghtDBU);
            DatabasePass = DataDB.Substring(indexPassword + 9, SubLenghtDBP);

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

            DbPass.Text = DatabasePass;
            DbUser.Text = DatabaseUser;
            DbName.Text = DatabaseName;
            DbServer.Text = DatabaseServer;
            ICON.Text = IconFile;
            Key.Text = License;
            TCP.Text = Tcp;
            UDP.Text = Udp;
            MAXC.Text = MaxClient;
            Server_Name.Text = ServerName;
            Hour1.Text = SchHour1;
            Minute1.Text = SchMinute1;
            Hour2.Text = SchHour2;
            Minute2.Text = SchMinute2;
            Hour3.Text = SchHour3;
            Minute3.Text = SchMinute3;
        }

        void start()
        {
            var proc = new Process();
            proc.StartInfo.FileName = "start_Server.bat";
            proc.StartInfo.UseShellExecute = false;
            // set up output redirection
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.EnableRaisingEvents = true;
            proc.StartInfo.CreateNoWindow = true;
            // see below for output handler
            proc.ErrorDataReceived += proc_DataReceived;
            proc.OutputDataReceived += proc_DataReceived;

            proc.Start();

            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            proc.WaitForExit();
        }

        void proc_DataReceived(object sender, DataReceivedEventArgs e)
        {
            // output will be in string e.Data
            SetText(e.Data);
            //Console.WriteLine(e.Data);
        }

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
                //this.LTIME.Update();
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
                if (IsProcessOpen("FXServer"))
                {
                    Status_Server.ForeColor = Color.Green;
                    Stop.Enabled = true;
                    apply.Enabled = false;
                    change.Enabled = false;
                }
                else
                {
                    Status_Server.ForeColor = Color.Red;
                    Start.Enabled = true;
                    Stop.Enabled = false;
                    apply.Enabled = false;
                    change.Enabled = true;
                }
            }
        }

        void OverWrite(string file, string FindText, string TextForEdit)
        {
            int indexLine = GetIndex(file, FindText);
            lineChanger(TextForEdit, file, indexLine);
            
        }


        private void Button3_Click(object sender, EventArgs e)
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

            apply.Enabled = true;
            change.Enabled = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
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
            MessageBox.Show("Configuration Saved");
            apply.Enabled = false;
            change.Enabled = true;
        }

        void AutoRestart()
        {          
                StopProc("FXServer");
                MessageBox.Show("Restarting");
            CheckProcess();
             Thread.Sleep(1000);
            Thread StartServer = new Thread(start);
            StartServer.Start();
            MessageBox.Show("Restart Success");
            CheckProcess();
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
                    MessageBox.Show("Server Restart On Schedule 1");
                    Schedule1RUN = false;
                }
                if (Hour != Hour1.Text || Minute != Minute1.Text)
                {
                    Schedule1RUN = true;
                }

                if (Hour == Hour2.Text && Minute == Minute2.Text && Schedule2 == true && Schedule2RUN == true)
                {
                    AutoRestart();
                    MessageBox.Show("Server Restart On Schedule 2");
                    Schedule2RUN = false;
                }
                if (Hour != Hour2.Text || Minute != Minute2.Text)
                {
                    Schedule2RUN = true;
                }

                if (Hour == Hour3.Text && Minute == Minute3.Text && Schedule3 == true && Schedule3RUN == true)
                {
                    AutoRestart();
                    MessageBox.Show("Server Restart On Schedule 3");
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
            for (int i = 0; i < 10; i++)
            {
                DateTime LocalTime = DateTime.Now;
                 Hour = LocalTime.Hour.ToString();
                 Minute = LocalTime.Minute.ToString();
                 Second = LocalTime.Second.ToString();
                printTime(Hour + ":" + Minute + ":" + Second);
                Thread.Sleep(1000);
                
                if (i == 9) { i = 0; }
            }
           
        }

        

        private void Label4_Click(object sender, EventArgs e)
        {

        }


        private void Button1_Click_1(object sender, EventArgs e)

        {
            if (File.Exists("start_server.bat"))
            {

                Thread StartServer = new Thread(start);
                StartServer.Start();
                textBox1.Text = "";
                Start.Enabled = false;
                Thread.Sleep(1500);
                CheckProcess();
                if (!isrunning)
                {
                 MessageBox.Show("Server Failed to Start"); }
            }
            else { MessageBox.Show("start_server.bat not found. make sure file in same directory"); }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            StopProc("FXServer");
            Thread.Sleep(500);
            CheckProcess();
            
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

        private void EditSch1_Click(object sender, EventArgs e)
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
            lineChanger("Schedule1_H = " + Hour1.Text, "Config.cfg", 2);
            lineChanger("Schedule1_M = " + Minute1.Text, "Config.cfg", 3);
            SchHour1 = Hour1.Text;
            SchMinute1 = Minute1.Text;
            MessageBox.Show("Schedule 1 Saved");
        }

        private void ApplySch2_Click_1(object sender, EventArgs e)
        {
            Hour2.Enabled = false;
            Minute2.Enabled = false;
            ApplySch2.Enabled = false;
            EditSch2.Enabled = true;
            lineChanger("Schedule2_H = " + Hour2.Text, "Config.cfg", 4);
            lineChanger("Schedule2_M = " + Minute2.Text, "Config.cfg", 5);
            SchHour2 = Hour2.Text;
            SchMinute2 = Minute2.Text;
            MessageBox.Show("Schedule 2 Saved");
        }

        private void ApplySch3_Click_1(object sender, EventArgs e)
        {
            Hour3.Enabled = false;
            Minute3.Enabled = false;
            ApplySch3.Enabled = false;
            EditSch3.Enabled = true;
            lineChanger("Schedule3_H = " + Hour3.Text, "Config.cfg", 6);
            lineChanger("Schedule3_M = " + Minute3.Text, "Config.cfg", 7);
            SchHour3 = Hour3.Text;
            SchMinute3 = Minute3.Text;
            MessageBox.Show("Schedule 3 Saved");
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

        public void CreateEntry(string file,string Tag, string textBeforeTag) //npcName = "item1"
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
                MessageBox.Show("please select a category");

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
                string  ResourceSelect = selectitem.ToString();
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

        private void Search_Click(object sender, EventArgs e)
        {
            
            checkedListBox1.ClearSelected();
            string s = SearchText.Text;
            int h = checkedListBox1.FindString(s);
            if (h == -1)
            {
                checkedListBox1.ClearSelected();
                MessageBox.Show("Resources Not Found");
            }
            else
            {
                checkedListBox1.SetSelected(h, true);
            }
           // var selectitem = checkedListBox1.SelectedItem;
           // ResourceSelected = selectitem.ToString();
           // Console.WriteLine(ResourceSelected);
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
    }
}
