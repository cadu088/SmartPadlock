using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPadlock.Classes
{
    internal class createUser
    {
        public bool Create(User usuario)
        {
            var aplicationControler = new aplicationControler();
            Console.Clear();
            Console.WriteLine("|-| Parece que você é novo por aqui, então para começar digite uma senha de uso unico!");
            Console.WriteLine("|-| Essa senha será usada para confirmação dos seus dados sempre que voltar aqui!");
            Console.WriteLine("|-| Recomendamos que use uma senha forte e que seja de no minimo 10 caracteres.");
            Console.WriteLine();

            bool verificaSenha = false;
            while (!verificaSenha)
            {
                Console.Write("|-| Digite uma senha unica de no minimo 10 caracteres para continuar: ");
                usuario.PASSWORD_USER = Console.ReadLine();
                verificaSenha = usuario.PASSWORD_USER.Length >= 10;
            }

            //criar user dps de verificar senha
            if (aplicationControler.CriptografarUser(new Contract(usuario, false), new Contract(usuario, true), ""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
