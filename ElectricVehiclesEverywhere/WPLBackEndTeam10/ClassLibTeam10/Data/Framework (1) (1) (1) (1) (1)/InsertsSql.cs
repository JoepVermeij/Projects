using ClassLibTeam10.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Data.Framework
{
    public static class InsertsSql
    {
        public static InsertResult Insert(this SqlConnection sqlConn, IEntity T )
        {
            InsertResult ir = new InsertResult();
            try
            {
                PropertyInfo[] nonIdProperties = PropertyHandler.GetNonIdProperties(T);
                string columns = string.Join(", ", nonIdProperties.Select(p => p.Name));
                string values = string.Join(", ", nonIdProperties.Select(p => $"@{p.Name}"));
                string insertQuery = $@"insert into {T.GetTableName()} ({columns}) 
                                values ({values})";
                using (SqlCommand insertCommand = new SqlCommand(insertQuery.ToString()))
                {
                    foreach (var item in nonIdProperties)
                    {
                        insertCommand.Parameters.AddWithValue($"@{item.Name}", item.GetValue(T) ?? DBNull.Value);
                    }
                    return sqlConn.InsertRecord(insertCommand);
                }

            }
            catch (Exception ex)
            {
                return new InsertResult() { Succeeded = false, Error = ex.Message };
            }

        }
        private static InsertResult InsertRecord(this SqlConnection sqlConn, SqlCommand insertCommand)
        {
            try
            {
                insertCommand.CommandText += "SET @new_id = SCOPE_IDENTITY();";
                insertCommand.Parameters.Add("@new_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                insertCommand.Connection = sqlConn;
                int rows = insertCommand.ExecuteNonQuery();
                int newId = Convert.ToInt32(insertCommand.Parameters["@new_id"].Value);
                return new InsertResult() { Succeeded = true, NewId = newId, Rows = rows};
            }
            catch (Exception ex)
            {
                return new InsertResult() { Succeeded = false, Error = ex.Message };
            }
        }
    }
}
