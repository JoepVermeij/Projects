xwusing ClassLibTeam10.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Data.Framework
{
    public static class DeleteSql
    {
        public static DeleteResult DeleteRow(this SqlConnection sqlConn,IEntity T)
        {

            try
            {
                var pk = PropertyHandler.GetPrimaryKeyProperty(T);
                int idValue = (int)pk.First().GetValue(T);
                return sqlConn.DeleteRow(pk.First().Name, idValue, T.GetTableName());
            }
            catch (Exception ex)
            {
                throw new Exception("Primary key bestond niet of was geen int.");
            }

        }
        public static DeleteResult DeleteRow(this SqlConnection sqlconn,string id, int idValue, string tableName)
        {
            try
            {
                string query = $"delete from {tableName} where {id} = {idValue}";
                using (SqlCommand deleteCommand = new SqlCommand(query))
                {
                    return sqlconn.DeleteRecord(deleteCommand);
                }
            }
            catch (Exception ex)
            {
                return new DeleteResult() { Succeeded = false, Error = ex.Message };
            }


        }
        public static DeleteResult DeleteAll(this SqlConnection sqlconn, string table)
        {
            try
            {
                string query = $"delete from {table}";
                using (SqlCommand deleteCommand = new SqlCommand(query))
                {
                    return sqlconn.DeleteRecord(deleteCommand);
                }
            }
            catch (Exception ex)
            {
                return new DeleteResult() { Succeeded = false, Error = ex.Message };
            }
        }
        private static DeleteResult DeleteRecord(this SqlConnection sqlConn, SqlCommand deleteCommand)
        {
            try
            {
                deleteCommand.Connection = sqlConn;
                int rows = deleteCommand.ExecuteNonQuery();
                return new DeleteResult() { Succeeded = true, Rows = rows };
            }
            catch (Exception ex)
            {
                return new DeleteResult() { Succeeded = false, Error = ex.Message };
            }
        }

        
    }
}
