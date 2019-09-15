using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using EmployeeMVC.Models;

namespace EmployeeMVC.Repository
{
    public class Repository : IRepository
    {
        private readonly string _connectionString = WebConfigurationManager.AppSettings["connectionString"];

        #region Employees Methods

        public ResponseModel InsertEmployee(EmployeeModel employee)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    const string query = "INSERT INTO Employee VALUES(@Name,@Position,@Office,@Salary,@PicturePath)";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Name", employee.Name);
                    sqlCommand.Parameters.AddWithValue("@Position", employee.Position);
                    sqlCommand.Parameters.AddWithValue("@Office", employee.Office);
                    sqlCommand.Parameters.AddWithValue("@Salary", employee.Salary);
                    sqlCommand.Parameters.AddWithValue("@PicturePath", employee.PicturePath);
                    sqlCommand.ExecuteNonQuery();
                }

                return new ResponseModel{IsSuccess = true, Message = "Employee added successfully!"};
            }
            catch (Exception ex)
            {
                return new ResponseModel { IsSuccess = false, Message = ex.Message};
            }
        }

        public List<EmployeeModel> GetEmployees()
        {
            var dtblEmployees = new DataTable();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var sqlDataAdapter = new SqlDataAdapter("Select * from Employee", sqlConnection);
                sqlDataAdapter.Fill(dtblEmployees);
            }

            return LoadToEmployeeClass(dtblEmployees).ToList();
        }

        public ResponseModel UpdateEmployee(EmployeeModel employee)
        {
            var response = new ResponseModel();
            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    const string query = "UPDATE Employee SET Name=@Name, Position=@Position, Office=@Office, Salary=@Salary, PicturePath=@PicturePath Where EmployeeId=@EmployeeId";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Name", employee.Name);
                    sqlCommand.Parameters.AddWithValue("@Position", employee.Position ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("@Office", employee.Office ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("@Salary", employee.Salary);
                    sqlCommand.Parameters.AddWithValue("@PicturePath", employee.PicturePath);
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);

                    sqlCommand.ExecuteNonQuery();
                }

                response.IsSuccess = true;
                response.Message = "Edited Successfully";
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
            }


            return response;
        }

        public ResponseModel DeleteEmployee(int employeeId)
        {
            var response = new ResponseModel();

            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    const string query = "DELETE FROM Employee Where EmployeeId=@EmployeeId";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);

                    sqlCommand.ExecuteNonQuery();
                }

                response.IsSuccess = true;
                response.Message = "Successfully deleted.";

            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
            }

            return response;
        }

        public ResponseModel GetEmployeeById(int employeeId)
        {
            var response = new ResponseModel { IsSuccess = false };

            var dtEmployee = new DataTable();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                const string query = "SELECT * FROM Employee WHERE EmployeeId = @EmployeeId";
                var sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                sqlDataAdapter.Fill(dtEmployee);
            }

            if (dtEmployee.Rows.Count > 0)
            {
                var employeeModel = LoadToEmployeeClass(dtEmployee);
                response.IsSuccess = true;
                response.Result = employeeModel.ElementAt(0);
            }

            return response;
        }

        #endregion

        #region Tasks Methods

        public ResponseModel InsertTask(TaskModel employee)
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetTasks()
        {
            var dtbTasks = new DataTable();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var sqlDataAdapter = new SqlDataAdapter("Select * from EmployeeTasks", sqlConnection);
                sqlDataAdapter.Fill(dtbTasks);
            }

            return LoadToTaskClass(dtbTasks).ToList();
        }

        public ResponseModel UpdateTask(TaskModel employee)
        {
            throw new NotImplementedException();
        }

        public ResponseModel DeleteTask(int employeeId)
        {
            throw new NotImplementedException();
        }

        public ResponseModel GetTaskById(int employeeId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods


        private static IEnumerable<EmployeeModel> LoadToEmployeeClass(DataTable dataTable)
        {
            var employeeList = new List<EmployeeModel>();

            foreach (DataRow datarow in dataTable.Rows)
            {
                var employee = new EmployeeModel
                {
                    EmployeeId = Convert.ToInt32(datarow["EmployeeId"].ToString()),
                    Name = datarow["Name"].ToString(),
                    Position = datarow["Position"].ToString(),
                    Office = datarow["Office"].ToString(),
                    Salary = Convert.ToInt32(datarow["Salary"].ToString()),
                    PicturePath = datarow["PicturePath"].ToString()
                };
                if (!string.IsNullOrEmpty(datarow["Salary"].ToString())) employee.Salary = Convert.ToInt32(datarow["Salary"].ToString());


                employeeList.Add(employee);
            }

            return employeeList;
        }

        private static IEnumerable<TaskModel> LoadToTaskClass(DataTable dataTable)
        {
            var taskList = new List<TaskModel>();

            foreach (DataRow datarow in dataTable.Rows)
            {
                var task = new TaskModel
                {
                    EmployeeId = Convert.ToInt32(datarow["EmployeeId"].ToString()),
                    TaskId = Convert.ToInt32(datarow["TaskId"]),
                    TaskDescription = datarow["TaskDescription"].ToString()
                };


                taskList.Add(task);
            }

            return taskList;
        }

        #endregion


    }
}