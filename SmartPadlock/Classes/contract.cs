using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SmartPadlock.Classes
{
    public enum ContractType
    {
        User,
        Password
    }

    public class Contract
    {
        public string Contrato { get; private set; }

        private byte[] tmpSource;

        public byte[] Assinatura { get; private set; }

        public Contract(User DATA_USER, ContractType TYPE_CONTRACT)
        {
            //CONTAR QUANTOS TICKES ATÉ A DATA DE NASCIMENTO
            float ticks = (float)(DATA_USER.DT_BIRTH_USER.Ticks / Math.Pow(DATA_USER.DT_BIRTH_USER.Ticks.ToString().Length, 10));

            //CONTAR QUANTOS CARACTERES ANTES DO @ 
            int emailCaracter = DATA_USER.EMAIL_USER.Split('@')[0].Length * 26;
            //SE TYPE_CONTRACT FOR TRUE CONTAMOS A QUANTIDADE DE CARACTER E MULTIPLICAMOS PELA SOBRA DA DIVISÃO POR 3

            float passCont = 0.0f;
            //Verificador de caracter
            double hashVerificador = 0;

            if (TYPE_CONTRACT == ContractType.Password)
            {
                hashVerificador = Math.Pow((verificaCaracter(DATA_USER.NAME_USER) +
                verificaCaracter(DATA_USER.EMAIL_USER) +
                verificaCaracter(DATA_USER.DT_BIRTH_USER.ToString()) +
                verificaCaracter(DATA_USER.PASSWORD_USER )), -1);

                passCont = DATA_USER.PASSWORD_USER.Length * (DATA_USER.PASSWORD_USER.Length % 3 == 0 ? DATA_USER.PASSWORD_USER.Length : (DATA_USER.PASSWORD_USER.Length % 3));
            }
            else
            {
                hashVerificador = Math.Pow((verificaCaracter(DATA_USER.NAME_USER) +
                verificaCaracter(DATA_USER.EMAIL_USER) +
                verificaCaracter(DATA_USER.DT_BIRTH_USER.ToString()) +
                verificaCaracter(DATA_USER.EMAIL_USER + DATA_USER.DT_BIRTH_USER.ToString())), -1) / 0.63;
            }

            //Verificador basico;
            float hashPrimario = (float)((ticks * emailCaracter * (passCont == 0.0f ? (ticks / emailCaracter) : passCont)));

            //Concatena e cria o esqueleto 
            string hashPrefab = (hashPrimario).ToString("E15", CultureInfo.InvariantCulture)
                .Split('.')[1]
                .Split('E')[0] + hashVerificador.ToString().Split(',')[1];
;

            if (hashPrefab.Length < 40)
            {
                int precisao = 40 - hashPrefab.Length;
                hashPrefab += new string('0', precisao);
            }

            else
            {
                string hashPrefabProvisorio = "";
                for (int i = 0; i < 40; i++)
                {
                    hashPrefabProvisorio += hashPrefab[i];
                }
                hashPrefab = hashPrefabProvisorio;
            }


            Contrato = hashPrefab;

            //Gerando Assinatura hash
            string dataAss = "";
            if (TYPE_CONTRACT == ContractType.Password)
            {
                dataAss = (
                verificaCaracter(DATA_USER.PASSWORD_USER) +
                DATA_USER.PASSWORD_USER.ToString() +
                DATA_USER.EMAIL_USER +
                DATA_USER.DT_BIRTH_USER.ToString() +
                DATA_USER.NAME_USER +
                DATA_USER.PASSWORD_USER.Length.ToString() +
                DATA_USER.DT_BIRTH_USER.Ticks.ToString()
                );

            }
            else
            {
                dataAss = (
                verificaCaracter(DATA_USER.EMAIL_USER) +
                verificaCaracter(DATA_USER.NAME_USER+ DATA_USER.EMAIL_USER + DATA_USER.DT_BIRTH_USER.ToString()) +
                DATA_USER.EMAIL_USER +
                DATA_USER.DT_BIRTH_USER.ToString() +
                DATA_USER.NAME_USER +
                verificaCaracter(DATA_USER.NAME_USER) +
                DATA_USER.DT_BIRTH_USER.Ticks.ToString()
                );
            }

            tmpSource = ASCIIEncoding.ASCII.GetBytes(dataAss);
            Assinatura = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            //Console.WriteLine(ByteArrayToString(Assinatura));
        }

        private string ByteArrayToString(byte[] arrInput)
        {
            //Converter o hash para string
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        private float verificaCaracter(string DATA)
        {
            //Criar conta unica pelos caracteres;
            float resultado = 0;
            foreach (char estado in DATA)
            {
                resultado += DATA.IndexOf(estado);
            }
            return resultado;
        }

        public bool validarContrato(byte[] tmpNewHash)
        {
            //validar hash desse contrato com o de outro;
            bool bEqual = false;
            if (tmpNewHash.Length == Assinatura.Length)
            {
                int i = 0;
                while ((i < tmpNewHash.Length) && (tmpNewHash[i] == Assinatura[i]))
                {
                    i += 1;
                }
                if (i == tmpNewHash.Length)
                {
                    bEqual = true;
                }
            }
            return bEqual;
        }
    }
}
