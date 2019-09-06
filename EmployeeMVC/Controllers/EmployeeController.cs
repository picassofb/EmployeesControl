using System;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Configuration;
using EmployeeMVC.Models;

namespace EmployeeMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string _connectionString = WebConfigurationManager.AppSettings["connectionString"];

        // GET: Employee
        [HttpGet]
        public ActionResult Index()
        {

            DataTable dtbTasks = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("Select * from EmployeeTasks", sqlConnection);
                sqlDataAdapter.Fill(dtbTasks);
            }

            ViewData["Tasks"] = dtbTasks;

            DataTable dtblEmployees = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("Select * from Employee", sqlConnection);
                sqlDataAdapter.Fill(dtblEmployees);
            }

            return View(dtblEmployees);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        [HttpGet]
        public ActionResult Create()
        {

            return View(new EmployeeModel());
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeModel employeeModel)
        {
            try
            {
                if (employeeModel.PictureUpload != null)
                {
                    var extension = Path.GetExtension(employeeModel.PictureUpload.FileName);
                    var filename = Guid.NewGuid() + extension;
                    employeeModel.PicturePath = "~/Files/Images/" + filename;
                    employeeModel.PictureUpload.SaveAs(Path.Combine(Server.MapPath("~/Files/Images/"), filename));
                }

                using (var sqlConnection=new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    const string query = "INSERT INTO Employee VALUES(@Name,@Position,@Office,@Salary,@PicturePath)";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Name", employeeModel.Name);
                    sqlCommand.Parameters.AddWithValue("@Position", employeeModel.Position);
                    sqlCommand.Parameters.AddWithValue("@Office", employeeModel.Office);
                    sqlCommand.Parameters.AddWithValue("@Salary", employeeModel.Salary);
                    sqlCommand.Parameters.AddWithValue("@PicturePath", employeeModel.PicturePath);
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

        // GET: Employee/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EmployeeModel employeeModel = new EmployeeModel();
            DataTable dtEmployee = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Employee WHERE EmployeeId = @EmployeeId";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@EmployeeId", id);
                sqlDataAdapter.Fill(dtEmployee);
            }

            if (dtEmployee.Rows.Count > 0)
            {
                employeeModel.EmployeeId = Convert.ToInt32(dtEmployee.Rows[0][0].ToString());
                employeeModel.Name = dtEmployee.Rows[0][1].ToString();
                employeeModel.Position = dtEmployee.Rows[0][2].ToString();
                employeeModel.Office = dtEmployee.Rows[0][3].ToString();
                if (!string.IsNullOrEmpty(dtEmployee.Rows[0][4].ToString())) employeeModel.Salary = Convert.ToInt32(dtEmployee.Rows[0][4].ToString());
                employeeModel.PicturePath = dtEmployee.Rows[0][5].ToString();
                

                return View(employeeModel);
            }

            return RedirectToAction("Index");


        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(EmployeeModel employeeModel)
        {
            if (employeeModel.PictureUpload != null)
            {
                var extension = Path.GetExtension(employeeModel.PictureUpload.FileName);
                var filename = Guid.NewGuid() + extension;
                employeeModel.PicturePath = "~/Files/Images/" + filename;
                employeeModel.PictureUpload.SaveAs(Path.Combine(Server.MapPath("~/Files/Images/"), filename));
            }

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                const string query = "UPDATE Employee SET Name=@Name, Position=@Position, Office=@Office, Salary=@Salary, PicturePath=@PicturePath Where EmployeeId=@EmployeeId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Name", employeeModel.Name);
                sqlCommand.Parameters.AddWithValue("@Position", employeeModel.Position ?? string.Empty);
                sqlCommand.Parameters.AddWithValue("@Office", employeeModel.Office ?? string.Empty);
                sqlCommand.Parameters.AddWithValue("@Salary", employeeModel.Salary);
                sqlCommand.Parameters.AddWithValue("@PicturePath", employeeModel.PicturePath);
                sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeModel.EmployeeId);

                sqlCommand.ExecuteNonQuery();
            }

            TempData["message"] = "Edited successfully!";
            return RedirectToAction("Index");
        }

        // GET: Employee/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                const string query = "DELETE FROM Employee Where EmployeeId=@EmployeeId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@EmployeeId", id);

                sqlCommand.ExecuteNonQuery();
            }



            return RedirectToAction("Index");
        }

        // POST: Employee/Delete/5
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
