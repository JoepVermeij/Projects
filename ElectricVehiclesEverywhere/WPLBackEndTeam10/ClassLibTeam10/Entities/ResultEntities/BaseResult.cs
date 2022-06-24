using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Data.Framework
{
    public abstract class BaseResult
    {
        public int Rows { get; set; }
        public bool Succeeded { get; set; }
        public string Error { get; set; }
    }
}
