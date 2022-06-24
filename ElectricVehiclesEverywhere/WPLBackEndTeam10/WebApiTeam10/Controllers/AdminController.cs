using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibTeam10.Entities;
using Newtonsoft.Json;
using System.Data.SqlClient;
using ClassLibTeam10.Settings;
using ClassLibTeam10.Data.Framework;
using ClassLibTeam10.Data;
using System.Data;
using ClassLibTeam10.Business;

namespace WebApiTeam10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly SqlConnection sqlConn;
        public AdminController(SqlConnection conn)
        {
            sqlConn = conn;
        }

        [HttpPost("check")]
        public ActionResult<WebToken> IsAdmin(WebToken webtoken)
        {
            if (webtoken.IsLoggedIn())
            {
                if (sqlConn.IsAdmin(webtoken))
                {
                    SelectResult result = sqlConn.GetKlanten();
                    if (result.Succeeded)
                    {
                        var dt = result.DataTable;
                        string jsonResult = JsonConvert.SerializeObject(dt);
                        return Ok(jsonResult);
                    }
                    else
                    {
                        return this.Problem("Select is niet gelukt");
                    }
                }
                else
                {
                    return Problem("nietadmin");
                }
            }
            else
            {
                return Problem("Klant bestaat niet");
            }
        }
    }
}
