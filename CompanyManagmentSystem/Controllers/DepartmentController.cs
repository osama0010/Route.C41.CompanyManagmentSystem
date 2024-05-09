using CompanyManagmentSystem.BLL.Interfaces;
using CompanyManagmentSystem.BLL.Repositories;
using CompanyManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using System;

namespace CompanyManagmentSystem.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IHostEnvironment _env;

        public DepartmentController(IDepartmentRepository departmentRepository, IHostEnvironment env) // Ask CLR for Creating an Object from Class Implmenting IDepartmentRepos[
        {
            _departmentRepository = departmentRepository;
            _env = env;
        }
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid) //Server side validation
            {
                var count = _departmentRepository.Add(department);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        [HttpGet]
        public IActionResult Details(int? id, string ViewName)
        {
            if(!id.HasValue) 
                return BadRequest(); //400

            var department = _departmentRepository.Get(id.Value);

            if (department == null)
                return NotFound(); //404

            return View(ViewName, department);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
            #region MyRegion
            //if(!id.HasValue) 
            //    return BadRequest();

            //var department = _departmentRepository.Get(id.Value);

            //if (department == null)
            //    return NotFound();

            //return View(department); 
            #endregion
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id, Department department)
        {
                if (id != department.Id)
                    return BadRequest(new ViewResult());

                if (!ModelState.IsValid)
                    return View(department);

            try
            {
                _departmentRepository.Update(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //1.To Log Exceptions
                //2.Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An error has occured during updating the department");

                return View(department);
            }
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(Department department)
        {
            try
            {
                _departmentRepository.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An error has occured during updating the department");

                return View(department);
            }
        }

    }
}
