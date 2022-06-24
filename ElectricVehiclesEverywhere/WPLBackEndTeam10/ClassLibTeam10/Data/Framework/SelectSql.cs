using ClassLibTeam10.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Data.Framework
{
    internal static class SelectSql
    {
        internal static SelectResult SelectAll(this SqlConnection sqlConn, string table)
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
        internal static SelectResult SelectAll(this SqlConnection sqlConn, IEntity T)
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
        
        internal static SelectResult SelectRows(this SqlConnection sqlConn, string table, string condition)
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
        internal static SelectResult SelectRowsAndColumns(this SqlConnection sqlConn, string table, string columns, string condition)
        {
            try
            {
                string selectQuery = $"Select {columns} from {table} where {condition}";
                return sqlConn.SelectRecords(selectQuery);
            }
            catch (Exception ex)
            {
                return new SelectResult() { Succeeded = false, Error = ex.Message };
            }
        }
        internal static SelectResult SelectRows(this SqlConnection sqlConn, IEntity T, string condition)
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
        internal static SelectResult SelectAdmin(this SqlConnection sqlConn, IEntity T, string condition)
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
        internal static SelectResult SelectColumns(this SqlConnection sqlConn, string table, string columns)
        {
            try
            {
                string selectQuery = $"Select {columns} from {table}";
                return sqlConn.SelectRecords(selectQuery);
            }
            catch (Exception ex)
            {
                return new SelectResult() { Succeeded = false, Error = ex.Message };
            }
        }

        internal static SelectResult SelectWithJoin(this SqlConnection sqlconn, string table1, string table2, string joinColumn1, string joinColumn2)
        {
            try
            {
                string selectQuery = $"select * from {table1} join {table2} on {table1}.{joinColumn1} = {table2}.{joinColumn2}";
                return sqlconn.SelectRecords(selectQuery);
            }
            catch (Exception ex)
            {
                return new SelectResult() { Succeeded = false, Error = ex.Message };
            }

        }
        internal static SelectResult SelectWithJoinRows(this SqlConnection sqlconn, string table1, string table2,string joinColumn1, string joinColumn2,string condition)
        {
            try
            {
                string selectQuery = $"select * from {table1} join {table2} on {table1}.{joinColumn1} = {table2}.{joinColumn2} where {condition}";
                return sqlconn.SelectRecords(selectQuery);
            }
            catch (Exception ex)
            {
                return new SelectResult() { Succeeded = false, Error = ex.Message };
            }
            
        }
        // De link tussen de 2 tabellen moet zeker te vinden zijn via attributes, mss voor later?
        internal static SelectResult SelectWithJoinRowsAndColumns(this SqlConnection sqlconn, string table1,
            string table2, List<string> columns1, List<string> columns2,
            string joinColumn1, string joinColumn2, string condition)
        {
            try
            {
                string selectQuery = "select ";

                columns1 = columns1.Select(x => table1 + "." + x).ToList();
                columns2 = columns2.Select(x => table2 + "." + x).ToList();
                columns1 = columns1.Union(columns2).ToList();
                var tempString = String.Join(", ", columns1.ToArray());
                selectQuery +=
                    $"{tempString} from {table1} join {table2} on {table1}.{joinColumn1} = {table2}.{joinColumn2} where {condition}";
                return sqlconn.SelectRecords(selectQuery);
            }
            catch (Exception ex)
            {
                return new SelectResult() { Succeeded = false, Error = ex.Message };
            }
            
        }
        internal static SelectResult TempSelectNaam(this SqlConnection sqlConn, string table,string naam)
        {
            string selectQuery = $"select b.ProductId, b.startDatum, b.einddatum from bestellingen b join producten p on b.ProductId = p.ProductId where p.naam ='{naam}'";
            return sqlConn.SelectRecords(selectQuery);
        }
        //internal static SelectResult TempSelectNaamByID(this SqlConnection sqlConn, string table, int id)
        //{
        //    string selectQuery = $"select naam from producten where ProductId ='{id}'";
        //    return sqlConn.SelectRecords(selectQuery);
        //}
        internal static SelectResult TempSelectProductsbyProductId(this SqlConnection sqlConn, string table, int id)
        {
            string selectQuery = $"SELECT ProductId FROM producten WHERE naam = (SELECT naam FROM producten WHERE ProductId='{id}')";
            return sqlConn.SelectRecords(selectQuery);
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


    }
}
