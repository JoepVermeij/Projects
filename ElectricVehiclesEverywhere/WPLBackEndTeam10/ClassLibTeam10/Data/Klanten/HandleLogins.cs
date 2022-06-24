using ClassLibTeam10.Entities;
using ClassLibTeam10.Mail;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Data.Framework
{
    public static class HandleLogins
    {
        //houdt ingelogde users bij
        static ConcurrentDictionary<string, string> loggedIn = new ConcurrentDictionary<string, string>() { };
        static HandleLogins()
        {
            loggedIn.TryAdd("naam@mail.com", "ditiseentoken");
        }
        // TODO: Heeft verbetering nodig
        public static bool TryLogin(this SqlConnection sqlConn, Login login)
        {
            //Does mail and/or salt exist
            string salt;
            try
            {
                salt = sqlConn.GetSalt(login);
            }
            catch
            {
                //error means no mail or salt exists for this login
                return false;
            }
            
            string wachtwoordToHash = login.Wachtwoord + salt;
            login.Wachtwoord = HandlePasswords.ComputeSha256Hash(wachtwoordToHash);
            string condition = $"email = '{login.Email}' and wachtwoord = '{login.Wachtwoord}'";
            //string condition = $"email = @email and wachtwoord = @wachtwoord"; en dit parametriseren om SQL injectie te voorkomen
            SelectResult result = sqlConn.SelectRows("klanten", condition);
            if (result.Rows >= 1) {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static WebToken CreateToken(this Login login)
        {
            WebToken webToken = new WebToken(login.Email);

            if (webToken.IsEmailAlreadyInList())
            {
                loggedIn[webToken.Email] = webToken.Token;
            }
            else
            {
                loggedIn.TryAdd(webToken.Email, webToken.Token);
            }
            return webToken;
        }
        //is token in loggedIn list?
        public static bool IsWebTokenInList(this WebToken webtoken)
        {
            if (webtoken.IsEmailAlreadyInList())
            {
                if (loggedIn[webtoken.Email] == webtoken.Token)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
        public static bool IsEmailAlreadyInList(this WebToken webToken)
        {
            if (webToken.Email!=null&&loggedIn.ContainsKey(webToken.Email))
            {
                return true;
            }
            return false;
        }
        public static bool DeleteWebToken(WebToken webToken)
        {
            if (loggedIn.ContainsKey(webToken.Email))
            {
                return loggedIn.TryRemove(webToken.Email, out string value1);
            }
            else
            {
                return false;
            }
        }
    }
}
