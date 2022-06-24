using ClassLibTeam10.Entities;
using ClassLibTeam10.Mail;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Data.Framework
{
    public static class HandlePasswords
    {
        static ConcurrentDictionary<string, Login> changePasswordRequests = new ConcurrentDictionary<string, Login>() { };
        //hashed wachtwoord van klant
        public static string Hash(this Klant klant)
        {
            string wachtwoord = klant.Wachtwoord;
            klant.Salt = CalculateSalt();
            string savePasword = wachtwoord + klant.Salt;
            return ComputeSha256Hash(savePasword);
        }
        //hashed wachtwoord van login
        public static string Hash(this Login login)
        {
            string wachtwoord = login.Wachtwoord;
            string salt = CalculateSalt();
            string savePasword = wachtwoord + salt;
            return ComputeSha256Hash(savePasword);
        }
        public static string GetSalt(this SqlConnection sqlConn, Login login)
        {
            try
            {
                string condition = $"email = '{login.Email}'";
                string columns = "salt";
                SelectResult sr = sqlConn.SelectRowsAndColumns("klanten", columns, condition);
                string salt = sr.DataTable.Rows[0][0].ToString();
                return salt;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        private static string CalculateSalt()
        {
            Random random = new Random();
            string y = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                int x = random.Next(33, 126);
                char a = Convert.ToChar(x);
                y += a;
            }
            return y;
        }
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        //hashed nieuw wachtwoord en voert het samen met email toe aan changePasswordRequests
        //verstuurt een email om de wachtwoordverandering te confirmen
        /// <summary>
        ///hashed nieuw wachtwoord en voert het samen met email toe aan changePasswordRequests
        ///verstuurt een email om de wachtwoordverandering te confirmen
        /// </summary>
        /// <param name="sqlConn">Connection string naar Db</param>
        /// <param name="login"></param>
        /// <returns></returns>
        public static void ProcessPasswordRequest(this SqlConnection sqlConn,Login login)
        {
            
            login.Wachtwoord = sqlConn.GetHashedPassword(login);
            WebToken webToken = new WebToken(login.Email);
            string token = webToken.Token;
            changePasswordRequests.TryAdd(token, login);
            webToken.SendPasswordRequestMail();


        }
        ///
        public static string GetHashedPassword(this SqlConnection sqlConn,Login login)
        {
            //Does mail and/or salt exist
            string salt;
            try
            {
                salt = sqlConn.GetSalt(login);
            }
            catch
            {
                //TODO moet nog tegoei afgemaakt worden
                throw;
            }

            string wachtwoordToHash = login.Wachtwoord + salt;
            return HandlePasswords.ComputeSha256Hash(wachtwoordToHash);
        }
        public static UpdateResult ChangePassword(this SqlConnection sqlConn,WebToken notInDbToken)
        {
            Klant klant = new Klant();
            klant.KlantId = sqlConn.GetKlantId(notInDbToken.Email);
            string wachtwoord = changePasswordRequests[notInDbToken.Token].Wachtwoord;
            return sqlConn.UpdateColumn(klant, "wachtwoord", wachtwoord);

        }
        public static bool IsValidToken(WebToken notInDbToken)
        {
            if (changePasswordRequests.Keys.Contains(notInDbToken.Token))
            {
                return true;
            }
            else return false;
        }
        public static void SendPasswordRequestMail(this WebToken webToken)
        {
            PxlMail mail = new PxlMail(webToken.Email);
            mail.Subject = "EVE: Password Change";
            var body = new StringBuilder();
            string action = @"https://localhost:44371/api/mail/wachtwoordemailconfirmed";
            body.AppendLine("<html><head><style>");
            //insert styling hier

            body.AppendLine("form{background: #182c4c;display: block;overflow: auto;}");
            body.AppendLine("p{width: 80%;color: #fff;font-family: Roboto, sans-serif;text-align: center;padding: 1rem 0;border-bottom: 2px solid #00c040;margin: 0 auto;}");
            body.AppendLine("button{display: block;color: #fff;background: #00c040;padding: 1rem 5rem;border: none;border-radius: 25px;margin: 2.5rem auto;transition: all ease-in-out .5s;}");
            body.AppendLine("button:hover{color: #00c040;background: #fff;}");



            body.AppendLine("</style></head><body>");
            body.AppendLine($"<form method = \"post\" action ={action} \"some_page\" class=\"inline\">");
            body.AppendLine($"<input type = \"hidden\" name = \"Email\" value = \"{webToken.Email}\" >");
            body.AppendLine($"<input type = \"hidden\" name = \"Token\" value = \"{webToken.Token}\" >");
            body.AppendLine("<p>");
            body.AppendLine("To complete your password change, please click the link below");
            body.AppendLine("</p>");
            body.AppendLine($"<button type = \"submit\" name = \"submit_param\" value = {webToken.Token} class=\"link-button\">");
            body.AppendLine("Click here to confirm password change");
            body.AppendLine("</button></form>");
            body.AppendLine("</body></html>");
            mail.Body = body.ToString();
            mail.SendMail();
        }
        
    }
}
