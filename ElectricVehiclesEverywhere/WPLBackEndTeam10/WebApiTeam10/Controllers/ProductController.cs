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
    public class ProductController : ControllerBase
    {
        private readonly SqlConnection sqlConn;
        public ProductController(SqlConnection conn)
        {
            sqlConn = conn;
        }
        [HttpGet]
        public ActionResult GetProducten()
        {
            //NEEDS FIXING
            SelectResult result = sqlConn.GetProducten();
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
        [HttpGet("ByName")]
        public ActionResult GetProductenByName(string naam)
        {
            try
            {
                SelectResult result = sqlConn.GetProductenByName(naam);
                return Ok(result);
            }
            catch (Exception)
            {

                return Problem("Select is niet gelukt");
            }
            
        }
        [HttpGet("FirstByName")]
        public ActionResult GetFirstProductByName(string naam)
            {
            try
            {
                SelectResult result = sqlConn.GetProductenByName(naam);
                Product product = HandleProducts.CreateFullProduct(result.DataTable.DefaultView[0]);
                return Ok(product);
            }
            catch (Exception)
            {

                return Problem("Select is niet gelukt");
            }

        }
    }
}
