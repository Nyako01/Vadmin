using Microsoft.Owin.Hosting;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Owin;
using Server_Manager_for_Fivem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using SystemPerformance;
using Vadmin;

namespace Fivem_Server_Manager
{
    public partial class Form1 : Form
    {

        string appversion = "1.6";

        string FileConfigServer1, FileConfigServer2;
        public static string SelectFileConfigServer = "";
        string FileConfigApp = "Config.cfg";
        public static string LocationFivemServer = "";
        public static string Theme = "";
        string ServerName, License, Tcp, Udp, MaxClient, IconFile, DatabaseServer, DatabaseName, DatabaseUser, DatabasePass, WebRip, WebApi = "";
        string[] cntxt, Pid, Pname, Pip, Pping, Pbname, Pbsteam, Pbexp, Pbpermanent, Pbreason;

        public string PlayerListData = "{0, -5}{1, -50}{2, -40}{3, 0}";
        public string Id, SteamId, NamePlayer, Ip, PingPlayer;
        int playerDetected1 = 0;
        int playerDetected2 = 0;
        int intervalCheckPlayer = 5;

        public static string SelectedResource, SelectedId, CategoryResource, Rstat1, Rstat2;
        string Hour, Minute, Second;
        public string SchHour1, SchMinute1, ExcCmdMnt1 = "", ExcCmd1 = "", SchHour2, SchMinute2, ExcCmdMnt2 = "", ExcCmd2 = "", SchHour3, SchMinute3, ExcCmdMnt3 = "", ExcCmd3 = "";
        int ClearCacheCfg, OSync, Servermode;
        bool Schedule1 = false;
        bool Schedule2 = false;
        bool Schedule3 = false;
        bool AutoClearCache = false;
        bool OneSync = false;

        bool Schedule1RUN = true;
        bool Schedule2RUN = true;
        bool Schedule3RUN = true;
        bool CmdTrig1 = true;
        bool CmdTrig2 = true;
        bool CmdTrig3 = true;
        int CheckStatusTime = 0;
        int Change_ApplyMode = 0;
        int Start_StopMode = 0;
        bool ServerStarted = false;
        // bool ServerStoped = false;
        bool isrunning = false;
        //int selecteditem = -1;

        bool checkinternet()
        {
            Ping pinger = new Ping();
            PingReply checknet = pinger.Send("8.8.8.8");
            return checknet.Status == IPStatus.Success;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public Form1()
        {
            InitializeComponent();

            File.Delete("Debug.txt");
            //ServerMode.Text = "Test Server";

            PlayerList.Columns.Add("ID", 40);
            PlayerList.Columns.Add("NAME", 150);
            PlayerList.Columns.Add("IP", 100);
            PlayerList.Columns.Add("PING", 70);

            ResourceList.Columns.Add("Resource Name", 200);
            ResourceList.Columns.Add("Category", 100);
            ResourceList.Columns.Add("Enable/Disable", 100);
            ResourceList.Columns.Add("Start/Stop", 100);

            Console.WriteLine(WebApi.Length);

            WebApi = GetData(FileConfigApp, "WebApi", 9, false);
            WebRip = GetData(FileConfigApp, "WebRemote", 11, false);
            WebIP.Text = WebRip;

            if (WebRip.Length > 15)
            {
                WebHost(WebRip);
            }
            Tips();
            ReadConfig();
            ReadBanlistDB();
            // WebIP.Text = "http://192.168.100.126:44811";
            //if (WebIP.TextLength > 10)
            //{
            //    sendData(WebIP.Text);
            //}

            Thread Servermodeselect = new Thread(Monitoring);
            Servermodeselect.Start();



            if (IsProcessOpen("FXServer"))
            {
                StopProc("FXServer");
            }
            //SaveConfigProgram();


        }

        void sendData(string url)
        {
            if (url.Length > 15)
            {
                string oneS_EN;
                if (OneSync)
                {
                    oneS_EN = "ENABLE";
                }
                else
                {
                    oneS_EN = "DISABLE";
                }

                string mapdata = "";
                if (ServerStarted)
                {
                    WebClient client = new WebClient();
                    string infodata = client.DownloadString("http://" + Tcp + "/info.json");
                    Console.WriteLine(infodata);
                    int mapindex = infodata.IndexOf("fivem-map");
                    int endmapindex = infodata.IndexOf(",", mapindex);
                    Console.WriteLine(mapindex);
                    Console.WriteLine(endmapindex);
                    mapdata = infodata.Substring(mapindex, (endmapindex - mapindex) - 1);
                    Console.WriteLine(mapdata);
                }
                string md = "";
                if (Servermode == 0)
                {
                    md = "Public Server";
                }
                else
                {
                    md = "Test Server";
                }

                var stat3 = new
                {
                    ID = 3,
                    Ip = Tcp,
                    ServerName = ServerName,
                    PlayerMAX = MaxClient,
                    PlayerON = playerDetected1.ToString(),
                    Onesync = oneS_EN,
                    Map = mapdata,
                    ServerMode = md
                };

                string stat3_data = JsonConvert.SerializeObject(stat3);
                PostData(url + "/api/Dashboard", stat3_data);
            }
        }
        bool st = false;
        void WebStatus(string url)
        {
            if (url.Length > 15)
            {
                WebClient client = new WebClient();
                string WebStat1 = client.DownloadString(url + "/api/Dashboard/1");
                string WebStat2 = client.DownloadString(url + "/api/Dashboard/2");
                //Console.WriteLine(WebStat);
                int stat1 = WebStat1.IndexOf("Start");
                int stat2 = WebStat2.IndexOf("Start");

                if (stat1 > 0 /*&& stat2 == -1*/ && !isrunning && st == false)
                {
                    Console.WriteLine("server started");
                    //st = true;
                    Button1_Click_1(new object(), new EventArgs());

                }

                if (stat1 == -1 /*&& stat2 > 0*/ && isrunning && st == true)
                {
                    Console.WriteLine("server stop");
                    //st = false;
                    Button1_Click_1(new object(), new EventArgs());

                }

            }

            var ConsoleText = new
            {
                ID = 1,
                ConsoleIN = cntxt
            };
            //Console.WriteLine(stat3);
            string ConsoleText_Data = JsonConvert.SerializeObject(ConsoleText);
            PostData(url + "/api/Console", ConsoleText_Data);

            if (playerDetected1 > 0)
            {
                var Playerdata = new
                {
                    ID = 2,
                    PlayerID = Pid,
                    PlayerName = Pname,
                    PlayerIP = Pip,
                    PlayerPing = Pping
                };
                //Console.WriteLine(Playerdata + "\n");
                string Player_data = JsonConvert.SerializeObject(Playerdata);
                PostData(url + "/api/Player", Player_data);
            }

        }

        string InputConsoleWeb;
        void WebData()
        {

            WebClient client = new WebClient();

            string ConsoleWeb = client.DownloadString(WebApi + "api/Console/1");
            //Console.WriteLine(ConsoleWeb);
            if (ConsoleWeb.Length > 3 && ConsoleWeb != "null")
            {
                Console.WriteLine("Get data");
                int a = ConsoleWeb.LastIndexOf("\"");
                Console.WriteLine(a);
                InputConsoleWeb = ConsoleWeb.Substring(1, a - 1);
                Console.WriteLine("saved " + InputConsoleWeb);
                var ConsoleText = new
                {
                    ID = 2,
                    ConsoleOUT = ""
                };
                //Console.WriteLine(stat3);
                string ConsoleText_Data = JsonConvert.SerializeObject(ConsoleText);
                PostData(WebApi + "/api/Console", ConsoleText_Data);
                Console.WriteLine("Source data Cleared");

                if (ServerStarted && InputConsoleWeb.Length > 3)
                {
                    StreamWriter Send = proc.StandardInput;
                    Send.WriteLine(InputConsoleWeb);

                    InputConsoleWeb = "";
                }
            }

            string PlayerManager = client.DownloadString(WebApi + "api/Player/");
            //Console.WriteLine(PlayerManager);

            if (PlayerManager.Length > 4 && PlayerManager != "null")
            {
                Console.WriteLine("Get data");
                int a = PlayerManager.LastIndexOf("\"");
                Console.WriteLine(a);
                string action = PlayerManager.Substring(1, a - 1);
                Console.WriteLine(action);

                var Paction = new
                {
                    ID = 1,
                    PlayerAction = ""
                };
                //Console.WriteLine(stat3);
                string Paction_Data = JsonConvert.SerializeObject(Paction);
                PostData(WebApi + "/api/Player", Paction_Data);
                Console.WriteLine("Source data Cleared");

                if (action == "Kick all")
                {
                    kickAllPlayer();
                }


                int UnBanAction = action.IndexOf("UB");
                if (UnBanAction != -1)
                {
                    int indexname = action.IndexOf("PlayerName");
                    Console.WriteLine(indexname);
                    Console.WriteLine(action.Length);
                    int lenghtname = action.Length - (indexname + 11);
                    Console.WriteLine(lenghtname);
                    string BanPlayerName = action.Substring(indexname + 11, lenghtname);
                    Console.WriteLine(BanPlayerName);
                    DeleteDB("user_banlist", "Name", BanPlayerName);
                    ReadBanlistDB();
                }

                int BanAction = action.IndexOf("Ban");
               
                if (BanAction != -1)
                {
                    //Console.WriteLine(action);
                    int indexname = action.IndexOf("PlayerName");
                    Console.WriteLine(indexname);
                    int indexreason = action.IndexOf("Reason");
                    Console.WriteLine(indexreason);
                    int indexexp = action.IndexOf("Exp");
                    Console.WriteLine(indexexp);
                    int lenghtname = (indexreason - 1) - (indexname + 11);
                    int lenghtreason = (action.Length - (indexreason + 7));

                    string BanPlayerName = action.Substring(indexname + 11, lenghtname);
                    string BanReason = action.Substring(indexreason + 7, lenghtreason);


                    Console.WriteLine(BanPlayerName);
                    Console.WriteLine(BanReason);

                    string Sid = ReadDB("users", "name", BanPlayerName, "identifier");
                    Console.WriteLine(Sid);
                    string banlistcolumn = "Name, SteamID, ExpiredDate, Permanent, Reason, Banned";
                    string banlistcolumnValue = "'" + BanPlayerName + "','" + Sid + "','Permanent','1','" + BanReason + "','1'";
                    InsertDB("user_banlist", banlistcolumn, banlistcolumnValue);
                    foreach (string h in Pname)
                    {
                        Console.WriteLine(h + ",");
                    }
                    Console.WriteLine(BanPlayerName + ",");
                    int i = Array.IndexOf(Pname, BanPlayerName + " ");
                    Console.WriteLine(i);
                    string idplayer = Pid[i];
                    Console.WriteLine(idplayer);
                    if (ServerStarted && i > -1)
                    {
                        StreamWriter Send = proc.StandardInput;
                        Send.WriteLine("clientkick " + idplayer + " You has been Banned Permanently because " + BanReason + "." +
                            " Contact Administrators to request Unban");
                    }
                    ReadBanlistDB();
                }
                
            }

        }

        public static List<string> Playerbanname = new List<string>();
        public static List<string> Playerbansteamid = new List<string>();
        public static List<string> Playerbanexp = new List<string>();
        public static List<string> Playerbanpermanent = new List<string>();
        public static List<string> Playerbanreason = new List<string>();

        void ReadBanlistDB()
        {
            MySqlConnection db = new MySqlConnection();

            string DBdata = "SERVER=" + DatabaseServer + ";" + "DATABASE=" +
            DatabaseName + ";" + "UID=" + DatabaseUser + ";" + "PASSWORD=" + DatabasePass + ";";
            Console.WriteLine(DBdata);
            db.ConnectionString = DBdata;
            try
            {

                db.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM user_banlist", db);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    Playerbanname.Clear();
                    Playerbansteamid.Clear();
                    Playerbanexp.Clear();
                    Playerbanpermanent.Clear();
                    Playerbanreason.Clear();

                    while (reader.Read())
                    {
                        // access your record colums by using reader
                        Console.WriteLine(reader["Name"]);
                        Console.WriteLine(reader["SteamID"]);
                        Console.WriteLine(reader["ExpiredDate"]);
                        Console.WriteLine(reader["Permanent"]);
                        Console.WriteLine(reader["Reason"]);
                        Playerbanname.Add(reader["Name"].ToString());
                        Playerbansteamid.Add(reader["SteamID"].ToString());
                        Playerbanexp.Add(reader["ExpiredDate"].ToString());
                        Playerbanpermanent.Add(reader["Permanent"].ToString());
                        Playerbanreason.Add(reader["Reason"].ToString());
                    }
                    Console.WriteLine("finish Reading");
                    Pbname = Playerbanname.ToArray();
                    Pbsteam = Playerbansteamid.ToArray();
                    Pbexp = Playerbanexp.ToArray();
                    Pbpermanent = Playerbanpermanent.ToArray();
                    Pbreason = Playerbanreason.ToArray();

                    if (WebApi.Length > 15)
                    {
                        var Playerdata = new
                        {
                            ID = 3,
                            BanPlayerName = Pbname,
                            BanPlayerExp = Pbexp,
                            BanPlayerReason = Pbreason
                        };
                        //Console.WriteLine(Playerdata + "\n");
                        string Player_data = JsonConvert.SerializeObject(Playerdata);
                        PostData(WebApi + "/api/Player", Player_data);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.Close();
            }
        }

        string ReadDB(string location, string find, string findvalue, string selectData)
        {
            MySqlConnection db = new MySqlConnection();
            string re = "";
            string DBdata = "SERVER=" + DatabaseServer + ";" + "DATABASE=" +
            DatabaseName + ";" + "UID=" + DatabaseUser + ";" + "PASSWORD=" + DatabasePass + ";";
            db.ConnectionString = DBdata;
            try
            {
                db.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM " + location + " WHERE " + find + "='" + findvalue + "';", db);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    var getdata = reader[selectData];
                    re = getdata.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return re;
        }

        void InsertDB(string Location, string Column, string ColumnValue)
        {
            MySqlConnection db = new MySqlConnection();

            string DBdata = "SERVER=" + DatabaseServer + ";" + "DATABASE=" +
            DatabaseName + ";" + "UID=" + DatabaseUser + ";" + "PASSWORD=" + DatabasePass + ";";
            db.ConnectionString = DBdata;
            try
            {
                db.Open();
                string Query = "INSERT INTO " + Location + "(" + Column + ") values(" + ColumnValue + ");";

                MySqlCommand command = new MySqlCommand(Query, db);
                MySqlDataReader reader;
                reader = command.ExecuteReader();
                while (reader.Read())
                {

                }
                Console.WriteLine("Data saved");
                db.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void DeleteDB(string location, string find, string findvalue)
        {
            MySqlConnection db = new MySqlConnection();

            string DBdata = "SERVER=" + DatabaseServer + ";" + "DATABASE=" +
            DatabaseName + ";" + "UID=" + DatabaseUser + ";" + "PASSWORD=" + DatabasePass + ";";
            db.ConnectionString = DBdata;
            try
            {
                db.Open();
                string Query = "DELETE FROM " + location + " WHERE " + find + "='" + findvalue + "';";

                MySqlCommand command = new MySqlCommand(Query, db);
                MySqlDataReader reader;
                reader = command.ExecuteReader();
                while (reader.Read())
                {

                }
                Console.WriteLine("Data Deleted");
                db.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        void WebHost(string url)
        {
            try
            {
                //var url = "http://192.168.100.126:44811";
                WebApp.Start(url, builder => builder.UseFileServer(enableDirectoryBrowsing: true));


                WebApp.Start<WebAPI>(url: WebApi);
                Console.WriteLine("Server is up and running");

                Console.WriteLine("Listening at " + url + " and " + WebApi);
            }
            catch (Exception w)
            {
                MessageBox.Show(w.ToString());
            }
        }

        void Monitoring()
        {
            if (WebApi.Length > 15)
            {
                try
                {
                    PerformanceCounter diskPerformanceCounter = new PerformanceCounter();


                    diskPerformanceCounter.CategoryName = "PhysicalDisk";
                    diskPerformanceCounter.CounterName = "% Idle Time";
                    diskPerformanceCounter.InstanceName = "_Total";

                    //string a = "Realtek PCIe Gbe Family Controller";





                    var thisDevice = new PerformanceTracker();
                    thisDevice.Start();
                    while (true)
                    {

                        float currentDisk = diskPerformanceCounter.NextValue();

                        currentDisk = 100 - currentDisk;

                        float cpu = thisDevice.Current_CPU_Usage; //returns a floating number denoting CPU usage of the device on which the assembly is being executed
                        float ram = thisDevice.Percent_RAM_Used; //returns a floating number to denote the percentage of RAM being used on the device on which the assembly is being executed	

                        int CpuPercent = (int)cpu;
                        int RamPercent = (int)ram;
                        int DiskPercent = (int)currentDisk;



                        // Console.WriteLine(CpuPercent + "%  " + RamPercent + "%  " + DiskPercent + "%  " + NetPercent + "kb/s");

                        var stat3 = new
                        {
                            ID = 5,
                            CPU = CpuPercent.ToString(),
                            RAM = RamPercent.ToString(),
                            DISK = DiskPercent.ToString(),

                        };

                        string stat3_data = JsonConvert.SerializeObject(stat3);
                        PostData(WebApi + "/api/Dashboard", stat3_data);

                        Thread.Sleep(1000);
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        void Tips()
        {

            ToolTip tips = new ToolTip();
            tips.AutoPopDelay = 5000;
            tips.SetToolTip(Kall, "Kick All Online Player");
            tips.SetToolTip(ServerMode, "Select Different Configuration For Start Server for Public or for Test a Resource");
            tips.SetToolTip(OSyc, "Option to Enable Advanced Feature of Fivem Server");
            tips.SetToolTip(ClearCache, "Clear/Delete cache Folder");
            tips.SetToolTip(UpdateSoft, "Download Latest Version of Server Manager for Fivem");
            tips.SetToolTip(UpdateServ, "Visit Download Page Fivem Server");

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
                    if (WebApi.Length > 15)
                    {
                        var stat3 = new
                        {
                            ID = 5,
                            CPU = "0",
                            RAM = "0",
                            DISK = "0",
                            NET = "0"
                        };

                        string stat3_data = JsonConvert.SerializeObject(stat3);
                        PostData(WebApi + "/api/Dashboard", stat3_data);
                    }
                    if (IsProcessOpen("FXServer"))
                    {
                        StopProc("FXServer");
                    }
                    StopProc("Vadmin");

                }
                else if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

        }

        DialogResult result;
        bool ShowMessage = false;
        string NewProgram;
        void AutoCheckUpdate(object source, ElapsedEventArgs e)
        {
            WebClient client = new WebClient();

            if (!checkinternet() && ShowNotifyUp.Checked)
            {
                AutoClosingMessageBox.Show("Failed to Check New Version. Please Check Your Internet Connection", "Error", 2000);
                //MessageBox.Show("Failed to Check New Version. Please Check Your Internet Connection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    //WriteToFileThreadSafe(">Checking Version", "Debug.txt");

                    string s = client.DownloadString("https://raw.githubusercontent.com/Oky12/Vadmin/master/Version");
                    // Console.WriteLine(s);
                    NewProgram = s;
                    int a = s.IndexOf(appversion);

                    //WriteToFileThreadSafe(">New Version " + s + ">Installed Version " + appversion, "Debug.txt");
                    InstalledSoft.BeginInvoke(new MethodInvoker(() => InstalledSoft.Text = "Installed: V" + appversion));
                    NewSoft.BeginInvoke(new MethodInvoker(() => NewSoft.Text = "New: V" + s));

                    if (a == -1)
                    {
                        if (!ShowMessage && ShowNotifyUp.Checked)
                        {
                            ShowMessage = true;
                            result = MessageBox.Show("New Version of Server Manager for Fivem is Available. Click YES to Visit Download Page or Click NO to Hide This Message and Enable Update Button", "Check for Update",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        }
                        if (result == DialogResult.Yes)
                        {
                            result = DialogResult.OK;
                            //ShowMessage = false;
                            WriteToFileThreadSafe(">Open Browser to Download Page" + a, "Debug.txt");

                            Process.Start("https://github.com/Oky12/Vadmin/releases");
                            //StopProc("Fivem Server Manager");
                        }
                        else
                        {
                            ShowMessage = false;
                            UpdateSoft.BeginInvoke(new MethodInvoker(() => UpdateSoft.Enabled = true));
                            ShowNotifyUp.BeginInvoke(new MethodInvoker(() => ShowNotifyUp.Checked = false));

                        }
                    }

                    if (isrunning && ServerStarted)
                    {
                        //WriteToFileThreadSafe(">Checking Version", "Debug.txt");                   
                        int Lip = Tcp.Length;
                        int Iip = Tcp.IndexOf(":");
                        string ip = Tcp.Substring(0, Iip);
                        string port = Tcp.Substring(Iip + 1, Lip - (Iip + 1));
                        WriteToFileThreadSafe(">Get Installed Version from" + ip + port, "Debug.txt");
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
                        // WriteToFileThreadSafe(">Get Version " + LocalVer, "Debug.txt");
                        InstalledServ.BeginInvoke(new MethodInvoker(() => InstalledServ.Text = "Installed: V" + LocalVer));


                    }
                }
                catch (Exception)
                {

                    //AutoClosingMessageBox.Show("request Latest Version data timed out", "Error", 3000);
                }
            }
        }

        void addRowPlayerList(string id, string name, string ip, string ping)
        {
            string[] Row = { id, name, ip, ping };
            ListViewItem item = new ListViewItem(Row);
            PlayerList.BeginInvoke(new MethodInvoker(() => PlayerList.Items.Add(item)));

        }

        void addRowResourceList(string ResourceName, string Category, string OnOff, string StartStop)
        {
            string[] Row = { ResourceName, Category, OnOff, StartStop };
            ListViewItem item = new ListViewItem(Row);
            ResourceList.Items.Add(item);

        }

        private void CheckProcess(object source, ElapsedEventArgs e)
        {

            //Console.WriteLine(ServerStarted);
            isrunning = IsProcessOpen("FXServer");
            if (isrunning && ServerStarted)
            {

                StatusText("STARTED");
                //button1.Enabled = false;
                //BrowserFolder.Enabled = false;
                
                
            }
            if (isrunning && !ServerStarted)
            {
                StatusText("STARTING");
            }
            if (!isrunning)
            {

                StatusText("STOP");
                //button1.Enabled = true;
                //BrowserFolder.Enabled = true;
                
                
            }

            if (!isrunning && ServerStarted)
            {
                WriteToFileThreadSafe(">Server Error or Crash", "Debug.txt");
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
            string SubData = "";
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
            var index = 0;
            if (File.Exists(file))
            {
                string[] c = File.ReadAllLines(file);
                index = Array.FindIndex(c, row => row.Contains(text)) + 1;

            }
            return index;

        }

        void ScanResource(string file, string startTag, string endTag)
        {

            int StartLineResource = GetIndex(file, startTag);
            int EndLineResource = GetIndex(file, endTag);
            if (StartLineResource == 0 || EndLineResource == 0)
            {
                MessageBox.Show(startTag + " or " + endTag + " Tag not found inside " + SelectFileConfigServer, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        string Categori = startTag.Trim('#');
                        if (StatusResource == "start ")
                        {
                            int ResourcesName = L - 6;
                            string RName = StartLineData.Substring(6, ResourcesName);
                            addRowResourceList(RName, Categori, "Enable", "Start");

                        }
                        else if (StatusResource == "#start")
                        {
                            int ResourcesName = L - 7;
                            string RName = StartLineData.Substring(7, ResourcesName);
                            addRowResourceList(RName, Categori, "Disable", "Stop");
                        }

                    }

                }
            }
        }


        public void ReadConfig()
        {


            if (File.Exists(FileConfigApp))
            {
                WriteToFileThreadSafe(">" + FileConfigApp + " Detected", "Debug.txt");
                LocationFivemServer = GetData(FileConfigApp, "PathFivemServer", 18, false);
                BoxPathServer.Text = LocationFivemServer;
                FileConfigServer1 = GetData(FileConfigApp, "PublicFivemConfig", 20, false);
                PublicServerFile.Text = FileConfigServer1;
                FileConfigServer2 = GetData(FileConfigApp, "TestFivemConfig", 18, false);
                TestServerFile.Text = FileConfigServer2;

                //WriteToFileThreadSafe(">Read Clear Cache option", "Debug.txt");
                string ClearCacheRestart = GetData(FileConfigApp, "ClearCacheWhenRestart", 24, false);
                if (ClearCacheRestart != "")
                {
                    ClearCacheCfg = Int32.Parse(ClearCacheRestart);
                }
                //WriteToFileThreadSafe(">Read OneSync option", "Debug.txt");
                string OnSy = GetData(FileConfigApp, "OneSync", 10, false);
                if (OnSy != "")
                {
                    OSync = Int32.Parse(OnSy);
                }
                //WriteToFileThreadSafe(">Read Schedule Restart 1", "Debug.txt");
                SchHour1 = GetData(FileConfigApp, "Schedule1_H", 14, false);
                SchMinute1 = GetData(FileConfigApp, "Schedule1_M", 14, false);
                ExcCmdMnt1 = GetData(FileConfigApp, "ExcCommMnt1", 14, false);
                ExcCmd1 = GetData(FileConfigApp, "EscCmd1", 10, false);
                //WriteToFileThreadSafe(">Read Schedule Restart 2", "Debug.txt");
                SchHour2 = GetData(FileConfigApp, "Schedule2_H", 14, false);
                SchMinute2 = GetData(FileConfigApp, "Schedule2_M", 14, false);
                ExcCmdMnt2 = GetData(FileConfigApp, "ExcCommMnt2", 14, false);
                ExcCmd2 = GetData(FileConfigApp, "EscCmd2", 10, false);
                //WriteToFileThreadSafe(">Read Schedule Restart 3", "Debug.txt");
                SchHour3 = GetData(FileConfigApp, "Schedule3_H", 14, false);
                SchMinute3 = GetData(FileConfigApp, "Schedule3_M", 14, false);
                ExcCmdMnt3 = GetData(FileConfigApp, "ExcCommMnt3", 14, false);
                ExcCmd3 = GetData(FileConfigApp, "EscCmd3", 10, false);


                string SM = GetData(FileConfigApp, "ServerMode", 13, false);
                Console.WriteLine(SM);
                if (SM == "0")
                {
                    ServerMode.Text = "Public Server";
                    SelectFileConfigServer = FileConfigServer1;
                    Servermode = 0;
                    //SaveConfigProgram();
                }
                if (SM == "1")
                {
                    ServerMode.Text = "Test Server";
                    SelectFileConfigServer = FileConfigServer2;
                    Servermode = 1;
                    //SaveConfigProgram();
                }

                if (OnSy == "1")
                {
                    OSyc.Checked = true;
                }
                else
                {
                    OSyc.Checked = false;

                }
                if (ClearCacheRestart == "1")
                {

                    optClearCache.Checked = true;
                }
                else
                {

                    optClearCache.Checked = false;
                }

                Hour1.Text = SchHour1;
                Minute1.Text = SchMinute1;
                if (ExcCmd1.Length != 0 && ExcCmdMnt1.Length != 0)
                {
                    Note1.Text = "Execute \"" + ExcCmd1 + "\" Command in " + ExcCmdMnt1 + " Minute Before Restart";
                }
                else
                {
                    Note1.Text = "(Empty)";
                }
                Hour2.Text = SchHour2;
                Minute2.Text = SchMinute2;
                if (ExcCmd2.Length != 0 && ExcCmdMnt2.Length != 0)
                {
                    Note2.Text = "Execute \"" + ExcCmd2 + "\" Command in " + ExcCmdMnt2 + " Minute Before Restart";
                }
                else
                {
                    Note2.Text = "(Empty)";
                }
                Hour3.Text = SchHour3;
                Minute3.Text = SchMinute3;
                if (ExcCmd3.Length != 0 && ExcCmdMnt3.Length != 0)
                {
                    Note3.Text = "Execute \"" + ExcCmd3 + "\" Command in " + ExcCmdMnt3 + " Minute Before Restart";
                }
                else
                {
                    Note3.Text = "(Empty)";
                }
            }
            else
            {
                WriteToFileThreadSafe(">" + FileConfigApp + " Not Detected. Make Sure The file in Same Directory", "Debug.txt");
                MessageBox.Show(FileConfigApp + " Not Detected. Make Sure The file in Same Directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


            if (!File.Exists(LocationFivemServer + "\\run.cmd"))
            {

                WriteToFileThreadSafe(">Fivem Location Not Found", "Debug.txt");

                MessageBox.Show("Fivem Server Not Found. make sure you have set the folder fivem server location", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectTab(tabPage5);
            }
            else
            {

                WriteToFileThreadSafe(">Fivem Location Detected on " + LocationFivemServer, "Debug.txt");



                if (File.Exists(SelectFileConfigServer))
                {

                    WriteToFileThreadSafe(">" + SelectFileConfigServer + " Detected", "Debug.txt");

                    int indexDB = GetIndex(SelectFileConfigServer, "set mysql_connection_string");
                    if (indexDB != 0)
                    {
                        String DataDB = GetLine(SelectFileConfigServer, indexDB);
                        int lenghDataDB = DataDB.Length;
                        int indexServer = DataDB.IndexOf("server");
                        int indexDatabase = DataDB.IndexOf("database");
                        int indexUsername = DataDB.IndexOf("userid");
                        int indexPassword = DataDB.IndexOf("password");
                        int SubLenghtDBS = (indexDatabase - indexServer) - 8;
                        int SubLenghtDBN = (indexUsername - indexDatabase) - 10;
                        int SubLenghtDBU = (indexPassword - indexUsername) - 8;
                        int SubLenghtDBP = (lenghDataDB - indexPassword) - 10;
                        if (SubLenghtDBS > 1)
                        {
                            WriteToFileThreadSafe(">Read Database Server", "Debug.txt");
                            DatabaseServer = DataDB.Substring(indexServer + 7, SubLenghtDBS);
                        }
                        else
                        {
                           // DatabaseServer = "(Empty)";
                        }
                        if (SubLenghtDBN > 1)
                        {
                            WriteToFileThreadSafe(">Read Database Name", "Debug.txt");
                            DatabaseName = DataDB.Substring(indexDatabase + 9, SubLenghtDBN);
                        }
                        else
                        {

                            //DatabaseName = "(Empty)";
                        }
                        if (SubLenghtDBU > 1)
                        {
                            WriteToFileThreadSafe(">Read Database User", "Debug.txt");
                            DatabaseUser = DataDB.Substring(indexUsername + 7, SubLenghtDBU);
                        }
                        else
                        {
                            //DatabaseUser = "(Empty)";
                        }
                        if (SubLenghtDBP > 1)
                        {
                            WriteToFileThreadSafe(">Read Database Password", "Debug.txt");
                            DatabasePass = DataDB.Substring(indexPassword + 9, SubLenghtDBP);
                            int endtag = DatabasePass.IndexOf("\"");
                            if (endtag != -1)
                            {
                                DatabasePass = DatabasePass.TrimEnd('"');
                            }
                            Console.WriteLine(DatabasePass);
                        }
                        else
                        {
                            //DatabasePass = "(Empty)";
                        }

                        DbPass.Text = DatabasePass;
                        DbUser.Text = DatabaseUser;
                        DbName.Text = DatabaseName;
                        DbServer.Text = DatabaseServer;
                    }
                    WriteToFileThreadSafe(">Read Server Name", "Debug.txt");
                    ServerName = GetData(SelectFileConfigServer, "sv_hostname", 13, true);
                    WriteToFileThreadSafe(">Read Max Client", "Debug.txt");
                    MaxClient = GetData(SelectFileConfigServer, "sv_maxclients", 14, false);
                    WriteToFileThreadSafe(">Read License", "Debug.txt");
                    License = GetData(SelectFileConfigServer, "sv_licenseKey", 14, false);
                    WriteToFileThreadSafe(">Read Udp", "Debug.txt");
                    Udp = GetData(SelectFileConfigServer, "endpoint_add_udp", 18, true);
                    WriteToFileThreadSafe(">Read Tcp", "Debug.txt");
                    Tcp = GetData(SelectFileConfigServer, "endpoint_add_tcp", 18, true);
                    WriteToFileThreadSafe(">Read Icon file", "Debug.txt");
                    IconFile = GetData(SelectFileConfigServer, "load_server_icon", 17, false);

                    ResourceList.Items.Clear();
                    WriteToFileThreadSafe(">Scan & Read Resource with Tag #System", "Debug.txt");
                    ScanResource(SelectFileConfigServer, "#System", "#endSystem");
                    WriteToFileThreadSafe(">Scan & Read Resource with Tag #InESXFolder", "Debug.txt");
                    ScanResource(SelectFileConfigServer, "#InESXFolder", "#endInESXFolder");
                    WriteToFileThreadSafe(">Scan & Read Resource with Tag #OutESXFolder", "Debug.txt");
                    ScanResource(SelectFileConfigServer, "#OutESXFolder", "#endOutESXFolder");
                    WriteToFileThreadSafe(">Scan & Read Resource with Tag #Addons", "Debug.txt");
                    ScanResource(SelectFileConfigServer, "#Addons", "#endAddons");

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

                    if (WebApi.Length > 15)
                    {
                        sendData(WebApi);
                    }
                }
                else
                {
                    WriteToFileThreadSafe(">" + SelectFileConfigServer + " Not Detected. Make Sure The file in Same Directory", "Debug.txt");
                    MessageBox.Show(SelectFileConfigServer + " Not Detected. Make Sure The file in Same Directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }
        Process proc = new Process();
        public static List<string> consoleText = new List<string>();
        public static List<string> Playerid = new List<string>();
        public static List<string> Playername = new List<string>();
        public static List<string> Playerip = new List<string>();
        public static List<string> Playerping = new List<string>();

        void start()
        {

            WriteToFileThreadSafe(">Starting Server With OneSync OFF", "Debug.txt");
            File.Delete("LOG.txt");
            proc.StartInfo.FileName = LocationFivemServer + "\\run.cmd";
            if (OneSync)
            {
                WriteToFileThreadSafe(">Starting Server With OneSync ON", "Debug.txt");
                proc.StartInfo.Arguments = " +exec " + SelectFileConfigServer + " +set onesync_enabled 1";
            }
            else
            {
                WriteToFileThreadSafe(">Starting Server With OneSync OFF", "Debug.txt");
                proc.StartInfo.Arguments = " +exec " + SelectFileConfigServer;
            }
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
            WriteToFileThreadSafe(">Server Started", "Debug.txt");
            StreamReader read = proc.StandardOutput;

            String line = read.ReadLine();
            while (line != null)
            {

                int PL = line.IndexOf(" steam:");
                if (PL != -1)
                {
                    PrintPlayerList(line);
                    Thread.Sleep(10);
                    addRowPlayerList(Id, NamePlayer, Ip, PingPlayer + "ms");
                    Playerid.Add(Id);
                    Playername.Add(NamePlayer);
                    Playerip.Add(Ip);
                    Playerping.Add(PingPlayer + "ms");
                    Pid = Playerid.ToArray();
                    Pname = Playername.ToArray();
                    Pip = Playerip.ToArray();
                    Pping = Playerping.ToArray();
                    playerDetected1++;

                }
                else
                {


                    WriteToFileThreadSafe("[" + Hour + ":" + Minute + ":" + Second + "]: " + line, "LOG.txt");
                    SetText(line);

                    consoleText.Add(line);
                    cntxt = consoleText.ToArray();
                    int a = line.IndexOf("server thread hitch");
                    //Console.WriteLine(a);
                    if (a != -1 && isrunning)
                    {

                        ServerStarted = true;

                        if (WebApi.Length > 15)
                        {
                            sendData(WebApi);
                            var stat1 = new
                            {
                                ID = 1,
                                Status1 = "Start"
                            };

                            string stat1_data = JsonConvert.SerializeObject(stat1);
                            PostData(WebApi + "/api/Dashboard", stat1_data);

                            var stat2 = new
                            {
                                ID = 2,
                                Status2 = "START"
                            };

                            string stat2_data = JsonConvert.SerializeObject(stat2);
                            PostData(WebApi + "/api/Dashboard", stat2_data);
                            //CheckProcess();
                        }
                    }
                }

                line = read.ReadLine();

            }
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

        public void SaveConfigProgram()
        {
            Console.WriteLine("Saving Config");
            string[] list = new string[] { "#Config",
                "PathFivemServer = " + LocationFivemServer,
                "ServerMode = " + Servermode,
                "PublicFivemConfig = " + FileConfigServer1,
                "TestFivemConfig = " + FileConfigServer2,
                "ClearCacheWhenRestart = " + ClearCacheCfg,
                "OneSync = " + OSync,
                "Schedule1_H = " + SchHour1,
                "Schedule1_M = " + SchMinute1,
                "ExcCommMnt1 = " + ExcCmdMnt1,
                "EscCmd1 = " + ExcCmd1,
                "Schedule2_H = " + SchHour2,
                "Schedule2_M = " + SchMinute2,
                "ExcCommMnt2 = " + ExcCmdMnt2,
                "EscCmd2 = " + ExcCmd2,
                "Schedule3_H = " + SchHour3,
                "Schedule3_M = " + SchMinute3,
                "ExcCommMnt3 = " + ExcCmdMnt3,
                "EscCmd3 = " + ExcCmd3,
                "WebRemote =" + WebRip,
                "WebApi = " + WebApi
            };

            foreach (string ls in list)
            {
                Console.WriteLine(ls);
            }
            File.WriteAllLines("Config.cfg", list);

        }

        void OverWrite(string file, string FindText, string TextForEdit)
        {
            int indexLine = GetIndex(file, FindText);
            if (indexLine != 0)
            {
                lineChanger(TextForEdit, file, indexLine);
            }

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
                CN.Visible = true;
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
                    WebIP.Enabled = true;
                    MoveValueLTT();
                }
            }
            if (Change_ApplyMode == 0)
            {
                if (isrunning)
                {
                    MessageBox.Show("please restart server to take effect", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                change.Text = "Change";
                // BrowserFolder.Enabled = false;
                CN.Visible = false;
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
                WebIP.Enabled = false;


                Console.WriteLine("saving");

                string NewServerName = BoxServerName.Text;
                string RawDataServerName = "sv_hostname " + "\"" + NewServerName + "\"";
                OverWrite(SelectFileConfigServer, "sv_hostname", RawDataServerName);

                string NewTcp = BoxTcp.Text;
                string RawDataTcp = "endpoint_add_tcp " + "\"" + NewTcp + "\"";
                OverWrite(SelectFileConfigServer, "endpoint_add_tcp", RawDataTcp);

                string NewUdp = BoxUdp.Text;
                string RawDataUdp = "endpoint_add_udp " + "\"" + NewUdp + "\"";
                OverWrite(SelectFileConfigServer, "endpoint_add_udp", RawDataUdp);

                string NewLc = BoxLc.Text;
                string RawDataLc = "sv_licenseKey " + NewLc;
                OverWrite(SelectFileConfigServer, "sv_licenseKey", RawDataLc);

                string NewMcl = BoxClient.Text;
                string RawDataMcl = "sv_maxclients " + NewMcl;
                OverWrite(SelectFileConfigServer, "sv_maxclients", RawDataMcl);

                string NewIcon = BoxIcon.Text;
                string RawDataIcon = "load_server_icon " + NewIcon;
                OverWrite(SelectFileConfigServer, "load_server_icon", RawDataIcon);

                string NewDbServer = BoxDbServer.Text;
                string NewDbName = BoxDbName.Text;
                string NewDbUser = BoxDbUser.Text;
                string NewDbPass = BoxDbPass.Text;
                string RawDataDb = "set mysql_connection_string " + "\"server=" + NewDbServer + ";database=" + NewDbName + ";userid=" + NewDbUser + ";password=" + NewDbPass + "\"";
                OverWrite(SelectFileConfigServer, "set mysql_connection_string", RawDataDb);

                string NewWebIP = WebIP.Text;
                string RawDataWebIP = "WebRemote =" + NewWebIP;
                OverWrite(FileConfigApp, "WebRemote", RawDataWebIP);
                Console.WriteLine(NewWebIP);
                int portPos = NewWebIP.LastIndexOf(":");
                Console.WriteLine(portPos);
                string port = NewWebIP.Substring(portPos + 1, (NewWebIP.Length - portPos) - 2);
                Console.WriteLine(port);
                int P = Int32.Parse(port);
                Console.WriteLine(P);
                string iplink = NewWebIP.Substring(0, (NewWebIP.Length - port.Length) - 1);
                Console.WriteLine(iplink);
                int newport = P + 1;
                Console.WriteLine(newport);
                string WebApiLink = iplink + newport.ToString() + "/";
                Console.WriteLine(WebApiLink);

                string RawDataWebApi = "WebApi = " + WebApiLink;
                OverWrite(FileConfigApp, "WebApi", RawDataWebApi);


                if (WebRip != NewWebIP)
                {
                    MessageBox.Show("Web Remote Link has been Change. The program will be Restart", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    string bat = "RegHTTP.bat";
                    string oldlink1 = "netsh http delete urlacl url=" + WebRip;
                    string oldlink2 = "netsh http delete urlacl url=" + WebApi;
                    string newlink1 = "netsh http add urlacl url=" + NewWebIP + " user=Everyone";
                    string newlink2 = "netsh http add urlacl url=" + WebApiLink + " user=Everyone";
                    lineChanger(oldlink1, bat, 33);
                    lineChanger(oldlink2, bat, 34);
                    lineChanger(newlink1, bat, 35);
                    lineChanger(newlink2, bat, 36);
                    OverWrite("WebRemote/Scripts/Action.js", "var webAPI", "var webAPI = '" + WebApiLink + "';");
                    Process batrun = new Process();
                    batrun.StartInfo.FileName = @"cmd.exe";
                    //batrun.StartInfo.Verb = "runas";
                    batrun.StartInfo.Arguments = "/C" + bat;
                    batrun.StartInfo.CreateNoWindow = false;
                    batrun.Start();
                }
                else
                {

                    ReadConfig();
                    MessageBox.Show("Configuration Saved", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public void DeleteLinesFromFile(string file, string strLineToDelete)
        {
            string strFilePath = file;
            string strSearchText = strLineToDelete;
            string strOldText;
            string n = "";
            StreamReader sr = File.OpenText(strFilePath);
            while ((strOldText = sr.ReadLine()) != null)
            {
                if (!strOldText.Contains(strSearchText))
                {
                    n += strOldText + Environment.NewLine;
                }
            }
            sr.Close();
            File.WriteAllText(strFilePath, n);
        }

        void AutoRestart()
        {
            WriteToFileThreadSafe(">Server Restart", "Debug.txt");
            textBox1.Clear();
            StopProc("FXServer");
            //MessageBox.Show("Restarting");
            //CheckProcess();
            ServerStarted = false;
            if (AutoClearCache)
            {
                Directory.Delete("cache", true);
            }
            Thread.Sleep(1000);
            Thread StartServer = new Thread(start);
            StartServer.Start();
            AutoClosingMessageBox.Show("Restart Success", "info", 1000);
            //CheckProcess();
        }

        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                MessageBox.Show(text, caption);
            }

            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }

            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow(null, _caption);
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }

        void Schedule(object source, ElapsedEventArgs e)
        {


            int H = 0;
            if (Hour != null) { H = Int32.Parse(Hour); }
            int H1 = Int32.Parse(SchHour1);
            int H2 = Int32.Parse(SchHour2);
            int H3 = Int32.Parse(SchHour3);
            int M = 0;
            if (Minute != null) { M = Int32.Parse(Minute); }
            int M1 = Int32.Parse(SchMinute1);
            int M2 = Int32.Parse(SchMinute2);
            int M3 = Int32.Parse(SchMinute3);

            // Console.WriteLine(H + ":" + M);
            int excH1 = H1;
            int excM1 = M1 - Int32.Parse(ExcCmdMnt1);
            if (excM1 < 0)
            {
                excM1 = 60 - Int32.Parse(ExcCmdMnt1);
                excH1 = excH1 - 1;
            }

            //Console.WriteLine(H1 + ":" + M1);
            //onsole.WriteLine(excH1 + ":" + excM1);
            int excH2 = H2;
            int excM2 = M2 - Int32.Parse(ExcCmdMnt2);
            if (excM2 < 0)
            {
                excM2 = 60 - Int32.Parse(ExcCmdMnt2);
                excH2 = excH2 - 1;
            }
            //Console.WriteLine(H2 + ":" + M2);
            //Console.WriteLine(excH2 + ":" + excM2);

            int excH3 = H3;
            int excM3 = M3 - Int32.Parse(ExcCmdMnt3);
            if (excM3 < 0)
            {
                excM3 = 60 - Int32.Parse(ExcCmdMnt3);
                excH3 = excH3 - 1;
            }
            //Console.WriteLine(H3 + ":" + M3);
            //Console.WriteLine(excH3 + ":" + excM3);


            Thread.Sleep(10);
            if (Hour == Hour1.Text && Minute == Minute1.Text && Schedule1 == true && Schedule1RUN == true && isrunning)
            {
                Schedule1RUN = false;
                kickAllPlayer();
                AutoRestart();
                AutoClosingMessageBox.Show("Server Restart On Schedule 1", "Info", 1000);
                //MessageBox.Show("Server Restart On Schedule 1", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            if (Hour != Hour1.Text || Minute != Minute1.Text)
            {
                Schedule1RUN = true;
            }

            if (Hour == Hour2.Text && Minute == Minute2.Text && Schedule2 == true && Schedule2RUN == true && isrunning)
            {
                Schedule2RUN = false;
                kickAllPlayer();
                AutoRestart();
                AutoClosingMessageBox.Show("Server Restart On Schedule 1", "Info", 1000);
            }
            if (Hour != Hour2.Text || Minute != Minute2.Text)
            {
                Schedule2RUN = true;
            }

            if (Hour == Hour3.Text && Minute == Minute3.Text && Schedule3 == true && Schedule3RUN == true && isrunning)
            {
                Schedule3RUN = false;
                kickAllPlayer();
                AutoRestart();
                AutoClosingMessageBox.Show("Server Restart On Schedule 1", "Info", 1000);
            }
            if (Hour != Hour3.Text || Minute != Minute3.Text)
            {
                Schedule3RUN = true;
            }

            if (H == excH1 && M == excM1 && CmdTrig1 == true && isrunning && Schedule1 == true)
            {
                StreamWriter Send = proc.StandardInput;
                Send.WriteLine(ExcCmd1);
                CmdTrig1 = false;
            }
            if (H != excH1 || M != excM1)
            {
                CmdTrig1 = true;
            }

            if (H == excH2 && M == excM2 && CmdTrig2 == true && isrunning && Schedule2 == true)
            {
                StreamWriter Send = proc.StandardInput;
                Send.WriteLine(ExcCmd2);
                CmdTrig2 = false;
            }
            if (H != excH2 || M != excM2)
            {
                CmdTrig2 = true;
            }
            if (H == excH3 && M == excM3 && CmdTrig3 == true && isrunning && Schedule3 == true)
            {
                StreamWriter Send = proc.StandardInput;
                Send.WriteLine(ExcCmd3);
                CmdTrig3 = false;
            }
            if (H != excH3 || M != excM3)
            {
                CmdTrig3 = true;
            }
        }
        void Time(object source, ElapsedEventArgs e)
        {

            //for (int i = 0; i < 10; i++)
            //{
            try
            {
                DateTime LocalTime = DateTime.Now;
                Hour = LocalTime.Hour.ToString();
                Minute = LocalTime.Minute.ToString();
                Second = LocalTime.Second.ToString();
                printTime(Hour + ":" + Minute + ":" + Second);
                if (WebApi.Length > 15)
                {
                    WebStatus(WebApi);
                    WebData();

                    var stat2 = new
                    {
                        ID = 4,
                        PlayerON = playerDetected1.ToString()
                    };

                    string stat2_data = JsonConvert.SerializeObject(stat2);
                    PostData(WebApi + "/api/Dashboard", stat2_data);
                }
                //WebClient data = new WebClient();
                //string json = data.DownloadString("http://192.168.100.126:44811//api/Dashboard");
                //Console.WriteLine(json);
                CheckStatusTime++;
                //Console.WriteLine(CheckStatusTime);
                if (CheckStatusTime >= intervalCheckPlayer && isrunning && ServerStarted)
                {
                    Playerid.Clear();
                    Playername.Clear();
                    Playerip.Clear();
                    Playerping.Clear();
                    //Console.WriteLine("send status");
                    // WriteToFileThreadSafe("send status", "Debug.txt");
                    PlayerList.BeginInvoke(new MethodInvoker(() => PlayerList.Items.Clear()));
                    StreamWriter Send = proc.StandardInput;
                    Send.WriteLine("status");
                    CheckStatusTime = 0;
                    playerDetected1 = 0;
                }
                else
                {
                    //playerDetected1 = 0;
                }
                //if (i == 9) { i = 0; }
                // }
            }
            catch (Exception)
            {

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
                contextMenuStrip1.Items[0].Enabled = false;
                contextMenuStrip1.Items[1].Enabled = false;
                contextMenuStrip1.Items[2].Enabled = false;
                contextMenuStrip1.Items[3].Enabled = true;
                contextMenuStrip1.Items[4].Enabled = true;
                contextMenuStrip1.Items[5].Enabled = true;
                contextMenuStrip2.Items[0].Enabled = true;

                if (WebApi.Length > 15)
                {
                    var stat1 = new
                    {
                        ID = 1,
                        Status1 = "Start"
                    };

                    string stat1_data = JsonConvert.SerializeObject(stat1);
                    PostData(WebApi + "/api/Dashboard", stat1_data);
                }
                Start.BeginInvoke(new MethodInvoker(() => Start.Text = "STOP"));

                StartServer = new Thread(start);
                tabControl1.BeginInvoke(new MethodInvoker(() => tabControl1.SelectTab(tabPage2)));

                StartServer.Start();
                textBox1.BeginInvoke(new MethodInvoker(() => textBox1.Text = ""));
                st = true;
                Thread.Sleep(2500);
                //CheckProcess();
                RSTART.BeginInvoke(new MethodInvoker(() => RSTART.Enabled = true));
                if (!isrunning)
                {
                    MessageBox.Show("Server Failed to Start", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AutoRestart();
                }
            }

            if (Start_StopMode == 0)
            {
                contextMenuStrip1.Items[0].Enabled = true;
                contextMenuStrip1.Items[1].Enabled = true;
                contextMenuStrip1.Items[2].Enabled = true;
                contextMenuStrip1.Items[3].Enabled = false;
                contextMenuStrip1.Items[4].Enabled = false;
                contextMenuStrip1.Items[5].Enabled = false;
                contextMenuStrip2.Items[0].Enabled = false;

                WriteToFileThreadSafe(">Server Stop", "Debug.txt");
                PlayerList.BeginInvoke(new MethodInvoker(() => PlayerList.Items.Clear()));

                //proc.CancelOutputRead();
                Start.BeginInvoke(new MethodInvoker(() => Start.Text = "START"));
                StopProc("FXServer");
                //proc.Kill();
                //StartServer.Abort();
                st = false;
                ServerStarted = false;
                if (WebApi.Length > 15)
                {
                    var stat1 = new
                    {
                        ID = 1,
                        Status1 = "Stop"
                    };

                    string stat1_data = JsonConvert.SerializeObject(stat1);
                    PostData(WebApi + "/api/Dashboard", stat1_data);

                    var stat2 = new
                    {
                        ID = 2,
                        Status2 = "STOP"
                    };

                    string stat2_data = JsonConvert.SerializeObject(stat2);
                    PostData(WebApi + "/api/Dashboard", stat2_data);
                    sendData(WebApi);
                }
                //ServerStoped = true;
                Thread.Sleep(500);
                //CheckProcess();
                RSTART.BeginInvoke(new MethodInvoker(() => RSTART.Enabled = false));
            }
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
            EditSchedule edit = new EditSchedule(this);

            edit.ShowDialog();
        }

        public void testcrossform()
        {
            Console.WriteLine("Execute from another form");
        }

        public InstallResource progressBarForm = new InstallResource();

        private void OpenFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string outESX = LocationFivemServer + "\\server-data\\resources\\";
            string inESX = outESX + "\\[esx]\\";
            if (CategoryResource == "OutESXFolder")
            {
               
                if (Directory.Exists(outESX + SelectedResource))
                {
                    Process.Start(outESX + SelectedResource);
                }
                else
                {
                    MessageBox.Show("Resources " + SelectedResource + " Folder Not Found or Already Deleted", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (CategoryResource == "InESXFolder")
            {
                
                if (Directory.Exists(outESX + SelectedResource))
                {
                    Process.Start(inESX + SelectedResource);
                }
                else
                {
                    MessageBox.Show("Resources " + SelectedResource + " Folder Not Found or Already Deleted", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (isrunning)
            {
                MessageBox.Show("Please Stop Server to Install new Resource", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                progressBarForm.FormClosed += ResourceInstaller;
                progressBarForm.ShowDialog();
                //Task task = new Task(RunComparisons);
                // task.Start();
            }
        }

        void ResourceInstaller(object sender, FormClosedEventArgs e)
        {
            ReadConfig();
        }

        public void RunComparisons()
        {
            for (int i = 1; i <= 100; i++)
            {
                System.Threading.Thread.Sleep(10);
                progressBarForm.UpdateProgressBar(1, i);
            }
            MessageBox.Show("Installing Resource Complete");
            progressBarForm.BeginInvoke(new MethodInvoker(() => progressBarForm.Close()));
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(CheckProcess);
            aTimer.Interval = 500;
            aTimer.Enabled = true;

            System.Timers.Timer bTimer = new System.Timers.Timer();
            bTimer.Elapsed += new ElapsedEventHandler(Time);
            bTimer.Interval = 1000;
            bTimer.Enabled = true;

            System.Timers.Timer cTimer = new System.Timers.Timer();
            cTimer.Elapsed += new ElapsedEventHandler(Schedule);
            cTimer.Interval = 500;
            cTimer.Enabled = true;

            System.Timers.Timer dTimer = new System.Timers.Timer();
            dTimer.Elapsed += new ElapsedEventHandler(AutoCheckUpdate);
            dTimer.Interval = 5000;
            dTimer.Enabled = true;


            //WebClient client = new WebClient();
            //client.DownloadProgressChanged += Client_DownloadProgress;

        }

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string outESX = LocationFivemServer + "\\server-data\\resources\\";
            string inESX = outESX + "\\[esx]\\";
            if (CategoryResource == "OutESXFolder")
            {
                DeleteLinesFromFile(SelectFileConfigServer, SelectedResource);
                if (Directory.Exists(outESX + SelectedResource))
                {
                    Directory.Delete(outESX + SelectedResource, true);
                }
                else
                {
                    MessageBox.Show("Resources " + SelectedResource + " Folder Not Found or Already Deleted", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (CategoryResource == "InESXFolder")
            {
                DeleteLinesFromFile(SelectFileConfigServer, SelectedResource);
                if (Directory.Exists(outESX + SelectedResource))
                {
                    Directory.Delete(inESX + SelectedResource, true);
                }
                else
                {
                    MessageBox.Show("Resources " + SelectedResource + " Folder Not Found or Already Deleted", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            ReadConfig();
        }



        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }


        private void RSTART_Click(object sender, EventArgs e)
        {
            AutoRestart();
        }

        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ListViewItem lst = ResourceList.FindItemWithText(SearchText.Text);
                if (lst != null)
                {
                    ResourceList.EnsureVisible(lst.Index);

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
                SearchText.Text = "Search";
            }
            if (ThemeSelect.SelectedIndex == 1)
            {
                SearchText.ForeColor = Color.White;
            }
            else
            {
                SearchText.ForeColor = Color.Gray;
            }
            //this.SearchText.Focus();
        }

        private void SearchText_Enter(object sender, EventArgs e)
        {
            if (SearchText.Text == "Search")
            {
                SearchText.Text = "";
            }
            if (ThemeSelect.SelectedIndex == 1)
            {
                SearchText.ForeColor = Color.White;
            }
            else
            {
                SearchText.ForeColor = Color.Black;
            }

        }


        private void SearchText_TextChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ResourceList.Items)
            {
                if (item.Text == SearchText.Text)
                {
                    item.BackColor = Color.LightSteelBlue;
                    // return;
                }
                else
                {
                    item.BackColor = Color.White;
                }
            }

        }

        private void Button2_Click_1(object sender, EventArgs e)
        {

            StreamWriter Send = proc.StandardInput;
            Send.WriteLine(textBox3.Text);
        }

        void PrintPlayerList(string text)
        {
            int ldata = text.Length;
            int startindexname = text.IndexOf(" ", 15);
            int lastindexname = text.LastIndexOf(" ", text.Length - 10);
            string databeforename = text.Substring(0, startindexname);
            string dataaftername = text.Substring(lastindexname + 1, ldata - lastindexname - 1);
            string dataname = text.Substring(startindexname + 1, lastindexname - startindexname);
            NamePlayer = dataname;
            int spacedataidnsteam = databeforename.IndexOf(" ");
            string id = databeforename.Substring(0, spacedataidnsteam);
            string steamid = databeforename.Substring(spacedataidnsteam + 1, databeforename.Length - spacedataidnsteam - 1);
            Id = id;
            SteamId = steamid;
            int ldataaftername = dataaftername.Length;
            int spacedataipnping = dataaftername.IndexOf(" ");
            string ip = dataaftername.Substring(0, ldataaftername - (ldataaftername - spacedataipnping));
            string ping = dataaftername.Substring(spacedataipnping + 1, (ldataaftername - spacedataipnping) - 1);
            Ip = ip;
            PingPlayer = ping;

            //int space1 = text.IndexOf(" ", 0);
            //int space2 = text.IndexOf(" ", space1 + 2);
            //int space3 = text.IndexOf(" ", space2 + 2);
            //int space4 = text.IndexOf(" ", space3 + 2);

            //int LD2 = (space2 - space1) - 1;
            //int LD3 = (space3 - space2) - 1;
            //int LD4 = (space4 - space3) - 1;
            //int LD5 = (text.Length - space4) - 1;

            //string ID = text.Substring(0, space1);
            //Console.WriteLine(ID);
            //Id = ID;
            //string Steam = text.Substring(space1 + 1, LD2);
            //Console.WriteLine(Steam);
            //SteamId = Steam;
            //string Name = text.Substring(space2 + 1, LD3);
            //Console.WriteLine(Name);
            //NamePlayer = Name;
            //string IP = text.Substring(space3 + 1, LD4);
            //Console.WriteLine(IP);
            //Ip = IP;
            //string Ping = text.Substring(space4 + 1, LD5);
            //Console.WriteLine(Ping);
            //PingPlayer = Ping;

        }



        private void BrowserFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Find and Select Fivem Server Folder";
            folderBrowserDialog1.ShowDialog();
            string FivemFolder = folderBrowserDialog1.SelectedPath;

            if (File.Exists(FivemFolder + "\\run.cmd"))
            {
                Console.WriteLine("Save Fivem Folder Location");
                BoxPathServer.Text = FivemFolder;
                LocationFivemServer = FivemFolder;
                SaveConfigProgram();
                Thread.Sleep(100);
                MessageBox.Show("Fivem Server Detected", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReadConfig();
                MoveValueLTT();

            }
            else
            {
                MessageBox.Show("Fivem Server Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (isrunning)
            {
                ResourceList.SelectedItems[0].SubItems[0].Text = SelectedResource;
                ResourceList.SelectedItems[0].SubItems[1].Text = CategoryResource;
                ResourceList.SelectedItems[0].SubItems[2].Text = Rstat1;
                ResourceList.SelectedItems[0].SubItems[3].Text = "Start";
                StreamWriter Send = proc.StandardInput;
                Send.WriteLine("start " + SelectedResource);
            }
        }

        private void OSyc_CheckedChanged(object sender, EventArgs e)
        {
            if (OSyc.Checked)
            {
                if (isrunning)
                {
                    MessageBox.Show("please restart server to take effect", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                OSync = 1;
                Console.WriteLine("save onesync enable");
                SaveConfigProgram();
                OneSync = true;
            }
            else
            {
                if (isrunning)
                {
                    MessageBox.Show("please restart server to take effect", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                OSync = 0;
                Console.WriteLine("save onesync disable");
                SaveConfigProgram();
                OneSync = false;
            }
        }

        private void PlayerList_MouseClick(object sender, MouseEventArgs e)
        {
            string id = PlayerList.SelectedItems[0].SubItems[0].Text;
            Console.WriteLine(id);
            SelectedId = id;
        }

        private void ThemeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // int c = 0;
            if (ThemeSelect.SelectedIndex == 1)
            {
                Theme = "Dark";
                Console.WriteLine("Dark Theme Selected");
                this.BackColor = Color.FromArgb(64, 64, 64);
                label2.ForeColor = Color.White;
                Start.ForeColor = Color.White;
                Start.BackColor = Color.FromArgb(64, 64, 64);
                RSTART.ForeColor = Color.White;
                RSTART.BackColor = Color.FromArgb(64, 64, 64);


                //theme on tab setting
                tabPage5.BackColor = Color.FromArgb(64, 64, 64);
                label9.ForeColor = Color.White;
                label10.ForeColor = Color.White;
                label11.ForeColor = Color.White;
                label12.ForeColor = Color.White;
                label20.ForeColor = Color.White;
                label23.ForeColor = Color.White;
                label24.ForeColor = Color.White;
                label25.ForeColor = Color.White;
                label26.ForeColor = Color.White;
                label27.ForeColor = Color.White;
                label28.ForeColor = Color.White;
                label29.ForeColor = Color.White;
                label8.ForeColor = Color.White;
                LTIME.ForeColor = Color.White;

                optClearCache.ForeColor = Color.White;
                OSyc.ForeColor = Color.White;
                checkBox1.ForeColor = Color.White;
                checkBox2.ForeColor = Color.White;
                checkBox3.ForeColor = Color.White;
                Hour1.ForeColor = Color.White;
                Hour2.ForeColor = Color.White;
                Hour3.ForeColor = Color.White;
                Minute1.ForeColor = Color.White;
                Minute2.ForeColor = Color.White;
                Minute3.ForeColor = Color.White;
                Note1.ForeColor = Color.White;
                Note2.ForeColor = Color.White;
                Note3.ForeColor = Color.White;
                ClearCache.BackColor = Color.FromArgb(64, 64, 64);
                ClearCache.ForeColor = Color.White;
                BrowserFolder.BackColor = Color.FromArgb(64, 64, 64);
                BrowserFolder.ForeColor = Color.White;
                button3.BackColor = Color.FromArgb(64, 64, 64);
                button3.ForeColor = Color.White;
                button4.BackColor = Color.FromArgb(64, 64, 64);
                button4.ForeColor = Color.White;
                ThemeSelect.BackColor = Color.FromArgb(64, 64, 64);
                ThemeSelect.ForeColor = Color.White;
                ServerMode.BackColor = Color.FromArgb(64, 64, 64);
                ServerMode.ForeColor = Color.White;
                BoxPathServer.BackColor = Color.FromArgb(64, 64, 64);
                BoxPathServer.ForeColor = Color.White;
                PublicServerFile.BackColor = Color.FromArgb(64, 64, 64);
                PublicServerFile.ForeColor = Color.White;
                TestServerFile.BackColor = Color.FromArgb(64, 64, 64);
                TestServerFile.ForeColor = Color.White;
                label31.ForeColor = Color.White;
                label17.ForeColor = Color.White;
                label32.ForeColor = Color.White;
                UpdateServ.ForeColor = Color.White;
                UpdateServ.BackColor = Color.FromArgb(64, 64, 64);
                UpdateSoft.ForeColor = Color.White;
                UpdateSoft.BackColor = Color.FromArgb(64, 64, 64);
                InstalledServ.ForeColor = Color.White;
                InstalledSoft.ForeColor = Color.White;
                NewSoft.ForeColor = Color.White;
                ShowNotifyUp.ForeColor = Color.White;
                EditSch1.ForeColor = Color.White;
                EditSch1.BackColor = Color.FromArgb(64, 64, 64);

                //Theme Tab resource Manager
                tabPage4.BackColor = Color.FromArgb(64, 64, 64);
                SearchText.BackColor = Color.FromArgb(64, 64, 64);
                button1.BackColor = Color.FromArgb(64, 64, 64);
                button1.ForeColor = Color.White;
                ResourceList.BackColor = Color.FromArgb(64, 64, 64);
                ResourceList.ForeColor = Color.White;
                progressBarForm.BackColor = Color.FromArgb(64, 64, 64);
                progressBarForm.BoxPathResource.BackColor = Color.FromArgb(64, 64, 64);
                progressBarForm.BoxPathResource.ForeColor = Color.White;
                progressBarForm.comboBox1.BackColor = Color.FromArgb(64, 64, 64);
                progressBarForm.comboBox1.ForeColor = Color.White;
                progressBarForm.BrowserFolder.BackColor = Color.FromArgb(64, 64, 64);
                progressBarForm.BrowserFolder.ForeColor = Color.White;
                progressBarForm.InstallR.BackColor = Color.FromArgb(64, 64, 64);
                progressBarForm.InstallR.ForeColor = Color.White;

                //Theme Tab Console
                tabPage2.BackColor = Color.FromArgb(64, 64, 64);
                textBox1.BackColor = Color.FromArgb(64, 64, 64);
                textBox1.ForeColor = Color.White;
                textBox3.BackColor = Color.FromArgb(64, 64, 64);
                textBox3.ForeColor = Color.White;
                button2.BackColor = Color.FromArgb(64, 64, 64);
                button2.ForeColor = Color.White;

                //Theme Tab PlayerList
                tabPage3.BackColor = Color.FromArgb(64, 64, 64);
                Kall.BackColor = Color.FromArgb(64, 64, 64);
                Kall.ForeColor = Color.White;
                PlayerList.BackColor = Color.FromArgb(64, 64, 64);
                PlayerList.ForeColor = Color.White;

                //Theme Tab System Info
                tabPage1.BackColor = Color.FromArgb(64, 64, 64);
                CN.BackColor = Color.FromArgb(64, 64, 64);
                CN.ForeColor = Color.White;
                change.BackColor = Color.FromArgb(64, 64, 64);
                change.ForeColor = Color.White;
                BoxClient.BackColor = Color.FromArgb(64, 64, 64);
                BoxClient.ForeColor = Color.White;
                BoxDbName.BackColor = Color.FromArgb(64, 64, 64);
                BoxDbName.ForeColor = Color.White;
                BoxDbPass.BackColor = Color.FromArgb(64, 64, 64);
                BoxDbPass.ForeColor = Color.White;
                BoxDbServer.BackColor = Color.FromArgb(64, 64, 64);
                BoxDbServer.ForeColor = Color.White;
                BoxDbUser.BackColor = Color.FromArgb(64, 64, 64);
                BoxDbUser.ForeColor = Color.White;
                BoxIcon.BackColor = Color.FromArgb(64, 64, 64);
                BoxIcon.ForeColor = Color.White;
                BoxLc.BackColor = Color.FromArgb(64, 64, 64);
                BoxLc.ForeColor = Color.White;
                BoxServerName.BackColor = Color.FromArgb(64, 64, 64);
                BoxServerName.ForeColor = Color.White;
                BoxTcp.BackColor = Color.FromArgb(64, 64, 64);
                BoxTcp.ForeColor = Color.White;
                BoxUdp.BackColor = Color.FromArgb(64, 64, 64);
                BoxUdp.ForeColor = Color.White;
                Server_Name.ForeColor = Color.White;
                TCP.ForeColor = Color.White;
                UDP.ForeColor = Color.White;
                MAXC.ForeColor = Color.White;
                Key.ForeColor = Color.White;
                ICON.ForeColor = Color.White;
                DbName.ForeColor = Color.White;
                DbPass.ForeColor = Color.White;
                DbServer.ForeColor = Color.White;
                DbUser.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label16.ForeColor = Color.White;
                label18.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label1.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label13.ForeColor = Color.White;
                label14.ForeColor = Color.White;
                label15.ForeColor = Color.White;


            }
            else
            {
                Theme = "Default";
                Console.WriteLine("Default Theme Selected");

                this.BackColor = Color.White;
                label2.ForeColor = Color.Black;
                Start.ForeColor = Color.Black;
                Start.UseVisualStyleBackColor = true;
                RSTART.ForeColor = Color.Black;
                RSTART.UseVisualStyleBackColor = true;


                //theme on tab setting
                tabPage5.BackColor = Color.White;
                label9.ForeColor = Color.Black;
                label10.ForeColor = Color.Black;
                label11.ForeColor = Color.Black;
                label12.ForeColor = Color.Black;
                label20.ForeColor = Color.Black;
                label23.ForeColor = Color.Black;
                label24.ForeColor = Color.Black;
                label25.ForeColor = Color.Black;
                label26.ForeColor = Color.Black;
                label27.ForeColor = Color.Black;
                label28.ForeColor = Color.Black;
                label29.ForeColor = Color.Black;
                label8.ForeColor = Color.Black;
                LTIME.ForeColor = Color.Black;

                optClearCache.ForeColor = Color.Black;
                OSyc.ForeColor = Color.Black;
                checkBox1.ForeColor = Color.Black;
                checkBox2.ForeColor = Color.Black;
                checkBox3.ForeColor = Color.Black;
                Hour1.ForeColor = Color.Black;
                Hour2.ForeColor = Color.Black;
                Hour3.ForeColor = Color.Black;
                Minute1.ForeColor = Color.Black;
                Minute2.ForeColor = Color.Black;
                Minute3.ForeColor = Color.Black;
                Note1.ForeColor = Color.Black;
                Note2.ForeColor = Color.Black;
                Note3.ForeColor = Color.Black;
                ClearCache.UseVisualStyleBackColor = true;
                ClearCache.ForeColor = Color.Black;
                BrowserFolder.UseVisualStyleBackColor = true;
                BrowserFolder.ForeColor = Color.Black;
                button3.UseVisualStyleBackColor = true;
                button3.ForeColor = Color.Black;
                button4.UseVisualStyleBackColor = true;
                button4.ForeColor = Color.Black;
                ThemeSelect.BackColor = Color.White;
                ThemeSelect.ForeColor = Color.Black;
                ServerMode.BackColor = Color.White;
                ServerMode.ForeColor = Color.Black;
                BoxPathServer.BackColor = Color.White;
                BoxPathServer.ForeColor = Color.Black;
                PublicServerFile.BackColor = Color.White;
                PublicServerFile.ForeColor = Color.Black;
                TestServerFile.BackColor = Color.White;
                TestServerFile.ForeColor = Color.Black;
                EditSch1.ForeColor = Color.Black;
                EditSch1.UseVisualStyleBackColor = true;
                label31.ForeColor = Color.Black;
                label17.ForeColor = Color.Black;
                label32.ForeColor = Color.Black;
                UpdateServ.ForeColor = Color.Black;
                UpdateServ.UseVisualStyleBackColor = true;
                UpdateSoft.ForeColor = Color.Black;
                UpdateSoft.UseVisualStyleBackColor = true;
                InstalledServ.ForeColor = Color.Black;
                InstalledSoft.ForeColor = Color.Black;
                NewSoft.ForeColor = Color.Black;
                ShowNotifyUp.ForeColor = Color.Black;

                //Theme Tab resource Manager
                tabPage4.BackColor = Color.White;
                SearchText.BackColor = Color.White;
                button1.UseVisualStyleBackColor = true;
                button1.ForeColor = Color.Black;
                ResourceList.BackColor = Color.White;
                ResourceList.ForeColor = Color.Black;
                progressBarForm.BackColor = Color.White;
                progressBarForm.BoxPathResource.BackColor = Color.White;
                progressBarForm.BoxPathResource.ForeColor = Color.Black;
                progressBarForm.comboBox1.BackColor = Color.White;
                progressBarForm.comboBox1.ForeColor = Color.Black;
                progressBarForm.BrowserFolder.UseVisualStyleBackColor = true;
                progressBarForm.BrowserFolder.ForeColor = Color.Black;
                progressBarForm.InstallR.UseVisualStyleBackColor = true;
                progressBarForm.InstallR.ForeColor = Color.Black;

                //Theme Tab Console
                tabPage2.BackColor = Color.White;
                textBox1.BackColor = Color.White;
                textBox1.ForeColor = Color.Black;
                textBox3.BackColor = Color.White;
                textBox3.ForeColor = Color.Black;
                button2.UseVisualStyleBackColor = true;
                button2.ForeColor = Color.Black;

                //Theme Tab PlayerList
                tabPage3.BackColor = Color.White;
                Kall.UseVisualStyleBackColor = true;
                Kall.ForeColor = Color.Black;
                PlayerList.BackColor = Color.White;
                PlayerList.ForeColor = Color.Black;

                //Theme Tab System Info
                tabPage1.BackColor = Color.White;
                CN.UseVisualStyleBackColor = true;
                CN.ForeColor = Color.Black;
                change.UseVisualStyleBackColor = true;
                change.ForeColor = Color.Black;
                BoxClient.BackColor = Color.White;
                BoxClient.ForeColor = Color.Black;
                BoxDbName.BackColor = Color.White;
                BoxDbName.ForeColor = Color.Black;
                BoxDbPass.BackColor = Color.White;
                BoxDbPass.ForeColor = Color.Black;
                BoxDbServer.BackColor = Color.White;
                BoxDbServer.ForeColor = Color.Black;
                BoxDbUser.BackColor = Color.White;
                BoxDbUser.ForeColor = Color.Black;
                BoxIcon.BackColor = Color.White;
                BoxIcon.ForeColor = Color.Black;
                BoxLc.BackColor = Color.White;
                BoxLc.ForeColor = Color.Black;
                BoxServerName.BackColor = Color.White;
                BoxServerName.ForeColor = Color.Black;
                BoxTcp.BackColor = Color.White;
                BoxTcp.ForeColor = Color.Black;
                BoxUdp.BackColor = Color.White;
                BoxUdp.ForeColor = Color.Black;
                Server_Name.ForeColor = Color.Black;
                TCP.ForeColor = Color.Black;
                UDP.ForeColor = Color.Black;
                MAXC.ForeColor = Color.Black;
                Key.ForeColor = Color.Black;
                ICON.ForeColor = Color.Black;
                DbName.ForeColor = Color.Black;
                DbPass.ForeColor = Color.Black;
                DbServer.ForeColor = Color.Black;
                DbUser.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label16.ForeColor = Color.Black;
                label18.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label1.ForeColor = Color.Black;
                label7.ForeColor = Color.Black;
                label13.ForeColor = Color.Black;
                label14.ForeColor = Color.Black;
                label15.ForeColor = Color.Black;
            }
        }




        private void EnableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string EnR = "start " + SelectedResource;
            OverWrite(SelectFileConfigServer, SelectedResource, EnR);
            ReadConfig();
        }

        private void UpdateServ_Click(object sender, EventArgs e)
        {
            Process.Start("https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/");
        }

        private void UpdateSoft_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Oky12/Vadmin/releases/download/V" + NewProgram + "/Vadmin" + NewProgram + ".zip");
        }

        private void ContextMenuStrip1_Opened(object sender, EventArgs e)
        {
            enableToolStripMenuItem1.ToolTipText = "Set to Enable " + SelectedResource + " and Start the Resources when Starting Server";
            disableToolStripMenuItem.ToolTipText = "Set to Disable " + SelectedResource + " and not Start the Resources when Starting Server";
            renameToolStripMenuItem.ToolTipText = "Change Name of " + SelectedResource + " in " + SelectFileConfigServer + " and Change Name Folder too";
            startToolStripMenuItem.ToolTipText = "Start " + SelectedResource + " Resources When Server Started";
            stopToolStripMenuItem.ToolTipText = "Stop " + SelectedResource + " Resources When Server Started";
            restartToolStripMenuItem.ToolTipText = "Restart " + SelectedResource + " Resources When Resources & Server Started";
        }

        private void EditSch1_Click(object sender, EventArgs e)
        {
            EditSchedule edit = new EditSchedule(this);

            edit.ShowDialog();
        }

        void PostData(string url, string json)
        {
            try
            {
                HttpClient client = new HttpClient();

                //Print the Json object
                //Console.WriteLine(json);

                //Console.WriteLine(my_jsondata);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;
                //Console.WriteLine(result);
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Failed Sending Data to Web Remote", "Error", 2000);
            }
        }

        public string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private void LinkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            

        }


        private void DisableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DnR = "#start " + SelectedResource;
            OverWrite(SelectFileConfigServer, SelectedResource, DnR);
            ReadConfig();
        }

        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameResource Rename = new RenameResource();
            Rename.FormClosed += Rname;
            Rename.ShowDialog();
        }

        void Rname(object sender, FormClosedEventArgs e)
        {
            ReadConfig();
        }

        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            //e.DrawBackground();

        }
        void kickAllPlayer()
        {
            Console.WriteLine("Kicking all player");
            playerDetected2 = PlayerList.Items.Count;
            if (playerDetected2 > 0)
            {
                for (int i = 0; i < playerDetected2; i++)
                {
                    string idplayer = PlayerList.Items[i].Text;
                    Console.WriteLine(idplayer);
                    StreamWriter Send = proc.StandardInput;
                    Send.WriteLine("clientkick " + idplayer + " You has Been Kicked");
                }
                //MessageBox.Show("Kicking " + playerDetected2 + " Player", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AutoClosingMessageBox.Show("Kicking " + playerDetected2 + " Player", "Info", 2000);
                playerDetected2 = 0;
                PlayerList.Items.Clear();

            }
        }
        private void Kall_Click(object sender, EventArgs e)
        {

            kickAllPlayer();
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isrunning)
            {
                ResourceList.SelectedItems[0].SubItems[0].Text = SelectedResource;
                ResourceList.SelectedItems[0].SubItems[1].Text = CategoryResource;
                ResourceList.SelectedItems[0].SubItems[2].Text = Rstat1;
                ResourceList.SelectedItems[0].SubItems[3].Text = "Stop";

                StreamWriter Send = proc.StandardInput;
                Send.WriteLine("stop " + SelectedResource);
            }
        }

        private void RestartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isrunning && Rstat2 == "Start")
            {
                StreamWriter Send = proc.StandardInput;
                Send.WriteLine("restart " + SelectedResource);
            }
            else
            {
                MessageBox.Show(SelectedResource + "Resources Not Started. Please Start the Resources to use this Option"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void KickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("what is the reason you kicked him from the server?", "Reason Kick", "", 0, 0);
            if (isrunning && input != "")
            {
                Console.WriteLine("Send kick");
                StreamWriter Send = proc.StandardInput;
                Send.WriteLine("clientkick " + SelectedId + " " + input);
                Send.WriteLine("status");
            }

        }



        private void ServerMode_SelectedValueChanged(object sender, EventArgs e)
        {
            if (ServerMode.SelectedIndex == 0)
            {
                Console.WriteLine("save Server mode");
                Console.WriteLine("Public Server");
                SelectFileConfigServer = FileConfigServer1;
                Servermode = 0;

                SaveConfigProgram();
                ReadConfig();
                if (isrunning)
                {
                    MessageBox.Show("Please Restart Server to Load Public Server Config", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }

            if (ServerMode.SelectedIndex == 1)
            {

                Console.WriteLine("save Server mode");
                Console.WriteLine("Test Server");
                SelectFileConfigServer = FileConfigServer2;
                Servermode = 1;
                SaveConfigProgram();
                ReadConfig();
                if (isrunning)
                {
                    MessageBox.Show("Please Restart Server to Load Test Server Config", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        private void Label33_Click(object sender, EventArgs e)
        {

        }



        private void Button3_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Config files (*.cfg)|*.cfg";
            openFileDialog1.Title = "Find Config Public Server";
            //openFileDialog1.ShowDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                FileConfigServer1 = openFileDialog1.FileName;
                PublicServerFile.Text = FileConfigServer1;
                Console.WriteLine(FileConfigServer1);
                SaveConfigProgram();
                ReadConfig();
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Config files (*.cfg)|*.cfg";
            openFileDialog1.Title = "Find Config Test Server";
            //openFileDialog1.ShowDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                FileConfigServer2 = openFileDialog1.FileName;
                TestServerFile.Text = FileConfigServer2;
                Console.WriteLine(FileConfigServer2);
                SaveConfigProgram();
                ReadConfig();
            }

        }

        private void ResourceList_MouseClick(object sender, MouseEventArgs e)
        {
            string Stat2 = ResourceList.SelectedItems[0].SubItems[3].Text;
            Console.WriteLine(Stat2);
            Rstat2 = Stat2;
            string Stat1 = ResourceList.SelectedItems[0].SubItems[2].Text;
            Console.WriteLine(Stat1);
            Rstat1 = Stat1;
            string category = ResourceList.SelectedItems[0].SubItems[1].Text;
            Console.WriteLine(category);
            CategoryResource = category;
            string Resource = ResourceList.SelectedItems[0].SubItems[0].Text;
            Console.WriteLine(Resource);
            SelectedResource = Resource;
        }


        private void CN_Click(object sender, EventArgs e)
        {
            Change_ApplyMode = 0;
            change.Text = "Change";
            CN.Visible = false;
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
            WebIP.Enabled = false;
        }

        private void ClearCache_Click(object sender, EventArgs e)
        {

            if (isrunning)
            {
                MessageBox.Show("please Stop server to Clear Cache", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (Directory.Exists("cache"))
            {
                MessageBox.Show("Clear Cache Success", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Directory.Delete("cache", true);
            }
            else { MessageBox.Show("Cache Folder Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void OptClearCache_CheckedChanged(object sender, EventArgs e)
        {
            if (optClearCache.Checked)
            {
                if (isrunning)
                {
                    MessageBox.Show("please restart server to take effect", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                AutoClearCache = true;
                ClearCacheCfg = 1;
                SaveConfigProgram();
            }
            else
            {
                if (isrunning)
                {
                    MessageBox.Show("please restart server to take effect", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                ClearCacheCfg = 0;
                SaveConfigProgram();
                AutoClearCache = false;
            }
        }


    }

}




