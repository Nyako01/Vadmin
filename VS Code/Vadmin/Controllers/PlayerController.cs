using System.Web.Http;
using Vadmin.Models;

namespace Vadmin.Controllers
{
    public class PlayerController : ApiController
    {
        public static string[] PlayerD = { "" };
        public static string[] Playerid = { "" };
        public static string[] Playername = { "" };
        public static string[] Playerip = { "" };
        public static string[] Playerping = { "" };
        public static string Playeraction = "";
        public static string[] Playerbanname = { "" };
        public static string[] Playerbanexp = { "" };
        public static string[] Playerbanreason = { "" };
        public string GET()
        {
            return Playeraction;
        }

        public string[] Get(int id)
        {

            string[] Rdata = { "" };
            switch (id)
            {
                case 1:
                    Rdata = new string[Playerid.Length];
                    Playerid.CopyTo(Rdata, 0);
                    break;
                case 2:
                    Rdata = new string[Playername.Length];
                    Playername.CopyTo(Rdata, 0);
                    break;
                case 3:
                    Rdata = new string[Playerip.Length];
                    Playerip.CopyTo(Rdata, 0);
                    break;
                case 4:
                    Rdata = new string[Playerping.Length];
                    Playerping.CopyTo(Rdata, 0);
                    break;
                case 5:
                    Rdata = new string[Playerbanname.Length];
                    Playerbanname.CopyTo(Rdata, 0);
                    break;
                case 6:
                    Rdata = new string[Playerbanexp.Length];
                    Playerbanexp.CopyTo(Rdata, 0);
                    break;
                case 7:
                    Rdata = new string[Playerbanreason.Length];
                    Playerbanreason.CopyTo(Rdata, 0);
                    break;

            }


            return Rdata;
        }

        public void POST(Player pl)
        {
            switch (pl.ID)
            {
                case 1:
                    Playeraction = pl.PlayerAction;
                    break;
                case 2:
                    Playerid = pl.PlayerID;
                    Playername = pl.PlayerName;
                    Playerip = pl.PlayerIP;
                    Playerping = pl.PlayerPing;
                    break;
                case 3:
                    Playerbanname = pl.BanPlayerName;
                    Playerbanexp = pl.BanPlayerExp;
                    Playerbanreason = pl.BanPlayerReason;
                    break;
            }
        }
    }
}
