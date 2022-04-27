using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;


namespace MiniFrameWork.Util
{
    static public class SenhaADN
    {
        /// <summary>
        /// Tamanho máximo da senha de acordo com a estrutura do banco de dados.
        /// </summary>
        private const Int16 MAXIMO_CARACTERES = 40;
        private const String CARACTERES_MAIUSCULOS = "abcdefgijkmnopqrstwxyz";
        private const String CARACTERES_MINUSCULOS = "ABCDEFGHJKLMNPQRSTWXYZ";
        private const String CARACTERES_NUMERICOS = "0123456789";
        private const String CARACTERES_ESPECIAIS = "!@#$%&*()_+=-?{}/:;[]\\|";
        private const String CARACTERES = CARACTERES_MAIUSCULOS + CARACTERES_MINUSCULOS + CARACTERES_NUMERICOS + CARACTERES_ESPECIAIS;


        /// <summary>
        /// Gerar senha nova com parametro da quantidade mininma de caracteres da senha.
        /// </summary>
        /// <returns>Retorna senha aleatória.</returns>
        public static String gerarSenha(int minCaracteres)
        {
            Int32 posicaoCaractere;
            Int32 posicaoMaxima = CARACTERES.Length;
            Int32 quantidadeCaracteres = minCaracteres;
            List<char> senha = new List<char>();
            Random rnd = new Random();
            StringBuilder retorno = new StringBuilder();

            posicaoCaractere = rnd.Next(0, posicaoMaxima);

            for (int i = 0; i < quantidadeCaracteres; i++)
            {
                while (senha.Contains(CARACTERES[posicaoCaractere]))
                {
                    posicaoCaractere = rnd.Next(0, posicaoMaxima);
                }

                senha.Add(CARACTERES[posicaoCaractere]);
            }

            foreach (Char caractere in senha)
            {
                retorno.Append(caractere);
            }

            return retorno.ToString();
        }

        /// <summary>
        /// Define o tamanho mínimo da senha   
        /// </summary>
        /// <returns>Retorna senha</returns>
        private static Int32 MinCarateresSenha()
        {
            return 5;
        }

        /// <summary>
        /// Criptografar senha em MD5.
        /// </summary>
        /// <param name="senha">Senha a ser criptografada.</param>
        /// <returns>Retorna senha criptografada em MD5.</returns>
        static public String CriptografarSenha(String senha)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] input = Encoding.Default.GetBytes(senha);
            byte[] output = md5.ComputeHash(input);

            return Encoding.Default.GetString(output);
        }


    }
}
