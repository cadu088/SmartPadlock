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

        public Aplication(string name, string url, string password)
        {
            APP_ID = name;
            URL = url;
            PASS_ID = password;
        }

        public bool AlterPassword(string newPassword)
        {
            if(APP_ID == null || URL == null)
            {
                Console.WhiteLine("Não é possivel alterar a senha de uma aplicação nula!");
                return false;
            }
            PASS_ID = newPassword;
            return true;
        }

    }
}
