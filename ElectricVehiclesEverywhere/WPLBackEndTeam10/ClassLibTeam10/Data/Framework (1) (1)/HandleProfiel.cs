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
    internal static class HandleProfiel
    {
        public static Klant GetProfiel(this SqlConnection sqlConn, WebToken webtoken)
        {
            string condition = $"email = '{webtoken.Email}'";
            Klant klant = new Klant();
            SelectResult result = sqlConn.SelectRows(klant, condition);
            DataRow row = result.DataTable.Rows[0];
            return CreateKlant(row);
        }
        private static Klant CreateKlant(DataRow row)
        {

            Klant klant = new Klant();
            PropertyInfo[] properties = klant.GetType().GetProperties();
            //loops over all properties of klant
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
                    prop.SetValue(klant, value);
                }
            }
            return klant;
        }
        public static int GetKlantId(this SqlConnection sqlConn, WebToken webtoken)
        {
            try
            {
                string condition = $"email = {webtoken.Email}";
                SelectResult sr = sqlConn.SelectRows("klanten", condition);
                return (int)sr.DataTable.Rows[0][0];
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        internal static int GetKlantId(this SqlConnection sqlConn, string email)
        {
            try
            {
                string condition = $"Email = '{email}'";
                SelectResult sr = sqlConn.SelectRows("klanten", condition);
                return (int)sr.DataTable.Rows[0][0];
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
