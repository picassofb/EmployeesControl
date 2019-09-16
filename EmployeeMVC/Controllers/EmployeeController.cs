using System;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.IO;
using System.Web.Configuration;
using EmployeeMVC.Models;
using EmployeeMVC.Repository;

namespace EmployeeMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string _connectionString = WebConfigurationManager.AppSettings["connectionString"];

        private readonly IRepository _repo;

        public EmployeeController()
        {
            _repo = new Repository.Repository();
        }


        // GET: Employee
        [HttpGet]
        public ActionResult Index()
        {
            return View(_repo.GetEmployees());
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
        public ActionResult Create(EmployeeModel employee)
        {
            employee = UploadImage(employee);

            var response = _repo.InsertEmployee(employee);

            TempData["message"] = response;

            if (response.IsSuccess) return RedirectToAction("Index");

            return View();


        }

        // GET: Employee/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var employee = _repo.GetEmployeeById(id);

            if (employee.IsSuccess)
            {
                return View(employee.Result);
            }

            return RedirectToAction("Index");


        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(EmployeeModel employee)
        {
            employee = UploadImage(employee);

            var response = _repo.UpdateEmployee(employee);

            TempData["message"] = response;

            return RedirectToAction("Index");
        }

        // GET: Employee/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {

            var response = _repo.DeleteEmployee(id);

            TempData["message"] = response;

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


        #region Private Methods

        private EmployeeModel UploadImage(EmployeeModel employee)
        {
            if (employee.PictureUpload == null) return employee;

            var extension = Path.GetExtension(employee.PictureUpload.FileName);
            var filename = Guid.NewGuid() + extension;
            employee.PicturePath = "~/Files/Images/" + filename;
            employee.PictureUpload.SaveAs(Path.Combine(Server.MapPath("~/Files/Images/"), filename));

            return employee;
        }

        #endregion

    }
}
