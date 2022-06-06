using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPadlock.Classes
{
    public class Login
    {
        public bool status = false;

        public async Task<bool> CreateAsync(User usuario)
        {
            try
            {
                var aplicationControler = new aplicationControler();
                Console.Clear();
                Console.WriteLine("|-| Login |-|");
                Console.Write("|-| Para verificar suas senhas digite a senha de uso unico: ");

                bool verificaSenha = false;
                usuario.PASSWORD_USER = Console.ReadLine();
                verificaSenha = usuario.PASSWORD_USER.Length >= 10;

                while (!verificaSenha)
                {
                    Console.Write("|-| Digite uma senha valida para continuar: ");
                    usuario.PASSWORD_USER = Console.ReadLine();
                    verificaSenha = usuario.PASSWORD_USER.Length >= 10;
                }
                Task<string> data = aplicationControler.DescriptografarUserAsync(new Contract(usuario, false), new Contract(usuario, true));

                string response = await data;

                if(response == "Senha invalida!")
                {
                    Console.Clear();
                    Console.WriteLine("|-| Senha invalida!");
                    status = false;
                    aplicationControler.login = false;
                    return false;
                }
                Console.WriteLine(response);
                status = aplicationControler.login;
                return true;
            }catch (Exception ex)
            {
                status = false;
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
