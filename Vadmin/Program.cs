using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Fivem_Server_Manager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
