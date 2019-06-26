using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Fivem_Server_Manager
{
    public partial class InstallResource : Form
    {
        
        string NewRes;

        public InstallResource()
        {
            InitializeComponent();



        }

        private void InstallResource_Load(object sender, EventArgs e)
        {

        }


        static long GetFolderSize(string s)
        {
            string[] fileNames = Directory.GetFiles(s, "*.*");
            long size = 0;

            // Calculate total size by looping through files in the folder and totalling their sizes
            foreach (string name in fileNames)
            {
                // length of each file.
                FileInfo details = new FileInfo(name);
                size += details.Length;
            }
            return size;
        }

        static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }


        public void UpdateProgressBar(int index, int value)
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (index == 1)
                {
                    progressBar1.Value = value;
                }
                else
                {
                    //progressBar2.Value = value;
                }
            });
        }

        private void BrowserFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Find and Select Fivem Server Folder";
            folderBrowserDialog1.ShowDialog();
            NewRes = folderBrowserDialog1.SelectedPath;
            BoxPathResource.Text = NewRes;
        }

        int fileCount;

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
        private int map(int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        private void InstallR_Click(object sender, EventArgs e)
        {
            string Rfivemlocation = Form1.LocationFivemServer + "\\server-data\\resources\\";
            Console.WriteLine(Rfivemlocation);

            string SourceFolderResource = BoxPathResource.Text;
            if (SourceFolderResource != "(Empty)")
            {
                int indexname = SourceFolderResource.LastIndexOf("\\") + 1;
                int LenName = SourceFolderResource.Length;
                LenName = LenName - indexname;
                string NameFolderResource = SourceFolderResource.Substring(indexname, LenName);
                Console.WriteLine(Rfivemlocation + NameFolderResource);
                fileCount = Directory.GetFiles(SourceFolderResource, "*.*", SearchOption.AllDirectories).Length; // Will Retrieve count of all files in directry and sub directries
                Console.WriteLine(fileCount);
                // progressBar1.Maximum = fileCount;
                Console.WriteLine(Rfivemlocation + "\\" + "[esx]\\" + NameFolderResource);
                if (comboBox1.Text == "InFolderESX" && !Directory.Exists(Rfivemlocation + "\\[esx]\\" + NameFolderResource))
                {
                    DirectoryCopy(SourceFolderResource, Rfivemlocation + "\\[esx]\\" + NameFolderResource, true);
                    MessageBox.Show("Install Resource Complete");
                    string fileconfig = Form1.SelectFileConfigServer;
                    Console.WriteLine(fileconfig);
                    insertLineToSimpleFile(fileconfig, "#InESXFolder", "start " + NameFolderResource, true);
                }
                else if(comboBox1.Text == "InFolderESX" && Directory.Exists(Rfivemlocation + "\\[esx]\\" + NameFolderResource))
                {
                    MessageBox.Show("already installed");
                }

                if (comboBox1.Text == "OutFolderESX" && !Directory.Exists(Rfivemlocation + NameFolderResource))
                {
                    DirectoryCopy(SourceFolderResource, Rfivemlocation + NameFolderResource, true);
                    MessageBox.Show("Install Resource Complete");
                    string fileconfig = Form1.SelectFileConfigServer;
                    Console.WriteLine(fileconfig);
                    insertLineToSimpleFile(fileconfig, "#OutESXFolder", "start " + NameFolderResource, true);
                    
                }
                else if(comboBox1.Text == "OutFolderESX" && Directory.Exists(Rfivemlocation + NameFolderResource))
                {
                    MessageBox.Show("already installed");
                }

               

            }

            progressBar1.Value = 0;                                    
        }


        private void InstallResource_FormClosing(object sender, FormClosingEventArgs e)
        {
           // e.Cancel = true;
            //Hide();
        }
        int i = 0;
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {


            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                i++;
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }


            // If copying subdirectories, copy them and their contents to new location.

            foreach (DirectoryInfo subdir in dirs)
            {
                //i++;
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }

            int percent = map(i, 0, fileCount, 0, 100);
            if (percent > 100)
            {
                percent = 100;
            }
            progressBar1.Value = percent;
        }
    }
}
