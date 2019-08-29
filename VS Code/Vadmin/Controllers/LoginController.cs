using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Vadmin.Models;

namespace Vadmin.Controllers
{
    public class LoginController : ApiController
    {
        public static string[] user = { "" };
        public static string[] pass = { "" };
        public static string[] lvl = { "" };
        public static string[] img = { "" };


        public string[] GET(int id)
        {
            string[] Rdata = { "" };
            switch (id)
            {
                case 1:
                    Rdata = new string[user.Length];
                    user.CopyTo(Rdata, 0);
                    break;
                case 2:
                    Rdata = new string[pass.Length];
                    pass.CopyTo(Rdata, 0);
                    break;
                case 3:
                    Rdata = new string[lvl.Length];
                    lvl.CopyTo(Rdata, 0);
                    break;
                case 4:
                    Rdata = new string[img.Length];
                    img.CopyTo(Rdata, 0);
                    break;
            }
            return Rdata;
        }

        public void Post(Login p)
        {
            user = p.Username;
            pass = p.Password;
            lvl = p.Lvl;
            img = p.Image;
        }
    }
}
