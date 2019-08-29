using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vadmin.Models
{
    public class Dashboard
    {
       public int ID { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Ip { get; set; }
        public string ServerName { get; set; }
        public string PlayerON { get; set; }
        public string PlayerMax { get; set; }
        public string Onesync { get; set; }
        public string Map { get; set; }
        public string ServerMode { get; set; }
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string DISK { get; set; }
        public string NET { get; set; }
        
    }
}