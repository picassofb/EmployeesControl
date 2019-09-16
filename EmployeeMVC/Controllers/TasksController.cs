using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Mvc;
using EmployeeMVC.Models;
using EmployeeMVC.Repository;

namespace EmployeeMVC.Controllers
{
    public class TasksController : Controller
    {
        private readonly string _connectionString = WebConfigurationManager.AppSettings["connectionString"];

        private readonly IRepository _repo;

        public TasksController()
        {
            _repo = new Repository.Repository();
        }

        // GET: Tasks
        public ActionResult Index()
        {
            return View(_repo.GetEmployees());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.Employees = UpdateEmployeeSelect(0);  

            return View(new TaskModel());
        }

        // POST: Tasks/Create
        [HttpPost]
        public ActionResult Create(TaskModel task)
        {
            var response = _repo.InsertTask(task);

            TempData["message"] = response;

            if (response.IsSuccess) return RedirectToAction("Index");

            return View();
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Employees = UpdateEmployeeSelect(id);

            var response = _repo.GetTaskById(id);

            if (response.IsSuccess) return View(response.Result);

            return RedirectToAction("Index");
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        public ActionResult Edit(TaskModel task)
        {
            var response = _repo.UpdateTask(task);

            TempData["message"] = response;

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

        #region Private Methods

        private List<SelectListItem> UpdateEmployeeSelect(int idEmpployeeToSearch)
        {
            var employeesList = _repo.GetEmployees();

            var employees = new List<SelectListItem>();
            foreach (var employee in employeesList)
            {
                employees.Add(new SelectListItem
                {
                    Text = employee.Name,
                    Value = employee.EmployeeId.ToString(),
                    Selected = employee.EmployeeId == idEmpployeeToSearch
                });
            }

            return employees;
        }

        #endregion

    }
}
