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

namespace WebApiTeam10.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly SqlConnection sqlConn;
        public StudentsController(SqlConnection conn)
        {
            sqlConn = conn;
        }
        [HttpGet]
        public ActionResult GetStudents()
        {
            throw new NotImplementedException();
            //string jsonResult;
            //var dt = sqlConn.SelectAll("students").DataTable;
            ////convert datatable naar string
            //jsonResult = JsonConvert.SerializeObject(dt);
            //return Ok(jsonResult);

        }

        [HttpPost]
        public ActionResult<Student> AddStudent(Student student)
        {
            throw new NotImplementedException();
            //try
            //{
            //    throw new Exception("Er is een probleem opgetreden");
            //    var s = sqlConn.Insert(student);
            //    return Ok(s);
            //}
            //catch (Exception ex)
            //{
            //    var ja = this.Problem(ex.Message);
            //    return ja;
            //}

        }

    }
}