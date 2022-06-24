using ClassLibTeam10.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Data.Framework
{
    public static class HandleLogins
    {
        static Dictionary<string, string> loggedIn = new Dictionary<string, string>() { { "naam@mail.com", "ditiseentoken" } };
        
        public static bool TryLogin(this SqlConnection sqlConn, Login login)
        {
            string condition = $"email = '{login.Email}' and wachtwoord = '{login.Wachtwoord}'";
            SelectResult result = sqlConn.SelectRows("klanten", condition);
            if (result.Rows > 1) {
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
                loggedIn.Add(webToken.Email, webToken.Token);
            }
            return webToken;
        }
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
            if (loggedIn.ContainsKey(webToken.Email))
            {
                return true;
            }
            return false;
        }
    }

}
