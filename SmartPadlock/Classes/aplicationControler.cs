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

    public class aplicationControler
    {
        public bool login;
        public string ReadingJSON(object request)
        {
            try
            {
                string response = JsonConvert.SerializeObject(request);
                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Erro ao converter dados no bloco ReadingJSON";
            }
        }

        public List<Aplication> DeserializeAplicationJSON(string request)
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
                Console.WriteLine(ex.Message + " - Erro ao converter dados no bloco DeserializeAplicationJSON");
                return response;
            }
        }
        public List<UserFile> DeserializeUserFileJSON(string request)
        {
            List<UserFile> response = new List<UserFile>();
            try
            {
                response = JsonConvert.DeserializeObject<List<UserFile>>(request);
                //if(response == null)
                //{
                //    throw new Exception("Erro de conversão resposta nula.");
                //}
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " - Erro ao converter dados no bloco DeserializeAplicationJSON");
                return response;
            }
        }



        public async Task<string> DescriptografarUserAsync(string destino, byte[] chave)
        {
            try
            {
                using (FileStream fileStream = new(destino, FileMode.Open))
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

                        byte[] key = chave;

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
        public bool CriptografarUser(string destino, byte[] chave, string conteudo)
        {
            try
            {
                if (!string.IsNullOrEmpty(conteudo))
                    if (File.Exists(destino))
                        File.Delete(destino);

                using (FileStream fileStream = new(destino, FileMode.OpenOrCreate))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] key = chave;
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
                                encryptWriter.WriteLine(conteudo);
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


        public async Task<string> DescriptografarUserAsync(Contract contratoOrigem, Contract contrato)
        {
            return await DescriptografarUserAsync(("C:\\Encrypted\\" + contratoOrigem.Contrato + ".txt"), contrato.Assinatura);
        }
        public async Task<string> DescriptografarUserAsync(byte[] chave)
        {
            return await DescriptografarUserAsync("C:\\Encrypted\\List\\6969696969696969696969.txt", chave);
        }


        public bool CriptografarUser(string chave, string conteudo)
        {
            return CriptografarUser("C:\\Encrypted\\List\\6969696969696969696969.txt", Encoding.ASCII.GetBytes(chave), conteudo);
        }
        public bool CriptografarUser(Contract contratoOrigem, Contract contrato, string view)
        {
            return CriptografarUser("C:\\Encrypted\\" + contratoOrigem.Contrato + ".txt", contrato.Assinatura, view);
        }
    }
}
