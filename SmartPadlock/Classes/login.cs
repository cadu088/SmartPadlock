using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPadlock.Classes
{
    internal class login
    {

        public bool Create(User usuario)
        {
            try
            {
                var aplicationControler = new aplicationControler();
                Console.Clear();
                Console.WriteLine("| Login |");
                Console.Write("Para verificar suas senhas digite a senha de uso unico: ");

                bool verificaSenha = false;
                usuario.PASSWORD_USER = Console.ReadLine();
                verificaSenha = usuario.PASSWORD_USER.Length >= 10;

                while (!verificaSenha)
                {
                    Console.WriteLine("Digite uma senha valida para continuar: ");
                    usuario.PASSWORD_USER = Console.ReadLine();
                    verificaSenha = usuario.PASSWORD_USER.Length >= 10;
                }


                Console.WriteLine("Seus dados são");
                Task<string> data = aplicationControler.DescriptografarUserAsync(new Contract(usuario, false), new Contract(usuario, true));
                Console.WriteLine(data.Result);
                return true;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
