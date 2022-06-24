using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.VermeijJoep
{
    public class VermeijJoep : Scrummaster
    {
        private string voorNaam = "Joep";
        private string achterNaam = "Vermeij";
        private List<string> quotes = new List<string>() 
        { 
            "Be yourself; everyone else is already taken.",
            "I'm selfish, impatient and a little insecure. I make mistakes, I am out of control and at times hard to handle. But if you can't handle me at my worst, then you sure as hell don't deserve me at my best.",
            "Two things are infinite: the universe and human stupidity; and I'm not sure about the universe.",
            "So many books, so little time.",
            "A room without books is like a body without a soul.",
            "Be who you are and say what you feel, because those who mind don't matter, and those who matter don't mind.",
            "You've gotta dance like there's nobody watching, Love like you'll never be hurt, Sing like there's nobody listening, And live like it's heaven on earth.",
            "You know you're in love when you can't fall asleep because reality is finally better than your dreams.",
            "You only live once, but if you do it right, once is enough.",
            "Be the change that you wish to see in the world."
        };
        public VermeijJoep()
        {
            this.Hobbies = new List<Hobby>();
            Hobby fitness = new Hobby("Fitness", "Sporten");
            Hobby gaming = new Hobby("Gaming", "Computer");
            Hobby koken = new Hobby("Koken", "keuken");

            Hobbies.Add(fitness);
            Hobbies.Add(gaming);
            Hobbies.Add(koken);
        }
        public override string GetFirstName()
        {
            return voorNaam;
        }
        public override string GetLastName()
        {
            return achterNaam;
        }
        public override string GetFullName()
        {
            return voorNaam + " " + achterNaam;
        }
        public override string GetRandomPersonalQuote()
        {
            Random random = new Random();
            return quotes[random.Next(10)];
        }
        public override List<string> GetThreeRandomQuotes()
        {
            Random random = new Random();
            List<string> drieQuotes = new List<string>()
            {
                quotes[random.Next(10)],
                quotes[random.Next(10)],
                quotes[random.Next(10)]
            };
            return drieQuotes;
        }
        public override DateTime GetBirthDay()
        {
            DateTime geboorteJaar = new DateTime(1995, 06, 05);
            return geboorteJaar;
        }
        public override int GetAge()
        {
            DateTime geboorteJaar = new DateTime(1995, 06, 05);
            return DateTime.Now.Year - geboorteJaar.Year;

        }
    }
}
