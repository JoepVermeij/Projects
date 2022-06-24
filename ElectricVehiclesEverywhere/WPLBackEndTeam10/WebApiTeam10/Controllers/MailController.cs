using ClassLibTeam10.Business;
using ClassLibTeam10.Data.Bestellingen;
using ClassLibTeam10.Data.Framework;
using ClassLibTeam10.Entities;
using ClassLibTeam10.Entities.DbEntities;
using ClassLibTeam10.Entities.GeneralEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTeam10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly SqlConnection sqlConn;
        public MailController(SqlConnection conn)
        {
            sqlConn = conn;
        }
        [HttpPost("updatewachtwoord")]
        public ActionResult<Login> RequestChangePassword(Login login)
        {
            try
            {
                //generate token
                //new dict wachtwoord + token
                //sturen token op
                sqlConn.ProcessPasswordRequest(login);
                return Ok("request succeeded");
            }
            catch (Exception ex)
            {
                return Ok("request failed");
            }
            
        }
        [HttpPost("wachtwoordemailconfirmed")]
        public ActionResult<WebToken> ChangePassword([FromForm]WebToken notInDbToken)
        {
            if (HandlePasswords.IsValidToken(notInDbToken))
            {
                UpdateResult ur = sqlConn.ChangePassword(notInDbToken);
                return Ok("Je wachtwoord is veranderd");
            }
            else
            {
                return Ok("er is iets foutgelopen, probeer later nog eens");
            }
            
        }
        [HttpPost("deleteprofiel")]
        public ActionResult<string> RequestDeleteProfile(WebToken webToken)
        {
            try
            {
                bool succeeded = sqlConn.ProcessProfileDeleteRequest(webToken);
                return Ok("\"Mail is verzonden!\"");
            }
            catch (Exception ex)
            {
                return Problem("Er is iets misgelopen");
            }

        }
        [HttpPost("deleteprofielconfirmed")]
        public ActionResult<string> DeleteProfile([FromForm] WebToken webToken)
        {
            if (HandleProfiel.IsValidToken(webToken))
            {
                try
                {
                    sqlConn.DeleteKlantById(webToken);

                    return Ok("\"Je account is verwijderd\"");
                }
                catch (Exception ex)
                {
                    return Problem("Er is iets misgelopen");
                }
            }
            else
            {
                return Ok("Je token is niet meer valid, probeer opnieuw!");
            }

        }
        [HttpPost("deleteproduct")]
        public ActionResult<Klant> RequestDeleteProduct(BestellingCheck bestellingCheck)
        {
            try
            {
                bool succeeded = sqlConn.ProcessProductDeleteRequest(bestellingCheck);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost("deleteproductconfirmed")]
        public ActionResult<WebToken> DeleteProduct([FromForm] Bestelling bestelling)
        {
            try
            {
                if (bestelling.StartDatum < DateTime.Now)
                {
                    return Problem("Is al Gebruikt");
                }
                sqlConn.DeleteByObject(bestelling);
                return Ok("Je product is verwijderd");
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }
    }
}
