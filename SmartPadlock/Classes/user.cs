using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using SmartPadlock.Classes;

namespace SmartPadlock.Classes
{
    public class User
    {
        public string NAME_USER { get; set; }
        public DateTime DT_BIRTH_USER { get; set; }
        public string EMAIL_USER { get; set; }
        public string PASSWORD_USER { get; set; }

        public DateTime DT_CREATE = new DateTime();

        public void Create(string name, DateTime birth, string email, string password)
        {
           NAME_USER = name;
           DT_BIRTH_USER = birth;
           EMAIL_USER = email;
           PASSWORD_USER = password;
           
        }




        public bool BuscarUser(bool sign)
        {
            Contract contratoOrigem = new Contract(this, sign);
            try
            {
                string path = ("C:\\Encrypted\\" + contratoOrigem.contrato + ".txt");
                bool result = File.Exists(path);
                if (result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
