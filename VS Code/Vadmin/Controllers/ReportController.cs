using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Vadmin.Models;

namespace Vadmin.Controllers
{
    public class ReportController : ApiController
    {
        public static string[] Action = { "" };
        public static string[] Player = { "" };
        public static string[] Reason = { "" };
        public static string[] Time = { "" };
        public static string[] Reporter = { "" };

        public string[] GET(int id)
        {
            string[] Rdata = { "" };
            switch (id)
            {
                case 1:
                    Rdata = new string[Action.Length];
                    Action.CopyTo(Rdata, 0);
                    break;
                case 2:
                    Rdata = new string[Player.Length];
                    Player.CopyTo(Rdata, 0);
                    break;
                case 3:
                    Rdata = new string[Reason.Length];
                    Reason.CopyTo(Rdata, 0);
                    break;
                case 4:
                    Rdata = new string[Time.Length];
                    Time.CopyTo(Rdata, 0);
                    break;
                case 5:
                    Rdata = new string[Reporter.Length];
                    Reporter.CopyTo(Rdata, 0);
                    break;
            }
                    return Rdata;
        }
        public string GET()
        {
            return "Api detected";
        }

        public static List<string> action = new List<string>();
        public static List<string> player = new List<string>();
        public static List<string> reason = new List<string>();
        public static List<string> time = new List<string>();
        public static List<string> reporter = new List<string>();

        public void POST(Report r)
        {
            time.Add(r.time);
            action.Add(r.action);
            reporter.Add(r.reporter);
            player.Add(r.player);
            reason.Add(r.reason);

            Time = time.ToArray();
            Action = action.ToArray();
            Reporter = reporter.ToArray();
            Player = player.ToArray();
            Reason = reason.ToArray();
            
        }
    }
}
