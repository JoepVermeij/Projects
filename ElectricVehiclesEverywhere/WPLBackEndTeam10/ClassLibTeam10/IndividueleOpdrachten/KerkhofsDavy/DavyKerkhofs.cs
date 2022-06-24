using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.KerkhofsDavy
{
    public class DavyKerkhofs : WebDeveloper
    {
        #region Constructors

        public DavyKerkhofs()
        {
            this.Hobbies = new List<Hobby>();

            Hobby gaming = new Hobby();
            gaming.AantalUrenPerWeek = 20;
            Hobbies.Add(gaming);

            Hobby dj = new Hobby();
            dj.Naam = "DJ";
            dj.Beschrijving = "Platen op een draaitafel of platenspeler leggen en aan elkaar mixen door beatmatching";
            dj.AantalUrenPerWeek = 10;
            Hobbies.Add(dj);

            Hobby f1 = new Hobby();
            f1.Naam = "F1";
            f1.Beschrijving = "Het volgen van nieuws over, en het kijken van races in de F1";
            f1.AantalUrenPerWeek = 5;
            Hobbies.Add(f1);
        }

        #endregion

        #region Fields

        public string achternaam = "Kerkhofs";
        
        public string voornaam = "Davy";

        public string[] quotes = new string[] 
        { "You only live once",
          "Live Love Laugh",
          "Work hard, Play hard",
          "Before you criticize someone, walk a mile in their shoes. That way, you’ll be a mile from them, and you’ll have their shoes.",
          "If Cinderella’s shoe fit perfectly, then why did it fall off?",
          "If being awesome was a crime, I would be serving a life sentence.",
          "Sometimes when I close my eyes, I can't see.",
          "An apple a day keeps anyone away, if you throw it hard enough.",
          "Roses are red, my name is not Davy, this makes no sense, microwave.",
          "If you don’t understand my silence how will you understand my words?"
        };

        public DateTime verjaardag = new DateTime(1985,12,23);

        

        #endregion

        #region Properties

        public override string GetFirstName() => voornaam;

        public override string GetLastName() => achternaam;

        public override string GetFullName() => $"{voornaam} {achternaam}";

        public override string GetRandomPersonalQuote() => $"{quotes[new Random().Next(quotes.Length)]}";
            
        public override List<string> GetThreeRandomQuotes()
        {
            List<string> threeRandomQuotes = new List<string>()
            {
                quotes[new Random().Next(10)],
                quotes[new Random().Next(10)],
                quotes[new Random().Next(10)]
            };
            return threeRandomQuotes;
        }

        public override DateTime GetBirthDay() => verjaardag;

        public override int GetAge() => (int)((DateTime.Today - verjaardag).TotalDays / 365.2425);

        #endregion
    }
}
