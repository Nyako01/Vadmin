using System.Web.Http;
using Vadmin.Models;

namespace Vadmin.Controllers
{
    public class ConsoleController : ApiController
    {
        public static string[] ConsoleReceive;
        public static string ConsoleSend;

        public string[] GET()
        {
            return ConsoleReceive;
        }

        public string GET(int ID)
        {
            string Send = "";
            switch (ID)
            {
                case 1:
                    Send = ConsoleSend;
                    break;

            }
            return Send;
        }

        public void Post(Console C)
        {
            switch (C.ID)
            {
                case 1:
                    ConsoleReceive = C.ConsoleIN;
                    break;
                case 2:
                    ConsoleSend = C.ConsoleOUT;
                    break;

            }
        }
    }
}
