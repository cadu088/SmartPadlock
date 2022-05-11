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
            if (APP_ID == null || URL == null)
            {
                Console.WhiteLine("Não é possivel alterar a senha de uma aplicação nula!");
                return false;
            }
            PASS_ID = newPassword;
            return true;
        }

        public bool AlterData(string newData, string index)
        {
            if (index == "name")
            {
                APP_ID = newData;
            }
            else if (index == "url")
            {
                URL = newData;
            }
            else
            {
                Console.WhiteLine("O index utilizado não é valido... Utilize name para alterar o nome da aplicação ou url para alterar o link da aplicação!");
                return false;
            }
            return true;

        }

        public void ReadingCliente()
        {
            Console.WhiteLine();
            Console.WhiteLine("_________________________________");
            Console.WhiteLine("Aplicação: " + APP_ID);
            Console.WhiteLine("URL: " + URL);
            Console.WhiteLine("Criação: " + DT_CREATE);
            Console.WhiteLine("Senha: " + PASS_ID);
            Console.WhiteLine("_________________________________");
            Console.WhiteLine();
        }
    }
}
