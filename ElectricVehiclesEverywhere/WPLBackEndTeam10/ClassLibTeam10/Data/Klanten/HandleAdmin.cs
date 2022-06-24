using ClassLibTeam10.Entities;
using ClassLibTeam10.Mail;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibTeam10.Data.Framework
{
    public static class HandleAdmin
    {
        public static bool IsAdmin(this SqlConnection sqlConn, WebToken webtoken)
        {
            string condition = $"email = '{webtoken.Email}'";
            SelectResult sr = sqlConn.SelectRowsAndColumns("klanten", "IsAdmin", condition);
            //SelectResult sr = sqlConn.SelectAdmin("klanten", $"email = '{webtoken.Email}'");
            return (bool)sr.DataTable.Rows[0][0];

        }
        public static bool IsAdmin(this SqlConnection sqlConn, string email)
        {
            string condition = $"email = '{email}'";
            SelectResult sr = sqlConn.SelectRowsAndColumns("klanten", "IsAdmin", condition);
            return (bool)sr.DataTable.Rows[0][0];

        }
    }
}
