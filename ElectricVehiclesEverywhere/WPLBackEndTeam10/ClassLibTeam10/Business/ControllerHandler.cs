using ClassLibTeam10.Data;
using ClassLibTeam10.Data.Framework;
using ClassLibTeam10.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Business
{

    //Deze klasse roept interne methodes uit Handle klasses op
    public static class ControllerHandler
    {
        public static SelectResult GetKlanten(this SqlConnection sqlConn)
        {
            return sqlConn.SelectAll("klanten");
        }

        public static SelectResult GetProducten(this SqlConnection sqlConn)
        {
            return sqlConn.SelectAll("producten");
        }
        public static SelectResult GetProductenByName(this SqlConnection sqlConn, string naam)
        {
            return sqlConn.GetProductenByColumnValue("naam", naam);
        }
        public static int GetKlantId(this SqlConnection sqlConn, WebToken webtoken)
        {
            return sqlConn.GetKlantId(webtoken.Email);
        }
        public static InsertResult AddKlantDataBase(this SqlConnection sqlConn, Klant T)
        {
            return sqlConn.Insert(T);
        }
        public static InsertResult AddObjectToDataBase(this SqlConnection sqlConn, IEntity T)
        {
            return sqlConn.Insert(T);
        }

        public static UpdateResult UpdateByObject(this SqlConnection sqlConn, IEntity T)
        {
            return sqlConn.UpdateFullRow(T);
        }
        public static UpdateResult UpdatePublicPropertiesByObject(this SqlConnection sqlConn, IEntity T)
        {
            
            return sqlConn.UpdatePublicPropertiesRow(T);
        }
        public static DeleteResult DeleteByObject(this SqlConnection sqlConn, IEntity T)
        {
            return sqlConn.DeleteRow(T);
        }
        public static bool DoesLoginExist(this SqlConnection sqlConn, Login login)
        {
            return sqlConn.TryLogin(login);
        }
        public static WebToken Login(this Login login)
        {
            return login.CreateToken();
        }
        public static bool IsLoggedIn(this WebToken webtoken)
        {
            return webtoken.IsWebTokenInList();
        }
        public static Klant HashPassword(this Klant klant)
        {
            klant.Wachtwoord = klant.Hash();
            return klant;
        }
       public static Klant GetPublicInfoFromToken(this SqlConnection sqlConn, WebToken webtoken)
        {
            return sqlConn.GetProfiel(webtoken);
        }
    }
}
