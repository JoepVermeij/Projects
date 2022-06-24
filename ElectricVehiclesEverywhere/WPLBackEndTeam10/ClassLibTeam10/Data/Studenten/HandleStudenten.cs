using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ClassLibTeam10.Data.Framework;

namespace ClassLibTeam10.Data.Studenten
{
    public static class HandleStudenten
    {
        public static SelectResult GetStudenten(this SqlConnection sqlConn)
        {
            return sqlConn.SelectAll("students");
        }
    }
}
