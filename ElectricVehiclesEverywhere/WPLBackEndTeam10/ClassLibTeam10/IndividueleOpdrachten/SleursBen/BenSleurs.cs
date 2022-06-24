using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.SleursBen
{
    public class BenSleurs : Tester
    {
        public BenSleurs()
        {
            this.Hobbies = new List<Hobby>();

            Hobby Gaming = new Hobby();
            Gaming.AantalUrenPerWeek = 18;
            Hobbies.Add(Gaming);

            Hobby Gitaar = new Hobby();
            Gitaar.Naam = "Gitaar";
            Gitaar.Beschrijving = "Muziek proberen te maken met dit meestal 6-snarige instrument";
            Gitaar.AantalUrenPerWeek = 12;
            Hobbies.Add(Gitaar);

            Hobby Nieuws = new Hobby();
            Nieuws.Naam = "Nieuws";
            Nieuws.Beschrijving = "Het lezen of bekijken van videos/artikels over het nieuws, wetenschap, filosofie en politiek";
            Nieuws.AantalUrenPerWeek = 30;
            Hobbies.Add(Nieuws);

        }
        private string voornaam = "Ben";
        private string achternaam = "Sleurs";
        private List<string> quotes = new List<string>()
        {
            "Ik schrijf niet graag 1 quote",
            "Ik schrijf niet graat 2 quotes",
            "Ik schrijf niet graat 3 quotes",
            "Ik schrijf niet graat 4 quotes",
            "Ik schrijf niet graat 5 quotes",
            "Ik schrijf niet graat 6 quotes",
            "Ik schrijf niet graat 7 quotes",
            "Ik schrijf niet graat 8 quotes",
            "Jij niet weten?",
            "Ikke dom"

        };
        Random random = new Random();

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
            return $"{voornaam} {achternaam}";
        }
        public override string GetRandomPersonalQuote()
        {
            return quotes[random.Next(quotes.Count())];
        }

        public override List<string> GetThreeRandomQuotes()
        {
            //List<string> result = new List<string>(quotes);
            //for (int i = 0; i < quotes.Count-3; i++)
            //{
            //    result.RemoveAt(random.Next(quotes.Count - i));
            //}
            //return result;

            List<string> copy = new List<string>(quotes);
            List<string> result = new List<string>();
            int index;
            for (int i = 0; i < 3; i++)
            {
                index = random.Next(copy.Count);
                result.Add(copy[index]);
                copy.RemoveAt(index);
            }
            return result;
            
        }

        public override DateTime GetBirthDay()
        {
            return new DateTime(1993, 28, 06);
        }

        public override int GetAge()
        {
            TimeSpan age = new TimeSpan();
            age = System.DateTime.Now - this.GetBirthDay();
            return (int)Math.Floor(age.TotalDays / 365.2425);
        }

    }
}
