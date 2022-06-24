using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Mail
{
    static class MailServer
    {
        const string mailServer = "smtp.gmail.com";
        const int portNumber = 587;
        const string userName = "top.eve.info@gmail.com";
        const string password = "EVE12345";
        public static string MailForm => userName;
        public static SmtpClient GetSmtpClient()
        {
            var smpClient = new SmtpClient(mailServer, portNumber);
            smpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = userName,
                Password = password
            };
            smpClient.EnableSsl = true;
            return smpClient;
        }
    }
}
