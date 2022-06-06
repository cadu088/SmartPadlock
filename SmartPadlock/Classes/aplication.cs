using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPadlock.Classes
{
    public class Aplication
    {
        public string APP_ID;

        public string URL;

        public string PASS_ID;

        public DateTime DT_CREATE;

        public void AplicationCreate(string name, string url, string password)
        {
            APP_ID = name;
            URL = url;
            PASS_ID = password;
            DT_CREATE = DateTime.Now;
        }

        public bool AlterPassword(string newPassword)
        {
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
            return true;

        }

        public void ReadingCliente()
        {
            Console.WriteLine("|-|");
            Console.WriteLine("|-|------------------------------------------|-|");
            Console.WriteLine("|-| Aplicação: " + APP_ID);
            Console.WriteLine("|-| URL: " + URL);
            Console.WriteLine("|-| Criação: " + DT_CREATE);
            Console.WriteLine("|-| Senha: " + PASS_ID);
            Console.WriteLine("|-|------------------------------------------|-|");
            Console.WriteLine("|-|");
        }
    }
}
