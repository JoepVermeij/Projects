
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibTeam10.IndividueleOpdrachten.VermeijJoep
{
    public class Hobby
    {
        private string naam;
        public string Naam
        {
            get { return naam; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    naam = "Gaming";
                }
                else
                {
                    naam = value;
                }
            }
        }
        private string beschrijving;
        public string Beschrijving
        {
            get { return beschrijving; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    beschrijving = "Het spelen van een videospel op een elektronisch systeem.";
                }
                else
                {
                    beschrijving = value;
                }
            }
        }
        public int AantalUrenPerWeek { get; set; }
        public Hobby()
        {
            this.Naam = "";
            this.Beschrijving = "";
        }
        public Hobby(string naam, string beschrijving)
        {
            this.Naam = naam;
            this.Beschrijving = beschrijving;
        }
        public void SetAantalUrenPerWeek(int[] x)
        {
            if (x == null || x.Length == 0)
            {
                AantalUrenPerWeek = 2;
            }
            else
            {
                decimal totaal=0;
                for (int i = 0; i < x.Length; i++)
                {
                    totaal += x[i];
                }
                AantalUrenPerWeek = Convert.ToInt32(Math.Floor(totaal / (x.Length)));
            }
        }
        public override string ToString()
        {
            return $"{this.Naam}: {this.Beschrijving}";
        }
    }
}
