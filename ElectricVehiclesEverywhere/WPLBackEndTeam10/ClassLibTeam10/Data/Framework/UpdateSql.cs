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
    internal static class UpdateSql
    {
        internal static UpdateResult UpdateFullRow(this SqlConnection sqlConn, IEntity T)
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
                    return sqlConn.UpdateRecord(updateCommand);
                }
            }
            catch (Exception ex)
            {
                return new UpdateResult() { Succeeded = false, Error = ex.Message };
            }
        }
        internal static UpdateResult UpdatePartialRow(this SqlConnection sqlConn,IEntity T, Attribute attr, bool includeAttr)
        {
            //
            throw new NotImplementedException();
        }
        //Requires IEntity to have correct ID!!
        internal static UpdateResult UpdateColumn(this SqlConnection sqlConn, IEntity T, string columnName, string value)
        {
            try
            {
                PropertyInfo id = PropertyHandler.GetPrimaryKeyProperty(T).First();
                if (id==null)
                {
                    throw new Exception("er ging iets fout met de id getter");
                }
                string updateQuery = $"Update {T.GetTableName()} Set {columnName}=@{columnName} where {id.Name}=@{id.Name}";
                using (SqlCommand updateCommand = new SqlCommand(updateQuery))
                {
                    updateCommand.Parameters.AddWithValue($"@{columnName}", value);
                    updateCommand.Parameters.AddWithValue($"@{id.Name}",id.GetValue(T)?? DBNull.Value );
                    return sqlConn.UpdateRecord(updateCommand);
                }
            }
            catch (Exception ex)
            {
                return new UpdateResult() { Succeeded = false, Error = ex.Message };
            }

            
        }
        internal static UpdateResult UpdatePublicPropertiesRow(this SqlConnection sqlConn, IEntity T)
        {
            try
            {
                PropertyInfo[] publicProperties = PropertyHandler.GetPublicProperties(T);
                PropertyInfo[] publicPropertiesWithoutId = PropertyHandler.GetNonIdProperties(publicProperties);
                string sets = string.Join(", ", publicPropertiesWithoutId.Select(p => $"{p.Name} = @{p.Name}"));
                string id = PropertyHandler.GetPrimaryKeyProperty(T).First().Name;
                string updateQuery = $"Update {T.GetTableName()} Set {sets} where {id} = @{id}";
                using (SqlCommand updateCommand = new SqlCommand(updateQuery))
                {
                    foreach (PropertyInfo item in publicProperties)
                    {
                        updateCommand.Parameters.AddWithValue($"@{item.Name}", item.GetValue(T) ?? DBNull.Value);
                    }
                    return sqlConn.UpdateRecord(updateCommand);
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
