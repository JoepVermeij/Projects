using ClassLibTeam10.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Data.Framework
{
    public static class HandlePasswords
    {
        #region nietlogin functies
        internal static string Hash(this Klant klant)
        {
            string wachtwoord = klant.Wachtwoord;
            klant.Salt = CalculateSalt();
            string savePasword = wachtwoord + klant.Salt;
            return ComputeSha256Hash(savePasword);
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
        private static string ComputeSha256Hash(string rawData)
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

        #endregion
    }
}
