using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Entities
{
    public class WebToken
    {
        public WebToken()
        {

        }
        public WebToken(string email)
        {
            this.Email = email;
            this.Token = GenerateToken();
        }
        public string Email { get; set; }
        public string Token { get; set; }

        internal string GenerateToken()
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 30; i++)
            {
                int x;
                do
                {
                    x = random.Next(33, 126);
                } while (x==44||x==34||x==96);
                char a = Convert.ToChar(x);
                sb.Append(a);
            }
            return sb.ToString();
        }
    }
}
