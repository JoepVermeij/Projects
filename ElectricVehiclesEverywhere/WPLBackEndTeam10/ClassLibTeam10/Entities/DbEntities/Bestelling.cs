using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibTeam10.Attributes;

namespace ClassLibTeam10.Entities.DbEntities
{
    public class Bestelling : IEntity
    {
        [PrimaryKey]
        public int BestelId { get; set; }
        [ForeignKey]
        public int ProductId { get; set; }
        [ForeignKey]
        public int KlantId { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public string GetTableName()
        {
            return "bestellingen";
        }
    }
}
