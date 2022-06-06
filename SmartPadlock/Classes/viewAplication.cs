using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPadlock.Classes
{
    public class viewAplication
    {
        private List<Aplication> aplications = new List<Aplication>();
        private aplicationControler controler = new aplicationControler();
        public async Task<bool> createViewAsync(Login isLogin, User user)
        {
            try
            {
                if (isLogin.status)
                {
                    
                    Task<string> data = controler.DescriptografarUserAsync(new Contract(user, false), new Contract(user, true));

                    string response = await data;

                    if (response != null)
                    {
                        aplications = controler.DeserializeJSON(response);
                    }
                }
                else { return false; }

                return true;
               
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<Aplication> removeAplication(Login isLogin,Aplication removeAt)
        {
            if (isLogin.status)
            {
                aplications.Remove(removeAt);
                return aplications;
            }
            else
            {
                Console.WriteLine("Efetue login para excluir!");
                return aplications;
            }
        }

        public List<Aplication> addAplication(Login isLogin, Aplication addAt)
        {
            
            
            if (isLogin.status)
            {
                if (aplications == null)
                {
                    List<Aplication> apps = new List<Aplication>() { addAt };
                    aplications = apps;
                }
                else
                {
                    aplications.Add(addAt);
                }
                
                return aplications;
            }
            else
            {
                Console.WriteLine("Efetue login para adicionar!");
                return aplications;
            }
        }

        public List<Aplication> view(Login isLogin)
        {
            if (isLogin.status)
            {
                return aplications;
            }
            else
            {
                Console.WriteLine("Efetue login para visualizar!");
                return aplications;
            }
        }

        public bool saveAplication(Login isLogin, User user)
        {
            if (isLogin.status)
            {
                try
                {
                    return controler.CriptografarUser(new Contract(user, false), new Contract(user, true), controler.ReadingJSON(aplications));
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Efetue login para visualizar!");
                return false;
            }

        }
    }
}
