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
using ClassLibTeam10.Entities.GeneralEntities;

namespace WebApiTeam10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //jonguh
    public class KlantenController : ControllerBase
    {
        private readonly SqlConnection sqlConn;
        public KlantenController(SqlConnection conn)
        {
            sqlConn = conn;
        }
        [HttpGet]
        public ActionResult GetKlanten()
        {
            //NEEDS FIXING
            SelectResult result = sqlConn.GetKlanten();
            if (result.Succeeded)
            {
                var dt = result.DataTable;
                string jsonResult = JsonConvert.SerializeObject(dt);
                return Ok(jsonResult);
            }
            else
            {
                return Problem("Select is niet gelukt");
            }


        }
        [HttpPost("add")]
        public ActionResult<Klant> AddKlant(Klant klant)
        {
            try
            {
                klant = klant.HashPassword();
                InsertResult result = sqlConn.AddObjectToDataBase(klant);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPost("updateprofile")]
        public ActionResult<Klant> UpdateKlant(Klant klant)
        {
            try
            {
                UpdateResult result = sqlConn.UpdatePublicPropertiesByObject(klant);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPost("delete")]
        public ActionResult<Klant> DeleteKlant(Klant klant)
        {
            try
            {
                DeleteResult result = sqlConn.DeleteByObject(klant);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPost("deleteprofile")]
        public ActionResult<Klant> DeleteProfile(WebToken webtoken)
        {
            //bestaat webtoken check (als niet throw problem)
            //delete
            try
            {
                return Ok(sqlConn.DeleteKlantById(webtoken));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        
        [HttpPost("getprofielinfo")]
        public ActionResult<Klant> GetProfielInfo(WebToken webtoken)
        {
            try
            {
                if (webtoken.IsLoggedIn())
                {
                    Klant klant = sqlConn.GetPublicInfoFromToken(webtoken);
                    return Ok(klant);
                }
                else return Unauthorized("User is niet ingelogd");

            }
            catch (Exception ex)
            {
                return Problem("klant bestaat niet");
            }
        }
        [HttpPost("getKlantId")]
        public ActionResult<WebToken> GetKlantId(WebToken webtoken)
        {
            try
            {
                if (webtoken.IsLoggedIn())
                {
                    return Ok(HandleProfiel.GetKlantId(sqlConn,webtoken)); ;
                }
                else
                {
                    return Ok("user is not logged in");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("getBankGegevens")]
        public ActionResult<BankGegevens> GetBankGegevens(WebToken webToken)
        {
            try
            {
                SelectResult sr = HandleProfiel.GetBankgegevensByToken(sqlConn, webToken);
                string jsonResult = JsonConvert.SerializeObject(sr.DataTable);
                return Ok(jsonResult);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPost("updateBankGegevens")]
        public ActionResult<BankGegevens> UpdateBankGegevens(BankgegevensCheck body)
        {
            try
            {
                if (body.WebToken.IsLoggedIn())
                {
                    UpdateResult ur = HandleProfiel.UpdateBankGegevens(sqlConn, body);
                    return Ok(true);
                }
                else return Unauthorized();
            }
            catch (Exception)
            {
                return Problem("er is iets misgelopen");
            }
        }
    }
}
