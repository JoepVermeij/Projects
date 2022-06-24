using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibTeam10.Attributes;

namespace ClassLibTeam10.Entities
{
    public class Product : IEntity
    {
        [PrimaryKey]
        public int ProductId { get; set; }

        public string Merk { get; set; }

        public string  Model { get; set; }

        public string Soort { get; set; }

        public string ImageUrl { get; set; }

        public int ActieRadius { get; set; }

        public int Zitplaatsen { get; set; }

        public int TopSnelheid { get; set; }

        public decimal Accelerate { get; set; }

        public decimal Prijs { get; set; }

        public string KleurVoertuig { get; set; }

        public int LaadTijd { get; set; }

        public string UitlegGebruik { get; set; }

        public int Pk { get; set; }

        public int Vermogen { get; set; }

        public string Type { get; set; }

        public bool RijbewijsA { get; set; }

        public bool RijbewijsB { get; set; }
        public int KmStand { get; set; }
        public string Naam { get; set; }

        public string GetTableName()
        {
            return "producten";
        }
    }
}
