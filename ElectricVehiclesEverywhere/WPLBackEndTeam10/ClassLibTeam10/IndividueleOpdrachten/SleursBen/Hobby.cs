using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.SleursBen
{
    public class Hobby
    {
        #region Constructors
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
        #endregion
        #region Properties
        private string naam;
        public string Naam
        {
            get { return naam; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    naam= "Gaming";
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
                if (String.IsNullOrWhiteSpace(value))
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
        #endregion
        #region Methods
        public void SetAantalUrenPerWeek(int[] uren)
        {
            if (uren== null || uren.Length == 0)
            {
                this.AantalUrenPerWeek = 2;
            }
            else
            {
                double gemiddelde = uren.Sum() / uren.Length;
                this.AantalUrenPerWeek= (int)Math.Floor(gemiddelde);
            }
        }
        public override string ToString()
        {
            return $"{this.Naam}: {this.Beschrijving}";
        }
        #endregion
    }
}
