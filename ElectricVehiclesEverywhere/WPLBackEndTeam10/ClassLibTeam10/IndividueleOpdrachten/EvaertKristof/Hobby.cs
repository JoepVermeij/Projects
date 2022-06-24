using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.EvaertKristof
{
     public class Hobby
    {
        private string naam = "Gaming";

        public string Naam
        {
            get { return naam; }
            set
            {

                if (value == null ||string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value) )
                {

                    naam = "Gaming";
                }
                else
                {
                    naam = value;
                }

            }
        }
        private string beschrijving = "Het spelen van een videospel op een elektronisch systeem.";

        public string Beschrijving
        {
            get { return beschrijving; }
            set
            {
                if (value == null ||string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value) )
                {
                    beschrijving = "Het spelen van een videospel op een elektronisch systeem.";
                }
                else
                {
                    beschrijving = value;
                }
            }
        }

        private int aantalUrenPerWeek;

        public int AantalUrenPerWeek
        {
            get { return aantalUrenPerWeek; }
            set { aantalUrenPerWeek = value; }
        }


        public Hobby()
        {

        }

        public Hobby(string naam, string beschrijving)
        {
            this.naam = naam;
            this.beschrijving = beschrijving;
        }
        public int SetAantalUrenPerWeek(int[] x)
        {
            double som = 0;
            if (x == null || x.Length == 0)
            {
                AantalUrenPerWeek = 2;
            }
            else
            {
                for (int i = 0; i < x.Length; i++)
                {
                    som += x[i];
                }
                AantalUrenPerWeek = Convert.ToInt32(Math.Floor(som / x.Length));

            }
            return AantalUrenPerWeek;
        }
        public override string ToString()
        {
            return $"{this.Naam}: {this.Beschrijving}";
        }
    }
}
