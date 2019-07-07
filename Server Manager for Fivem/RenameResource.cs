using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fivem_Server_Manager;
using System.IO;

namespace Server_Manager_for_Fivem
{
    public partial class RenameResource : Form
    {
        public RenameResource()
        {
            InitializeComponent();
            string th = Form1.Theme;
            if (th == "Dark")
            {
                this.BackColor = Color.FromArgb(64, 64, 64);
                textBox1.BackColor = Color.FromArgb(64, 64, 64);
                button1.BackColor = Color.FromArgb(64, 64, 64);
                textBox1.ForeColor = Color.White;
                button1.ForeColor = Color.White;
                label1.ForeColor = Color.White;
            }
        }

        void OverWrite(string file, string FindText, string TextForEdit)
        {
            int indexLine = GetIndex(file, FindText);
            if (indexLine != 0)
            {
                lineChanger(TextForEdit, file, indexLine);
            }

        }

        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
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

        private void Button1_Click(object sender, EventArgs e)
        {
            string NameR = Form1.SelectedResource;
            string CatR = Form1.CategoryResource;
            string StatR = Form1.Rstat1;
            Console.WriteLine(NameR + " " + CatR + " " + StatR);
            string LocSV = Form1.LocationFivemServer;
            Console.WriteLine(LocSV + "\\server-data\\resources\\" + NameR);
            if (StatR == "Enable")
            {
                string Rewrite = "start " + textBox1.Text;
                Console.WriteLine(Rewrite);
                try
                {
                    Console.WriteLine("Renamed");
                    OverWrite(Form1.SelectFileConfigServer, NameR, Rewrite);
                    if (CatR == "OutESXFolder")
                    {
                        Directory.Move(LocSV + "\\server-data\\resources\\" + NameR, "E:\\" + NameR);
                        Directory.Move("E:\\" + NameR, LocSV + "\\server-data\\Resources\\" + textBox1.Text);
                    }

                    if (CatR == "InESXFolder")
                    {
                        Directory.Move(LocSV + "\\server-data\\resources\\[esx]\\" + NameR, "E:\\" + NameR);
                        Directory.Move("E:\\" + NameR, LocSV + "\\server-data\\Resources\\[esx]\\" + textBox1.Text);
                    }

                    if (CatR == "Addons")
                    {
                        Directory.Move(LocSV + "\\server-data\\resources\\[addons]\\" + NameR, "E:\\" + NameR);
                        Directory.Move("E:\\" + NameR, LocSV + "\\server-data\\Resources\\[addons]\\" + textBox1.Text);
                    }
                }
                catch
                {
                    MessageBox.Show("Failed Rename Resources " + NameR + " Folder. Because Not Found or Already Deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            if (StatR == "Disable")
            {
                string Rewrite = "#start " + textBox1.Text;
                Console.WriteLine(Rewrite);
                try
                {
                    Console.WriteLine("Renamed");
                    OverWrite(Form1.SelectFileConfigServer, NameR, Rewrite);
                    if (CatR == "OutESXFolder")
                    {
                        Directory.Move(LocSV + "\\server-data\\resources\\" + NameR, "E:\\" + NameR);
                        Directory.Move("E:\\" + NameR, LocSV + "\\server-data\\Resources\\" + textBox1.Text);
                    }

                    if (CatR == "InESXFolder")
                    {
                        Directory.Move(LocSV + "\\server-data\\resources\\[esx]\\" + NameR, "E:\\" + NameR);
                        Directory.Move("E:\\" + NameR, LocSV + "\\server-data\\Resources\\[esx]\\" + textBox1.Text);
                    }

                    if (CatR == "Addons")
                    {
                        Directory.Move(LocSV + "\\server-data\\resources\\[addons]\\" + NameR, "E:\\" + NameR);
                        Directory.Move("E:\\" + NameR, LocSV + "\\server-data\\Resources\\[addons]\\" + textBox1.Text);
                    }
                }
                catch
                {
                    MessageBox.Show("Failed Rename Resources " + NameR + " Folder. Because Not Found or Already Deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            this.Close();
        }
    }
}
