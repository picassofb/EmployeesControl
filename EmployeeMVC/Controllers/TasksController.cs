using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Mvc;
using EmployeeMVC.Models;

namespace EmployeeMVC.Controllers
{
    public class TasksController : Controller
    {
        
        private readonly string _connectionString = WebConfigurationManager.AppSettings["connectionString"];

        // GET: Tasks
        public ActionResult Index()
        {
            DataTable dtblTasks = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var query = " select empT.TaskId, empT.EmployeeId, emp.Name, empT.TaskDescription from EmployeeTasks as empT" +
                            " inner join Employee as emp on empT.EmployeeId = emp.EmployeeId";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.Fill(dtblTasks);
            }

            return View(dtblTasks);
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            DataTable dtblTasks = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var query = " select EmployeeId, Name from Employee";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.Fill(dtblTasks);
            }

            List<SelectListItem> employees = new List<SelectListItem>();
            for (int i = 0; i < dtblTasks.Rows.Count; i++)
            {
                employees.Add(new SelectListItem
                {
                    Text = dtblTasks.Rows[i][1].ToString(),
                    Value = dtblTasks.Rows[i][0].ToString(),
                });
            }

            ViewData["Employees"] = employees;  

            return View(new TaskModel());
        }

        // POST: Tasks/Create
        [HttpPost]
        public ActionResult Create(TaskModel tasksModel)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    const string query = "INSERT INTO EmployeeTasks VALUES(@EmployeeId,@TaskDescription)";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", tasksModel.EmployeeId);
                    sqlCommand.Parameters.AddWithValue("@TaskDescription", tasksModel.TaskDescription);
                    sqlCommand.ExecuteNonQuery();
                }

                TempData["message"] = "Added successfully!";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dtblTasks = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var query = " select EmployeeId, Name from Employee";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.Fill(dtblTasks);
            }

            List<SelectListItem> employees = new List<SelectListItem>();
            for (int i = 0; i < dtblTasks.Rows.Count; i++)
            {
                employees.Add(new SelectListItem
                {
                    Text = dtblTasks.Rows[i][1].ToString(),
                    Value = dtblTasks.Rows[i][0].ToString(),
                    Selected = dtblTasks.Rows[i][0].ToString() == id.ToString()
                });
            }

            ViewData["Employees"] = employees;



            TaskModel taskModel = new TaskModel();
            DataTable dtEmployeeTask = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM EmployeeTasks WHERE TaskId = @TaskId";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@TaskId", id);
                sqlDataAdapter.Fill(dtEmployeeTask);
            }

            if (dtEmployeeTask.Rows.Count > 0)
            {
                taskModel.TaskId = Convert.ToInt32(dtEmployeeTask.Rows[0][0].ToString());
                taskModel.EmployeeId = Convert.ToInt32(dtEmployeeTask.Rows[0][1].ToString());
                taskModel.TaskDescription = dtEmployeeTask.Rows[0][2].ToString();

                return View(taskModel);
            }

            return RedirectToAction("Index");
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        public ActionResult Edit(TaskModel tasksModel)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                const string query = "UPDATE EmployeeTasks SET EmployeeId=@EmployeeId, TaskDescription=@TaskDescription WHERE TaskId=@TaskId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@EmployeeId", tasksModel.EmployeeId);
                sqlCommand.Parameters.AddWithValue("@TaskDescription", tasksModel.TaskDescription ?? string.Empty);
                sqlCommand.Parameters.AddWithValue("@TaskId", tasksModel.TaskId);
                sqlCommand.ExecuteNonQuery();
            }

            TempData["message"] = "Edited successfully!";
            return RedirectToAction("Index");
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tasks/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
