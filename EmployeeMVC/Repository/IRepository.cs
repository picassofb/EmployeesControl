using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeMVC.Models;

namespace EmployeeMVC.Repository
{
    public interface IRepository
    {
        ResponseModel InsertEmployee(EmployeeModel employee);
        List<EmployeeModel> GetEmployees();
        ResponseModel UpdateEmployee(EmployeeModel employee);
        ResponseModel DeleteEmployee(int employeeId);
        ResponseModel GetEmployeeById(int employeeId);

        ResponseModel InsertTask(TaskModel employee);
        List<TaskModel> GetTasks();
        ResponseModel UpdateTask(TaskModel employee);
        ResponseModel DeleteTask(int employeeId);
        ResponseModel GetTaskById(int employeeId);
    }
}