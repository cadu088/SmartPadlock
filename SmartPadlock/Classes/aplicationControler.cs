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
                string response = JsonConvert.SerializeObject(request);
                int i = response.Length;
                return response;

            }
            catch (Exception ex)
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
                //if(response == null)
                //{
                //    throw new Exception("Erro de conversão resposta nula.");
                //}
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
                                login = true;
                                return decryptedMessage;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                login = false;
                return "Senha invalida!";
            }
        }

        public bool CriptografarUser(Contract contratoOrigem, Contract contrato, string view)
        {
            try
            {
                if (view != "")
                {
                    if (File.Exists(("C:\\Encrypted\\" + contratoOrigem.contrato + ".txt")))
                    {
                        File.Delete(("C:\\Encrypted\\" + contratoOrigem.contrato + ".txt"));
                    }
                }
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
                                encryptWriter.WriteLine(view);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na criptografia. {ex}");
                return false;
            }
        }
    }
}
