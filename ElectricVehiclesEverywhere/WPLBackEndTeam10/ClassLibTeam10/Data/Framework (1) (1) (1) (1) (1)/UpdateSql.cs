using ClassLibTeam10.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Data.Framework
{
    public static class UpdateSql
    {
        public static UpdateResult UpdateRow(this SqlConnection SqlConn, IEntity T)
        {
            try
            {
                PropertyInfo[] nonIdProperties = PropertyHandler.GetNonIdProperties(T);
                string sets = string.Join(", ", nonIdProperties.Select(p => $"{p.Name} = @{p.Name}"));
                string id = PropertyHandler.GetPrimaryKeyProperty(T).First().Name;
                string insertQuery = $"Update {T.GetTableName()} Set {sets} where {id} = @{id}";
                using (SqlCommand updateCommand = new SqlCommand(insertQuery))
                {
                    foreach (PropertyInfo item in T.GetType().GetProperties())
                    {
                        updateCommand.Parameters.AddWithValue($"@{item.Name}", item.GetValue(T) ?? DBNull.Value);
                    }
                    return SqlConn.UpdateRecord(updateCommand);
                }
            }
            catch (Exception ex)
            {
                return new UpdateResult() { Succeeded = false, Error = ex.Message };
            }
        }
        private static UpdateResult UpdateRecord(this SqlConnection sqlConn,SqlCommand updateCommand)
        {
            try
            {
                updateCommand.Connection = sqlConn;
                int rows = updateCommand.ExecuteNonQuery();
                return new UpdateResult() { Succeeded = true, Rows = rows };
            }
            catch (Exception ex)
            {
                return new UpdateResult() { Succeeded = false, Error = ex.Message };
            }

        }
    }
}
