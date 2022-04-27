
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Configuration;
//using Segurancaapi.Camadas.Entidades;
using MiniFrameWork.Dados;
//using WebApiFrameWork.Controllers;
using MiniFrameWork.Util;

namespace WebApiFrameWork.util
{
    static public class Util
    {
        public static bool SendEmail(string emailDestinatario, string AssuntoEmail, string CorpoEmail, bool isAnexo = false, string fileAttachment = null, Dictionary<string, object> host = null)
        {
            SmtpClient client = new SmtpClient();
            NetworkCredential networkCredential = new NetworkCredential();
            //CredentialCache.DefaultCredentials

            string emailHost = string.Empty;
            string emailRequerSSL = string.Empty;
            string emailRequerAutenticacao = string.Empty;
            string emailDominio = string.Empty;
            string emailUsuario = string.Empty;
            string emailSenha = string.Empty;
            string emailRemetente = string.Empty;
            string porta = string.Empty;
            string anexos = string.Empty;

            emailHost = host["HOST_SMTP"].ToString();
            emailRequerSSL = "true";
            emailRequerAutenticacao = "true";
            //emailDominio = SCR_PluginVar.EmailDominio;
            emailUsuario = host["USUARIO_EMAIL"].ToString();
            emailSenha = host["SENHA_USUARIO_EMAIL"].ToString();
            emailRemetente = emailUsuario;
            porta = host["PORTA_HOST_SMTP"].ToString();
            //emailDestinatario = "claudio.magno@adn.com.br";


            client.Port = Convert.ToInt16(porta);
            client.Host = emailHost;
            client.EnableSsl = true;
            //client.Timeout = 10000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(emailUsuario, emailSenha);

            //if (!string.IsNullOrEmpty(emailDominio))
            //    networkCredential.Domain = emailDominio;


            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(emailRemetente);
            String[] arrayTo = emailDestinatario.Split(',');
            foreach (string email in arrayTo)
            {
                if (!string.IsNullOrEmpty(email.Trim()))
                {
                    mm.To.Add(new MailAddress(email.Trim()));
                }
            }

            if (isAnexo)
                mm.Attachments.Add(new Attachment(fileAttachment));

            mm.IsBodyHtml = true;
            mm.Subject = AssuntoEmail;
            mm.Body = CorpoEmail;

            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            try
            {
                client.Send(mm);
            }
            catch// (Exception ex)
            {
                //throw new Exception("Não foi possível enviar email ao usuário " + ex.Message);
                return false;
            }
            finally
            {
                // excluirArquivo(fileAttachment);
            }

            return true;

        }

        public static string createEmailBody(string[] campos)

        {

            string body = string.Empty;

            var arq = Directory.GetCurrentDirectory() + @"\APP_DATA\TemplateEmail.html";
            body = System.IO.File.ReadAllText(arq, Encoding.UTF8);



            body = body.Replace("{Title}", campos[0]);
            body = body.Replace("{comunicado}", campos[1]);
            body = body.Replace("{message}", campos[2]);
            body = body.Replace("{observacao}", campos[3]);
            body = body.Replace("{UserName}", campos[4]);
            body = body.Replace("{contato}", campos[5]);

            return body;

        }

        public static Decimal? calculaImposto(Decimal? basecalculo, Decimal? aliq, Decimal? imposto)
        {
            if (basecalculo != null && aliq != null)
            {
                return basecalculo * aliq / 100;
            }
            else if (basecalculo != null && imposto != null)
            {
                return imposto * 100 / basecalculo;
            }
            else if (aliq != null && imposto != null)
            {
                return imposto * 100 / aliq;
            }
            else
            {
                return 0;
            }
        }

        

        public static void RotinaEnvioSenha(string email, string siteenvio, string token, Dictionary<string, object> host = null)
        {


            String site = String.Format("{0}/Login/RecuperaSenha/{1}", siteenvio, token);
            string Texto = String.Format("Solicitação de troca de senha no Portal dos Fornecedores da Cetrel <br/><br/> Acesse o link abaixo <br/> <a href='{0}'>Link para recuperar senha</a> <br/><br/> Digite a sua nova senha e confirme clicando em Enviar <br/> Em seu proximo acesso, sua nova senha já estará configurada.", site);


            SendEmail(email, "Recuperação de senha", FormatarTextoHTML(Texto), false, null, host);




            //EnviarEmail(email, "Recuperação de senha", FormatarTextoHTML(Texto), string.Empty);
        }

        public static string FormatarTextoHTML(string texto)
        {

            string textoHTML = String.Format(@"
                                <hr />
                                <table align='center' border='0' cellpadding='1' cellspacing='1' style='height:50px; width:100%'>
	                                <tbody>
                                         <span style='text-align: center;'><img src='https://www.cetrel.com.br/wp-content/uploads/2019/10/logo-cetrel-header.png' alt=''></span>
                                        < tr >
			                                <td style='text-align: center; background-color: rgb(14, 61, 146);'><span style='font-family:verdana,geneva,sans-serif'><span style='color:#FFFFFF'><strong><span style='font-size:20px'>Cetrel - Portal dos Fornecedores</span></strong></span></span></td>
		                                </tr>
	                                </tbody>
                                </table>

                                <p style='text-align: center;'>&nbsp;</p>

                                <p style='text-align: center;'><span style='font-family:verdana,geneva,sans-serif'>{0}</span></p>

                                <p>&nbsp;</p>

                                <address><span style='font-size:9px'>Est&aacute; mensagem foi gerada autom&aacute;ticamente pelo Portal do Fornecedor , n&atilde;o responder.</span></address>

                                <address>&nbsp;</address>", texto);

            return textoHTML;


        }

        #region REENVIO SENHA 

        public static void RotinaEnvioSenha(String tx_email, String token,   string siteenvio, IDatabaseMF banco, ConfigEmail configEmail)
        {
            #region HTML
            
            String html;
           
                String site = String.Format("{0}/authentication/recuperasenha/{1}", siteenvio, token);
                html = @"
                                    <div style='margin:0px;background-color:#e1e1e1'>
                                        <style type='text/css'>
                                        @font-face {
                                            font-family: 'My Custom Font';
                                            src: url(fonts/Visby/Webfont/VisbyCF-Thin-webfont.ttf) format('truetype');
                                        }
                                        @font-face {
                                            font-family: 'My Custom Font2';
                                            src: url(fonts/Visby/Webfont/VisbyCF-Bold-webfont.ttf) format('truetype');
                                        }
                                        .customfont {
                                            font-family: 'My Custom Font', Verdana, Tahoma;
                                        }
                                        .customfontB {
                                            font-family: 'My Custom Font2', Verdana, Tahoma;
                                        }
                                        </style>

                                        <div style='background-color: rgb(171, 230, 217);margin:0px;'>

                                        <h1 style='text-align:center;width:100%;padding-top:20px;padding-bottom:20px;margin:0px;'     >
                                        <img style='text-align:center;' src='http://www.adn.com.br/assets/images/logo-adn.png' alt=''>

                                        </h1>
                                        </div>
                                        <div style='background-color:#f8f8f8;margin:20px; '>
                                        <h1 style='color:#0000;text-align:center;width:100%;padding-top:20px;padding-bottom:5px;margin:0px;' class='customfontB'    >Alteração de senha</h1>
                                        <div style='padding-left:20px;padding-right:20px;color:#e1e1e1'><hr  ></div>
                                        <div style='text-align:center;margin-bottom:20px'  class='customfontB'>Solicitação de troca de senha no Workflow</div>
                                        <div style='text-align:center;margin-bottom:10px'  class='customfontB'>Acesse o link abaixo</div>
                                        <div style='text-align:center;margin-bottom:40px'  class='customfontB'><a href='" + site + @"'>Link para recuperar senha</a></div>
                                        <div style='text-align:center;margin-bottom:10px'  class='customfontB'>Digite a sua nova senha e confirme clicando em Enviar</div>
                                        <div style='text-align:center;margin-bottom:10px'  class='customfontB'>Em seu proximo acesso, sua nova senha já estará configurada.</div>
                                        <div style='text-align:center;margin-top:30px;margin-bottom:30px;padding-bottom:30px;'  class='customfontB'>Para ajuda e suporte, envie um email para:</p>
                                        suporte@adn.com.br</div>
                                        </div>
                                        </div>
                                      </div>


                                ";

          

            #endregion


            List<String> enviarPara = new List<String>();
            enviarPara.Add(tx_email);
            MiniFrameWork.Util.SendMail.send(configEmail, "Recuperação de Senha", html, enviarPara);
        }

    
        #endregion


        #region ENVIO E-MAIL

        /// <summary>
        /// Método para enviar email
        /// </summary>
        /// <param name="emailDestinatario">Email do destinatário</param>
        /// <param name="mensagem">senha do usuário</param>
        // public static void EnviarEmail(string emailDestinatario, String Assunto, String Corpo, String ArquivoAnexo)
        // {
        //     try
        //     {
        //         //Cria o objeto que envia o e-mail
        //         SmtpClient client = new SmtpClient();
        //         NetworkCredential networkCredential;

        //         client = new SmtpClient(ConfigurationManager.AppSettings["emailHost"]);
        //         client.EnableSsl = ConfigurationManager.AppSettings["emailRequerSSL"].Equals("true");

        //         try
        //         {
        //             if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["emailPort"]))
        //             {
        //                 client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["emailPort"].ToString());
        //             }
        //         }
        //         catch { }

        //         if (ConfigurationManager.AppSettings["emailRequerAutenticacao"].Equals("true"))
        //         {
        //             networkCredential = new NetworkCredential();
        //             networkCredential.Domain = ConfigurationManager.AppSettings["emailDominio"];
        //             networkCredential.UserName = ConfigurationManager.AppSettings["emailUsuario"];
        //             networkCredential.Password = ConfigurationManager.AppSettings["emailSenha"];
        //             client.Credentials = networkCredential;
        //         }

        //         //Cria o endereço de email do remetente
        //         MailAddress de = new MailAddress(ConfigurationManager.AppSettings["emailRemetente"]);
        //         //Cria o endereço de email do destinatário -->
        //         MailAddress para = new MailAddress(emailDestinatario);

        //         MailMessage emailMensagem = new MailMessage(de, para);

        //         if (!String.IsNullOrEmpty(ArquivoAnexo))
        //             emailMensagem.Attachments.Add(new Attachment(ArquivoAnexo));

        //         emailMensagem.IsBodyHtml = true;

        //         string corpoEmail = String.Empty;

        //         //Assunto do email 
        //         emailMensagem.Subject = Assunto;

        //         //Conteúdo do email
        //         emailMensagem.Body = Corpo;


        //         emailMensagem.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
        //         emailMensagem.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");


        //         //Envia o email
        //         client.Send(emailMensagem);
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new Exception("Não foi possível enviar email ao usuário - " + ex.Message);
        //     }
        // }
        #endregion

        #region FORMATAR CNPJ E CPF

        public static string FormatCNPJ(string CNPJ)
        {
            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }

        /// <summary>
        /// Formatar uma string CPF
        /// </summary>
        /// <param name="CPF">string CPF sem formatacao</param>
        /// <returns>string CPF formatada</returns>
        /// <example>Recebe '99999999999' Devolve '999.999.999-99'</example>

        public static string FormatCPF(string CPF)
        {
            return Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
        }
        /// <summary>
        /// Retira a Formatacao de uma string CNPJ/CPF
        /// </summary>
        /// <param name="Codigo">string Codigo Formatada</param>
        /// <returns>string sem formatacao</returns>
        /// <example>Recebe '99.999.999/9999-99' Devolve '99999999999999'</example>

        public static string SemFormatacao(string Codigo)
        {
            return Codigo.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
        }
        #endregion


        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static List<byte[]> SplitByteArray(byte[] bytes, int BlockLength)
        {
            List<byte[]> _byteArrayList = new List<byte[]>();

            byte[] buffer;

            for (int i = 0; i < bytes.Length; i += BlockLength)
            {
                if ((i + BlockLength) > bytes.Length)
                {
                    buffer = new byte[bytes.Length - i];
                    Buffer.BlockCopy(bytes, i, buffer, 0, bytes.Length - i);
                }
                else
                {
                    buffer = new byte[BlockLength];
                    Buffer.BlockCopy(bytes, i, buffer, 0, BlockLength);
                }

                _byteArrayList.Add(buffer);
            }

            return _byteArrayList;
        }

        public static string FormateCPF(string cnpj)
        {
            return cnpj = cnpj.Substring(0, 9) + "0000" + cnpj.Substring(9);
        }



    }
}
