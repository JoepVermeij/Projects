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

    public class LogInController : ControllerBase
    {
        private readonly SqlConnection sqlConn;
        public LogInController(SqlConnection conn)
        {
            sqlConn = conn;
        }

        [HttpPost]
        public ActionResult<Login> Login(Login login)
        {
          
            if (sqlConn.DoesLoginExist(login))
            {
                return Ok(login.Login());
            }
            else
            {
                return Unauthorized("login does not exist");
            }
        }
        [HttpPost("check")]
        public ActionResult<bool> IsLoggedIn(WebToken webtoken)
        {
            if (webtoken.IsLoggedIn())
            {
                return Ok(true);
            }
            else
            {
                return Unauthorized(false);
            }
        }
    }
}