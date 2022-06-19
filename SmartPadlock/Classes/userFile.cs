using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.IO;

namespace SmartPadlock.Classes
{
    public class UserFile 
    {
        public string localUser;
        public string UserName;

        //public UserFile(User user)
        //{
        //    Contract contratoOrigem = new Contract(user, ContractType.User);
        //    localUser = contratoOrigem.Contrato;
        //    UserName = replaceName(user.NAME_USER);
        //}

        public UserFile(string LocalUser, string userName)
        {
            localUser = LocalUser;
            UserName = userName;
        }

        
    }
}
