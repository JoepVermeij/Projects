using ClassLibTeam10.Entities;
using ClassLibTeam10.Entities.DbEntities;
using ClassLibTeam10.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClassLibTeam10.Business;
using ClassLibTeam10.Entities.GeneralEntities;

namespace ClassLibTeam10.Data.Framework
{
    public static class HandleProfiel
    {
        static List<string> deleteProfileRequests = new List<string>() { };
        public static SelectResult GetKlanten(SqlConnection sqlConn)
        {
            return SelectSql.SelectAll(sqlConn, "klanten");
        }
        public static UpdateResult UpdateFullKlant(SqlConnection sqlConn, Klant klant)
        {
            return UpdateSql.UpdateFullRow(sqlConn, klant);
        }
        public static InsertResult InsertKlant(SqlConnection sqlConn, Klant klant)
        {
            return InsertsSql.Insert(sqlConn, klant);
        }

        public static InsertResult InsertProduct(SqlConnection sqlConn, Product product)
            
        {
            return InsertsSql.Insert(sqlConn, product);
        }
        public static InsertResult InsertBestelling(SqlConnection sqlConn, Bestelling bestelling)
        {
            return InsertsSql.Insert(sqlConn, bestelling);
        }


        public static Klant GetProfiel(this SqlConnection sqlConn, WebToken webtoken)
        {
            string condition = $"email = '{webtoken.Email}'";
            Klant klant = new Klant();
            SelectResult result = sqlConn.SelectRows(klant, condition);
            DataRow row = result.DataTable.Rows[0];
            return CreateKlant(row);
        }
        //maakt klant exclusief salt en wachtwoord
        public static Klant CreateKlant(DataRow row)
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
        //maakt klant, inclusief salt en wachtwoord
        public static Klant CreateFullKlant(this SqlConnection sqlConn, DataRowView row)
        {

            Klant klant = new Klant();
            PropertyInfo[] properties = klant.GetType().GetProperties();
            //loops over all properties of klant
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                var value = row[i];
                if (value == DBNull.Value)
                {
                    value = null;
                }
                prop.SetValue(klant, value);

            }
            return klant;
        }

        
        public static Klant CreateFullKlant(DataRowView row)
        {

            Klant klant = new Klant();
            PropertyInfo[] properties = klant.GetType().GetProperties();
            //loops over all properties of klant
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                var value = row[i];
                
              
                if (value == DBNull.Value )
                {
                    value = null;
                }
                prop.SetValue(klant, value);

            }
            return klant;
        }

        // public static Bestelling CreateFullBestelling(DataRowView row)
        // {
        //
        //     Bestelling bestelling = new Bestelling();
        //     PropertyInfo[] properties = bestelling.GetType().GetProperties();
        //     //loops over all properties of klant
        //     for (int i = 0; i < properties.Length; i++)
        //     {
        //         PropertyInfo prop = properties[i];
        //         var value = row[i];
        //         if (value == DBNull.Value)
        //         {
        //             value = null;
        //         }
        //         prop.SetValue(bestelling, value);
        //
        //     }
        //     return bestelling;
        // }
        // public static Bestelling CreateFullBestelling(SqlConnection sqlConnection, DataRowView row)
        // {
        //
        //     Bestelling bestelling = new Bestelling();
        //     PropertyInfo[] properties = bestelling.GetType().GetProperties();
        //     //loops over all properties of klant
        //     for (int i = 0; i < properties.Length; i++)
        //     {
        //         PropertyInfo prop = properties[i];
        //         var value = row[i];
        //         if (value == DBNull.Value)
        //         {
        //             value = null;
        //         }
        //         prop.SetValue(bestelling, value);
        //
        //     }
        //     return bestelling;
        // }
        //
        // public static Product CreateFullProduct(DataRowView row)
        // {
        //
        //     Product product = new Product();
        //     PropertyInfo[] properties = product.GetType().GetProperties();
        //     //loops over all properties of klant
        //     for (int i = 0; i < properties.Length; i++)
        //     {
        //         PropertyInfo prop = properties[i];
        //         var value = row[i];
        //         if (value == DBNull.Value)
        //         {
        //             value = null;
        //         }
        //         prop.SetValue(product, value);
        //
        //     }
        //     return product;
        // }
        //
        // public static Product CreateFullProduct(SqlConnection sqlconn, DataRowView row)
        // {
        //
        //     Product product = new Product();
        //     PropertyInfo[] properties = product.GetType().GetProperties();
        //     //loops over all properties of klant
        //     for (int i = 0; i < properties.Length; i++)
        //     {
        //         PropertyInfo prop = properties[i];
        //         var value = row[i];
        //         if (value == DBNull.Value)
        //         {
        //             value = null;
        //         }
        //         prop.SetValue(product, value);
        //
        //     }
        //     return product;
        // }

        public static int GetKlantId(this SqlConnection sqlConn, WebToken webtoken)
        {
            try
            {
                string condition = $"email = '{webtoken.Email}'";
                SelectResult sr = sqlConn.SelectRows("klanten", condition);
                return (int)sr.DataTable.Rows[0][0];
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public static int GetKlantId(this SqlConnection sqlConn, string email)
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

        public static DeleteResult DeleteKlantById(this SqlConnection sqlConn, WebToken webtoken)
        {

            try
            {
                int klantId = GetKlantId(sqlConn, webtoken);
                DeleteResult result = DeleteSql.DeleteRow(sqlConn, "KlantId", klantId, "klanten");
                if (!HandleLogins.DeleteWebToken(webtoken))
                {
                    return new DeleteResult();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static bool ProcessProfileDeleteRequest(this SqlConnection sqlConn, WebToken webToken)
        {

            string token = webToken.Token;
            deleteProfileRequests.Add(token);
            webToken.SendDeleteRequestMail();
            return true;


        }
        public static bool IsValidToken(WebToken webToken)
        {
            if (deleteProfileRequests.Contains(webToken.Token))
            {
                return true;
            }
            else return false;
        }

        public static SelectResult GetBankgegevensByToken(SqlConnection sqlConn, WebToken webtoken)
        {
            if (webtoken.IsLoggedIn())
            {
                string columns = "Iban,RekeningHouder,Vervaldatum";
                string condition = $"email='{webtoken.Email}'";
                
                return SelectSql.SelectRowsAndColumns(sqlConn, "klanten", columns, condition);
            }
            else return new SelectResult() { Succeeded = false, Error = "Webtoken is niet ingelogd"};

        }
        public static UpdateResult UpdateBankGegevens(SqlConnection sqlConn, BankgegevensCheck body)
        {
            string klantCondition = $"email='{body.WebToken.Email}'";
            SelectResult sr = SelectSql.SelectRows(sqlConn, "klanten", klantCondition);
            DataRowView row = sr.DataTable.DefaultView[0];
            Klant klant = CreateFullKlant(row);
            klant.RekeningHouder = body.BankGegevens.RekeningHouder;
            klant.Iban = body.BankGegevens.Iban;
            klant.VervalDatum = body.BankGegevens.VervalDatum;
            return UpdateSql.UpdateFullRow(sqlConn, klant);

        }
        internal static void SendDeleteRequestMail(this WebToken webToken)
        {
            //TODO: SUBMIT KNOP OP MAIL ZELF MOET NOG JUISTE VALUES DOOR TE STUREN
            PxlMail mail = new PxlMail(webToken.Email);
            mail.Subject = "EVE: Delete Account";
            var body = new StringBuilder();
            string action = @"https://localhost:44371/api/mail/deleteprofielconfirmed";
            //body.AppendLine(@"To complete your password change, please click the link below");
            body.AppendLine("<html><head><style>");
            //insert styling hier

            body.AppendLine("form{background: #182c4c;display: block;overflow: auto;}");
            body.AppendLine("p{width: 80%;color: #fff;font-family: Roboto, sans-serif;text-align: center;padding: 1rem 0;border-bottom: 2px solid #00c040;margin: 0 auto;}");
            body.AppendLine("button{display: block;color: #fff;background: #00c040;padding: 1rem 5rem;border: none;border-radius: 25px;margin: 2.5rem auto;transition: all ease-in-out .5s;}");
            body.AppendLine("button:hover{color: #00c040;background: #fff;}");



            body.AppendLine("</style></head><body>");
            body.AppendLine($"<form method = \"post\" action ={action} \"some_page\" class=\"inline\">");
            body.AppendLine($"<input type = \"hidden\" name = \"Email\" value = \"{webToken.Email}\" >");
            body.AppendLine($"<input type = \"hidden\" name = \"Token\" value = \"{webToken.Token}\" >");
            body.AppendLine("<p>");
            body.AppendLine("To delete your profile, please click the link below");
            body.AppendLine("</p>");
            body.AppendLine($"<button type = \"submit\" name = \"submit_param\" value = {webToken.Token} class=\"link-button\">");
            body.AppendLine("Click here to delete profile");
            body.AppendLine("</button></form>");
            body.AppendLine("<body><html>");
            mail.Body = body.ToString();
      
            mail.SendMail();
        }
    }
}
