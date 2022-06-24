using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Entities
{
    public class Login
    {
        public Login()
        {

        }
        public Login(string email, string wachtwoord)
        {
            this.Email = email;
            this.Wachtwoord = wachtwoord;
        }
        public string Email { get; set; }
        public string Wachtwoord { get; set; }
    }
}
