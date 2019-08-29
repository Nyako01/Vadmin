using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vadmin.Models
{
   public class Report
    {
        public string time { get; set; }
        public string action { get; set; }
        public string reporter { get; set; }
        public string player { get; set; }
        public string reason { get; set; }
    }
}
