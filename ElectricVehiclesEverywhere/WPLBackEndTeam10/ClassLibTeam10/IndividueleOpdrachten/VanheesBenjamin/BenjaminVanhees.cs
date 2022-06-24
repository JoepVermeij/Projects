using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.VanheesBenjamin
{
    public class BenjaminVanhees : Designer
    {
        private string voornaam = "Benjamin", achternaam = "Vanhees";
        private List<string> quotes = new List<string>()
        {
            "The way to get started is to quit talking and begin doing.",
            "Your time is limited, so don't waste it living someone else's life. Don't be trapped by dogma – which is living with the results of other people's thinking.",
            "If life were predictable it would cease to be life, and be without flavor.",
            "Spread love everywhere you go. Let no one ever come to you without leaving happier.",
            "Always remember that you are absolutely unique. Just like everyone else.",
            "Tell me and I forget. Teach me and I remember. Involve me and I learn.",
            "The best and most beautiful things in the world cannot be seen or even touched — they must be felt with the heart.",
            "It is during our darkest moments that we must focus to see the light.",
            "Whoever is happy will make others happy too.",
            "Many of life's failures are people who did not realize how close they were to success when they gave up."

        };
        public BenjaminVanhees()
        {
            Hobbies = new List<Hobby>();
            Hobby lezen = new Hobby("lezen", "fantasieboeken lezen");
            Hobby lezen2 = new Hobby("lezen", "fantasieboeken lezen");
            Hobby lezen3 = new Hobby("lezen", "fantasieboeken lezen");
            Hobbies.Add(lezen);
            Hobbies.Add(lezen2);
            Hobbies.Add(lezen3);
        }
        public override string GetFirstName()
        {
            return voornaam;
        }
        public override string GetLastName()
        {
            return achternaam;
        }
        public override string GetFullName()
        {
            return voornaam + " " + achternaam;
        }
        public override string GetRandomPersonalQuote()
        {
            Random random = new Random();
            return quotes[random.Next(quotes.Count)-1];
        }
        public override List<string> GetThreeRandomQuotes()
        {
            List<string> threeRandomQuotesList = new List<string>();
            do
            {
                string quote = GetRandomPersonalQuote();
                if (!threeRandomQuotesList.Contains(quote))
                {
                    threeRandomQuotesList.Add(quote);
                }
            } while (threeRandomQuotesList.Count < 3);
            return threeRandomQuotesList;
        }
        public override DateTime GetBirthDay()
        {
            return new DateTime(1993, 7, 27);
        }
        public override int GetAge()
        {
            DateTime geboortedatum = GetBirthDay();
            int leeftijd = DateTime.Today.Year - geboortedatum.Year;
            if (geboortedatum.Date > DateTime.Today.AddYears(-leeftijd)) leeftijd--;
            return leeftijd;
        }
    }
}
