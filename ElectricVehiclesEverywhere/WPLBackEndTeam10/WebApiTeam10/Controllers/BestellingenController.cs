using ClassLibTeam10.Data.Bestellingen;
using ClassLibTeam10.Entities;
using ClassLibTeam10.Entities.DbEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ClassLibTeam10.Entities.GeneralEntities;
using ClassLibTeam10.Business;
using ClassLibTeam10.Data.Framework;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;

//een comment
namespace WebApiTeam10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestellingenController : ControllerBase
    {
        private readonly SqlConnection sqlConn;
        public BestellingenController(SqlConnection conn)
        {
            sqlConn = conn;
        }
        [HttpGet("OccupiedDates")]
        public ActionResult GetOccupiedDates(string naam)
        {
            try
            {
                var dates = HandleBestellingen.GetOccupiedDates(sqlConn, naam);
                return Ok(dates);
            }
            catch (Exception)
            {

                return Problem("Select is niet gelukt");
            }

        }
        [HttpPost("GetBestellingenFromWebToken")]
        public ActionResult GetBestellingenFromWebtoken(WebToken webtoken)
        {
            
            if (!HandleLogins.IsWebTokenInList(webtoken))
            {
                return Unauthorized("User is not logged in");
            }
            else
            {
                SelectResult result = HandleBestellingen.GetBestellingenByToken(sqlConn, webtoken);
                var jsonResult = JsonConvert.SerializeObject(result.DataTable);
                return Ok(jsonResult);
            }

        }

        [HttpPost("OrderCheck")]
        public ActionResult PostOrderCheck(BestellingenCheck bestellingCheck)
        {
            Bestelling[] bestellingen = bestellingCheck.bestellingen;
            

            if (!bestellingCheck.WebToken.IsLoggedIn())
            {
                return Unauthorized("U bent niet ingelogd.");
            }
            else if (!sqlConn.IsAuthorized(bestellingCheck,out string errorMessage))
            {
                return StatusCode(403,errorMessage); // Forbid
            }
            else if (!sqlConn.AlleBestellingenAvailable(ref bestellingen, out string notAvailable))
            {
                return StatusCode(409, notAvailable); // Conflict
            }
            else if (!sqlConn.AddBestellingen(ref bestellingen, bestellingCheck.WebToken))
            {
                return Problem("Bestelling is mislukt");
            }
            else
            {
                bestellingCheck.WebToken.SendOrderConfirmationMail(sqlConn, bestellingen);
                return Ok("Bestelling is gelukt");
            }
        }
        public struct BestellingWithToken
        {
            public int BestelId { get; set; }
            public WebToken WebToken { get; set; }
        }
        [HttpPost("CancelBestelling")]
        public ActionResult<string> CancelBestelling(BestellingWithToken body)
        {
            if (!body.WebToken.IsLoggedIn())
            {
                return Unauthorized("Je bent niet meer ingelogd.");
            }
            else {
                try
                {
                    DeleteResult dr = HandleBestellingen.CancelBestelling(sqlConn,body.BestelId);
                    return Ok("\"Bestelling is succesvol verwijderd.\"");
                }
                catch (Exception ex)
                {
                    return Problem("er is iets misgelopen.");
                }
            }

        }

    }
}
