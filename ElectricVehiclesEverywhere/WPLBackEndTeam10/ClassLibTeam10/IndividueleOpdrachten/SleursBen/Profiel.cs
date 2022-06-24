using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.SleursBen
{
    public abstract class Profiel
    {
        public abstract string GetFirstName();
        public abstract string GetLastName();
        public abstract string GetFullName();
        public abstract string GetRandomPersonalQuote();
        public abstract List<string> GetThreeRandomQuotes();
        public abstract DateTime GetBirthDay();
        public abstract int GetAge();
        public List<Hobby> Hobbies { get; set; }
    }
}
