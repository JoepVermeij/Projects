using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Settings
{
    public static class Settings
    {
        public static class Database
        {
            private static string server = @"server=LAPTOP-8PID22OQ\SQLEXPRESS2";
            private static string projectDB = "Database=DB_2022_Team10";
            private static string individualDB = "Database=DB_Joep_Vermeij";
            private static string security = "Integrated security = true";
            private static string projConnString = $"{server};{projectDB};{security}";
            private static string indConnString = $"{server};{individualDB};{security}";

            private static string pxlServer = @"server=10.128.4.7";
            private static string pxlDatabase = @"database=pxl2022Team10"; 
            private static string pxlUser = "User Id=pxluser2022";
            private static string pxlPwd= "Password=pxluser2022";
            private static string pxlConnString= $"{  pxlServer};{pxlDatabase};{pxlUser};{pxlPwd}";
            public static string PxlConnectionString=> pxlConnString;
            public static string Server => server;
            public static string ProjectDb => projectDB;
            public static string IndividualDB => individualDB;
            public static string ProjectConnectionString => projConnString;
            public static string IndividualConnectionString => indConnString;
        }
    }
}