using System;

namespace ClassLibTeam10.Entities.GeneralEntities
{
    public class BankGegevens
    {
        public string Iban { get; set; }
        public string RekeningHouder { get; set; }
        public DateTime? VervalDatum { get; set; }
    }
}