using ClassLibTeam10.Data.Framework;
using ClassLibTeam10.Entities.DbEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using ClassLibTeam10.Business;
using ClassLibTeam10.Entities;
using ClassLibTeam10.Mail;
using ClassLibTeam10.Entities.GeneralEntities;

namespace ClassLibTeam10.Data.Bestellingen
{
    public static class HandleBestellingen
    {
        static List<int> deleteProductRequests = new List<int>() { };
        public static SelectResult GetBestellingen(SqlConnection sqlConn)
        {
            return SelectSql.SelectAll(sqlConn, "bestellingen");
        }
        public static UpdateResult UpdateFullBestelling(SqlConnection sqlConn, Bestelling bestelling)
        {
            return UpdateSql.UpdateFullRow(sqlConn, bestelling);
        }
        public static Bestelling CreateFullBestelling(DataRowView row)
        {

            Bestelling bestelling = new Bestelling();
            PropertyInfo[] properties = bestelling.GetType().GetProperties();
            //loops over all properties of klant
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                var value = row[i];
                if (value == DBNull.Value)
                {
                    value = null;
                }
                prop.SetValue(bestelling, value);

            }
            return bestelling;
        }
        public static Bestelling CreateFullBestelling(DataRow row)
        {

            Bestelling bestelling = new Bestelling();
            PropertyInfo[] properties = bestelling.GetType().GetProperties();
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
                    if (prop.PropertyType == typeof(DateTime) && value == null)
                    {
                        value = DateTime.Now;
                    }
                    prop.SetValue(bestelling, value);
                }
            }
            return bestelling;
        }
        public static bool AddBestellingen(this SqlConnection sqlConn, ref Bestelling[] bestellingen, WebToken webToken)
        {
            try
            {
                int klantId = HandleProfiel.GetKlantId(sqlConn, webToken);
                foreach (Bestelling bestelling in bestellingen)
                {
                    bestelling.KlantId = klantId;
                    sqlConn.AddObjectToDataBase(bestelling);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool IsAuthorized(this SqlConnection sqlConn, BestellingenCheck bestellingCheck, out string errorMessage)
        {
            errorMessage = string.Empty;
            Klant klant = sqlConn.GetPublicInfoFromToken(bestellingCheck.WebToken);
            foreach (Bestelling bestelling in bestellingCheck.bestellingen)
            {
                Product product = sqlConn.GetProduct(bestelling.ProductId);
                if (product.RijbewijsA && product.RijbewijsA != klant.RijbewijsA)
                {
                    errorMessage = "Rijbewijs A is nodig om dit voertuig te huren";
                    return false;
                }
                else if (product.RijbewijsB && product.RijbewijsB != klant.RijbewijsB)
                {
                    errorMessage = "Rijbewijs B is nodig om dit voertuig te huren";
                    return false;
                }
                else if(product.RijbewijsA && klant.Geboortedatum > DateTime.Now.AddYears(-16))
                {
                    errorMessage = "Je moet minstenst 16 jaar zijn om dit voertuig te huren";
                    return false;
                }
                else if (product.RijbewijsB && klant.Geboortedatum > DateTime.Now.AddYears(-18))
                {
                    errorMessage = "Je moet minstenst 18 jaar zijn om dit voertuig te huren";
                    return false;
                }
            }
            return true;
        }
        public static List<DateTime> GetOccupiedDates(SqlConnection sqlConn, string naam)
        {
            string condition = $"naam = '{naam}'";
            SelectResult alleProductIDs = SelectSql.SelectRowsAndColumns(sqlConn, "producten", "ProductId", condition);
            SelectResult alleBestellingenVanAuto = SelectSql.TempSelectNaam(sqlConn, "bestellingen", naam);
            var a = alleProductIDs.DataTable.DefaultView;
            var b = alleBestellingenVanAuto.DataTable.DefaultView;
            List<int> idList = ToIdList(a);
            List<(int,DateTime,DateTime)> bestellingList = ToBestellingList(b);
            return GetOccupiedDates(GetOccupiedDatesPerId(idList, bestellingList));
            
        }
        public static List<DateTime> GetOccupiedDates(SqlConnection sqlConn, Bestelling bestelling)
        {
            string condition = $"ProductId = '{bestelling.ProductId}'";
            SelectResult naam = SelectSql.SelectRowsAndColumns(sqlConn, "producten", "naam", condition);
            // SelectResult naam = SelectSql.TempSelectNaamByID(sqlConn, "bestellingen", bestelling.ProductId);
            var a = naam.DataTable.DefaultView;
            string naamProduct = a[0].Row.ItemArray[0].ToString();
            return GetOccupiedDates(sqlConn, naamProduct);
        }
        public static bool AlleBestellingenAvailable(this SqlConnection sqlconn, ref Bestelling[] bestellingen, out string notAvailable)
        {
            notAvailable = string.Empty;
            for (int i = 0; i < bestellingen.Length; i++)
            {
                if (!IsBestellingAvailable(sqlconn, ref bestellingen[i]))
                {
                    Product product = sqlconn.GetProduct(bestellingen[i].ProductId);
                    notAvailable = $"{product.Naam} van {bestellingen[i].StartDatum.ToLongDateString()} tot {bestellingen[i].EindDatum.ToLongDateString()} is niet beschikbaar";
                    return false;
                }
            }
            return true;
        }
        public static int GetAvailableProductId(SqlConnection sqlConn, List<(int, List<DateTime>)> occupiedDatesPerId, Bestelling bestelling)
        {
            SelectResult alleProductIDs = SelectSql.TempSelectProductsbyProductId(sqlConn, "producten", bestelling.ProductId);
            var a = alleProductIDs.DataTable.DefaultView;
            List<int> idList = ToIdList(a);
            for (int i = 0; i < idList.Count; i++)
            {
                bool available = true;
                if (occupiedDatesPerId.ElementAtOrDefault(i).Item2 != null)
                {
                    foreach (DateTime dateTime in occupiedDatesPerId[i].Item2)
                    {
                        if (dateTime >= bestelling.StartDatum && dateTime <= bestelling.EindDatum)
                        {
                            available = false;
                        }
                    }
                }
                if (available)
                {
                    return idList[i];
                }
            }
            return 0;
        }
        public static bool IsBestellingAvailable(SqlConnection sqlConn, ref Bestelling bestelling)
        {
            string condition = $"ProductId = '{bestelling.ProductId}'";
            SelectResult naam = SelectSql.SelectRowsAndColumns(sqlConn, "producten", "naam", condition);
            // SelectResult naam = SelectSql.TempSelectNaamByID(sqlConn, "bestellingen", bestelling.ProductId);
            
            
            var naamView = naam.DataTable.DefaultView;
            string naamProduct = naamView[0].Row.ItemArray[0].ToString();
            condition = $"naam = '{naamProduct}'";
            SelectResult alleProductIDs = SelectSql.SelectRowsAndColumns(sqlConn, "producten", "ProductId", condition);
            SelectResult alleBestellingenVanAuto = SelectSql.TempSelectNaam(sqlConn, "bestellingen", naamProduct);
            var a = alleProductIDs.DataTable.DefaultView;
            var b = alleBestellingenVanAuto.DataTable.DefaultView;
            List<int> idList = ToIdList(a);
            List<(int, DateTime, DateTime)> bestellingList = ToBestellingList(b);
            List<DateTime> occupiedDates = GetOccupiedDates(sqlConn, bestelling);
            foreach (DateTime occupiedDate in occupiedDates)
            {
                if (occupiedDate >= bestelling.StartDatum && occupiedDate <= bestelling.EindDatum)
                {
                    return false;
                }
            }
            int ProductId = GetAvailableProductId(sqlConn, GetOccupiedDatesPerId(idList, bestellingList), bestelling);
            if (ProductId  != 0)
            {
                bestelling.ProductId = ProductId;
                return true;
            }
            else
            {
                return false;
            }
        }
        private static List<int> ToIdList(DataView a)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < a.Count; i++)
            {
                result.Add((int)a[i].Row.ItemArray[0]);
            }
            return result;
        }
        private static List<(int,DateTime,DateTime)> ToBestellingList(DataView view)
        {
            List<(int, DateTime, DateTime)> result = new List<(int, DateTime, DateTime)>();
            for (int i = 0; i < view.Count; i++)
            {
                int a = (int)view[i].Row.ItemArray[0];
                DateTime b = (DateTime)view[i].Row.ItemArray[1];
                DateTime c = (DateTime)view[i].Row.ItemArray[2];

                result.Add((a, b, c));
            }
            return result;
        }

        private static List<(int,List<DateTime>)> GetOccupiedDatesPerId(List<int> ids, List<(int, DateTime, DateTime)> bestellingen)
        {
            List<(int, List<DateTime>)> result = new List<(int, List<DateTime>)>();
            for (int i = 0; i < ids.Count; i++)
            {
                List<(int, DateTime, DateTime)> filteredList = bestellingen.Where(x => x.Item1 == ids[i]).ToList();
                List<DateTime> tempList = new List<DateTime>();
                for (int j = 0; j < filteredList.Count; j++)
                {
                    int interval=(int)Math.Floor((filteredList[j].Item3 - filteredList[j].Item2).TotalDays)+1;
                    for (int k = 0; k < interval; k++)
                    {
                        tempList.Add(filteredList[j].Item2.AddDays(k));
                    }
                }
                result.Add((ids[i],tempList));
            }

            return result;

        }
        
        //returns list with dates where no vehicle of name is available
        private static List<DateTime> GetOccupiedDates(List<(int, List<DateTime>)> dates)
        {
            List<DateTime> result = dates[0].Item2;
            for (int i = 1; i < dates.Count; i++)
            {
                result = result.Intersect(dates[i].Item2).ToList();
            }

            return result;

        }
        private static string GetProductNaam(SqlConnection sqlConn, Bestelling bestelling)
        {
            // werkt dit?! testen!
            string condition = $"ProductId = '{bestelling.ProductId}'";
            SelectResult naam = SelectSql.SelectRowsAndColumns(sqlConn, "producten", "naam", condition);
            // SelectResult result = SelectSql.TempSelectNaamByID(sqlConn, "bestellingen", bestelling.ProductId);
            var a = naam.DataTable.DefaultView;
            return a[0].Row.ItemArray[0].ToString();

        }
        public static bool ProcessProductDeleteRequest(this SqlConnection sqlConn, BestellingCheck bestellingCheck)
        {

            int bestelId = bestellingCheck.bestelling.BestelId;
            deleteProductRequests.Add(bestelId);
            sqlConn.SendDeleteRequestMail(bestellingCheck);
            return true;
        }
        internal static void SendDeleteRequestMail(this SqlConnection sqlConn, BestellingCheck bestellingCheck)
        {
            //TODO: SUBMIT KNOP OP MAIL ZELF MOET NOG JUISTE VALUES DOOR TE STUREN
            PxlMail mail = new PxlMail(bestellingCheck.WebToken.Email);
            mail.Subject = "EVE: Delete Product";
            var body = new StringBuilder();
            string action = @"https://localhost:44371/api/mail/deleteproductconfirmed";
            //body.AppendLine(@"To complete your password change, please click the link below");
            body.AppendLine("<html><head><style>");
            //insert styling hier

            body.AppendLine("form{background: #182c4c;display: block;overflow: auto;}");
            body.AppendLine("p{width: 80%;color: #fff;font-family: Roboto, sans-serif;text-align: center;padding: 1rem 0;border-bottom: 2px solid #00c040;margin: 0 auto;}");
            body.AppendLine("button{display: block;color: #fff;background: #00c040;padding: 1rem 5rem;border: none;border-radius: 25px;margin: 2.5rem auto;transition: all ease-in-out .5s;}");
            body.AppendLine("button:hover{color: #00c040;background: #fff;}");



            body.AppendLine("</style></head><body>");
            body.AppendLine($"<form method = \"post\" action ={action} \"some_page\" class=\"inline\">");
            body.AppendLine($"<input type = \"hidden\" name = \"BestelId\" value = \"{bestellingCheck.bestelling.BestelId}\" >");
            body.AppendLine("<p>");
            body.AppendLine("To delete The following Product, please click the link below");
            body.AppendLine("</p>");
            body.AppendLine("<p>");
            body.AppendLine($"Bestelling {bestellingCheck.bestelling.BestelId}: {bestellingCheck.bestelling.ProductId} - {GetProductNaam(sqlConn, bestellingCheck.bestelling)} - Startdatum:{bestellingCheck.bestelling.StartDatum.ToLongDateString()} - Einddatum:{bestellingCheck.bestelling.EindDatum.ToLongDateString()}");
            body.AppendLine("</p>");
            body.AppendLine($"<button type = \"submit\" name = \"submit_param\" value = {bestellingCheck.WebToken.Token} class=\"link-button\">");
            body.AppendLine("Click here to delete profile");
            body.AppendLine("</button></form>");
            body.AppendLine("</body></html>");
            mail.Body = body.ToString();

            mail.SendMail();
        }
        public static void SendOrderConfirmationMail(this WebToken webToken, SqlConnection sqlConn, Bestelling[] bestellingen)
        {
            PxlMail mail = new PxlMail(webToken.Email);
            mail.Subject = "EVE: Bevesteging Bestelling";
            var body = new StringBuilder();
            body.AppendLine("<html><head><style>");
            //insert styling hier

            body.AppendLine("div{background: #182c4c;display: block;overflow: auto;}");
            body.AppendLine("p{width: 80%;color: #fff;font-family: Roboto, sans-serif;text-align: center;padding: 1rem 0;border-bottom: 2px solid #00c040;margin: 0 auto;}");


            body.AppendLine("</style></head><body><div>");
            body.AppendLine("<p>");
            body.AppendLine("Bestelling Overzicht:");
            body.AppendLine("</p>");
            foreach (Bestelling bestelling in bestellingen)
            {
                body.AppendLine("<p>");
                body.AppendLine($"Bestelling: {GetProductNaam(sqlConn, bestelling)} - Startdatum: {bestelling.StartDatum.ToLongDateString()} - Einddatum: {bestelling.EindDatum.ToLongDateString()}");
                body.AppendLine("</p></div>");
            }
            body.AppendLine("</body></html>");
            mail.Body = body.ToString();
            mail.SendMail();
        }

        public static SelectResult GetBestellingenByToken(this SqlConnection sqlconn, WebToken webtoken)
        {
            
            int klantId = HandleProfiel.GetKlantId(sqlconn, webtoken);
            string condition = $"bestellingen.klantID = {klantId}";
            List<string> columns1 = new List<string>() { "bestelid","startdatum", "einddatum" };
            List<string> columns2 = new List<string>() { "merk", "model", "prijs" };
            return SelectSql.SelectWithJoinRowsAndColumns(sqlconn, "bestellingen", "producten", columns1, columns2,
                "productid", "productid", condition);
        }
        public static DeleteResult CancelBestelling(SqlConnection sqlConn,int bestelId)
        {
            Bestelling bestelling = new Bestelling();
            bestelling.BestelId = bestelId;
            return DeleteSql.DeleteRow(sqlConn, bestelling);
        }
    }
}
