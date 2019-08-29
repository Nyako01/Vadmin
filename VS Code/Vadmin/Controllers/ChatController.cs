using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Vadmin.Models;

namespace Vadmin.Controllers
{
    public class ChatController : ApiController
    {
        public static string[] Time = { "" };
        public static string[] ID = { "" };
        public static string[] Name = { "" };
        public static string[] Message = { "" };
        public static string a = "";

        public string GET()
        {
            return a;
        }

        public string[] Get(int id)
        {
            string[] Rdata = { "" };
            switch (id)
            {
                case 1:
                    Rdata = new string[ID.Length];
                    ID.CopyTo(Rdata, 0);
                    break;
                case 2:
                    Rdata = new string[Name.Length];
                    Name.CopyTo(Rdata, 0);
                    break;
                case 3:
                    Rdata = new string[Message.Length];
                    Message.CopyTo(Rdata, 0);
                    break;
                case 4:
                    Rdata = new string[Time.Length];
                    Time.CopyTo(Rdata, 0);
                    break;

            }
            return Rdata;
        }

        public static List<string> time = new List<string>();
        public static List<string> id = new List<string>();
        public static List<string> name = new List<string>();
        public static List<string> message = new List<string>();
        public void POST(Chat ch)
        {
            time.Add(ch.time);
            id.Add(ch.id);
            name.Add(ch.name);
            message.Add(ch.message);

            Time = time.ToArray();
            ID = id.ToArray();
            Name = name.ToArray();
            Message = message.ToArray();
        }
    }
}
