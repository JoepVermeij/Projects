using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.VanheesBenjamin
{
    public class Hobby
    {
        private string naam;
        public string Naam
        {
            get { return naam; }
            set { naam = (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value)) ? "Gaming" : value; }

        }
        private string beschrijving;

        public string Beschrijving
        {
            get { return beschrijving; }
            set { beschrijving = (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value)) ? "Het spelen van een videospel op een elektronisch systeem." : value; }  
        }
        private int aantalUrenPerWeek;

        public int AantalUrenPerWeek
        {
            get { return aantalUrenPerWeek; }
            set { aantalUrenPerWeek = value; }
        }


        public Hobby() : this("","")
        {

        }
        public Hobby(string naam, string beschrijving)
        {
            this.Naam = naam;
            this.Beschrijving = beschrijving;
        }
        public void SetAantalUrenPerWeek(int[] urenPerWeekArray)
        {
            if (urenPerWeekArray == null || urenPerWeekArray.Length == 0)
            {
                aantalUrenPerWeek = 2;
            }
            else
            {
                int gemiddeldeUrenPerWeek = 0;
                foreach (int item in urenPerWeekArray)
                {
                    gemiddeldeUrenPerWeek += item;
                }
                aantalUrenPerWeek = gemiddeldeUrenPerWeek/urenPerWeekArray.Length;
            }
        }
        public override string ToString()
        {
            return $"{this.Naam}: {this.Beschrijving}";
        }
    }
}
