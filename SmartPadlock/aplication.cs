using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPadlock
{
    internal class Aplication
    {
        public string APP_ID { get; private set; }
        public string URL { get; private set; }
        private string PASS_ID { get; set; }

        public DateTime DT_CREATE = DateTime.Now;
    }
}
