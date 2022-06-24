using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibTeam10.Attributes;

namespace ClassLibTeam10.Entities
{
    public class Klant : IEntity
    {
        [PrimaryKey]
        public int KlantId { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Email { get; set; }
        public string Telefoonnummer { get; set; }
        public DateTime? Geboortedatum { get; set; }
        public bool RijbewijsB { get; set; } = false;
        public bool RijbewijsA { get; set; } = false;
        public string Adres { get; set; }
        public int? Huisnummer { get; set; }
        public string Bus { get; set; }
        public int? Postcode { get; set; }
        public string WoonPlaats { get; set; }
        [Private]
        public string Wachtwoord { get; set; }
        public string Iban { get; set; }
        public string RekeningHouder { get; set; }
        public DateTime? VervalDatum { get; set; }
        [Private]
        public string Salt { get; set; }
        public bool IsAdmin { get; set; }



        public string GetTableName()
        {
            return "klanten";
        }


    }
}
