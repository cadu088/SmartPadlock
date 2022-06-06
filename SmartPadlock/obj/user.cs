using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace SmartPadlock.Classes
{
    public class User
    {
        public string NAME_USER { get; set; }
        public DateTime DT_BIRTH_USER { get; set; }
        public string EMAIL_USER { get; set; }
        public string PASSWORD_USER { get; set; }

        public DateTime DT_CREATE = new DateTime();

        public User(string name, DateTime birth, string email, string password)
        {
            NAME_USER = name;
            DT_BIRTH_USER = birth;
            EMAIL_USER = email;
            PASSWORD_USER = password;
        }

        public bool CadastrarSenha(string APP_ID, string PASS_ID)
        {
            return true;
        }

        public bool CriptografarUser(Contract contratoOrigem, Contract contrato)
        {
            try
            {
                using (FileStream fileStream = new(("C:\\Encrypted\\" + contratoOrigem.contrato + ".txt"), FileMode.OpenOrCreate))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] key = contrato.assinatura;
                        aes.Key = key;

                        byte[] iv = aes.IV;
                        fileStream.Write(iv, 0, iv.Length);

                        using (CryptoStream cryptoStream = new(
                            fileStream,
                            aes.CreateEncryptor(),
                            CryptoStreamMode.Write))
                        {
                            using (StreamWriter encryptWriter = new(cryptoStream))
                            {
                                encryptWriter.WriteLine(NAME_USER);
                                encryptWriter.WriteLine(EMAIL_USER);
                                encryptWriter.WriteLine(DT_BIRTH_USER);
                                encryptWriter.WriteLine(PASSWORD_USER);
                                encryptWriter.WriteLine(DT_CREATE);
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The encryption failed. {ex}");
                return false;
            }
        }

        public async Task<string> DescriptografarUserAsync(Contract contratoOrigem, Contract contrato)
        {
            try
            {
                using (FileStream fileStream = new(("C:\\Encrypted\\" + contratoOrigem.contrato + ".txt"), FileMode.Open))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] iv = new byte[aes.IV.Length];
                        int numBytesToRead = aes.IV.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                            if (n == 0) break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }

                        byte[] key = contrato.assinatura;

                        using (CryptoStream cryptoStream = new(fileStream,aes.CreateDecryptor(key, iv),CryptoStreamMode.Read))
                        {
                            using (StreamReader decryptReader = new(cryptoStream))
                            {
                                string decryptedMessage = await decryptReader.ReadToEndAsync();
                                return decryptedMessage;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Padding is invalid and cannot be removed.") return "Senha invalida!";
                return ex.Message;
            }
        }

        public bool BuscarUser(Contract contratoOrigem)
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
