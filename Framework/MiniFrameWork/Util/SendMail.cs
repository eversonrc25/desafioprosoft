using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;

namespace MiniFrameWork.Util
{
    public class ConfigEmail
    {
        public string host { get; set; } //"smtp.gmail.com"
        public int port { get; set; } //465
        public string user { get; set; } //"suporte@pcode.com.br"
        public String password { get; set; } // "Papo@852456"
        public String from { get; set; }//"suporte@pcode.com.br"

    }

    static public class SendMail
    {

        public static void send(ConfigEmail configEmail, string subject, string body, List<String> listTo)
        {

            SmtpClient client = new SmtpClient(configEmail.host);


            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Port = configEmail.port;
            client.Credentials = new NetworkCredential(configEmail.user, configEmail.password);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(configEmail.from);
            mailMessage.IsBodyHtml = true;
            foreach (String to in listTo)
            {
                mailMessage.To.Add(to);
            }
          
            mailMessage.Body = body;
            mailMessage.Subject = subject;
            client.Send(mailMessage);
        }

    }
}
