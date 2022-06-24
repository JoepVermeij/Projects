using ClassLibTeam10.Entities.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Entities.GeneralEntities
{
    public class BestellingCheck
    {
        public WebToken WebToken { get; set; }
        public Bestelling bestelling { get; set; }
    }
}
