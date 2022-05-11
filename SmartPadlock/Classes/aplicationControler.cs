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
    internal class aplicationControler
    {
        public bool login;
        public string ReadingJSON(List<Aplication> request)
        {
            try
            {
                return JsonConvert.SerializeObject(request);

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Erro ao converter dados no bloco ReadingJSON";
            }
        }

        public List<Aplication> DeserializeJSON(string request)
        {
            List<Aplication> response = new List<Aplication>();
            try
            {
                response = JsonConvert.DeserializeObject<List<Aplication>>(request);
                if(response == null)
                {
                    throw new Exception("Erro de conversão resposta nula.");
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " - Erro ao converter dados no bloco DeserializeJSON");
                return response;
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

                        using (CryptoStream cryptoStream = new(fileStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
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

        public bool CriptografarUser(Contract contratoOrigem, Contract contrato, User usuario)
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
                                encryptWriter.WriteLine(usuario.NAME_USER);
                                encryptWriter.WriteLine(usuario.EMAIL_USER);
                                encryptWriter.WriteLine(usuario.DT_BIRTH_USER);
                                encryptWriter.WriteLine(usuario.PASSWORD_USER);
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
    }
}
