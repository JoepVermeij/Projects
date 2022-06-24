using ClassLibTeam10.Data.Framework;
using ClassLibTeam10.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ClassLibTeam10.Data
{
    public static class HandleProducts
    {
        public static SelectResult GetProducten(SqlConnection sqlConn)
        {
            return SelectSql.SelectAll(sqlConn, "producten");
        }
        public static UpdateResult UpdateFullProduct(SqlConnection sqlConn, Product product)
        {
            return UpdateSql.UpdateFullRow(sqlConn, product);
        }
        public static SelectResult GetProductenByColumnValue(this SqlConnection sqlConn, string kolom,string naam)
        {
            string condition = $"{kolom} = '{naam}'";
            return sqlConn.SelectRows("producten", condition);
        }
        public static Product CreateFullProduct(DataRowView row)
        {

            Product product = new Product();
            PropertyInfo[] properties = product.GetType().GetProperties();
            //loops over all properties of klant
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                var value = row[i];
                if (value == DBNull.Value)
                {
                    value = null;
                }
                prop.SetValue(product, value);

            }
            return product;
        }
        public static Product CreateFullProduct(DataRow row)
        {

            Product product = new Product();
            PropertyInfo[] properties = product.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (!PropertyHandler.IsPrivateProperty(properties[i]))
                {
                    PropertyInfo prop = properties[i];
                    var value = row[i];
                    if (value == DBNull.Value)
                    {
                        value = null;
                    }
                    prop.SetValue(product, value);
                }
            }
            return product;
        }
        public static Product GetProduct(this SqlConnection sqlConn, int productId)
        {
            string condition = $"productId = '{productId}'";
            Product product = new Product();
            SelectResult result = sqlConn.SelectRows(product, condition);
            DataRow row = result.DataTable.Rows[0];
            return CreateFullProduct(row);
        }
    }
}
