using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Segurancaapi.Util
{
    public static class PassWordAcces
    {
        private const Int16 MAXIMO_CARACTERES = 40;
        private const String CARACTERES_MAIUSCULOS = "abcdefgijkmnopqrstwxyz";
        private const String CARACTERES_MINUSCULOS = "ABCDEFGHJKLMNPQRSTWXYZ";
        private const String CARACTERES_NUMERICOS = "0123456789";
        private const String CARACTERES_ESPECIAIS = "!@#$%&*()_+=-?{}/:;[]\\|";
        private const String CARACTERES = CARACTERES_MAIUSCULOS + CARACTERES_MINUSCULOS + CARACTERES_NUMERICOS + CARACTERES_ESPECIAIS;

        private const String REGEX_EMAIL = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        private const String REGEX_SENHA_COMPLEXA_C = @"^.*(?=.{4,})((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])).*$";
        private const String REGEX_SENHA_COMPLEXA_A = @"^.*(?=.{4,})((?=.*\d)(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9])).*$";
        private const String REGEX_SENHA_COMPLEXA_M = @"^.*(?=.{2,})((?=.*\d)(?=.*[a-zA-Z])).*$";
        private const String REGEX_SENHA_COMPLEXA_S = @"^.+$";
        private const String REGEX_NUMERO = @"^[0-9]+$";
        private const String FORMATO_REGEX_TEXTO_ENTRE_SIMBOLOS = @"[^a-zA-Z0-9]{0}[^a-zA-Z0-9]";


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
           
           	Encoding altEnc = Encoding.GetEncoding("ISO-8859-1");
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(altEnc.GetBytes(senha));

                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Append text to an existing file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt"), true))
                {
                    outputFile.WriteLine(altEnc.GetString(result));
                }
                return altEnc.GetString(result);
            }
                
           /*// MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
         
            byte[] input = iso.GetBytes(senha);
           // byte[] isoBytes = Encoding.Convert(Encoding.Default, iso, input);
          //  byte[] output = md5.ComputeHash(isoBytes);

            
            using (var md5 = MD5.Create())
                {
                    var result = md5.ComputeHash(input);
                    return iso.GetString(result);
                }*/

                        
          // return iso.GetString(output).ToString();
        }
    }
}


