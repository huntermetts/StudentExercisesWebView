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
    public class CohortController : Controller
    {

        /*
         Establishing connection
        */
        private readonly IConfiguration _configuration;

        public CohortController(IConfiguration configuration)
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


        // GET: Cohort
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT cohort.id as cId, cohort.[name] as cName " +
                        "FROM Cohort";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohortList = new List<Cohort>();

                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("cId")),
                           Name = reader.GetString(reader.GetOrdinal("cName"))
                        };
                        cohortList.Add(cohort);
                    }

                    reader.Close();
                    return View(cohortList);
                }
            }
        }

        // GET: Cohort/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = "SELECT cohort.id as cId, cohort.[name] as cName " +
                        "FROM Cohort" +
                        " WHERE cohort.id =  @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();



                    Cohort cohort = null;
                    if (reader.Read())
                    {
                        cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("cId")),
                            Name = reader.GetString(reader.GetOrdinal("cName")),
                        };

                    }

                    reader.Close();
                    return View(cohort);
                }
            }
        }

        // GET: Cohort/Create
        public ActionResult Create()
        {
            CohortCreateViewModel viewModel =
                new CohortCreateViewModel();
            return View(viewModel);
        }

        // POST: Cohort/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CohortCreateViewModel viewModel)
        {  
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO cohort ([name])
                                             VALUES (@name)";
                        cmd.Parameters.Add(new SqlParameter("@name", viewModel.CohortName));
                       

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }
        }

        // GET: Cohort/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cohort/Edit/5
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

        // GET: Cohort/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cohort/Delete/5
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