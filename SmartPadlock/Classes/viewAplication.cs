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
                    
                    Task<string> data = controler.DescriptografarUserAsync(new Contract(user, ContractType.User), new Contract(user, ContractType.Password));

                    string response = await data;

                    if (response != null)
                    {
                        aplications = controler.DeserializeAplicationJSON(response);
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

        public async Task<bool> saveAplication(Login isLogin, User user, List<UserFile> usersFile)
        {
            if (isLogin.status)
            {
                try
                {
                    UserFile userFile = new UserFile(new Contract(user, ContractType.User).Contrato, replaceName(user.NAME_USER));
                    if(usersFile.Count() == 0)
                    {
                        byte[] pass = Encoding.ASCII.GetBytes("1234567890@12345");
                        string descripto = await controler.DescriptografarUserAsync(pass);
                        usersFile = controler.DeserializeUserFileJSON(descripto);
                    }
                    if (!usersFile.Exists(x => x.localUser == userFile.localUser))
                    {
                        usersFile.Add(userFile);
                        if (controler.CriptografarUser("1234567890@12345", controler.ReadingJSON(usersFile)))
                        {
                            return controler.CriptografarUser(new Contract(user, ContractType.User), new Contract(user, ContractType.Password), controler.ReadingJSON(aplications));
                        }
                    }else
                    {
                        return controler.CriptografarUser(new Contract(user, ContractType.User), new Contract(user, ContractType.Password), controler.ReadingJSON(aplications));
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Efetue login para salvar!");
                return false;
            }

        }

        private string replaceName(string value)
        {
            string response = "";
            int de = value.Length / 4;
            int ate = de * 3;
            for (int i = 0; i < value.Length; i++)
            {
                if (i >= de && i <= ate)
                {
                    response += '*';
                }
                else
                {
                    response += value[i];
                }
            }
            return response;
        }
    }
}
