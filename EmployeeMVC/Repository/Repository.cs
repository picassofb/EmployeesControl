using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
                    sqlCommand.Parameters.AddWithValue("@Position", employee.Position ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("@Office", employee.Office ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("@Salary", employee.Salary ?? 0);
                    sqlCommand.Parameters.AddWithValue("@PicturePath", employee.PicturePath);
                    sqlCommand.ExecuteNonQuery();
                }

                return new ResponseModel { IsSuccess = true, Message = "Employee added successfully!"};
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

            var employees = ConvertDataTableToList<EmployeeModel>(dtblEmployees);

            foreach (var employee in employees)
            {
                employee.Tasks = GetTasksByEmployee(employee.EmployeeId);
            }

            return employees;
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
                var employeeModel = ConvertDataTableToList<EmployeeModel>(dtEmployee);

                response.IsSuccess = true;
                response.Result = employeeModel.ElementAt(0);
            }

            return response;
        }

        #endregion

        #region Tasks Methods

        public ResponseModel InsertTask(TaskModel task)
        {
            var response = new ResponseModel();

            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    const string query = "INSERT INTO EmployeeTasks VALUES(@EmployeeId,@TaskDescription)";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", task.EmployeeId);
                    sqlCommand.Parameters.AddWithValue("@TaskDescription", task.TaskDescription);
                    sqlCommand.ExecuteNonQuery();
                }

                response.IsSuccess = true;
                response.Message = "Task Created";
            }
            catch (Exception e)
            {
                response.IsSuccess = true;
                response.Message = e.Message;
            }


            return response;
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

            return ConvertDataTableToList<TaskModel>(dtbTasks);
        }

        public ResponseModel UpdateTask(TaskModel task)
        {
            var response = new ResponseModel();

            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    const string query = "UPDATE EmployeeTasks SET EmployeeId=@EmployeeId, TaskDescription=@TaskDescription WHERE TaskId=@TaskId";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", task.EmployeeId);
                    sqlCommand.Parameters.AddWithValue("@TaskDescription", task.TaskDescription ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("@TaskId", task.TaskId);
                    sqlCommand.ExecuteNonQuery();
                }
                response.IsSuccess = true;
                response.Message = "Task Edited.";
            }
            catch (Exception e)
            {
                response.IsSuccess = true;
                response.Message = e.Message;
            }

            return response;
        }

        public ResponseModel DeleteTask(int employeeId)
        {
            throw new NotImplementedException();
        }

        public ResponseModel GetTaskById(int employeeId)
        {
            var response=new ResponseModel();

            try
            {
                var dtEmployeeTask = new DataTable();

                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM EmployeeTasks WHERE TaskId = @TaskId";
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@TaskId", employeeId);
                    sqlDataAdapter.Fill(dtEmployeeTask);
                }

                if (dtEmployeeTask.Rows.Count > 0)
                {
                    var employeeTaskModel = ConvertDataTableToList<TaskModel>(dtEmployeeTask);

                    response.IsSuccess = true;
                    response.Result = employeeTaskModel.ElementAt(0);
                }

            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Result = e.Message;
            }

            return response;
        }

        #endregion

        #region Private Methods

        private List<TaskModel> GetTasksByEmployee(int employeeId)
        {
            var dtTasks = new DataTable();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                const string query = "SELECT * FROM EmployeeTasks WHERE EmployeeId = @EmployeeId";
                var sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                sqlDataAdapter.Fill(dtTasks);
            }

            return ConvertDataTableToList<TaskModel>(dtTasks);
        }

        private static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    //in case you have a enum/GUID datatype in your model
                    //We will check field's dataType, and convert the value in it.
                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            var convertedValue = GetValueByDataType(pro.PropertyType, dr[column.ColumnName]);
                            pro.SetValue(obj, convertedValue, null);
                        }
                        catch (Exception e)
                        {
                            //ex handle code                   
                            throw;
                        }
                        //pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        private static object GetValueByDataType(Type propertyType, object o)
        {
            if (o.ToString() == "null")
            {
                return null;
            }
            if (propertyType == (typeof(Guid)) || propertyType == typeof(Guid?))
            {
                return Guid.Parse(o.ToString());
            }
            else if (propertyType == typeof(int) || propertyType.IsEnum || propertyType == typeof(int?))
            {
                return Convert.ToInt32(o);
            }
            else if (propertyType == typeof(decimal))
            {
                return Convert.ToDecimal(o);
            }
            else if (propertyType == typeof(long))
            {
                return Convert.ToInt64(o);
            }
            else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            {
                return Convert.ToBoolean(o);
            }
            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                return Convert.ToDateTime(o);
            }
            return o.ToString();
        }

        #endregion


    }
}