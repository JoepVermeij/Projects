using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibTeam10.Attributes;

namespace ClassLibTeam10.Entities
{
    public class Student: IEntity
    {
        [PrimaryKey]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GetTableName()
        {
            return "students";
        }
    }
}
