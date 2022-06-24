using ClassLibTeam10.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Entities
{
    public interface IEntity
    {
        string GetTableName();
    }
}
