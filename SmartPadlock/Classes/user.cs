using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using SmartPadlock.Classes;

namespace SmartPadlock
{
    public class User
    {
        public string NAME_USER { get; set; }
        public DateTime DT_BIRTH_USER { get; set; }
        public string EMAIL_USER { get; set; }
        public string PASSWORD_USER { get; set; }

        public DateTime DT_CREATE = new DateTime();

        public User()
        {
            var aplicationControler = new aplicationControler();
            Console.Write("Digite o nome completo sem abreviações: ");
            NAME_USER = Console.ReadLine();
            bool verificaEmail = false;
            while (!verificaEmail)
            {
                Console.Write("Digite seu email: ");
                EMAIL_USER = Console.ReadLine();
                verificaEmail = EMAIL_USER.Contains("@");
            }
            Console.Write("Digite sua data de aniversario. Ex: 28/08/2002: ");
            int dia = int.Parse(Console.ReadLine());
            Console.Write("/");
            int mes = int.Parse(Console.ReadLine());
            Console.Write("/");
            int ano = int.Parse(Console.ReadLine());
            DT_BIRTH_USER = new DateTime(ano, mes, dia);



            if (BuscarUser(new Contract(this, false)))
            {
                Console.Clear();
                Console.WriteLine("| Usuario encontrado com sucesso! |");
                Console.WriteLine("-------------------------------");
                Console.WriteLine();
                var login = new login();

                if (login.Create(this))
                {
                    Console.WriteLine("Login efetuado com sucesso!");
                    aplicationControler.login = true;
                }
                else
                {
                    Console.WriteLine("Falha ao efetuar login!");
                    aplicationControler.login = false;
                };
            }
            else
            {
                var createUser = new createUser();
                if (createUser.Create(this))
                {
                    Console.Clear();
                    Console.WriteLine("| Usuario criado com sucesso! |");
                    Console.WriteLine("-------------------------------");
                    aplicationControler.login = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Falha ao criar usuario, tente novamente mais tarde...");
                    Console.WriteLine(":(");
                    aplicationControler.login = false;
                } ;
            }
        }




        private bool BuscarUser(Contract contratoOrigem)
        {
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
