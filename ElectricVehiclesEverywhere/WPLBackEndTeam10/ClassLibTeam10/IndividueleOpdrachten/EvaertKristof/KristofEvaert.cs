using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.EvaertKristof
{
    public class KristofEvaert : Csharpdeveloper
    {
        private string Voornaam = "Kristof";
        private string Naam = "Evaert";
        public  List<Hobby> Hobbies = new List<Hobby>();
       List<string> lijst = new List<string>();
        public KristofEvaert()
        {
            var eersteHobbie = new Hobby();
            eersteHobbie.Naam = "schaken";
            eersteHobbie.Beschrijving = "leuk om te doen";
            var tweedeHobbie = new Hobby();
            tweedeHobbie.Naam = "wandelen";
            tweedeHobbie.Beschrijving = "gezond he";
            var derdeHobbie = new Hobby();
            derdeHobbie.Naam = "fietsen";
            derdeHobbie.Beschrijving = "snelleeeeeer";
            Hobbies.Add(eersteHobbie); 
            Hobbies.Add(tweedeHobbie);
            Hobbies.Add(derdeHobbie);
        }

        public override string GetFirstName()
        {
            return Voornaam;
        }

        public override string GetLastName()
        {
            return Naam;
        }

        public override string GetFullName()
        {
            return Voornaam + " " + Naam;
        }

        public override string GetRandomPersonalQuote()
        {
            Random rnd = new Random();
            for (int i = 0; i < 3; i++)
            {
                int x =  rnd.Next(9);
                string a = lijst[x];
                return a;
            }
            return "";

        }

        public override List<string> GetThreeRandomQuotes()
        {
            
            lijst.Add("Vini Vidi Vici");
            lijst.Add("Als de dag van toen");
            lijst.Add("De onchtend stond heeft goud in de mond");
            lijst.Add("schaadt het niet dan baat het niet");
            lijst.Add("de vroege volgel vangt de wormen");
            lijst.Add("oost west thuis best");
            lijst.Add("what doesn kill you makes you stronger");
            lijst.Add("de appel valt niet ver van de boom");
            lijst.Add("geef er een lap, op");
            lijst.Add("na vandaag komt morgen");
            return lijst;

        }

        public override DateTime GetBirthDay()
        {
            DateTime gbDatum = new DateTime(1982,07,18);
            return gbDatum;
        }

        public override int GetAge()
        {
            TimeSpan leeftijd = DateTime.Now - GetBirthDay();

            int x = Convert.ToInt32(leeftijd);

            return x;
        }
    }
}
