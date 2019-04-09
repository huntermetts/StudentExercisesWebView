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
    public class ExerciseController : Controller
    {

        /*
         Establishing connection
        */
        private readonly IConfiguration _configuration;

        public ExerciseController(IConfiguration configuration)
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


        // GET: Exercise
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT exercise.id as eId, exercise.[name] as eName, exercise.[Language] as eLanguage " +
                        "FROM Exercise";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exerciseList = new List<Exercise>();

                    while (reader.Read())
                    {
                        Exercise exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("eId")),
                            Name = reader.GetString(reader.GetOrdinal("eName")),
                            Language = reader.GetString(reader.GetOrdinal("eLanguage"))
                        };
                        exerciseList.Add(exercise);
                    }

                    reader.Close();
                    return View(exerciseList);
                }
            }
        }

        // GET: Exercise/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = "SELECT exercise.id as eId, exercise.[name] as eName, exercise.[Language] as eLanguage " +
                        "FROM Exercise" +
                        " WHERE Exercise.id =  @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();



                    Exercise exercise = null;
                    if (reader.Read())
                    {
                        exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("eId")),
                            Name = reader.GetString(reader.GetOrdinal("eName")),
                            Language = reader.GetString(reader.GetOrdinal("eLanguage"))
                        };

                    }

                    reader.Close();
                    return View(exercise);
                }
            }
        }

        // GET: Exercise/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Exercise/Create
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

        // GET: Exercise/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Exercise/Edit/5
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

        // GET: Exercise/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Exercise/Delete/5
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