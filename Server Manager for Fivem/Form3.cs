using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fivem_Server_Manager
{
    public partial class EditSchedule : Form
    {
        //public string SchHour1, SchMinute1, ExcCmdMnt1, ExcCmd1, SchHour2, SchMinute2, ExcCmdMnt2, ExcCmd2, SchHour3, SchMinute3, ExcCmdMnt3, ExcCmd3;
        private Form1 MainForm;
        public EditSchedule(Form1 parentForm)
        {
            InitializeComponent();
            MainForm = parentForm;
            H1.Text = MainForm.SchHour1;
            H2.Text = MainForm.SchHour2;
            H3.Text = MainForm.SchHour3;
            M1.Text = MainForm.SchMinute1;
            M2.Text = MainForm.SchMinute2;
            M3.Text = MainForm.SchMinute3;
            ExcCmmMnt1.Text = MainForm.ExcCmdMnt1;
            ExcCmmMnt2.Text = MainForm.ExcCmdMnt2;
            ExcCmmMnt3.Text = MainForm.ExcCmdMnt3;
            Cmd1.Text = MainForm.ExcCmd1;
            Cmd2.Text = MainForm.ExcCmd2;
            Cmd3.Text = MainForm.ExcCmd3;
            if(MainForm.ThemeSelect.SelectedIndex == 1)
            {
                BackColor = Color.FromArgb(64, 64, 64);
                button1.ForeColor = Color.White;
                button1.BackColor = Color.FromArgb(64, 64, 64);
                H1.ForeColor = Color.White;
                H1.BackColor = Color.FromArgb(64, 64, 64);
                H2.ForeColor = Color.White;
                H2.BackColor = Color.FromArgb(64, 64, 64);
                H3.ForeColor = Color.White;
                H3.BackColor = Color.FromArgb(64, 64, 64);
                M1.ForeColor = Color.White;
                M1.BackColor = Color.FromArgb(64, 64, 64);
                M2.ForeColor = Color.White;
                M2.BackColor = Color.FromArgb(64, 64, 64);
                M3.ForeColor = Color.White;
                M3.BackColor = Color.FromArgb(64, 64, 64);
                ExcCmmMnt1.ForeColor = Color.White;
                ExcCmmMnt1.BackColor = Color.FromArgb(64, 64, 64);
                ExcCmmMnt2.ForeColor = Color.White;
                ExcCmmMnt2.BackColor = Color.FromArgb(64, 64, 64);
                ExcCmmMnt3.ForeColor = Color.White;
                ExcCmmMnt3.BackColor = Color.FromArgb(64, 64, 64);
                Cmd1.ForeColor = Color.White;
                Cmd1.BackColor = Color.FromArgb(64, 64, 64);
                Cmd2.ForeColor = Color.White;
                Cmd2.BackColor = Color.FromArgb(64, 64, 64);
                Cmd3.ForeColor = Color.White;
                Cmd3.BackColor = Color.FromArgb(64, 64, 64);

            }
            else
            {

            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MainForm.SchHour1 = H1.Text;
            MainForm.SchHour2 = H2.Text;
            MainForm.SchHour3 = H3.Text;
            MainForm.SchMinute1 = M1.Text;
            MainForm.SchMinute2 = M2.Text;
            MainForm.SchMinute3 = M3.Text;
            MainForm.ExcCmdMnt1 = ExcCmmMnt1.Text;
            MainForm.ExcCmdMnt2 = ExcCmmMnt2.Text;
            MainForm.ExcCmdMnt3 = ExcCmmMnt3.Text;
            MainForm.ExcCmd1 = Cmd1.Text;
            MainForm.ExcCmd2 = Cmd2.Text;
            MainForm.ExcCmd3 = Cmd3.Text;
            MainForm.SaveConfigProgram();
            MainForm.ReadConfig();
            Close();
        }
    }
}
