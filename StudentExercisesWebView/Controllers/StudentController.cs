using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesWebView.Models;

namespace StudentExercisesWebView.Controllers
{
    public class StudentController : Controller
    {

        /*
         Establishing connection
        */
        private readonly IConfiguration _configuration;

        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
        /*
        Establishing connection
       */

        // GET: Student
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select student.id as studentId, FirstName as studentFirstName, LastName as studentLastName, SlackHandle as studentSlackHandle, cohort.[name] as CohortName, CohortId as studentCohortId from " +
                        "cohort left join student on cohort.id = student.CohortId ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> studentList = new List<Student>();

                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("studentId")),
                            FirstName = reader.GetString(reader.GetOrdinal("studentFirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("studentLastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("studentSlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("studentCohortId")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };
                        studentList.Add(student);
                    }

                    reader.Close();
                    return View(studentList);
                }
            }
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = "select student.id as studentId, FirstName as studentFirstName, LastName as studentLastName, SlackHandle as studentSlackHandle, cohort.[name] as CohortName, CohortId as studentCohortId from " +
                        "cohort left join student on cohort.id = student.CohortId " +
                        "where student.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();



                    Student student = null;
                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("studentId")),
                            FirstName = reader.GetString(reader.GetOrdinal("studentFirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("studentLastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("studentSlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("studentCohortId")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))

                        };
                        
                    }

                    reader.Close();
                    return View(student);
                }
            }
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}