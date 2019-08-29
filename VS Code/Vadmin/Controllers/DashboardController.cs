using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Linq;
using Vadmin.Models;

namespace Vadmin.Controllers
{
    //[EnableCors(origins: "http://192.168.100.126:44812/api/Dashboard", headers: "*", methods: "*")]

    public class DashboardController : ApiController
    {
        

        public static string Status1 = "Stop";
        public static string Status2 = "STOP";
        public static string Ip = "Null";
        public static string ServerN = "Null";
        public static string PlayerON = "Null";
        public static string PlayerMAX = "Null";
        public static string OneSync = "Null";
        public static string Map = "Null";
        public static string ServerMode = "Null";
        public static string Cpu = "";
        public static string Ram = "-%";
        public static string Disk = "-%";
        public static string Net = "-Kb/s";
       

       
        
        // GET: api/Action/5
        public string Get(int id)
        {
            string Rdata = "";           
            switch (id)
            {
                case 1:
                    Rdata = Status1;
                    break;
                case 2:
                    Rdata = Status2;
                    break;
                case 3:
                    Rdata = Ip;
                    break;
                case 4:
                    Rdata = ServerN;
                    break;
                case 5:
                    Rdata = PlayerON;
                    break;
                case 6:
                    Rdata = PlayerMAX;
                    break;
                case 7:
                    Rdata = OneSync;
                    break;
                case 8:
                    Rdata = Map;
                    break;
                case 9:
                    Rdata = ServerMode;
                    break;
                case 10:
                    Rdata = Cpu;
                    break;
                case 11:
                    Rdata = Ram;
                    break;
                case 12:
                    Rdata = Disk;
                    break;
                case 13:
                    Rdata = Net;
                    break;
                case 14:
                    //Rdata = Console;
                    break;
            }


            return Rdata;
        }

        // POST: api/Action
        public void Post(Dashboard p)
        {                  

            switch (p.ID)
            {
                case 1:
                    Status1 = p.Status1;
                    break;
                case 2:
                    Status2 = p.Status2;
                    break;
                case 3:
                    Ip = p.Ip;
                    ServerN = p.ServerName;
                    PlayerMAX = p.PlayerMax;
                    OneSync = p.Onesync;
                    Map = p.Map;
                    ServerMode = p.ServerMode; 
                    break;
                case 4:
                    PlayerON = p.PlayerON;
                    break;
                case 5:
                    Cpu = p.CPU;
                    Ram = p.RAM;
                    Disk = p.DISK;
                    Net = p.NET;
                    break;
                case 6:
                    
                    break;

            }

        }

    }
}
