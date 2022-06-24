using ClassLibTeam10.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Data.Framework
{
    public static class SelectSql
    {
        public static SelectResult SelectAll(this SqlConnection sqlConn, string table)
        {
            try
            {
                string query = $"Select * from {table}";
                return sqlConn.SelectRecords(query);
            }
            catch (Exception ex)
            {
                return new SelectResult() { Succeeded = false, Error = ex.Message };
            }
        }
        public static SelectResult SelectAll(this SqlConnection sqlConn, IEntity T)
        {
            try
            {
                string selectQuery = $"Select * from {T.GetTableName()}";
                return sqlConn.SelectRecords(selectQuery);
            }
            catch (Exception ex)
            {
                return new SelectResult() { Succeeded = false, Error = ex.Message };
            }
        }
        //Deze gaat nog niet werken denk ik, heeft condition paramaters nodig?
        public static SelectResult SelectRows(this SqlConnection sqlConn, string table, string condition)
        {
            try
            {
                string selectQuery = $"Select * from {table} where {condition}";
                return sqlConn.SelectRecords(selectQuery);
            }
            catch (Exception ex)
            {
                return new SelectResult() { Succeeded = false, Error = ex.Message };
            }
        }
        public static SelectResult SelectRows(this SqlConnection sqlConn, IEntity T, string condition)
        {
            try
            {
                return sqlConn.SelectRows(T.GetTableName(), condition);
            }
            catch (Exception ex)
            {
                return new SelectResult() { Succeeded = false, Error = ex.Message };
            }
        }
        private static SelectResult SelectRecords(this SqlConnection sqlConn, string query)
        {
            try
            {
                SelectResult sr = new SelectResult();
                SqlCommand sqlComm = new SqlCommand(query, sqlConn);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlComm);
                sr.DataTable = new System.Data.DataTable();
                sr.Rows = adapter.Fill(sr.DataTable);
                sr.Succeeded = true;
                return sr;
            }
            catch (Exception ex)
            {
                return new SelectResult() { Succeeded = false, Error = ex.Message };
            }
        }
        public static bool CheckLogIn(this SqlConnection sqlConn, Login login)
        {
            string condition = $"email = '{login.Email}' and wachtwoord = '{login.Wachtwoord}'";
            SelectResult sr = sqlConn.SelectRows("klanten",condition);
            if (sr.Rows > 1)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

    }
}
