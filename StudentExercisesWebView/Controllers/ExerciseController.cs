using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesWebView.Models;
using StudentExercisesWebView.Models.ViewModels;

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
            ExerciseCreateViewModel viewModel =
              new ExerciseCreateViewModel();
            return View(viewModel);
        }

        // POST: Exercise/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExerciseCreateViewModel viewModel)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO exercise ([Name], Language)
                                             VALUES (@name, @language)";
                    cmd.Parameters.Add(new SqlParameter("@name", viewModel.ExerciseName));
                    cmd.Parameters.Add(new SqlParameter("@language", viewModel.ExerciseLanguage));


                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // GET: Exercise/Edit/5
        public ActionResult Edit(int id)
        {
            Exercise exercise = GetExerciseById(id);
            if (exercise == null)
            {
                return NotFound();
            }

            ExerciseEditViewModel viewModel = new ExerciseEditViewModel
            {
                ExerciseName = exercise.Name,
                ExerciseLanguage = exercise.Language
            };

            return View(viewModel);
        }

        // POST: Exercise/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ExerciseEditViewModel viewModel)
        {
           
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE exercise 
                                           SET [name] = @name, 
                                               [language] = @language
                                               WHERE id = @id;";
                        cmd.Parameters.Add(new SqlParameter("@name", viewModel.ExerciseName));
                        cmd.Parameters.Add(new SqlParameter("@language", viewModel.ExerciseLanguage));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
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



        /*
         Fuction to get an instructor by ID
         */
        private Exercise GetExerciseById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT exercise.id as eId, exercise.[name] as eName, exercise.[language] as eLang from Exercise
                                         WHERE  exercise.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Exercise exercise = null;

                    if (reader.Read())
                    {
                        exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("eId")),
                            Name = reader.GetString(reader.GetOrdinal("eName")),
                            Language = reader.GetString(reader.GetOrdinal("eLang")),

                        };
                    }

                    reader.Close();

                    return exercise;
                }
            }
        }


        /*
           Function to get all Exercises
       */
        private List<Exercise> GetAllExercises()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, [name], [language] from Exercise;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();

                    while (reader.Read())
                    {
                        exercises.Add(new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Language = reader.GetString(reader.GetOrdinal("language"))
                        });
                    }
                    reader.Close();

                    return exercises;
                }
            }

        }
    }
}